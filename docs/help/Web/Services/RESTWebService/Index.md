RESTWebService
==============

Assemble and publish a REST web service endpoint by implementing its web
methods through events using the RESTWebService function. The contract for the web service is imported
through a [Swagger](http://swagger.io/) API description file. The API
description import creates one event for each path defined in the
Swagger file.

Properties
----------   
#### Import API description

   Click the Import... button to import the Swagger specification
    from a JSON file.   
#### Base URI

   The URI location where the web service will be published. For
    example:   
     *http://localhost:8023/MyService*   
#### Default web message format

   The message format (JSON or XML) to use by default when serializing
    the response of a web service call.

Links
-----

[Wikipedia: Representation state transfer
(REST)](http://en.wikipedia.org/wiki/Representational_state_transfer)

[Swagger](https://helloreverb.com/developers/swagger)
