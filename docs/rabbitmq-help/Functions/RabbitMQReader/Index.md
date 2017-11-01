RabbitMQReader
==============

The RabbitMQReader fetches messages from a specified Queue on a RabbitMQ server.

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

-  #### Return option{#returnoptionproperty}

    How to return the messages.

    1.  *Next message*  
         Retrieves the next message in the queue. An exception will be
        thrown if the queue is empty.
    2.  *Next message, else empty*  
         Retrieves the next message in the queue or leaves all
        message-fields blank if the queue is empty.
    3.  *List of messages*  
         Returns the messages from the queue in a list.
    4.  *Loop messages*  
         Returns the messages from the queue one at a time in a loop.
<p>
-  #### Timeout

    *(Only shown if [Return option](#returnoptionproperty) is set to
    "Next message" or "Next message, else empty")*  
     If the queue is empty, the function will keep looking in the queue
    for new messages until the specified number of milliseconds run out.

-  #### Message count limit

    *(Only shown if [Return option](#returnoptionproperty) is set to
    "List of messages" or "Loop messages")*  
     Specifies the maximum number of messages that will be fetched from
    the queue. Use 0 for no limit.

-  #### Headers

    The list of expected headers. Each entry will create a corresponding
    property in the function's output to store the header's value. Any
    headers of the message that are not specified will be included in
    the *AdditionalHeaders* list property of the output.

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
