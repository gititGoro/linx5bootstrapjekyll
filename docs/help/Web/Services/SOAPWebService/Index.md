SOAPWebService
==============

Assemble and publish a SOAP web service endpoint by implementing its web
methods through events using the SOAPWebService. The contract for the web service is imported via
a [WSDL](http://en.wikipedia.org/wiki/Wsdl) file. The WSDL import
creates one event for each web method as defined in the WSDL where each
event's input contains all of the method's parameters and its output
holds the value that will be returned in the web method's response.

Properties
----------   
#### Model WSDL

   Click the Load WSDL button to import the
    [WSDL](http://en.wikipedia.org/wiki/Wsdl) from a URI (typically
    ending with *?wsdl*) or a local file.   
#### Endpoint URI

   The URI location where the web service will be published. For
    example:   
     *http://localhost:8023/MyService*   
#### Security mode

   The type of security to use.

   -   *None* specifies no web service security.
   -   *Transport* enables [transport-level
        security](http://en.wikipedia.org/wiki/Transport_Layer_Security).
        The scheme of the [Endpoint URI](#properties.endpointUri) must
        be *https* and the SSL certificate must have been configured on
        the appropriate port (see [How to: Configure a Port with an SSL
        Certificate](http://msdn.microsoft.com/en-us/library/ms733791.aspx)).
   -   *Message* enables message-level security
        ([WS-Security](http://en.wikipedia.org/wiki/WS-Security))
        through an [X.509
        certificate](http://en.wikipedia.org/wiki/X.509). This option
        reveals the [Service certificate
        property](#properties.serviceCertificate).
#### Service certificate

   The file path to the X.509 certificate to use to represent the
    service for message-level security.

Links
-----

[Wikipedia: SOAP](http://en.wikipedia.org/wiki/SOAP)

[Wikipedia: Web Service Description Language
(WSDL)](http://en.wikipedia.org/wiki/Wsdl)

[Wikipedia: Transport Layer
Security](http://en.wikipedia.org/wiki/Transport_Layer_Security)

[Wikipedia: WS-Security](http://en.wikipedia.org/wiki/WS-Security)

[Wikipedia: X.509](http://en.wikipedia.org/wiki/X.509)

[MSDN: How to: Configure a Port with an SSL
Certificate](http://msdn.microsoft.com/en-us/library/ms733791.aspx)
