GetCurrencies
============

Allows you to retrieve currencies for your organisation.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Currency code
You can retrieve an individual record by specifying the currency's code.
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
#### [Return type](#return-options) of currencies
A list of currencies or a single currency depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/currencies