---
layout: docs
title: GetAccounts
description: GetAccounts
group: Functions
feature: Account
component: GetAccounts
toc: true
redirect_from: docs/Functions/Account/GetAccounts/index
---
GetAccounts
============

Allows you to retrieve the full chart of accounts.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Account Id
Unique identifier of the account to filter by.
- #### Modified after
Only return items that were created or modified after this UTC date value.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Order by
Return items in a specific order. See [Order by filter](../../../Common/Filters/OrderBy/Index.md).
- #### Return options
Return Options: Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of Accounts
A list of accounts or a single account depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/accounts#GET
