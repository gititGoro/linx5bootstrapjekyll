GetAttachments
============

The GetAttachments function supports retrieving the list of attachments for a given document.

Properties
----------

-  #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
-  #### Endpoint
The name of the parent endpoint (e.g. Receipts, Invoices).
-  #### Item id
The guid of the document that that attachment belongs to (e.g. ReceiptID or InvoiceID).
-  #### Return options
Return Options: Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of Attachments
A list of attachments or a single attachment depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/attachments#GET