Robocopy
========

This function allows copying, mirroring or moving of files and directories by making use of the Windows Robocopy command.

Properties
----------

-  #### Mode

    The operation to perform on the source directory. The available modes are *Copy*, *Mirror*, *Move files* and *Move files and directories*.

-  #### Source directory

    Indicates the full path of the directory on which to perform the action.

-  #### Target directory

    Indicates the full path to the destination directory.

-  #### Copy subdirectories (/s)
    
    Copy subdirectories, excluding empty ones.

-  #### Include empty subdirectories (/e)
    Copy subdirectories, including empty ones.

-  #### Restart mode (/z)
    Copy files in restart mode.
 
-  #### Backup mode (/b)
    Copy files in backup mode.

-  #### Number of retries (/r:&lt;N&gt;)
    Indicates the number of retries on failed copies.

-  #### Time between retries (/w:&lt;N&gt;)
    Indicates the wait time between retries, in seconds.

-  #### File pattern
    File(s) to copy.  If no value is specified, all files (\*.*) will be copied.
  
-  #### Exclude files (/xf &lt;FileName&gt;[...])
    Excludes files that match the specified names or paths. Note that FileName can include wildcard characters (* and ?).

-  #### Exclude directories (/xd &lt;Directory&gt;[...])
    Excludes directories that match the specified names and paths.

-  #### Excludes changed files (/xct)
    Excludes changed files.

-  #### Excludes newer files (/xn)
    Excludes newer files.

-  #### Excludes older files (/xo)
    Excludes older files.

-  #### Excludes extra files and directories (/xx)
    Excludes extra files and directories.

-  #### Excludes lonely files and directories (/xl)
    Excludes lonely files and directories.

-  #### Includes same files (/is)
    Includes the same files.

-  #### Includes tweaked files (/it)
    Includes tweaked files.

-  #### Max file size (/max:&lt;N&gt;)
    Indicates the maximum file size (to exclude files bigger than N bytes).

-  #### Min file size (/min:&lt;N&gt;)
    Indicates the minimum file size (to exclude files smaller than N bytes).

-  #### Max age (/maxage:&lt;N&gt;)
    Indicates the maximum file age (to exclude files older than N days or date).

-  #### Min age (/minage:&lt;N&gt;)
    Indicates the minimum file age (exclude files newer than N days or date).

-  #### Max last access date (/maxlad:&lt;N&gt;)
    Indicates the maximum last access date (excludes files unused since N).

-  #### Min last access date (/minlad:&lt;N&gt;)
    Indicates the minimum last access date (excludes files used since N) If N is less than 1900, N specifies the number of days. Otherwise, N specifies a date in the format YYYYMMDD.

-  #### Log file (/log+:&lt;LogFile&gt;)
    Writes the status output to the log file (appends the output to the existing log file).

-  #### Overwrite file (/log:&lt;LogFile&gt;)
    Writes the status output to the log file (overwrites the existing log file).

-  #### List files only (/l)
	Indicates that files are to be listed only (and not copied, deleted, or time stamped).

-  #### Log all extra files (/x)
    Reports all extra files, not just those that are selected.

-  #### Verbose (/v)
    Produces verbose output, and shows all skipped files.

-  #### Include source file timestamps (/ts)
    Includes source file time stamps in the output.

-  #### Include full path (/fp)
    Includes the full path names of the files in the output.

-  #### Log size as bytes (/bytes)
    Prints sizes, as bytes.

-  #### Exclude file size (/ns)
    Indicates that file sizes are not to be logged.

-  #### Exclude file class (/nc)
    Indicates that file classes are not to be logged.

-  #### Exclude file names (/nfl)
    Indicates that file names are not to be logged.

-  #### Exclude directory names (/ndl)
    Indicates that directory names are not to be logged.

-  #### Exclude progress (/np)
    Indicates that the progress of the copying operation (the number of files or directories copied so far) will not be displayed.

-  #### Include ETA (/eta)
    Shows the estimated time of arrival (ETA) of the copied files.
 
Links
-----

- [Wikipedia: Robocopy](https://en.wikipedia.org/wiki/Robocopy)  
- [TechNet: Robocopy](https://technet.microsoft.com/en-us/library/cc733145.aspx)  
