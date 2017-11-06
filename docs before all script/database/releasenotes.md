---
layout: docs
title: releasenotes
description: Release Notes
group: database
toc: true
---
# Release Notes for Database plugin
<a id="1_12_100_660"></a>
## 1.12.100.660
- Fix object reference error when adding output field.

<a id="1_11_98_655"></a>
## 1.11.98.655
- Fix SqlBulkCopy does not show Loader.Write properties.
- Add Sql script generation to SqlEditor.
- Update Linx Plugin library.
- Update Oracle database client.
- Update UI styles.
- Improve SqlEditor drop zone.
- Improve MongoDb editors.
- Fix ExecuteSQL field mapping error.
- Fix ExecuteSQL output field creation when using temp table.
- Fix ODBC insert query adds record when connectionstring/query changes.
- Fix ExecuteSql Connection Editor overwrites properties.
- Fix Cannot assign String to ExecuteSQL.ConnectionString.

<a id="1_10_76_559"></a>
## 1.10.76.559
- Fix DBBulkCopy fails silently on exception.
- Update Linx plugin
- SqlEditor persists last size and layout.
- Allow MongoDbRead and MongoDbMapReduce to return string output.
- Fix ExecuteSql insert null fails.
- Improve SqlEditor UI.
- Persist size and position of custom editors between sessions.
- Fix ExecuteStoredProcedure error when returning binary.
- Fix duplicate parameters in connection string error.
- Fix SqlBulkCopy shows no properties when setting Write property.
<a id="1_9_66_521"></a>
## 1.9.66.521
- Fix ExecuteSql properties load when hidden.
- Fix DBBulkCopy fails silently on exception.
<a id="1_8_63_514"></a>
## 1.8.63.514
- MongoDbRead: Replace Loop results with Return options.
- MongoDbMapReduce: Replace Loop results with Return options.
- Update Linx plugin.
<a id="1_7_60_504"></a>
## 1.7.60.504
- SQLEditor: Fix Fetch button border.
- Update NuGet packages.
- Update Linx plugin.
- Update UI styles.
- MongoDbRead: Split Find and Aggregate operations.
- Add MongoDBMapReduce function.
- MongoDBRead: Change editors.
- MongoDBMapReduce: Add editors.
- Update plugin utilities.
<a id="1_6_45_451"></a>
## 1.6.45.451
- Fix validation error for multiple missing custom type fields.
- Fix parameter name clash with properties.
<a id="1_5_42_443"></a>
## 1.5.42.443
- Update Linx Plugin.
- Add variable panel to SQLEditor.
- Fix duplicate variable in ODBC sql causes error.
<a id="1_4_35_419"></a>
## 1.4.35.419
- Update database drivers.
- Improve tab order on editors.
- Fix Insert Expression on Mongo Editors.
<a id="1_3_31_407"></a>
## 1.3.31.407
- Fix DbBulkCopy output not changed when table is changed.
- Update Linx Plugin.
- Changed tab order on controls.
<a id="1_3_25_387"></a>
## 1.3.25.387
- Display new connection string editor when cannot parse existing connection string successfully. 
- Fix "SET FMTONLY ON" scenario for stored procedures that contain dynamic SQL (replace default parameter values with null).
<a id="1_3_22_381"></a>
## 1.3.22.381
- Fix reader with blank column names cause index out of bounds exception.
<a id="1_3_20_377"></a>
## 1.3.20.377
- Update Linx plugin
- Fix object reference bug for invalid number of result sets.
- Fix duplicate parameter bug.
- Add logging.
- Resolve dynamic transaction connectionstring when queried as reference.
- DbBulkCopy: Add Timeout property. Default is 30 seconds.
- Fix ExecuteSql and StoredProcedure forces use of Oracle even if OLEDB is selected.
- Fix StoredProcedure shows user defined functions.
- Add CustomType option to ExecuteSQL Output Editor.
- ExecuteSQL ResultTypeEditor: Make drag-drop consistent with ExecuteStoredProcedure grids.
- ExecuteSQL ResultTypeEditor: Name column now defaults to value in ColumnName.
- Fix ExecuteSQL: CustomType does not save.
- Fix ExecuteSQL ResultTypeEditor: Validation.
- Fix ExecuteSQL: Validation on CustomType of result type.
- ExecuteStoredProcedure: Add validation on CustomTypes of result sets.
- ExecuteSQL and ExecuteStoredProcedure: Replaced result set custom type selector with TypeReferenceEditor.
- ExecuteSQL ResultTypeEditor: Use TypeReferenceEditor for field types.
- Add CustomType option to ExecuteSQL Output Editor
- Update ExecuteSQL function output from Int32 to Int64
<a id="1_3_3_327"></a>
## 1.3.3.327
- Update MongoDb drivers.
- ExecuteSQL to use Transaction.
- BeginTransaction: Test all isolation levels.
- BeginTransaction: Added icon for Transaction object.
- ExecuteStoredProcedure to use Transaction.
- Update Linx plugin.
- Fix bug: Database connection wizard stays open after update.
<a id="1_2_3_303"></a>
## 1.2.3.303
- ConnectionEditor: Add Wizard to OLEDB type.
- Update Linx Plugin API.
- ExecuteSQL: Replace ConnectionString with ConnectionType and ConnectionString to give finer grained control over which drivers to use.
- Sort output tables.
- Fix bug: String as enumerable conversion error.
- ExecuteSQL (OLEDB): Fix locale settings related conversion issue with floating point valued sql parameters.
- Add BeginTransaction Function.
<a id="1_1_48_273"></a>
## 1.1.48.273
- Add DbBulkCopy function
- Update Linx plugin
- ExecuteStoredProcedure: Detect connection type when changing connection string
- ExecuteStoredProcedure: Fixed crash when setting connection string to a variable 
- UI changes
<a id="1_0_37_241"></a>
## 1.0.37.241
- Add ExecuteSQL function
- Add ExecuteStoredProcedure function
- Add MongoDBRead function
- Add MongoDBWrite function
