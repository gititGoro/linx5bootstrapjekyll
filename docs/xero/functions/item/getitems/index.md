---
layout: docs
title: getitems
description: getitems
group: functions
feature: item
component: getitems
toc: true
---
GetItems
============

Use this function to retrieve one or many items.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Item id / code
You can retrieve an individual record by specifying the ItemID or Code.
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
#### [Return type](#return-options) of Items
A list of items or a single item depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/items#GET
