---
layout: docs
title: GetContactGroups
description: GetContactGroups
group: Functions
feature: ContactGroup
component: GetContactGroups
toc: true
redirect_from: docs/Functions/ContactGroup/GetContactGroups/index
---
GetContactGroups
============

Allows you to retrieve a list of all the contact groups, or a single contact group with all its contacts (ContactID and Name).

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Contact group id / name
You can retrieve an individual record by specifying the ContactGroupID or Name.
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
#### [Return type](#return-options) of ContactGroups
A list of contact groups or a single contact group depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/contactgroups#GET
