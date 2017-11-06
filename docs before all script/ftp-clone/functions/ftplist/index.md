---
layout: docs
title: ftplist
description: ftplist
group: ftp-clone
feature: functions
component: ftplist
toc: true
---
FTPList
=======

FTPList returns a list of all the files and directories in the specified remote
path.

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

-  #### Remote path

    The path to the directory to list.

-  #### Loop results

    If checked, the files will be returned in a loop, else a list
    containing all the file information records will be created.

-  #### Recursive
	If checked, all files (including those in subdirectories) will be returned.

Links
-----

[Wikipedia: File Transfer
Protocol](http://en.wikipedia.org/wiki/File_Transfer_Protocol)
