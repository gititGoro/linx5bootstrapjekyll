CreateExpenseClaim
============

Allows you to submit expense claims for approval. To learn more about expense claims in Xero visit the [Xero Business Help Centre](https://help.xero.com/int/Payments_ExpenseClaims).

**Note** you cannot pay an expense claim via the Xero API. Payment needs to be done in the Xero app.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### ExpenseClaim
Expense claim to create.
- Mandatory Items:
     - **User**: See [Users](https://developer.xero.com/documentation/api/Users).
     - **Receipts**: See [Receipts](https://developer.xero.com/documentation/api/Receipts).


Output
-----
#### ExpenseClaim
The expense claim created.

Links
-----

https://developer.xero.com/documentation/api/expense-claims#PUT