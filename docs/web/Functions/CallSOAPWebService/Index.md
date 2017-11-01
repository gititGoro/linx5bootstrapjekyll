CallSOAPWebService
==================

The CallSOAPWebService function allows for consuming a SOAP web service.

Properties
----------

### Misc

-  #### Method name

    The name of the web method to call. The web method can be selected
    from a drop-down box on the editor window after specifying the URI
    (typically ending with *?wsdl* ) or the file path to the
    [WSDL](http://en.wikipedia.org/wiki/Wsdl) document describing the
    web service.

-  #### Service URL {#properties-serviceUrl}

    The URL to use when making the SOAP call.

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

-  #### Security mode {#properties-securityMode}

    The type of security to use.

    -   *None* specifies no web service security.
    -   *Transport* enables [transport-level
        security](http://en.wikipedia.org/wiki/Transport_Layer_Security).
        The scheme of the [Service URL](#properties-serviceUrl) must
        be *https*.
    -   *Message* enables message-level security
        ([WS-Security](http://en.wikipedia.org/wiki/WS-Security)).

-  #### Windows authentication

    If the service requires Windows-authentication *and* is hosted on a
    server on a different domain, then a Windows credential needs to be
    supplied.

### Windows authentication

-  #### Username

    The username to use to authenticate with the service.

-  #### Password

    The password to use to authenticate with the service.

-  #### Domain

    The domain of the host, if required.

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

- #### MessageEncoding

    *Default: Text*

    Selects if SOAP messages are encoded as Text/XML or MTOM.

    Allowed values:
    - *Text*  
        Encodes all values as text in the XML content using the binding's 
        text encoding.
    - *Mtom*  
        Efficiently encodes all lists of bytes, adding them as binary-coded 
        attachments referenced from the XML content.  

Links
-----

- [Wikipedia: SOAP](http://en.wikipedia.org/wiki/SOAP)
- [Wikipedia: Web Service Description Language
(WSDL)](http://en.wikipedia.org/wiki/Wsdl)
- [Wikipedia: Transport Layer
Security](http://en.wikipedia.org/wiki/Transport_Layer_Security)
- [Wikipedia: WS-Security](http://en.wikipedia.org/wiki/WS-Security)
- [All WCF timeouts explained](http://www.rauch.io/2015/06/25/all-wcf-timeouts-explained/)
