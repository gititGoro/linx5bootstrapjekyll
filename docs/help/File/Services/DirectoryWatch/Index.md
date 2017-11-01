DirectoryWatch
==============

Watch a directory for file changes, creations, renaming and deletions using the DirectoryWatch Service. 
Call a process or run some logic when the service event occurs.

Properties
----------

-  #### Buffer size

    You can set the buffer to 4 KB or larger, but it must not exceed 64
    KB. If you try to set the buffer size property to less than 4096
    bytes, your value is discarded and the buffer size property is set
    to 4096 bytes. For best performance, use a multiple of 4 KB on
    Intel-based computers.

-  #### Filter

    Add a file mask to filter specific file types (e.g. \*.txt)

    If no value is specified, all files (\*.\*) will be watched.

-  #### Include subdirectories

    A flag to indicate whether the service should process files in
    sub-folders of the specified path.

-  #### Notify filter

    The types of changes to watch for. These options only applicable
    when the "Watch for changes" property is true.

    1.  *Attributes* Occurs when a file or directory has been saved with
        changed attributes
    2.  *Creation time* Occurs when a file or directory was created
    3.  *Directory name* Occurs when a directory name has changed
    4.  *File name* Occurs when a filename has changed
    5.  *Last access* Occurs when a file was accessed
    6.  *Last write* Occurs when a file was overwritten or data was
        appended as well as when a folder was written to
    7.  *Security* Occurs when the security settings of a file or
        directory has changed
    8.  *Size* Occurs when a filesize has changed
<p>
-  #### Path

    The path of the directory to be watched.

-  #### Watch for changes

    Activates the "ChangedEvent".

-  #### Watch for creation

    Activates the "CreatedEvent".

-  #### Watch for deletions

    Activates the "DeletedEvent".

-  #### Watch for renaming

    Activates the "RenamedEvent".

Events
------

-  #### ChangedEvent

    Activated by the "Watch for changes" property. Raised when a file or
    directory has changed in the watched directory. Use the "Notify
    filter" property to specify the relevant changes to watch for.

-  #### CreatedEvent

    Activated by the "Watch for creation" property. Raised when a file
    or directory has been created in the watched directory.

-  #### DeletedEvent

    Activated by the "Watch for deletions" property. Raised when a file
    or directory has been deleted in the watched directory.

-  #### RenamedEvent

    Activated by the "Watch for renaming" property. Raised when a file
    or directory has been renamed in the watched directory.

-  #### ErrorEvent

    Raised when an error has occurred in the watched directory.

Links
-----

[MSDN FileSystemWatcher
Class](http://msdn.microsoft.com/en-us/library/system.io.filesystemwatcher.aspx)
