using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Twenty57.Linx.Components.File
{
	public static class RobocopyX
	{
		public const string ErrorTag = "ERROR";

		public static void Execute(string sourceDirectory, string targetDirectory, bool copySubdirectories, bool includeEmptySubdirectories, bool restartMode, bool backupMode, int numberofRetries, int timeBetweenRetries,
				string filePattern, string excludeFiles, string excludDirectories, bool excludesChangedFiles, bool excludesNewerFiles, bool excludesOlderFiles, bool excludesExtraFilesAndDirectories, bool excludesLonelyFilesAndDirectories, bool includesSameFiles, bool includesTweakedFiles, int maxFileSize, int minFileSize, string maxAge, string minAge, string maxLastAccessDate, string minLastAccessDate,
				string logFile, bool overwriteFile, bool listFilesOnly, bool logAllExtraFiles, bool verbose, bool includeSourceFileTimestamps, bool includeFullPath, bool logSizeAsBytes, bool excludeFileSize, bool excludeFileClass, bool excludeFileNames,
				 bool excludeDirectoryNames, bool excludeProgress, bool includeETA, RobocopyShared.ModeOption modeOption, Action<string> logger)
		{
			if (string.IsNullOrEmpty(sourceDirectory))
				throw new Exception("Source directory cannot be null or empty.");

			if (string.IsNullOrEmpty(targetDirectory))
				throw new Exception("Target directory cannot be null or empty.");

			if (!Directory.Exists(sourceDirectory))
				throw new DirectoryNotFoundException(string.Format("Source directory [{0}] does not exist.", sourceDirectory));

			sourceDirectory = sourceDirectory.TrimEnd(Path.DirectorySeparatorChar);
			targetDirectory = targetDirectory.TrimEnd(Path.DirectorySeparatorChar);

			StringBuilder commandLine = new StringBuilder();
			commandLine.Append(string.Format("\"{0}\" \"{1}\" ", sourceDirectory, targetDirectory));

			if (modeOption == RobocopyShared.ModeOption.Mirror)
				commandLine.Append(" /mir");
			if (modeOption == RobocopyShared.ModeOption.MoveFiles)
				commandLine.Append(" /mov");
			if (modeOption == RobocopyShared.ModeOption.MoveFilesAndDirs)
				commandLine.Append(" /move");
			if (copySubdirectories)
				commandLine.Append(" /s");
			if (includeEmptySubdirectories)
				commandLine.Append(" /e");
			if (restartMode)
				commandLine.Append(" /z");
			if (backupMode)
				commandLine.Append(" /b");
			if (numberofRetries > 0)
				commandLine.AppendFormat(" /r:{0}", numberofRetries);
			if (timeBetweenRetries > 0)
				commandLine.AppendFormat(" /w:{0}", timeBetweenRetries);

			if (!string.IsNullOrEmpty(filePattern))
				commandLine.AppendFormat(" {0}", filePattern);
			if (!string.IsNullOrEmpty(excludeFiles))
				commandLine.AppendFormat(" /xf {0}", excludeFiles);
			if (!string.IsNullOrEmpty(excludDirectories))
				commandLine.AppendFormat(" /xd {0}", excludDirectories);
			if (excludesChangedFiles)
				commandLine.Append(" /xct");
			if (excludesNewerFiles)
				commandLine.Append(" /xn");
			if (excludesOlderFiles)
				commandLine.Append(" /xo");
			if (excludesExtraFilesAndDirectories)
				commandLine.Append(" /xx");
			if (excludesLonelyFilesAndDirectories)
				commandLine.Append(" /xl");
			if (includesSameFiles)
				commandLine.Append(" /is");
			if (includesTweakedFiles)
				commandLine.Append(" /it");

			if (maxFileSize > 0)
				commandLine.AppendFormat(" /max:{0}", maxFileSize);
			if (minFileSize > 0)
				commandLine.AppendFormat(" /min:{0}", minFileSize);
			if (!string.IsNullOrEmpty(maxAge))
				commandLine.AppendFormat(" /maxage:{0}", maxAge);
			if (!string.IsNullOrEmpty(minAge))
				commandLine.AppendFormat(" /minage:{0}", minAge);
			if (!string.IsNullOrEmpty(maxLastAccessDate))
				commandLine.AppendFormat(" /maxlad:{0}", maxLastAccessDate);
			if (!string.IsNullOrEmpty(minLastAccessDate))
				commandLine.AppendFormat(" /maxlad:{0}", minLastAccessDate);
			if (!string.IsNullOrEmpty(logFile) && (!overwriteFile))
				commandLine.AppendFormat(" /log+:{0}", logFile);
			if (overwriteFile)
				commandLine.AppendFormat(" /log:{0}", logFile);
			if (listFilesOnly)
				commandLine.Append(" /l");
			if (logAllExtraFiles)
				commandLine.Append(" /x");
			if (verbose)
				commandLine.Append(" /v");
			if (includeSourceFileTimestamps)
				commandLine.Append(" /ts");
			if (includeFullPath)
				commandLine.Append(" /fp");
			if (logSizeAsBytes)
				commandLine.Append(" /bytes");
			if (excludeFileSize)
				commandLine.Append(" /ns");
			if (excludeFileClass)
				commandLine.Append(" /nc");
			if (excludeFileNames)
				commandLine.Append(" /nfl");
			if (excludeDirectoryNames)
				commandLine.Append(" /ndl");
			if (excludeProgress)
				commandLine.Append(" /np");
			if (includeETA)
				commandLine.Append(" /eta");

			ExecuteProcess(commandLine.ToString(), logger);
		}

		private static void ExecuteProcess(string command, Action<string> logger = null)
		{
			string outputData = String.Empty;
			string errorData = String.Empty;
			int exitCode = 0;

			using (Process robocopyProcess = new Process())
			{
				if (logger != null)
					logger(String.Format("Executing command <robocopy.exe {0}>", command));

				robocopyProcess.StartInfo.Arguments = command.ToString();
				robocopyProcess.StartInfo.FileName = "robocopy.exe";
				robocopyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
				robocopyProcess.StartInfo.UseShellExecute = false;
				robocopyProcess.StartInfo.CreateNoWindow = true;
				robocopyProcess.StartInfo.RedirectStandardOutput = true;
				robocopyProcess.StartInfo.RedirectStandardError = true;

				robocopyProcess.OutputDataReceived += (sender, args) =>
				{
					if (!String.IsNullOrEmpty(args.Data))
					{
						if (args.Data.Contains(ErrorTag))
							outputData += args.Data + Environment.NewLine;
					}
				};

				robocopyProcess.ErrorDataReceived += (sender, args) =>
				{
					if (!String.IsNullOrEmpty(args.Data))
						errorData += args.Data;
				};

				robocopyProcess.Start();
				robocopyProcess.BeginOutputReadLine();
				robocopyProcess.BeginErrorReadLine();
				robocopyProcess.WaitForExit();

				if (robocopyProcess.HasExited)
				{
					exitCode = robocopyProcess.ExitCode;

					if (exitCode != 0 && errorData.Length > 0)
					{
						throw new Exception(String.Format("Robocopy error: {0}", errorData));
					}

					if (exitCode != 0 && outputData.Length > 0)
					{
						throw new Exception(String.Format("Robocopy error: {0}", outputData));
					}
				}
				else
				{
					robocopyProcess.Kill();
				}
			}
		}
	}
}
