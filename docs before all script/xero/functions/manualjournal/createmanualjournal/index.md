---
layout: docs
title: createmanualjournal
description: createmanualjournal
group: functions
feature: manualjournal
component: createmanualjournal
toc: true
---
CreateManualJournal
============

Use this function to create a manual journal record.

**Note**: There are a few accounts that you can't use when entering manual journals in Xero. These include system accounts (accounts receivable, accounts payable & retained earnings) and bank accounts. You will receive a 400 validation error if you try and use these reserved accounts. Consider setting up one or more clearing accounts if you need to journal to a bank account.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Manual journal
ManualJournal to create.
- Mandatory fields:
     - **Narration**: Description of journal being posted.
     - **JournalLines**: See [JournalLines](https://developer.xero.com/documentation/api/manual-journals#LineItemsPOST). The JournalLines element must contain at least two individual JournalLine sub-elements.


Output
-----
#### ManualJournal
The manual journal created.

Links
-----

https://developer.xero.com/documentation/api/linked-transactions#PUT
