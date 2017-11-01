---
layout: docs
title: FinSwitchUploadFile
description: FinSwitchUploadFile
group: finswitch
feature: Functions
component: FinSwitchUploadFile
toc: true
redirect_from: docs/finswitch/Functions/FinSwitchUploadFile/index
---
FinSwitchUploadFile
===================

The FinSwitchUploadFile function uploads the specified file to the
FinSwitch server.

Properties
----------

### Authentication

-  #### Username

    The username to authenticate against.

-  #### Password

    The password to authenticate against.

### Details

-  #### Url

    The url of the FinSwitch web service.

-  #### Company code

    The company code for the uploaded file.

-  #### File

    The full path of the file to upload.

-  #### Mime type

    The type of content that the file contains.
    
### Settings
-  #### Settings {#properties-settings}

    Allows to override the default values of various settings on the 
    binding like timeouts and message size limits.
  
    Eg. `{"SendTimeout":"00:10:00","MaxReceiveMessageSize":"1048576"}`  
    The values above will set the operation timeout to 10 minutes 
    (overriding the default of 1 minute), and set the maximum message
    size that the binding will process to 1,048,576 bytes (overriding
    the default of 65,536 bytes).

    See [Binding Settings](#bindingSettings) below for the list of 
    available settings.

Output
------

-  An integer value that contains the process id for the upload file
    operation.
    
Binding Settings {#bindingSettings}
--------------------------

The following binding settings can be specified using the [Settings](#properties-settings)
property:

- #### OpenTimeout

    *Default: 1 minute*

    The interval of time provided for a connection to open. This time 
    span includes all necessary security handshakes, protocol negotiations 
    etc.

- #### CloseTimeout

    *Default: 1 minute*

    The interval of time provided for a connection to close. This time span 
    includes all necessary security handshakes, protocol negotiations etc.

- #### SendTimeout

     *Default: 1 minute*

     The interval of time provided for the entire service call to complete, 
     from sending the request to receiving the response.

- #### MaxReceivedMessageSize

     *Default: 65,536 bytes*

     The maximum size of received messages in bytes. 

- #### MaxBufferPoolSize

     *Default: 524,288 bytes*

     The maximum amount of memory, in bytes, allocated to the buffer manager
     for processing messages. 

     The buffer pool helps decrease the overhead due to memory allocation 
     (and subsequent gargabe collection) while processing messages. A buffer
     pool size that is too small can cause degradation of performance due to
     extensive allocation from the CLR garbage heap. 

- #### MaxBufferSize

     *Default: 65,536 bytes*

     Available only when [Security mode](#properties-securityMode) is set to
     *None*. Sets the maximum size, in bytes, of the buffer that receives 
     messages from the channel. 

- #### TextEncoding

     *Default: UTF8*

     The character encoding that is used for transmitting text.  
     Allowed values: *BigEndianUnicode*, *Default*, *Unicode*, *UTF32*, 
     *UTF7*, *UTF8*

- #### TransferMode

    *Default: Buffered*

     Available only when [Security mode](#properties-securityMode) is set to
     *None*. Selects if messages are sent buffered or streamed.
  
     Allowed values: 
     - *Buffered*  
        Request and response messages are both buffered.
     - *Streamed*  
        Request and response messages are both streamed.
     - *StreamedRequest*  
        The request message is streamed, and the response is buffered.
     - *StreamedResponse*  
        The request message is buffered, and the response is streamed.     

Links
-----

[FinSwitch](http://www.finswitch.com/)

[Wikipedia: MIME
types](http://en.wikipedia.org/wiki/Internet_media_type)
