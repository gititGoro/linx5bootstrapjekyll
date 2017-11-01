---
layout: docs
title: getusers
description: getusers
group: functions
feature: user
component: getusers
toc: true
---
---
layout: docs
title: getusers
description: getusers
group: functions
feature: user
component: getusers
toc: true
---
---
layout: docs
title: getusers
description: getusers
group: functions
feature: user
component: getusers
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: user
feature: getusers
component: index.md
toc: true
---
---
layout: docs
title: GetUsers
description: GetUsers
group: Functions
feature: User
component: GetUsers
toc: true
redirect_from: docs/Functions/User/GetUsers/index
---
GetUsers
============

Returns the users for a Xero organisation.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### User id
You can retrieve an individual record by specifying the UserID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Filter by any element. See [Where filter](../../../Common/Filters/Where/Index.md).
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
#### [Return type](#return-options) of Users
A list of users or a single user depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/users#GET
