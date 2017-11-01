FileOperations
==============

FileOperations allows for file copy, move, delete, file-exists, as well as create-temp-file queries.

Properties
----------

-  #### Action

    The function supports *Copy*, *Move*, *Delete*, *File exists* and *Create temp file*
    operations.

-  #### Source file path

    The file path where the file can be found.

-  #### Keep file name{#keepFileNameProperty}

    If checked, the file will retain the same name when moved or copied
    to a different folder (subject to the [File
    exists](#fileExistsProperty) option). Otherwise, the new file name
    must be specified.

-  #### Destination folder path

    The folder where the file should be moved or copied to (visible when
    [Keep file name](#keepFileNameProperty) is checked).

-  #### Destination file path

    The file path, including file name, where the file should be moved
    or copied to (visible when [Keep file name](#keepFileNameProperty)
    is not checked).

-  #### File exists{#fileExistsProperty}

    Three options are available for the case where the target file
    already exists in that directory.

    *Do nothing* skips the file operation and the function will do
    nothing.

    *Overwrite file* will cause the old file to be entirely overwritten
    by a new file.

    *Increment file name* will cause the new file to have a number
    appended to the filename. So, if the function is expecting to copy
    or move a file called *temp.txt* to a new folder, but that file
    already exists in that location, then the function will create a
    file with the name *temp\_1.txt*. The next time the function
    attempts to copy or move *temp.txt* to this folder, it will be
    called *temp\_2.txt*.


