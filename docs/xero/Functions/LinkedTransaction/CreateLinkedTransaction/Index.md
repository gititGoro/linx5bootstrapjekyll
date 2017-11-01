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