RabbitMQService
===============

The RabbitMQService listens to a queue on a RabbitMQ server and triggers an event when a new
message arrives.

Properties
----------

-  #### Return raw bytes

    If checked, the contents of the message's body will be returned as a
    list of bytes.

-  #### Content type

    The type of data that the body contains.

-  #### Character encoding

    The character encoding to use to decode the body to a string.

-  #### Queue name

    The name of the queue on the RabbitMQ server from which to fetch the
    messages.

-  #### Headers

    The list of expected headers. Each entry will create a corresponding
    property in the input of the
    [MessageReceived](#messagereceivedevent) event to store the header's
    value. Any headers of the message that are not specified will be
    included in the *AdditionalHeaders* property of the event's input.

-  #### Host name

    The IP address or domain name of the server to connect to.

-  #### Port

    The port number to connect to. RabbitMQ server runs on port 5672 by
    default.

-  #### Virtual host

    The virtual host to access on the RabbitMQ server.

-  #### Username

    The username to use for authentication.

- #### Password

    The password to use for authentication.

Events
------

-  ### MessageReceived{#messagereceivedevent}

    This event is fired whenever a message is received. The message
    object in the Data section of the event input contains the
    following properties:  

	####Properties:

    - **DeliveryTag**: A string that uniquely identifies the message.
    - **ExchangeName**: The name of the Exchange that the message was published to.
    - **RoutingKey**: The routing key of the message.
    - **CorrelationID**: The correlation ID of the message.
    - **ReplyTo**: The name of the reply-to queue of the message.
    - **Body**: The contents of the message.
    - **Headers**: Available if at least one entry was specified for Headers.  Contains one property per entry.
    - **AdditionalHeaders**: Stores the name and value of each header that was received for which there was no corresponding entry in the specified Headers.


Links
-----

[RabbitMQ web site](http://www.rabbitmq.com)

[AMQP Model](https://www.rabbitmq.com/tutorials/amqp-concepts.html)
