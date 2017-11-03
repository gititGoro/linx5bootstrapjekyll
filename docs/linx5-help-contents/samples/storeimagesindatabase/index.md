---
layout: docs
title: storeimagesindatabase
description: storeimagesindatabase
group: linx5-help-contents
feature: samples
component: storeimagesindatabase
toc: true
---
Store Images In Database
========================

This sample shows how you can save images to a database and retrieve them afterwards. This sample updates and reads photos of employees in the Northwind database. You can get sample Northwind databases [here](https://code.google.com/p/northwindextended/downloads/list).

This Linx Solution uses the **Database**, **File**, and **Utilities** plugins.

There are two Processes:

- **StoreImages**  
  Finds and reads every image file in the input folder and sets the image as the photo of the corresponding employee in the database. An OLE-header is added to each image before it is stored. The OLE-header is optional and is typically only necessary when the file format of the image is not always known.
- **LoadImages**  
  Writes the photo of every employee to an output folder. The OLE-header (which is 78 bytes long) is discarded before the file is saved.

#### To run the sample

1. Open the StoreImagesInDatabase solution with Linx Designer.
1. Click on the Settings button.
1. On the Settings tab change the values of the Settings to suit your environment (if you need help with the database connection string, have a look at this help topic).
1. Open the sample Process you want to run from the Solution Explorer pane on the left.

Click the Debug or Run buttons to run the Process.

#### Download
[StoreImagesInDatabase.zip](StoreImagesInDatabase.zip)
