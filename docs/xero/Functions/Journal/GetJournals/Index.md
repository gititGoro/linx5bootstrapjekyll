---
layout: docs
title: getjournals
description: getjournals
group: functions
feature: journal
component: getjournals
toc: true
---
GetJournals
============

Use this function to retrieve either one or many journals. A maximum of 100 journals will be returned in any response. Use the offset or If-Modified-Since filters (see below) with multiple API calls to retrieve larger sets of journals. Journals are ordered oldest to newest.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Journal id
You can retrieve an individual record by specifying the JournalID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Offset
Offset by a specified journal number. e.g. journals with a JournalNumber greater than the offset will be returned.
- #### Payments only
Set to true if you want to retrieve journals on a cash basis. Journals are returned on an accrual basis by default.
- #### Return options
Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of Journals
A list of journals or a single journal depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/journals#GET
