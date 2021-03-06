---
layout: docs
title: createemployee
description: createemployee
group: functions
feature: employee
component: createemployee
toc: true
---
CreateEmployee
============

Use this function to create an employee record.

**Note** if an existing employee matches your FirstName and LastName then you will receive an error.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Employee
Employee to create.
- Mandatory Items:
     - **FirstName**: First name of an employee (max length = 255).
     - **LastName**: Last name of an employee (max length = 255).


Output
-----
#### Employee
The employee created.

Links
-----

https://developer.xero.com/documentation/api/employees#PUT
