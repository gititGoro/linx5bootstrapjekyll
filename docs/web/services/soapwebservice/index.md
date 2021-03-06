---
layout: docs
title: soapwebservice
description: soapwebservice
group: web
feature: services
component: soapwebservice
toc: true
---
SOAPWebService
==============

Assemble and publish a SOAP web service endpoint by implementing its web
methods through events using the SOAPWebService. The contract for the web 
service is imported via a [WSDL](http://en.wikipedia.org/wiki/Wsdl) file. 
The WSDL import creates one event for each web method as defined in the WSDL 
where each event's input contains all of the method's parameters and its output
holds the value that will be returned in the web method's response.

Properties
----------

### Misc
   
-  #### Model WSDL

    Click the Load WSDL button to import the
    [WSDL](http://en.wikipedia.org/wiki/Wsdl) from a URI (typically
    ending with *?wsdl*) or a local file.

-  #### Endpoint URI {#properties-endpointUri}

    The URI location where the web service will be published. For
    example:   
    *http://localhost:8023/MyService*

-  #### Settings {#properties-settings}

    Allows to override the default values of various settings on the binding 
    like timeouts and message size limits.

    Eg. `{"SendTimeout":"00:10:00","MaxReceiveMessageSize":"1048576"}`  
    The values above will set the operation timeout to 10 minutes (overriding
    the default of 1 minute), and set the maximum message size that the binding
    will process to 1,048,576 bytes (overriding the default of 65,536 bytes).

    See [Binding Settings](#bindingSettings) below for the list of available 
    settings.

### Security

-  #### Security mode

    The type of security to use.

    -   *None* specifies no web service security.
    -   *Transport* enables [transport-level
        security](http://en.wikipedia.org/wiki/Transport_Layer_Security).
        The scheme of the [Endpoint URI](#properties-endpointUri) must
        be *https* and the SSL certificate must have been configured on
        the appropriate port (see [How to: Configure a Port with an SSL
        Certificate](http://msdn.microsoft.com/en-us/library/ms733791.aspx)).
    -   *Message* enables message-level security
        ([WS-Security](http://en.wikipedia.org/wiki/WS-Security))
        through an [X.509
        certificate](http://en.wikipedia.org/wiki/X.509). This option
        reveals the [Service certificate
        property](#properties-serviceCertificate).

-  #### Service certificate {#properties-serviceCertificate}

    The file path to the X.509 certificate to use to represent the
    service for message-level security.

Binding Settings {#bindingSettings}
----------------

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

- #### ReceiveTimeout

    *Default: 10 minutes*

    The interval of time to wait since the last message received before the 
    connection will be dropped. Applicable only when 
    [Security mode](#properties-securityMode) is set to `Transport` or 
    `Message`. 

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
    *None*. Selects if messages are buffered or streamed.
  
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
- [Wikipedia: X.509](http://en.wikipedia.org/wiki/X.509)
- [MSDN: How to: Configure a Port with an SSL
  Certificate](http://msdn.microsoft.com/en-us/library/ms733791.aspx)
- [All WCF timeouts explained](http://www.rauch.io/2015/06/25/all-wcf-timeouts-explained/)
