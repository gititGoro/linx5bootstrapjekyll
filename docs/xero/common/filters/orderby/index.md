---
layout: docs
title: orderby
description: orderby
group: common
feature: filters
component: orderby
toc: true
---
# Order by Filter

A list of items can be returned in a specific order. To specify the ordering, provide a property to order by and wether the order is ascending or descending.

### Example

#### Order contacts by email address

- **Ascending**
    <pre>EmailAddress</pre> becomes <pre><em>https://api.xero.com/api.xro/2.0/Contacts?order=EmailAddress</em></pre> in the query string.

- **Descending**
     <pre>EmailAddress DESC</pre> becomes <pre><em>https://api.xero.com/api.xro/2.0/Contacts?order=EmailAddress%20DESC</em></pre> in the query string.

## Links

- [Xero API Request filters](https://developer.xero.com/documentation/api/requests-and-responses).
