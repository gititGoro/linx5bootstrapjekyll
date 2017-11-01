---
layout: docs
title: UpdateAccount
description: UpdateAccount
group: Functions
feature: Account
component: UpdateAccount
toc: true
redirect_from: docs/Functions/Account/UpdateAccount/index
---
UpdateAccount
============

Use this method to update account details.
#### Limitations
- Accounts of type 'BANK' cannot be updated.
- You can only update accounts one at a time (i.e. you'll need to do multiple API calls to update many accounts).
- You cannot update the status to archived when also updating other values.

Properties
----------

-  #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
-  #### Account Id
Unique identifier of the account to update.
-  #### Account
Account to update.


Output
-----
#### Account
Account updated.

Links
-----

https://developer.xero.com/documentation/api/accounts#POST
