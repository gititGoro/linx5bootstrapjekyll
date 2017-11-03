---
layout: docs
title: createcreditnoteallocation
description: createcreditnoteallocation
group: functions
feature: creditnoteallocation
component: createcreditnoteallocation
toc: true
---
CreateCreditNoteAllocation
============

Allows you to allocate credit notes to outstanding invoices. For more information on credit notes in Xero visit the [Xero Business Help Centre](https://help.xero.com/int/Accounts_AR_AddCredit).

**Note:** To refund credit notes use the [CreatePayment](../../Payment/CreatePayment/Index.md) function.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Credit note id
The CreditNoteID of the credit note to allocate.
- #### Allocation
The data for the allocation.


Output
-----
#### CreditNoteAllocation
The credit note allocation created.

Links
-----

https://developer.xero.com/documentation/api/credit-notes#POST
