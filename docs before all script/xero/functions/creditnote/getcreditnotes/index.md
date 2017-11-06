---
layout: docs
title: getcreditnotes
description: getcreditnotes
group: functions
feature: creditnote
component: getcreditnotes
toc: true
---
GetCreditNotes
============

Allows you to retrieve a single credit note or a list of credit notes.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### credit note id / number
You can retrieve an individual record by specifying the CreditNoteID or credit note number.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only credit notes created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
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
#### [Return type](#return-options) of credit notes
A list of credit notes or a single credit note depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/credit-notes#GET
