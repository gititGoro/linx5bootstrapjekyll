---
layout: docs
title: createreceipt
description: createreceipt
group: functions
feature: receipt
component: createreceipt
toc: true
---
---
layout: docs
title: createreceipt
description: createreceipt
group: functions
feature: receipt
component: createreceipt
toc: true
---
---
layout: docs
title: createreceipt
description: createreceipt
group: functions
feature: receipt
component: createreceipt
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: receipt
feature: createreceipt
component: index.md
toc: true
---
---
layout: docs
title: CreateReceipt
description: CreateReceipt
group: Functions
feature: Receipt
component: CreateReceipt
toc: true
redirect_from: docs/Functions/Receipt/CreateReceipt/index
---
CreateReceipt
============

Allows you to add draft expense claim receipts.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Receipt
Receipt to create.
  - Mandatory fields:
    * **Date:** Date of receipt - YYYY-MM-DD.
    * **Contact:** See [contacts](https://developer.xero.com/documentation/api/contacts#).
    * **LineItems:** At least one line item is required to create a complete receipt.
    * **User:** The user in the organisation that the expense claim receipt is for.


Output
-----
#### Receipt
The receipt created.

Links
-----

https://developer.xero.com/documentation/api/receipts#PUT
