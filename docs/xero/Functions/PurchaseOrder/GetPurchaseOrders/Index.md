---
layout: docs
title: GetPurchaseOrders
description: GetPurchaseOrders
group: Functions
feature: PurchaseOrder
component: GetPurchaseOrders
toc: true
redirect_from: docs/Functions/PurchaseOrder/GetPurchaseOrders/index
---
GetPurchaseOrders
============

Allows you to retrieve purchase orders.

**Note:** When you retrieve multiple purchase orders, paging is enforced by default. 100 purchase orders are returned per page.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Purchase order id / number
You can retrieve an individual record by specifying the PurchaseOrderID or the PurchaseOrderNumber.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Date from
Filter by purchase order date (e.g. 2015-12-01).
- #### Date to
Filter by purchase order date (e.g. 2015-12-31).
- #### Status
Filter by purchase order status (e.g. DRAFT).
- #### Page
To specify a page, specify a page parameter e.g. 1. If there are 100 records in the response you will need to check if there is any more data by fetching the next page e.g 2 and continuing this process until no more results are returned.
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
#### [Return type](#return-options) of PurchaseOrders
A list of purchase orders or a single purchase order depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/purchase-orders#get
