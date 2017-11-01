---
layout: docs
title: MsmqService
description: MsmqService
group: msmq
feature: Services
component: MsmqService
toc: true
redirect_from: docs/msmq/Services/MsmqService/index
---
MSMQService
===========

The MSMQService listens to a MSMQ queue and triggers an event whenever a message
arrives.

Properties
----------

-  #### Queue

    The name of the queue to listen to.  
     See [Queue format](#queueformat).

Events
------

-  ### MessageReceived

    This event is fired whenever a message is received. The message
    itself is available in the "Data" section of the event input, and contains the following String properties:   

    #### Properties
    -  **Body**: The contents of the message.
    -  **CorrelationID**: An identifier used to label messages which are part of a multiple message exchange.
    -  **ID**: A unique identifier for this message.
    -  **Label**: An application specific label.
    -  **ReplyToQueue**: The name of the queue to send replies to.


Queue format{#queueformat}
------------

The format of the queue field depends on the type of queue you are
reading from, as described below:

  **Public queue**  
*\<Computer name\>\\\<Queue name\>*  
or  
*FormatName:Public=\<queue GUID\>*

  **Private queue**  
*\<Computer name\>\\Private\$\\\<Queue name\>*  
or  
*FormatName:DIRECT=OS:\<Computer name\>\\\<Queue name\>*
                                                

  **Private queue (using SPX)**  
*FormatName:DIRECT=SPX:\<Network number\>\<Host number\>\\\<Queue name\>* 
                                                

  **Private queue (using TCP)**  
*FormatName:DIRECT=TCP:\<IP address\>\\\<Queue name\>*
                                                

  **Private queue (using http(s))**  
*FormatName:DIRECT=http(s)://<Client name\>/msmq/<Queue name*\>

  **Journal queue**  
*\<Computer name\>\\\<Queue name\>\\Journal*                             

  **Computer Journal queue**  
*\<Computer name\>\\Journal\$*

  **Computer dead-letter queue**  
*\<Computer name\>\\Deadletter\$*

  **Computer transaction dead-letter queue**  
*\<Computer name\>\\XactDeadletter\$*

  **Distribution list**  
*FormatName:DL=\<Distribution list GUID\>*
                                                

  Reading from a queue on your local computer   If you want to read from a queue on your local computer, use a "." in the "\<Computer name\>" field.  


Links
-----

[Microsoft's MSMQ
overview](https://msdn.microsoft.com/en-us/library/ms711472(v=vs.85).aspx)
