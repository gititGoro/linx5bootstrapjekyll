---
layout: docs
title: getemployees
description: getemployees
group: functions
feature: employee
component: getemployees
toc: true
---
---
layout: docs
title: getemployees
description: getemployees
group: functions
feature: employee
component: getemployees
toc: true
---
---
layout: docs
title: getemployees
description: getemployees
group: functions
feature: employee
component: getemployees
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: employee
feature: getemployees
component: index.md
toc: true
---
---
layout: docs
title: GetEmployees
description: GetEmployees
group: Functions
feature: Employee
component: GetEmployees
toc: true
redirect_from: docs/Functions/Employee/GetEmployees/index
---
GetEmployees
============

Use this function to retrieve one or many employees.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Employee id
You can retrieve an individual record by specifying the EmployeeID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
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
#### [Return type](#return-options) of Employees
A list of employees or a single employee depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/employees#GET
