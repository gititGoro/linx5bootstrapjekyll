---
layout: docs
title: createcontact
description: createcontact
group: functions
feature: contact
component: createcontact
toc: true
---
CreateContact
============

Allows you to create individual contacts in a Xero organisation.

**Note** if an existing contact matches your ContactName or ContactNumber then you will receive an error.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Contact
Contact to create.
- Mandatory Items:
     - **Name**: Full name of contact/organisation (max length = 255).


Output
-----
#### Contact
The contact created.

Links
-----

https://developer.xero.com/documentation/api/contacts#PUT
