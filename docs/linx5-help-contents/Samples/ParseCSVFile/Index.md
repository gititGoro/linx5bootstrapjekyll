---
layout: docs
title: ParseCSVFile
description: ParseCSVFile
group: linx5-help-contents
feature: Samples
component: ParseCSVFile
toc: true
redirect_from: docs/linx5-help-contents/Samples/ParseCSVFile/index
---
Parse CSV File
==============

Parse a CSV file and import into a database.
This sample parses a CSV file and updates the Northwind database with new customers. You can get sample Northwind databases [here](https://code.google.com/p/northwindextended/downloads/list).

This Linx Solution uses the **File**, **Database**, and **Utilities** plugins.

There are three processes:
  
- **ImportCustomers**  
  Parses the CSV file using the [TextFileRead](https://linx.software/plugins/File/Functions/TextFileRead/) function and inserts the records into the database.
- **ExportImportedCustomers**  
  Writes the imported customers to file using the [TextFileWrite](https://linx.software/plugins/File/Functions/TextFileWrite/) function to check that the import succeeded.
- **DeleteImportedCustomers**  
  Deletes the imported customers.

#### To run the sample

1. Open the ParseCSVFile solution with Linx Designer.
1. Click on the Settings button.
1. On the Settings tab change the values of the Settings to suit your environment (if you need help with the database connection string, have a look at [this help topic](https://linx.software/plugins/Database/Functions/ExecuteSQL/)).
1. Open the sample Process you want to run from the Solution Explorer pane on the left.
1. Click the Debug or Run buttons to run the Process.

#### Download
[ParseCSVFile.zip](ParseCSVFile.zip)

#### Video
[Linx 5 ParseCSV Example ](https://www.youtube.com/watch?v=dogbt9zQZAE)
