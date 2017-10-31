FinSwitchDownloadFile
=====================

The FinSwitchDownloadFile function downloads the specified file from the
FinSwitch server.

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

-  #### Company code

    The company code of the file to download.

-  #### File type

    The type of file to download.

-  #### Category

    The relevant category of the file type. Possible values will be
    determined by the selected file type.

-  #### File date

    The date of the file to download.

-  #### Only download if changed

    Only download the file if it has changed. If the file has not
    changed, a blank file will be saved.

### Results

-  #### File path

    The full path where the downloaded file will be saved.

-  #### File exists

    If the file already exists, you can opt to

    *Increment file name* will add a number to the filename to make that
    name unique. This is a sequential number starting with 1.

    *Overwrite file* will replace all content in the file with the new
    content.

    *Throw exception* will stop the process and return an error.

Output
------

-  A string value that contains the full path where the downloaded file
    was saved.

Links
-----

[FinSwitch](http://www.finswitch.com/)
