---
layout: docs
title: CreateAttachment
description: CreateAttachment
group: Functions
feature: Attachment
component: CreateAttachment
toc: true
redirect_from: docs/Functions/Attachment/CreateAttachment/index
---
CreateAttachment
============

Allows you to create an attachment for a document. If an attachment already exists on the specified document, then the attachment being uploaded will overwrite it.

**Note:** When the file name includes special characters, the characters should not be encoded unless they're brackets. Brackets must be encoded in order for the call to go through, and all other characters must be unencoded.

Xero attachments can be uploaded to the following:
- Invoices
- Receipts
- Credit Notes
- Repeating Invoices
- Bank Transactions
- Contacts
- Accounts
- Manual Journals

**Note:** 10 attachments can be uploaded per document. You can replace any attachment already uploaded by specifying the name of the file in the [File name](#file-name) property.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Endpoint
The name of the parent endpoint (e.g. Receipts, Invoices).
- #### Item id
The guid of the document that that attachment belongs to (e.g. ReceiptID or InvoiceID).
- #### File name
The filename of the attachment that you are uploading.
- #### Include online
You can set an attachment to be included with the invoice when viewed online (through Xero). This functionality is available for accounts receivable invoices and accounts receivable credit notes. To enable an attachment to be viewed with the online invoice set this property to true.


Output
-----
#### Attachment
The attachment created.

Links
-----

https://developer.xero.com/documentation/api/attachments#POST
