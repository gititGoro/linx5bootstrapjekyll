---
layout: docs
title: updateattachment
description: updateattachment
group: functions
feature: attachment
component: updateattachment
toc: true
---
UpdateAttachment
============

Use this function to update an attachment's details.

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
The attachment updated.

Links
-----

https://developer.xero.com/documentation/api/attachments#POST
