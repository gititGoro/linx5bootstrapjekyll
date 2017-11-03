---
layout: docs
title: getreceipts
description: getreceipts
group: functions
feature: receipt
component: getreceipts
toc: true
---
GetReceipts
============

Use this method to retrieve either one or many draft receipts.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Receipt id
You can retrieve an individual record by specifying the ReceiptID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Order by
Return items in a specific order. See [Order by filter](../../../Common/Filters/OrderBy/Index.md).
- #### Return options
Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of Receipts
A list of receipts or a single receipt depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/receipts#GET
