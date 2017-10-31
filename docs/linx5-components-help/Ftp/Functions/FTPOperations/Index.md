FTPOperations
=============

FTPOperations performs one of a selection of basic file operations.

Properties
----------

-  #### Host

    The address of the FTP-site.

-  #### Port

    The port number to connect to.

-  #### Authentication type

    The authentication method to use (*Anonymous* or *Basic*).

-  #### Username

    The username to use to authenticate.

-  #### Password

    The password to use to authenticate.

-  #### Operation

    The file operation to perform.

    -   *Delete file* deletes the specified file.
    -   *Move* moves the specified file or directory.
    -   *Create directory* creates a new empty directory at the
        specified path.
    -   *Remove directory* deletes the specified directory (provided it
        is empty).
<p>
-  #### Remote path

    The path to the file or directory on the FTP-site to operate on.

-  #### Move to

    The full remote path (directory and file name) where the file or
    directory will be moved (visible if Move is selected for the
    [Operation](#operationProperty)).

Links
-----

[Wikipedia: File Transfer
Protocol](http://en.wikipedia.org/wiki/File_Transfer_Protocol)
