---
layout: docs
title: CreateBankTransfer
description: CreateBankTransfer
group: Functions
feature: BankTransfer
component: CreateBankTransfer
toc: true
redirect_from: docs/Functions/BankTransfer/CreateBankTransfer/index
---
CreateBankTransfer
============

Use this function to create a bank transfer.

The two sides of each bank transfer will automatically be recorded as RECEIVE-TRANSFER and SPEND-TRANSFER types in the GET BankTransactions endpoint.
The BankTransationIDs are returned in responses for successful calls.

**Note** the following functionality is not currently supported:
- You cannot specify the reference field.
- You cannot transfer between accounts in different currencies.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Bank transfer
The bank transfer to create.
  - Mandatory Items:
      - **FromBankAccount**: An [account](https://developer.xero.com/documentation/api/accounts) of Type BANK to transfer from.
      - **ToBankAccount**: An [account](https://developer.xero.com/documentation/api/accounts) of Type BANK to transfer to.
      - **Amount**.

Output
-----
#### BankTransfer
The bank transfer created.

Links
-----

https://developer.xero.com/documentation/api/bank-transfers#PUT
