FTPDownload
===========

FTPDownload downloads a file from an FTP-site.

Properties
----------

-  #### Type

    The type of FTP connection to create.  Possible values are *FTP* (default), *FTPS* or *SFTP*.
    
    FTPS uses explicit FTPS except where port 990 is specified when implicit FTPS is used.
-  #### Connection
	Connection values to connect to the FTP server.

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

	-  #### Timeout

		Period in seconds to establish a connection.

-  #### Remote folder

    The folder on the FTP-site that contains the file(s) to download.

-  #### Remote file name

    Pattern of the file types to download.

-  #### Destination folder

    The local folder where the file(s) should be copied to.

-  #### Include subfolders

    Download files in subfolders that match the specified pattern.

-  #### Purge destination

    Deletes local files that are not on the remote server.        

Links
-----

[Wikipedia: File Transfer Protocol](http://en.wikipedia.org/wiki/File_Transfer_Protocol)
