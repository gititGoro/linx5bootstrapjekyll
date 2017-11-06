---
layout: docs
title: ftpupload
description: ftpupload
group: ftp-clone
feature: functions
component: ftpupload
toc: true
---
FTPUpload
=========

FTPUpload uploads a file to an FTP-site.

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

-  #### Keep file name{#keepFileNameProperty}

    If checked, the file will be uploaded with the same file name as the
    original.

-  #### Remote path

    If [Keep file name](#keepFileNameProperty) is checked, the folder on
    the FTP-site where the file will be copied. Otherwise, both the
    folder path and the file name must be provided.  
     If a file with the same name already exists in the folder on the
    FTP-site, it will be overwritten.

-  #### Source file path

    The local file path to the file to upload.

Links
-----

[Wikipedia: File Transfer
Protocol](http://en.wikipedia.org/wiki/File_Transfer_Protocol)
