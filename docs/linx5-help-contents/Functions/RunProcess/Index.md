Running a process
=================

Use this method to control the flow of your project by creating a
controller process.

The RunProcess function can be invoked by dragging the process you want
to run from your Solution Explorer into the calling process. Use this
method when you want to run a process from inside another process.

The calling process waits for the RunProcess function to complete before
continuing to run subsequent functions.

Properties
----------

-  #### Input Values

    Input values that the process you are calling has been set up to
    require.

-  #### Process name

    The process being called. This field cannot be changed. In order to
    call a different process to the one in this field, you need to drag
    that process onto the canvas.

Controlling process sequence
----------------------------

This method can be used to create a 'controller process'. All a 'controller process' does is to kick off a set of processes in a specified sequence. 

The advantage of this is that a set of functions that complete one task is neatly contained in a process. This practice makes building, changing and debugging processes much easier. You can also start to re-use common processes when you use this method. 
