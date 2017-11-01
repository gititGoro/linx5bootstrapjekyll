---
layout: docs
title: createprepaymentallocation
description: createprepaymentallocation
group: functions
feature: prepaymentallocation
component: createprepaymentallocation
toc: true
---
---
layout: docs
title: createprepaymentallocation
description: createprepaymentallocation
group: functions
feature: prepaymentallocation
component: createprepaymentallocation
toc: true
---
---
layout: docs
title: createprepaymentallocation
description: createprepaymentallocation
group: functions
feature: prepaymentallocation
component: createprepaymentallocation
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: prepaymentallocation
feature: createprepaymentallocation
component: index.md
toc: true
---
---
layout: docs
title: CreatePrepaymentAllocation
description: CreatePrepaymentAllocation
group: Functions
feature: PrepaymentAllocation
component: CreatePrepaymentAllocation
toc: true
redirect_from: docs/Functions/PrepaymentAllocation/CreatePrepaymentAllocation/index
---
CreatePrepaymentAllocation
============

Use this function to allocate part or full amounts of a prepayment to outstanding invoices. For more information on prepayments in Xero visit the [Xero Business Help Centre](https://help.xero.com/int/BankAccounts_Prepayments).

**Note:**
- Create Prepayments using the [CreateBankTransaction](../../BankTransaction/CreateBankTransaction/Index.md) function.
- Refund Prepayments using the [CreatePayment](../../Payment/CreatePayment/Index.md) function.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Prepayment id
The PrepaymentID of the prepayment to allocate.
- #### Allocation
The data for the allocation.


Output
-----
#### PrepaymentAllocation
The prepayment allocation created.

Links
-----

https://developer.xero.com/documentation/api/prepayments#PUT
