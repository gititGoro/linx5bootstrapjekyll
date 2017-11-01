GetLinkedTransactions
============

Use this function to retrieve one or many linked transactions.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Linked transaction id
You can retrieve an individual record by specifying the LinkedTransactionID.
- #### Page
Up to 100 linked transactions will be returned in a single API call. Use the page parameter to specify the page to be returned e.g. page=1.
- #### Source transaction id
Filter by the SourceTransactionID. Get all the linked transactions created from a particular ACCPAY invoice.
- #### Contact id
Filter by the ContactID. Get all the linked transactions that have been assigned to a particular customer.
- #### Status
Get all the linked transactions that have a particular status.
- #### Target transaction id
Filter by the TargetTransactionID. Get all the linked transactions allocated to a particular ACCREC invoice.
- #### Return options
Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of LinkedTransactions
A list of linked transactions or a single linked transaction depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/linked-transactions#GET