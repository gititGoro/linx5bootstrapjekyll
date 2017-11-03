using System;
using System.Collections.Generic;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.File
{
	public class RobocopyCodeGenerator : FunctionCodeGenerator
	{
		public RobocopyCodeGenerator(IFunctionData data)
				: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			RobocopyShared.ModeOption modeOption = FunctionData.Properties[RobocopyShared.ModePropertyName].GetValue<RobocopyShared.ModeOption>();

			functionBuilder.AddCode(string.Format("Twenty57.Linx.Components.File.RobocopyX.Execute({0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},message => {39}.Log(message));",
					functionBuilder.GetParamName(RobocopyShared.SourceDirectoryPropertyName)
				, functionBuilder.GetParamName(RobocopyShared.TargetDirectoryPropertyName)
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.CopySubdirectoriesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.IncludeEmptySubdirectoriesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.RestartModePropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.BackupModePropertyName].GetValue<bool>())
				, functionBuilder.GetParamName(RobocopyShared.NumberOfRetriesPropertyName)
				, functionBuilder.GetParamName(RobocopyShared.TimeBetweenRetriesPropertyName)
				, functionBuilder.GetParamName(RobocopyShared.FilePatternPropertyName)
				, functionBuilder.GetParamName(RobocopyShared.ExcludeFilesPropertyName)
				, functionBuilder.GetParamName(RobocopyShared.ExcludeDirectoriesPropertyName)
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludesChangedFilesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludesNewerFilesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludesOlderFilesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludesExtraFilesAndDirectoriesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludesLonelyFilesAndDirectoriesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.IncludesSameFilesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.IncludesTweakedFilesPropertyName].GetValue<bool>())
				, functionBuilder.GetParamName(RobocopyShared.MaxFileSizePropertyName)
				, functionBuilder.GetParamName(RobocopyShared.MinFileSizePropertyName)
				, functionBuilder.GetParamName(RobocopyShared.MaxAgePropertyName)
				, functionBuilder.GetParamName(RobocopyShared.MinAgePropertyName)
				, functionBuilder.GetParamName(RobocopyShared.MaxLastAccessDatePropertyName)
				, functionBuilder.GetParamName(RobocopyShared.MinLastAccessDatePropertyName)
				, functionBuilder.GetParamName(RobocopyShared.LogFilePropertyName)
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.OverwriteFilePropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ListFilesOnlyPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.LogAllExtraFilesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.VerbosePropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.IncludeSourceFileTimestampsPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.IncludeFullPathPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.LogSizeAsBytesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludeFileSizePropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludeFileClassPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludeFileNamesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludeDirectoryNamesPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.ExcludeProgressPropertyName].GetValue<bool>())
				, CSharpUtilities.BoolAsString(FunctionData.Properties[RobocopyShared.IncludeETAPropertyName].GetValue<bool>())
				, CSharpUtilities.EnumAsString(modeOption)
				, functionBuilder.ContextParamName));

			functionBuilder.AddAssemblyReference(typeof(Robocopy));
		}
	}
}
