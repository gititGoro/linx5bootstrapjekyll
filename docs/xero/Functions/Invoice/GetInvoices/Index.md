---
layout: docs
title: GetInvoices
description: GetInvoices
group: Functions
feature: Invoice
component: GetInvoices
toc: true
redirect_from: docs/Functions/Invoice/GetInvoices/index
---
GetInvoices
============

Use this function to retrieve one or many invoices.

**Note**:
- When you retrieve multiple invoices, only a summary of the contact is returned and no line details are returned - this is to keep the response more compact.
- The line item details will be returned when you retrieve an individual invoice, either by specifying Invoice ID or Invoice Number or by using the optional paging parameter (below).

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Invoice id / number
You can retrieve an individual record by specifying the InvoiceID or Number.
- #### Invoice ids
Filter by a comma-separated list of InvoicesIDs. For faster response times we recommend using these explicit parameters instead of passing OR conditions into the Where filter.
- #### Invoice numbers
Filter by a comma-separated list of InvoiceNumbers. For faster response times we recommend using these explicit parameters instead of passing OR conditions into the Where filter.
- #### Contact ids
Filter by a comma-separated list of ContactIDs. For faster response times we recommend using these explicit parameters instead of passing OR conditions into the Where filter.
- #### Statuses
Filter by a comma-separated list of Statuses. For faster response times we recommend using these explicit parameters instead of passing OR conditions into the Where filter.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Page
e.g. page=1 – Up to 100 records will be returned in a single API call with line items shown for each record.
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
#### [Return type](#return-options) of Invoices
A list of invoices or a single invoice depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/invoices#get
