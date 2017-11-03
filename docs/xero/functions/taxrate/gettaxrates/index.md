---
layout: docs
title: gettaxrates
description: gettaxrates
group: functions
feature: taxrate
component: gettaxrates
toc: true
---
GetTaxRates
============

Returns tax rates for a Xero organisation.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Tax type
Filter by a [Tax Type](https://developer.xero.com/documentation/api/types#TaxTypes).
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
#### [Return type](#return-options) of TaxRates
A list of tax rates or a single tax rate depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/tax-rates#GET
