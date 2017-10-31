GetContacts
============

Allows you to retrieve a single contact or a list of contacts.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Contact id / number
You can retrieve an individual record by specifying the ContactID or ContactNumber.
- #### Contact ids
Filter by a comma separated list of ContactIDs. Allows you to retrieve a specific set of contacts in a single call.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only contacts created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00 **Note:** changes to the Balances, IsCustomer or IsSupplier values will not trigger a contact to be returned with the Modified-After filter.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Page
e.g. page=1 - Up to 100 contacts will be returned in a single API call.
- #### Include archived
e.g. includeArchived=true - Contacts with a status of ARCHIVED will be included in the response.
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
#### [Return type](#return-options) of Contacts
A list of contacts or a single contact depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/contacts#GET