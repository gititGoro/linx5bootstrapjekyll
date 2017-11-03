---
layout: docs
title: msmqsendmessage
description: msmqsendmessage
group: msmq
feature: functions
component: msmqsendmessage
toc: true
---
MSMQSendMessage
===============

MSMQSendMessage publishes a message to the specified MSMQ queue or distribution list.

Properties
----------

-  #### Destination queue

    The destination queue to send this message to.  
     See [Destination queue format](#destinationqueueformat).

-  #### Message body

    The contents of the message.

-  #### Label

    The application-specific label of the message.

-  #### Reply-to queue

    The queue to which replies should be sent.

-  #### Correlation ID

    ID used to identify replies.  
     See [Correlation ID format](#correlationidformat).

Destination queue format{#destinationqueueformat}
------------------------

The format of the destination queue field depends on the type of queue
you are sending to:


  **Public queue**  
*\<Computer name\>\\<Queue name\>*  
or  
*FormatName:Public=\<queue GUID\>*

  **Private queue**  
*\<Computer name\>\\Private$\\<Queue name\>*
or  
*FormatName:DIRECT=OS:\<Computer name\>\\<Queue name\>*
                                           

  **Private queue (using SPX)**  
*FormatName:DIRECT=SPX:\<Network number\>\<Host number\>\\<Queue name\>*
                                           

  **Private queue (using TCP)**  
*FormatName:DIRECT=TCP:\<IP address\>\\<Queue name\>*
                                           

  **Private queue (using http(s))**  
*FormatName:DIRECT=http(s)://<Client name\>/msmq/<Queue name\>*

  **Journal queue**  
*\<Computer name\>\\<Queue name\>\\Journal*
                                           

  **Computer Journal queue**  
*\<Computer name\>\\Journal$*

  **Computer dead-letter queue**  
*\<Computer name\>\\Deadletter$*

  **Computer transaction dead-letter queue**  
*\<Computer name\>\\XactDeadletter$*

  **Distribution list**  
*FormatName:DL=\<Distribution list GUID\>*\
                                           

  **Multicast addresss**  
*FormatName:DL=MULTICAST=\<Multicast IP Adress\>:\<Port\>*  
Multicast Ip addresses are in the range 224.0.0.0 to 239.255.255.255

  **Multiple destinations**  
Multiple destinations can be specified by seperating them with a comma.  
For example: *ComputerA\\Private$\\queue, ComputerB\\Private$\\queue*

  **Sending to a local computer**  
If you want to send to a queue on your local computer, use a "." in the "\<Computer name\>" field.
  ---------------------------------------------------------------------------------------------------------------------------------------------

Correlation ID format{#correlationidformat}
---------------------

Correlation ID must be in the format *<GUID\>\\<integer\>*  
 For example, *21ec2020-3aea-4069-a2dd-08002b30309d\\3*  
 Uppercase characters are allowed as part of the "guid" function,
however these will be converted to lowercase when the message is sent.

Links
-----

[Microsoft's MSMQ
overview](http://msdn.microsoft.com/en-us/library/ms711472(v=vs.85).aspx%20)

[Queue name
format](http://technet.microsoft.com/en-us/library/cc778392(v=ws.10).aspx)
