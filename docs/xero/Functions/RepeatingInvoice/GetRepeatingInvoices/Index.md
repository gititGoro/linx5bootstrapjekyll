---
layout: docs
title: GetRepeatingInvoices
description: GetRepeatingInvoices
group: Functions
feature: RepeatingInvoice
component: GetRepeatingInvoices
toc: true
redirect_from: docs/Functions/RepeatingInvoice/GetRepeatingInvoices/index
---
GetRepeatingInvoices
============

Use this method to retrieve either one or many repeating invoices.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Repeating invoice id / number
You can retrieve an individual record by specifying the RepeatingInvoiceID.
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
#### [Return type](#return-options) of RepeatingInvoices
A list of repeating invoices or a single repeating invoice depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/repeating-invoices#get
