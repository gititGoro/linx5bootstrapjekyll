---
layout: docs
title: SendEmail
description: SendEmail
group: linx5main
feature: Samples
component: SendEmail
toc: true
redirect_from: docs/linx5main/Samples/SendEmail/index
---
Send Email
==========

This sample sends emails with data retrieved from the Northwind database. You can get sample [Northwind databases here](https://code.google.com/p/northwindextended/downloads/list).

This Linx Solution uses the **Email** and **Database** plugins.

In this solution there are three Processes:

- **SendEmailToMadridCustomers**  
  Retrieves Madrid customers and sends plain text email.
- **SendFancyEmailToCityCustomers**  
  Retrieves customers from the City provided as a parameter, formats an html email and sends it.
- **SendEmailAndNotifyStats**  
  Retrieves Madrid customers, sends plain text email and notifies you via email how many emails were sent.

#### To run the sample

1. Open the SendEmail solution with Linx Designer.
1. Click on the Settings button.
1. On the Settings tab change the values of the Settings to suit your environment (if you need help with the database connection string, have a look at [this help topic](https://linx.software/plugins/Database/Functions/ExecuteSQL/)).
1. Open the sample Process you want to run from the Solution Explorer pane on the left.
1. Click the Debug or Run buttons to run the Process. For SendFancyEmailToCityCustomers you have to supply the City name as a parameter (bottom right in the debugger) or else nothing will be retrieved.

#### Download
[SendEmail.zip](SendEmail.zip)

#### Video
[Linx 5 SendEmail](https://www.youtube.com/watch?v=-eynMhbET-I)
