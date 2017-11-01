RESTWebService
==============

Assemble and publish a REST web service endpoint by implementing its web
methods through events using the RESTWebService function. The contract for the web service is imported
through a [Swagger](http://swagger.io/) API description file. The API
description import creates one event for each path defined in the
Swagger file.

Properties
----------   
-  #### API description

    The Swagger specification as a JSON document.

-  #### Base URI

    The URI location where the web service will be published. For
    example:   
     *http://localhost:8023/MyService*
   
-  #### Default web message format

    The message format (JSON or XML) to use by default when serializing
    the response of a web service call.

-  #### Settings {#properties-settings}

    Allows to override the default values of various settings on the binding 
    like timeouts and message size limits.

    Eg. `{"SendTimeout":"00:10:00","MaxReceivedMessageSize":"1048576"}`  
    The values above will set the operation timeout to 10 minutes (overriding
    the default of 1 minute), and set the maximum message size that the binding
    will process to 1,048,576 bytes (overriding the default of 65,536 bytes).

    See [Binding Settings](#bindingSettings) below for the list of available 
    settings.

Binding Settings {#bindingSettings}
----------------

The following binding settings can be specified using the [Settings](#properties-settings)
property:

- #### OpenTimeout

    *Default: 1 minute*

    The interval of time provided for a connection to open. This time span 
    includes all necessary protocol negotiations etc.

- #### CloseTimeout

    *Default: 1 minute*

    The interval of time provided for a connection to close. This time span 
    includes all necessary protocol negotiations etc.

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

    Sets the maximum size, in bytes, of the buffer that receives messages 
    from the channel. 

- #### TextEncoding

    *Default: UTF8*

    The character encoding that is used for transmitting text.  
    Allowed values: *BigEndianUnicode*, *Default*, *Unicode*, *UTF32*, 
    *UTF7*, *UTF8*

- #### TransferMode

    *Default: Buffered*

    Selects if messages are buffered or streamed.
  
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

- [Wikipedia: Representation state transfer
(REST)](http://en.wikipedia.org/wiki/Representational_state_transfer)
- [Swagger](https://helloreverb.com/developers/swagger)
- [All WCF timeouts explained](http://www.rauch.io/2015/06/25/all-wcf-timeouts-explained/)
