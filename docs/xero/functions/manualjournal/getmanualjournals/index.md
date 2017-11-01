---
layout: docs
title: getmanualjournals
description: getmanualjournals
group: functions
feature: manualjournal
component: getmanualjournals
toc: true
---
---
layout: docs
title: getmanualjournals
description: getmanualjournals
group: functions
feature: manualjournal
component: getmanualjournals
toc: true
---
---
layout: docs
title: getmanualjournals
description: getmanualjournals
group: functions
feature: manualjournal
component: getmanualjournals
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: manualjournal
feature: getmanualjournals
component: index.md
toc: true
---
---
layout: docs
title: GetManualJournals
description: GetManualJournals
group: Functions
feature: ManualJournal
component: GetManualJournals
toc: true
redirect_from: docs/Functions/ManualJournal/GetManualJournals/index
---
GetManualJournals
============

Use this function to retrieve one or many manual journals.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Manual journal id
You can retrieve an individual record by specifying the ManualJournalID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Order by
Return items in a specific order. See [Order by filter](../../../Common/Filters/OrderBy/Index.md).
- #### Page
Up to 100 manual journals will be returned in a single API call. Use the page parameter to specify the page to be returned e.g. page=1.
- #### Return options
Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of ManualJournals
A list of manual journals or a single manual journal depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/manual-journals#GET
