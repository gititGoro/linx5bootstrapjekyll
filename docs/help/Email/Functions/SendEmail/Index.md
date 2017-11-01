SendEmail
=========

The SendEmail function is used to send emails to an SMTP server for distribution
to recipients.

Electronic mail servers and other mail transfer agents use SMTP to send
and receive mail messages. User-level client mail applications typically
use SMTP only for sending messages to a mail server for relaying, as
does this Linx function.

Properties
----------

-  #### Smtp Server

    The server IP address, network name or URL.

-  #### Username

    The username to log into the Inbox. This is commonly the email
    address.

-  #### Password

    The password to log into the Inbox.

-  #### Body format

    The options are *Text* or *HTML*.

-  #### Enable SSL

    SSL is required by some mail servers, including Gmail.

-  #### Port

    The default port is 25, when SSL is enabled it is 587.

-  #### Priority

    Not every email system supports message priority. Clients that do
    support it, commonly add an exclamation mark to that email or give
    the email otherwise a higher visibility in the Inbox.

-  #### From

    The *From* address.

-  #### To

    The main recipient.

- #### CC

    Carbon copied recipients. CC recipients are visible to other
    recipients of an email.

- #### BCC

    Blind carbon copied recipients. BCC recipients are not be visible to
    other recipients.

- #### Subject

    The subject line of the email. This has to be a string that may not
    exceed 78 characters.

- #### Body

    The body of the email to be sent.

- #### Attachments

    Any attachments that need to be sent out with the email. This needs
    to point to files.

Links
-----

<http://en.wikipedia.org/wiki/Simple_Mail_Transfer_Protocol>

<http://whatismyipaddress.com/smtp>
