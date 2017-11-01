---
layout: docs
title: UpdateTaxRate
description: UpdateTaxRate
group: Functions
feature: TaxRate
component: UpdateTaxRate
toc: true
redirect_from: docs/Functions/TaxRate/UpdateTaxRate/index
---
UpdateTaxRate
============

Use this method to update a tax rate. Please note that system defined tax rates can't be updated.

All the existing tax components must be supplied when updating a tax rate. If a tax rate has three components, and only two are supplied while updating it, the third component will be deleted.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Tax type
Tax type of the tax rate to update. See [Tax Types](https://developer.xero.com/documentation/api/types/#TaxTypes).
- #### Tax rate
Tax rate to update.


Output
-----
#### TaxRate
Tax rate updated.

Links
-----

https://developer.xero.com/documentation/api/tax-rates#POST
