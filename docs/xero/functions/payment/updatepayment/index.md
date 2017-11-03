---
layout: docs
title: updatepayment
description: updatepayment
group: functions
feature: payment
component: updatepayment
toc: true
---
UpdatePayment
============

Use this method to delete (reverse) payments to invoices, credit notes, prepayments & overpayments. Note that payments created via batch payments and receipts are not supported. Payments cannot be modified, only created and deleted.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Payment Id
Unique identifier of the payment to update.
- #### Payment
Payment to update.


Output
-----
#### Payment
Payment updated.

Links
-----

https://developer.xero.com/documentation/api/payments#POST
