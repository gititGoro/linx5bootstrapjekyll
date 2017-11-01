---
layout: docs
title: CreateBankTransaction
description: CreateBankTransaction
group: Functions
feature: BankTransaction
component: CreateBankTransaction
toc: true
redirect_from: docs/Functions/BankTransaction/CreateBankTransaction/index
---
CreateBankTransaction
============

Use this function to create spend money, receive money, spend prepayment, receive prepayment, spend overpayment or receive overpayment transactions.

**Note:** you cannot create transfers using CreateBankTransaction. To create a bank transfer you need to use the [CreateBankTransfers](../../BankTransfer/CreateBankTransfer/Index.md) function.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Bank transaction
The bank transaction to create.
  - Mandatory Items:
      - **Type**: See [Bank Transaction Types](https://developer.xero.com/documentation/api/types#BankTransactionTypes).
      - **Contact**: See [Contacts](https://developer.xero.com/documentation/api/contacts#).
      - **LineItems**: See [LineItems](https://developer.xero.com/documentation/api/banktransactions#LineItemsPOST). The LineItems element can contain any number of individual LineItem sub-elements. At least *one* is required to create a bank transaction.
      - **BankAccount**: Bank account for transaction. Only accounts of [Type BANK](https://developer.xero.com/documentation/api/types#AccountTypes) will be accepted. See [BankAccount](https://developer.xero.com/documentation/api/contacts#).

Output
-----
#### BankTransaction
The bank transaction created.

Links
-----

https://developer.xero.com/documentation/api/banktransactions#POST
