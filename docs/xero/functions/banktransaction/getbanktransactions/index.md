---
layout: docs
title: getbanktransactions
description: getbanktransactions
group: functions
feature: banktransaction
component: getbanktransactions
toc: true
---
---
layout: docs
title: getbanktransactions
description: getbanktransactions
group: functions
feature: banktransaction
component: getbanktransactions
toc: true
---
---
layout: docs
title: getbanktransactions
description: getbanktransactions
group: functions
feature: banktransaction
component: getbanktransactions
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: banktransaction
feature: getbanktransactions
component: index.md
toc: true
---
---
layout: docs
title: GetBankTransactions
description: GetBankTransactions
group: Functions
feature: BankTransaction
component: GetBankTransactions
toc: true
redirect_from: docs/Functions/BankTransaction/GetBankTransactions/index
---
GetBankTransactions
============

Use this function to retrieve one or many bank transactions.

This function does not return payments applied to invoices, expense claims or transfers between bank accounts.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Bank transaction id
You can retrieve an individual record by specifying the BankTransactionID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss). Only bank transactions created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Order by
Return items in a specific order. See [Order by filter](../../../Common/Filters/OrderBy/Index.md).
- #### Page
e.g. page=1 – Up to 100 bank transactions will be returned in a single API call with line items shown for each bank transaction.
- #### Return options
Return Options: Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of bank transactions
A list of bank transactions or a single bank transaction depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/banktransactions#GET
