CreateTaxRate
============

Allows you to add a new tax rate for a Xero organisation.

**Tax Rate Constraints**

In order to properly complete tax forms, certain tax rates can only be used with certain account types. This will vary depending on the rate and country. Sending a line item with a tax rate that can't be use with the line's account code will generate the error "The TaxType code 'xxx' cannot be used with account code 'xxx'."

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Tax rate
Tax rate to create.


Output
-----
#### TaxRate
The tax rate created.

Links
-----

https://developer.xero.com/documentation/api/tax-rates#PUT