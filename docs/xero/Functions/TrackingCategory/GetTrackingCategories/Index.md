GetTrackingCategories
============

Retrieve tracking categories for a Xero organisation.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Tracking category id
You can retrieve an individual record by specifying the TrackingCategoryID.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Order by
Return items in a specific order. See [Order by filter](../../../Common/Filters/OrderBy/Index.md).
- #### Include archived
e.g. true - Categories and options with a status of ARCHIVED will be included in the response.
- #### Return options
Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of TrackingCategorys
A list of tracking categories or a single tracking category depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/tracking-categories#GET