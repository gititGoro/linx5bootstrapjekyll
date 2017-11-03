---
layout: docs
title: createprepayment
description: createprepayment
group: functions
feature: prepayment
component: createprepayment
toc: true
---
CreatePrepayment
============

Allows you to allocate part or full amounts of a prepayment to outstanding invoices. For more information on prepayments in Xero visit the [Xero Business Help Centre](https://help.xero.com/int/BankAccounts_Prepayments).

**Note:**
- Create prepayments using the [CreateBankTransaction](../../BankTransaction/CreateBankTransaction/Index.md) function.
- Refund prepayments using the [CreatePayment](../../Payment/CreatePayment/Index.md) function.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Prepayment
Prepayment to create.


Output
-----
#### Prepayment
The prepayment created.

Links
-----

https://developer.xero.com/documentation/api/prepayments#PUT
