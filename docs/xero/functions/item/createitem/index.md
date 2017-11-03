---
layout: docs
title: createitem
description: createitem
group: functions
feature: item
component: createitem
toc: true
---
CreateItem
============

Use this function to create an item record.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Item
Item to create.
- Mandatory fields:
     - **Code**: User defined item code (max length = 30).
- Mandatory fields on a tracked inventory item:
     - **InventoryAssetAccountCode**: The inventory asset account for the item. The account must be of type INVENTORY. The [COGSAccountCode](https://developer.xero.com/documentation/api/items#COGS) in PurchaseDetails is also required to create a tracked item.


Output
-----
#### Item
The item created.

Links
-----

https://developer.xero.com/documentation/api/items#PUT
