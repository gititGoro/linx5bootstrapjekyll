---
layout: docs
title: executesql
description: executesql
group: database
feature: functions
component: executesql
toc: true
---
ExecuteSQL
==========

<span class="recommendation">Use the "Row by row" option of this
function's Return options when retrieving large datasets!</span>

ExecuteSQL executes a SQL query and returns data where applicable. The
query can contain any SQL and calls supported by the database driver,
including stored procedures. If the return data is structured as a table
then the function can optionally loop through the table and return the data row by row.

Properties
----------

- #### Connection type {#connectiontype-property}
The type of database driver to use to connect to the database. The supported driver types are *SQL Server*, 
*Oracle*, *OLE DB*, and *ODBC*. Alternatively, instead of creating a new connection, choose *Use transaction* 
to use a Transaction object from a [BeginTransaction](../BeginTransaction/) function.

- #### Connection string
The [connection string](../../Tools/ConnectionEditor/) that specifies how to connect to the database.

- #### Transaction
Visible if [Connection type](#connectiontype-property) is set to *Use transaction*. Allows to select the 
Transaction object from a [BeginTransaction](../BeginTransaction/) function.

- #### SQL
The query or command to execute. The query can contain any SQL and
calls supported by the database driver, including stored procedures.
You can write SQL statements using the SQLEditor

- #### Timeout
The timeout value for the query in seconds.

- #### Return options

    - *First row*  
        The function will return the first row returned by the query. If
        no data is returned by the query, an error will be reported.
    - *First row, else empty row*  
        The function will return the first row returned by the query. If
        no data is returned by the query, the function will return a row
        containing default values.
    - *List of rows*  
        The function will return all rows in one list. The list can then
        be used later in the process without having to execute the query
        again.
    - *Row by row*  
        The function will automatically return one row at a time. You
        will see a "ForEachRow" loop icon as a child of this function.
        Any function you attach to the results will be inside of the
        loop. This is recommended whenever you expect to retrieve
        multiple items, but you don't need the complete list of items
        all at once.

- #### [Return type](#ReturnType)
The columns the query returns as well as their data types.

    If the Linx Designer has access to the database and you do not want
to change any type or column name, you can ignore this property. The
values will be populated automatically from your SQL query for you.

    If the Linx Designer does not have a connection to the database or
does not have a query to run at design time, you need to define the
query output columns using this property manually.

Return Type{#ReturnType}
-----------

You can also create a [custom type](https://linx.software/plugins/BuiltIn/Types/CustomType/) and assign an
entire query output to that type. If you create a *Person* type, for
example, you can select data for a person from the database and assign
them to the object.

Links
-----------

- [Wikipedia: Connection string](http://en.wikipedia.org/wiki/Connection_string)
- [Wikipedia: Database connection](http://en.wikipedia.org/wiki/Database_connection)
- [The Connection Strings Reference](http://www.connectionstrings.com)

Videos
-----------

[Linx 5: How to use the Linx 5 Execute SQL Control](https://www.youtube.com/watch?v=pR_C2ync8sI)
