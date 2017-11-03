---
layout: docs
title: updatebanktransaction
description: updatebanktransaction
group: functions
feature: banktransaction
component: updatebanktransaction
toc: true
---
UpdateBankTransaction
============

Use this function to update spend money and receive money transactions. Updates on spend prepayment, receive prepayment, spend overpayment or receive overpayment transactions are NOT currently supported.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Bank transaction id
The BankTransactionID of the bank transaction to update.
- #### Bank transaction
The bank transaction to update.


Output
-----
#### BankTransaction
The bank transaction updated.

Links
-----

https://developer.xero.com/documentation/api/banktransactions#POST
