---
layout: docs
title: createlinkedtransaction
description: createlinkedtransaction
group: functions
feature: linkedtransaction
component: createlinkedtransaction
toc: true
---
---
layout: docs
title: createlinkedtransaction
description: createlinkedtransaction
group: functions
feature: linkedtransaction
component: createlinkedtransaction
toc: true
---
---
layout: docs
title: createlinkedtransaction
description: createlinkedtransaction
group: functions
feature: linkedtransaction
component: createlinkedtransaction
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: linkedtransaction
feature: createlinkedtransaction
component: index.md
toc: true
---
---
layout: docs
title: CreateLinkedTransaction
description: CreateLinkedTransaction
group: Functions
feature: LinkedTransaction
component: CreateLinkedTransaction
toc: true
redirect_from: docs/Functions/LinkedTransaction/CreateLinkedTransaction/index
---
CreateLinkedTransaction
============

Use this function to create a linked transaction record.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Linked transaction
LinkedTransaction to create.
- Mandatory fields:
     - **SourceTransactionID**: The identifier of the source transaction (the purchase component of a billable expense). Either an invoice with a type of ACCPAY or a banktransaction of type SPEND.
     - **SourceLineItemID**: The line item identifier from the source transaction.


Output
-----
#### LinkedTransaction
The linked transaction created.

Links
-----

https://developer.xero.com/documentation/api/linked-transactions#PUT
