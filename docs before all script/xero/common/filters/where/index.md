---
layout: docs
title: where
description: where
group: common
feature: filters
component: where
toc: true
---
# Where Filter

The where parameter allows you to filter elements using operators on their properties.

**Please note:** even though the where filter supports complex queries we recommend you keep them as simple as possible. Long, complex where queries can cause time outs. To ensure your calls run efficiently against larger organisations it's a good idea to restrict your queries to simple == operations.

### Examples

- **Retrieve all Bank Accounts using the GetAccounts function**
    <pre>Type=="BANK"</pre> This would translate to the following URL once percent encoded. <pre><em>https://api.xero.com/api.xro/2.0/Accounts?where=Type%3D%3D%22BANK%22</em></pre>

- **Retrieve all contacts with specific text in the contact name using the GetContacts function**
    <pre>  Name.Contains("Peter")
    Name.StartsWith("P")
    Name.EndsWith("r")</pre>

- For optional elements such as email address, it is best that you add a not null at the start of the query. If you don’t include it you will get an exception if any of the optional elements aren’t set. This example is using the GetContacts function.
    <pre>EmailAddress!=null&&EmailAddress.StartsWith("boom")</pre>

- **Retrieve invoices with an invoice date between a date range**
    <pre>Date >= DateTime(2015, 01, 01) && Date < DateTime(2015, 12, 31)</pre>

## Links

- [Xero API Request filters](https://developer.xero.com/documentation/api/requests-and-responses).
