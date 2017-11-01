BinaryFileWrite
===============

The BinaryFileWrite function allows for writing binary files.

Properties
----------

-  #### File Path

    The path to the file you wish to write to. You can also set this property to the FileHandle 
    of a [FileOpen](../FileOpen/) function when its 'Is text' option is not checked.

-  #### Contents (binary)

    The binary data that will be written to the file.

-  #### File exists

    What to do if a file with the specified name is already there:

    *Append the data* will find the end of the file and add the data to the
    file in that location.

    *Increment the filename* will try and find a file with the specified name. 
    If one such file is found that file will not be touched, but a new file is created
    instead. The new file will have the specified filename with a '\_1'
    appended. If a list of files with the filename and appended numbers
    already exist, a new file is created and the next unallocated number
    is appended to that file ('\_2', '\_3' etc.).

    *Overwrite the file* will write the data to an empty new file with the specified
    filename. The old file will cease to exist.

    *Throw exception* should be used if you do not expect the file to exist and don't 
    want to use any of the other options. This option will halt the process and log an
    error on the server.

- #### File does not exist

    What to do if a file with the specified name is not already there:

    *Create the file* will create a file with the specified name.

    *Throw exception* should be used if you expect the file to exist and don't 
    want to create a new one. This option will halt the process and log an error on the server.
