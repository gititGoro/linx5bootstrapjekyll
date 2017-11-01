---
layout: docs
title: getbanktransfers
description: getbanktransfers
group: functions
feature: banktransfer
component: getbanktransfers
toc: true
---
---
layout: docs
title: getbanktransfers
description: getbanktransfers
group: functions
feature: banktransfer
component: getbanktransfers
toc: true
---
---
layout: docs
title: getbanktransfers
description: getbanktransfers
group: functions
feature: banktransfer
component: getbanktransfers
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: banktransfer
feature: getbanktransfers
component: index.md
toc: true
---
---
layout: docs
title: GetBankTransfers
description: GetBankTransfers
group: Functions
feature: BankTransfer
component: GetBankTransfers
toc: true
redirect_from: docs/Functions/BankTransfer/GetBankTransfers/index
---
GetBankTransfers
============

Use this function to retrieve one or many bank transfers.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Bank transfer id / number
You can retrieve an individual record by specifying the BankTransferID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss). Only bank transfers created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Order by
Return items in a specific order. See [Order by filter](../../../Common/Filters/OrderBy/Index.md).
- #### Return options
Return Options: Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of bank transfers
A list of bank transfers or a single bank transfer depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/bank-transfers#GET
