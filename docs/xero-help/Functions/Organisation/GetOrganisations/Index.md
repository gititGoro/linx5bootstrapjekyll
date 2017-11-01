GetOrganisations
============

Returns information about a Xero organisation.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Return options
Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of Organisations
A list of organisations or a single organisation depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/organisation