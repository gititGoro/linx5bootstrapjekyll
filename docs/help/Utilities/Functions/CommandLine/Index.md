CommandLine
===========

A command-line interface (CLI) is a means of interacting with a computer
program where the user (or client) issues commands to the program in the
form of successive lines of text (command lines). Operating system (OS)
command line interfaces are usually distinct programs supplied with the
operating system.

The CommandLine function allows processes to use a Windows command line
interface to run commands. Use this function when you want to do some
work in a Windows command line interface.

Properties
----------

-  #### Command

    The command to execute. This can be anything you can execute in the
    command line interface on Windows.

-  #### Wait for return

    If "Wait for return" is true, the command prompt will be integrated
    into the process flow in a synchronous fashion and any return values
    from the command prompt can be used further down in the process.

    If "Wait for return" is false, the command prompt will be started,
    but the process will continue straight away and not wait for the
    completion of that prompt (fire and forget).

-  #### Timeout

    The timeout value (in milliseconds) for the command prompt. This
    only applies if "Wait for return" is true and defines the time the
    process will wait for the command prompt to finish and provide a
    return value.

    A value of 0 indicates an infinite timeout.

-  #### Kill on timeout

    This only applies if "Wait for return" with a valid "Timeout" value
    is specified.

    When "Kill on timeout" is true, the instantiated command prompt will
    be closed by the process when the timeout occurs.

    When "Kill on timeout" is false, the command prompt will be ignored
    on timeout and the process will continue.

Output
------

Note that output is only available when the "Wait for return" value is
set to true.

-  #### ExitCode

    The exit code of the command that was executed.

-  #### HasTimedOut

    Value to indicate whether the command has timed out while executing.

-  #### Output

    The output of the command that was executed.


