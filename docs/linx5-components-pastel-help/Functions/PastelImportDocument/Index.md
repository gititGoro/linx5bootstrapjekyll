PastelImportDocument
====================

<span class="recommendation">In order to use this function, the necessary Pastel SDK files have to be installed on the host computer.</span>

Import customer or supplier documents into Pastel.

Properties
----------

-  #### Data path

    The location of the Pastel data files.

-  #### User id

	An optional user id to send with the document import.

-  #### Document type

	The document type to import.  Available options are:
	*Customer quotation, Customer sales orders, Customer invoices, Customer credit note, Customer debit note, Supplier purchase orders, Supplier GRNs, Supplier invoices, Supplier return/debit* or *Supplier credit to supplier*.

-  #### Create batch

	 If true, the document will not be updated to the General Ledger and will remain in a batch.

-  #### Header

    A list of pipe-separated ("|") strings that defines the header of the document.  The header string must use the following layout:

	<table>
		<thead>
			<tr>
				<th>Field</th>
				<th>Type</th>
				<th>Format</th>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td>Document Number</td>
				<td>Character</td>
				<td>8 characters maximum</td>
			</tr>
			<tr>
				<td>Deleted</td>
				<td>Character</td>
				<td>Y=Deleted, <space>=Not deleted</td>
			</tr>
			<tr>
				<td>Print Status</td>
				<td>Character</td>
				<td>Y=Printed, <space>=Not printed</td>
			</tr>
			<tr>
				<td>Customer Code</td>
				<td>Character</td>
				<td>7 characters maximum</td>
			</tr>
			<tr>
				<td>Date</td>
				<td>Character</td>
				<td>DD/MM/YYYY (Date defaults the period, unless it is not within the range of the year)</td>
			</tr>
			<tr>
				<td>Order Number</td>
				<td>Character</td>
				<td>15 characters maximum</td>
			</tr>
			<tr>
				<td>Inc/Exc</td>
				<td>Character</td>
				<td>Y=Inclusive, N=Exclusive</td>
			</tr>
			<tr>
				<td>Discount</td>
				<td>Numeric</td>
				<td>4 characters</td>
			</tr>
			<tr>
				<td>Invoice Message 1-3</td>
				<td>Character</td>
				<td>3 separate fields of 30 characters maximum each</td>
			</tr>
			<tr>
				<td>Delivery Address 1-5</td>
				<td>Character</td>
				<td>5 separate fields of 30 characters maximum each</td>
			</tr>
			<tr>
				<td>Sales Analysis Code</td>
				<td>Character</td>
				<td>5 characters maximum</td>
			</tr>
			<tr>
				<td>Settlement Terms Code</td>
				<td>Character</td>
				<td>00-32</td>
			</tr>
			<tr>
				<td>Job Code</td>
				<td>Character</td>
				<td>5 characters maximum</td>
			</tr>
			<tr>
				<td>Closing Date</td>
				<td>Character</td>
				<td>DD/MM/YYYY, expiry date for quotes, and expected delivery date for orders</td>
			</tr>
			<tr>
				<td>Telephone</td>
				<td>Character</td>
				<td>16 characters maximum</td>
			</tr>
			<tr>
				<td>Contact</td>
				<td>Character</td>
				<td>16 characters maximum</td>
			</tr>
			<tr>
				<td>Fax</td>
				<td>Character</td>
				<td>16 characters maximum</td>
			</tr>
			<tr>
				<td>Exchange Rate</td>
				<td>Numeric</td>
				<td>Exchange rate</td>
			</tr>
			<tr>
				<td>Optional</td>
				<td></td>
				<td></td>
			</tr>
			<tr>
				<td>Description</td>
				<td>Character</td>
				<td>40 characters</td>
			</tr>
			<tr>
				<td>ExemptRef</td>
				<td>Character</td>
				<td>16 characters</td>
			</tr>
			<tr>
				<td>Address (1-5)</td>
				<td>Character</td>
				<td>30 characters</td>
			</tr>
			<tr>
				<td>Ship / Deliver</td>
				<td>Character</td>
				<td>16 characters</td>
			</tr>
			<tr>
				<td>Freight</td>
				<td>Character</td>
				<td>10 characters</td>
			</tr>
			<tr>
				<td>On Hold</td>
				<td>Character</td>
				<td>Y=On hold, N=Not on hold</td>
			</tr>
		</tbody>
	</table>
	
	<span class="recommendation">For additional formatting options, please consult the Pastel SDK help file.</span>
	

-  #### Lines

    A pipe-separated ("|") string that defines the lines of the document.  Each line has the following layout:

	<table>
		<thead>
			<tr>
				<th>Field</th>
				<th>Type</th>
				<th>Format</th>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td>Cost Price</td>
				<td>Numeric</td>
				<td>Unit cost price</td>
			</tr>
			<tr>
				<td>Quantity</td>
				<td>Numeric</td>
				<td>Line Quantity</td>
			</tr>
			<tr>
				<td>Unit Selling Price</td>
				<td>Numeric</td>
				<td>Exclusive selling price per unit</td>
			</tr>
			<tr>
				<td>Inclusive Price</td>
				<td>Numeric</td>
				<td>Inclusive selling price per unit</td>
			</tr>
			<tr>
				<td>Unit</td>
				<td>Character</td>
				<td>4 characters maximum</td>
			</tr>
			<tr>
				<td>Tax Type</td>
				<td>Character</td>
				<td>00-30</td>
			</tr>
			<tr>
				<td>Discount Type</td>
				<td>Character</td>
				<td>0=None, 1=Settlement, 2=Overall, 3=Both</td>
			</tr>
			<tr>
				<td>Discount Percentage</td>
				<td>Character</td>
				<td>4 characters, omit decimals, e.g. 12.5%=1250</td>
			</tr>
			<tr>
				<td>Code</td>
				<td>Character</td>
				<td>15 characters maximum, inventory or GL</td>
			</tr>
			<tr>
				<td>Description</td>
				<td>Character</td>
				<td>40 characters maximum</td>
			</tr>
			<tr>
				<td>Line Type</td>
				<td>Character</td>
				<td>4=Inventory, 6=GL, 7=Remarks</td>
			</tr>
			<tr>
				<td>Multi-store</td>
				<td>Character</td>
				<td>3 characters maximum</td>
			</tr>
			<tr>
				<td>Cost Code</td>
				<td>Character</td>
				<td>5 characters maximum</td>
			</tr>
		</tbody>
	</table>

	<span class="recommendation">For additional formatting options, please consult the Pastel SDK help file.</span>
	

Output
------

A string value that contains the result of a successful import operation.

Links
-----------

- [Pastel Website](https://www.pastel.co.za/)
- [Download Pastel SDK](https://www.sage.com/za/partners/developers/sage-pastel-accounting)


