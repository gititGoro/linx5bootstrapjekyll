RabbitMQWriter
==============

The RabbitMQWriter publishes a message on a RabbitMQ server to the Exchange with the
specified name.

Properties
----------

-  #### Exchange

    The name of the Exchange to publish to.  
     If this property is left blank, the default Exchange will be
    selected. The default Exchange is a "direct" Exchange which will
    route the message to the queue having the name specified in the
    routing key.

-  #### Routing key

    The routing key that the Exchange will use to determine to which
    Queue(s) to send the message.

-  #### Persistent

    If checked, the message will be persisted and will not become lost
    if the RabbitMQ server crashes or restarts.

-  #### Reply to

    Use this field if you're expecting a response message. It specifies
    the name of the Queue where the response should be sent.

-  #### Correlation ID

    Use this field if you're expecting a response message. The response
    message (received through the reply-to queue) should have this
    unique string assigned to it so that the response message can be
    identified by the client sending this message.

-  #### Headers

    The set of headers to send with the message.  
     If the header's value is a list of bytes, the bytes will be
    included directly. Any other type of a value that is not a string
    will first be serialized to a JSON string. Strings are encoded to
    bytes using the UTF8 character encoding.  
     Besides allowing to transmit additional meta-data relating to the
    message, headers can help a Headers-type Exchange determine to which
    Queues to route the message. Headers should be used instead of a
    routing key wherever a Headers-type Exchange is used.

-  #### Message

    The content of the message.  
     If the value is a list of bytes, the bytes will be transmitted
    directly. A string value will be encoded using the selected
    character encoding. Any other type of value will first be serialized
    to a JSON string and then encoded using the selected character
    encoding.

-  #### Character encoding

    The character encoding to use when encoding the message to a stream
    of bytes. This property is ignored when the message content is
    already a list of bytes.

-  #### Host name

    The IP address or domain name of the server to connect to.

- #### Port

    The port number to connect to. RabbitMQ server runs on port 5672 by
    default.

- #### Virtual host

    The virtual host to access on the RabbitMQ server.

- #### Username

    The username to use for authentication.

- #### Password

    The password to use for authentication.

Links
-----

[RabbitMQ web site](http://www.rabbitmq.com)

[AMQP Model](https://www.rabbitmq.com/tutorials/amqp-concepts.html)
