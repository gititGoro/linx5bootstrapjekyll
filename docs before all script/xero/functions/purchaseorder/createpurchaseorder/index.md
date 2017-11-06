---
layout: docs
title: createpurchaseorder
description: createpurchaseorder
group: functions
feature: purchaseorder
component: createpurchaseorder
toc: true
---
CreatePurchaseOrder
============

Allows you to add purchase orders. To learn more about purchase orders in Xero visit the [Xero Business Help Centre](https://help.xero.com/int/PurchaseOrders).

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### PurchaseOrder
Purchase order to create.
  - Mandatory fields:
    * **Contact:**  You need to provide the ContactID or ContactNumber of an existing contact. For more information on creating contacts see [Contacts](../../Contact/CreateContact/Index.md).
    * **LineItems:** The LineItems collection can contain any number of individual LineItem sub-elements. At least one LineItem is required to create a complete PurchaseOrder.


Output
-----
#### PurchaseOrder
The purchase order created.

Links
-----

https://developer.xero.com/documentation/api/purchase-orders#put
