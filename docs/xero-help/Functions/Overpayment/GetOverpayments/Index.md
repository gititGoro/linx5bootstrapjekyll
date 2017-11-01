GetOverpayments
============

Use this function to retrieve overpayments.

**Note:**
- Create Overpayments using the [CreateBankTransaction](../../BankTransaction/CreateBankTransaction/Index.md) function.
- Refund Overpayments using the [CreatePayment](../../Payment/CreatePayment/Index.md) function.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Overpayment id
You can retrieve an individual record by specifying the OverpaymentID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss) . Only records created or modified since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Order by
Return items in a specific order. See [Order by filter](../../../Common/Filters/OrderBy/Index.md).
- #### Page
e.g. page=1 – Up to 100 overpayments will be returned in a single API call with line items shown for each overpayment.
- #### Return options
Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of Overpayments
A list of overpayments or a single overpayment depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/overpayments#GET