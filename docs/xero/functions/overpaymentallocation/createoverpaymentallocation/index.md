---
layout: docs
title: createoverpaymentallocation
description: createoverpaymentallocation
group: functions
feature: overpaymentallocation
component: createoverpaymentallocation
toc: true
---
CreateOverpaymentAllocation
============

Allows you to allocate overpayments to outstanding invoices. For more information on overpayments in Xero visit the [Xero Business Help Centre](https://help.xero.com/int/BankAccounts_Overpayments).

**Note:**
- Create Overpayments using the [CreateBankTransaction](../../BankTransaction/CreateBankTransaction/Index.md) function.
- Refund Overpayments using the [CreatePayment](../../Payment/CreatePayment/Index.md) function.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Overpayment id
The OverpaymentID of the overpayment to allocate.
- #### Allocation
The data for the allocation.


Output
-----
#### OverpaymentAllocation
The overpayment allocation created.

Links
-----

https://developer.xero.com/documentation/api/overpayments
