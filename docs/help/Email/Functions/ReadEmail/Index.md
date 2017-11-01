ReadEmail
=========

The ReadEmail function will connect to a mail server using either IMAP or POP3.

IMAP (Internet Message Access Protocol) provides access and retrieve
mail functions for a remote mail server so the mails can be used locally
while retaining them also on the server. Imap sets message flags so that
the user can keep track of which messages he or she has already seen,
already answered, and so on.

POP (Post Office Protocol) implements the offline mail access model,
where mail is retrieved and then deleted from the server where the
mailbox resides so it can be used on a local machine. Emails are hence
transferred permanently from a central server to one client machine.

Properties
----------

-  #### Protocol

    Protocol to use when reading the email.

-  #### Authentication

    The type of authentication needs to be one that the mail server is
    capable of.

-  #### User name

    The user name for the mailbox you wish to access.

-  #### Password

    The password for the mailbox you wish to access.

-  #### Port

    The port number used to access the mailbox.

-  #### Server name

    The IP address, URL or network name of the server.

-  #### Use SSL

    Using SSL (Secure Socket Layer) is more secure as the data is
    encrypted.

-  #### Folder

    The email folder on the server where the mails you wish to retrieve
    are stored (IMAP only).

-  #### Only unread items

    A flag to indicate whether only unread mails should be retrieved
    (IMAP only).

- #### Mark as read

    A flag to indicate whether retrieved emails should be marked as
    'read' (IMAP only).

- #### Move to folder

    A folder to move retrieved emails to (IMAP only).

- #### Delete after read

    A flag to indicate whether retrieved emails should be deleted from
    the original folder.

- #### Body as HTML

    A flag to indicate if the email's body should be retrieved formatted
    in HTML. Otherwise, the body will be retrieved as unformatted text.

- #### Attachment options

    Three actions are available for attachments.

    *None* will ignore attachments.

    *SaveToFile* will save the attachments as files to the specified
    directory. The filepath is an automatic output of this function and
    can be used to use an attachment later in the process.

    *ReturnData* will convert the attachment contents to a string and
    allow you to use that string further down in your process. This
    option will only work for text-based attachments, not binary ones.

- #### Save email

    A flag to indicate if the emails should be saved to disk.

- #### File directory

    The directory where emails and attachments will be saved. For
    emails, the subject line becomes the filename. Attachments already
    have filenames that will be used when saving.

    If a file of that name already exists, the function will add the
    year, month, day, hour, second and milliseconds to the filename in
    this format *yyyy-mm-dd-hh-ss-ffff* to make sure it is unique.

    If the filename is too long, it will be truncated.

- #### Email file type

    The file type used when saving emails to disk.

- #### Filter body

    Information to look for in the *body* of the emails.

- #### Filter from

    Information to look for in the *from field* of the emails.

- #### Filter from address

    Information to look for in the *from address fields* of the emails.

- #### Filter subject

    Information to look for in the *subject line* of the emails.

- #### Filter to

    Information to look for in the *to field* of the emails.

- #### Filter option

    If you have specified multiple RegEx search criteria, you can define
    whether these should be chained together with an *AND* or with an
    *OR*. AND means that all filters need to be true for the mail to be
    retrieved. OR means that ANY of the filters need to be true for the
    mail to be retrieved.

Links
-----

[IMAP](http://en.wikipedia.org/wiki/Internet_Message_Access_Protocol)  
 [POP3](http://en.wikipedia.org/wiki/Post_Office_Protocol)  
 [Regular expression](http://en.wikipedia.org/wiki/Regular_expression)  
 [Regular expression mode
modifiers](http://www.regular-expressions.info/modifiers.html)
