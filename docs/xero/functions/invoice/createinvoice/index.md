---
layout: docs
title: createinvoice
description: createinvoice
group: functions
feature: invoice
component: createinvoice
toc: true
---
---
layout: docs
title: createinvoice
description: createinvoice
group: functions
feature: invoice
component: createinvoice
toc: true
---
---
layout: docs
title: createinvoice
description: createinvoice
group: functions
feature: invoice
component: createinvoice
toc: true
---
---
layout: docs
title: index.md
description: index.md
group: invoice
feature: createinvoice
component: index.md
toc: true
---
---
layout: docs
title: CreateInvoice
description: CreateInvoice
group: Functions
feature: Invoice
component: CreateInvoice
toc: true
redirect_from: docs/Functions/Invoice/CreateInvoice/index
---
CreateInvoice
============

Create sales invoices or purchase bills. Learn more about Xero sales invoices at the [Xero Business Help Centre](https://help.xero.com/int/Invoices-Sales). Learn more about Xero purchase bills at the [Xero Business Help Centre](https://help.xero.com/int/Payments_Bills).


Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Invoice
Invoice to create.
- Mandatory Items:
     - **Type**: See [Invoice Types](https://developer.xero.com/documentation/api/types#InvoiceTypes).
     - **Contact**: See [Contacts](https://developer.xero.com/documentation/api/contacts#).
     - **LineItems**: See [LineItems](https://developer.xero.com/documentation/api/invoices#LineItemsPOST). The LineItems collection can contain any number of individual LineItem sub-elements. At least one is required to create a complete Invoice.


Output
-----
#### Invoice
The invoice created.

Links
-----

https://developer.xero.com/documentation/api/invoices#put
