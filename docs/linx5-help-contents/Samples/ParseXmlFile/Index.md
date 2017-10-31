Parse XML File
==============

This sample parses an XML file and inserts new orders into the Northwind database. You can get sample Northwind databases [here](https://code.google.com/p/northwindextended/downloads/list).

This Linx Solution uses the **Xml**, **Database**, and **Utilities** plugins.

There are three processes:

- **ImportOrders**  
  Parses the XML file using the XmlReader function and inserts the records into the database.
- **ExportImportedOrders**  
  Writes the imported orders to an XML file using the XmlWriter function to check that the import succeeded.
- **DeleteImportedOrders**  
  Deletes the imported orders.

#### To run the sample

1. Open the ParseXMLFile solution.
1. Click on the Settings button.
1. On the Settings tab change the values of the Settings to suit your environment (if you need help with the database connection string, 
have a look at [this](https://linx.software/plugins/Database/Tools/ConnectionEditor/) help topic).
1. Open the sample Process you want to run from the Solution Explorer pane on the left.
1. Click the Debug or Run buttons to run the Process.

#### Download
[ParseXMLFile.zip](ParseXMLFile.zip)
