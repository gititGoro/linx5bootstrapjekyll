MSMQReceiveMessage
==================

MSMQReceiveMessage fetches messages from an MSMQ queue.

Properties
----------

-  #### Queue

    The name of the queue.  
     See [Queue format](#queueformat).

-  #### Return options

    Controls how the messages will be returned from the function

    1.  *Next message*  
         Retrieves the next message in the queue. If the queue is empty,
        and exception will be thrown.
    2.  *Next message, else empty message*  
         Retrieves the next message in the queue. If the queue is empty,
        an empty message will be returned.
    3.  *List of messages*  
         Returns all messages in the queue in a list.
    4.  *Loop messages*  
         Loops through all messages in the queue.
<p>
-  #### Fetch timeout

    *(Only shown if "Return options" is set to "Next message" or "Next
    message, else empty message".)*  
     The length of time (in seconds) to wait for a new message to
    arrive.  
     If no message is received within this time limit, the function with
    either throw an exception, or return an empty message, depending on
    the value of "Return options".

-  #### Message count limit

    *(Only shown if "Return options" is set to "List messages" or "Loop
    messages".)*  
     Specifies the maximum number of messages to retrieve from the
    queue. Any remaining messages will be kept on the MSMQ queue and can
    be retrieved at a later stage.  
     A value of 0 will cause all available messages to be returned.

Queue format{#queueformat}
------------

The format of the queue field depends on the type of queue you are
reading from, as described below:

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
*FormatName:DIRECT=http(s)://<Client name\>/msmq/<Queue name*\>

  **Journal queue**  
*\<Computer name\>\\\<Queue name\>\\Journal*                             

  **Computer Journal queue**  
*\<Computer name\>\\Journal$*

  **Computer dead-letter queue**  
*\<Computer name\>\\Deadletter$*

  **Computer transaction dead-letter queue**  
*\<Computer name\>\\XactDeadletter$*

  **Distribution list**  
*FormatName:DL=\<Distribution list GUID\>*
                                                

  Reading from a queue on your local computer   If you want to read from a queue on your local computer, use a "." in the "\<Computer name\>" field.
  ----------------------------------------------------------------------------------------------------------------------------------------------------

Links
-----

[Microsoft's MSMQ
overview](http://msdn.microsoft.com/en-us/library/ms711472(v=vs.85).aspx%20)

[Queue name
format](http://technet.microsoft.com/en-us/library/cc778392(v=ws.10).aspx)
