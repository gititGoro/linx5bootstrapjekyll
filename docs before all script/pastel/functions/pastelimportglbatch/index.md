---
layout: docs
title: pastelimportglbatch
description: pastelimportglbatch
group: pastel
feature: functions
component: pastelimportglbatch
toc: true
---
PastelImportGLBatch
====================

<span class="recommendation">In order to use this function, the necessary Pastel SDK files have to be installed on the host computer.</span>

Import a general ledger or cashbook batch into Pastel.

Properties
----------

-  #### Data path

    The location of the Pastel data files.

-  #### GL path

    The location of the Pastel program files.

-  #### User id

	An optional user id to send with the batch import.

-  #### Entry type number

	The entry type number in Pastel.

-  #### Import string

	A pipe-separated ("|") string that defines the batch content.  The import string must use the following layout:

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
				<td>Period</td>
				<td>Numeric</td>
				<td>1-13</td>
			</tr>
			<tr>
				<td>Date</td>
				<td>Character</td>
				<td>DD/MM/YYYY</td>
			</tr>
			<tr>
				<td>GDC</td>
				<td>Character</td>
				<td>G=General Ledger, D=Customer, C=Supplier.</td>
			</tr>
			<tr>
				<td>Account Number</td>
				<td>Character</td>
				<td>7 characters e.g. "600000"</td>
			</tr>
			<tr>
				<td>Reference</td>
				<td>Character</td>
				<td>8 characters</td>
			</tr>
			<tr>
				<td>Description</td>
				<td>Character</td>
				<td>36 characters maximum</td>
			</tr>
			<tr>
				<td>Amount</td>
				<td>Numeric</td>
				<td>15.2 -ve=CR, +ve=DR</td>
			</tr>
			<tr>
				<td>Tax Type</td>
				<td>Numeric</td>
				<td>0-30, 0=no tax</td>
			</tr>
			<tr>
				<td>Tax Amount</td>
				<td>Numeric</td>
				<td>15.2 -ve=CR, +ve=DR</td>
			</tr>
			<tr>
				<td>Open Item Type</td>
				<td>Character</td>
				<td>See note below</td>
			</tr>
			<tr>
				<td>Job Code</td>
				<td>Character</td>
				<td>5 characters</td>
			</tr>
			<tr>
				<td>Matching Reference</td>
				<td>Character</td>
				<td>8 characters</td>
			</tr>
			<tr>
				<td>Discount Amount</td>
				<td>Numeric</td>
				<td>15.2 -ve=CR, +ve=DR</td>
			</tr>
			<tr>
				<td>Discount Tax Type</td>
				<td>Numeric</td>
				<td>0-30, 0=No tax</td>
			</tr>
			<tr>
				<td>Contra Account</td>
				<td>Character</td>
				<td>7 characters e.g. "600000"</td>
			</tr>
			<tr>
				<td>Exchange Rate</td>
				<td>Numeric</td>
				<td>Customer or supplier exchange rate</td>
			</tr>
			<tr>
				<td>Bank Exchange Rate</td>
				<td>Numeric</td>
				<td>Bank exchange rate</td>
			</tr>
		</tbody>
	</table>

	<span>Note: If there is a matching reference and the transaction is an allocation, Pastel finds the original document for the relevant customer or supplier, and allocates the transaction to that document. If the matching reference is blank, Pastel leaves it as unallocated.</span>

	<span class="recommendation">For additional formatting options, please consult the Pastel SDK help file.</span>
	

Output
------

A string value that contains the result of a successful import operation.

Links
-----------

- [Pastel Website](https://www.pastel.co.za/)
- [Download Pastel SDK](https://www.sage.com/za/partners/developers/sage-pastel-accounting)


