---
layout: docs
title: updateitem
description: updateitem
group: functions
feature: item
component: updateitem
toc: true
---
---
layout: docs
title: updateitem
description: updateitem
group: functions
feature: item
component: updateitem
toc: true
---
---
layout: docs
title: updateitem
description: updateitem
group: functions
feature: item
component: updateitem
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: item
feature: updateitem
component: index.md
toc: true
---
---
layout: docs
title: UpdateItem
description: UpdateItem
group: Functions
feature: Item
component: UpdateItem
toc: true
redirect_from: docs/Functions/Item/UpdateItem/index
---
UpdateItem
============

Use this method to update an item record.

**Note:**
- The Quantity and TotalCostPool elements are read-only. They cannot be explicitly set via the Items endpoint. The only way to change the quantity and value of tracked items is by creating accounting transactions. 
- Increase the value and quantity of a tracked item by creating purchase transactions (ACCPAY Invoices or SPEND BankTransactions) and decrease the value and quantity of tracked items by creating sales transactions (ACCREC Invoices or RECEIVE BankTransactions). Read more about inventory adjustments in the [Tracked Inventory in Xero](http://developer.xero.com/documentation/api-guides/tracked-inventory-in-xero/) guide.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Item id / code
Unique identifier of the item to update.
- #### Item
Item to update.


Output
-----
#### Item
Item updated.

Links
-----

https://developer.xero.com/documentation/api/items#POST
