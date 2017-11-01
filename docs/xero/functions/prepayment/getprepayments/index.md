---
layout: docs
title: getprepayments
description: getprepayments
group: functions
feature: prepayment
component: getprepayments
toc: true
---
---
layout: docs
title: getprepayments
description: getprepayments
group: functions
feature: prepayment
component: getprepayments
toc: true
---
---
layout: docs
title: getprepayments
description: getprepayments
group: functions
feature: prepayment
component: getprepayments
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: prepayment
feature: getprepayments
component: index.md
toc: true
---
---
layout: docs
title: GetPrepayments
description: GetPrepayments
group: Functions
feature: Prepayment
component: GetPrepayments
toc: true
redirect_from: docs/Functions/Prepayment/GetPrepayments/index
---
GetPrepayments
============

Use this function to retrieve prepayments.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Prepayment id
You can retrieve an individual record by specifying the PrepaymentID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Page
e.g. page=1 – Up to 100 records will be returned in a single API call with line items shown for each record.
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
#### [Return type](#return-options) of Prepayments
A list of prepayments or a single prepayment depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/prepayments#GET
