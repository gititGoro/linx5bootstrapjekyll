---
layout: docs
title: UpdatePayment
description: UpdatePayment
group: Functions
feature: Payment
component: UpdatePayment
toc: true
redirect_from: docs/Functions/Payment/UpdatePayment/index
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
