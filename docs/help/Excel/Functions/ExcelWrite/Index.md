# ExcelWrite

<span class="recommendation">This function supports files that have been created using Microsoft Excel 2007 or later (.xlsx file extension).</span>

ExcelWrite adds a row of data to an Excel spreadsheet file. The row is added to the bottom of a specified sheet, optionally above a defined footer section.

## Properties

- ### File path
The file path to the xlsx file to write to.

- ### File does not exist
The action to take when the specified file does not exist.

	- **Create file** creates a blank spreadsheet file to write to.
	- **Copy template** copies the template file to the destination file path before adding the row.
	- **Throw exception** will stop the process and report an error.

- ### Template file
The file path to xlsx file to copy when no file exists at the destination file path (only visible when the file does not exist option is set to **Copy template**)

- ### Sheet name
The name of the sheet to write to. If the file does not contain a sheet with the specified name, a new sheet with that name will be created.

- ### Footer rows
The number of rows to leave at the bottom of the sheet. The row will be inserted at the specified number of lines above the bottom of the sheet.

- ### Values
The collection of values to insert into the new row.

## Links
[Office Open XML file format](https://en.wikipedia.org/wiki/Office_Open_XML)