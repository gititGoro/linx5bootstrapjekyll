FinSwitchGetProcessStatus
=========================

The FinSwitchGetProcessStatus function returns the status of a FinSwitch
process, using the specified id.

Properties
----------

### Authentication

-  #### Username

    The username to authenticate against.

-  #### Password

    The password to authenticate against.

### Details

-  #### Url

    The url of the FinSwitch web service.

### Process

-  #### ID

    The id of the process to query.

Output
------

-  A string value that contains the process status. Possible values
    are:
    1.  Busy
    2.  Error
    3.  Finished
    4.  Pending
    5.  Aborted

Links
-----

[FinSwitch](http://www.finswitch.com/)
