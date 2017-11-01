---
layout: docs
title: MongoDBMapReduce
description: MongoDBMapReduce
group: database
feature: Functions
component: MongoDBMapReduce
toc: true
redirect_from: docs/database/Functions/MongoDBMapReduce/index
---
MongoDBMapReduce
===========

MongoDBMapReduce performs map-reduce style data aggregation on a Mongo database.

<span class="recommendation">Use 'Row by row' under 'Return options' for large datasets.</span>

Properties
----------

-  #### Collection

    The collection to read from.

-  #### Connection string

    A [connection string](http://docs.mongodb.org/manual/reference/connection-string/) to your database.

-  #### Query{#query}

    An optional mongo query document. For example, if we select from collection of documents with attributes
    "quantity" and "price", the criteria:
    
        { 
            $or: [
                    { quantity: { $gt: 100 } },
                    { price: { $lt: 9.95 } }
                ]
        }

    will return all documents in the collection with fields "quantity"
    greater than 100 or "price" less than 9.95.

    An editor is provided where the user can select database fields and variables to be added to the query.

    More information on query documents can be found [here](http://docs.mongodb.org/manual/tutorial/query-documents/).

-  #### Sort

    An optional property which sorts the input documents.This option is useful for optimization. For example, specify the sort key to be the same as the emit key so that there are fewer reduce operations. The sort key must be in an existing index for this collection.

-  #### Limit

    An optional property which specifies a maximum number of documents for the input into the map function.

-  #### Map

    A JavaScript function that associates or maps a value with a key and emits the key and value pair. The map function is responsible for transforming each input document into zero or more documents.
    It can access the variables defined in the scope parameter and has the following prototype:

        function() {
           ...
           emit(key, value);
        }

    For example, the following function associates the sku with a new object value that contains the count of 1 and the item qty for the order and emits the sku and value pair, for each item:

        function() {
	        for (var idx = 0; idx < this.items.length; idx++) {
		        var key = this.items[idx].sku;
		        var value = {
						        count: 1,
						        qty: this.items[idx].qty
					        };
		        emit(key, value);
	        }
        };

    When used in conjunction with the [Query](#query) property,
    results from the database are first filtered to match the criteria,
    before being sent to the map-reduce function.

    An editor is provided to add database fields and variables to the map.

-  #### Reduce
    	
    A JavaScript function that reduces to a single object all the values associated with a 
    particular key. The reduce function emits key-value pairs. For those keys that have multiple values, 
    MongoDB applies the reduce phase, which collects and condenses the aggregated data. 

    The reduce function has the following prototype:

        function(key, values) {
           ...
           return result;
        }

    For example, the following function reduces the countObjVals array to a single object reducedValue that contains the count and the qty fields. In reducedVal, the count field contains the sum of the count fields from the individual array elements, and the qty field contains the sum of the qty fields from the individual array elements:

         function(keySKU, countObjVals) {
	        reducedVal = { count: 0, qty: 0 };

	        for (var idx = 0; idx < countObjVals.length; idx++) {
		        reducedVal.count += countObjVals[idx].count;
		        reducedVal.qty += countObjVals[idx].qty;
	        }

	        return reducedVal;
        };

    An editor is provided to add database fields and variables to the reduction.

-  #### Finalize

    An optional JavaScript function that finalizes the output by modifying the output of Reduce.

    The finalize operation uses a custom JavaScript function to further condense or process the results of the aggregation. 
    It has the following prototype:

        function(key, reducedValue) {
           ...
           return modifiedObject;
        }

    The finalize function receives as its arguments a key value and the reducedValue from the reduce function. For example, the following function modifies the reducedVal object to add a computed field named avg and returns the modified object:

        function (key, reducedVal) {

	        reducedVal.avg = reducedVal.qty/reducedVal.count;

	        return reducedVal;

        };

    More information on MongoDb's map-reduce operations can be
    found [here](https://docs.mongodb.com/manual/reference/method/db.collection.mapReduce/).

    An editor is provided where the user can select database fields and variables to be added to the finalization criteria.


-  #### Output type

    The [custom type](https://linx.software/plugins/BuiltIn/Types/CustomType/) that will be used to store the
    results from the database. The names of the fields in the custom
    type should match the field names of the documents returned from the
    Mongo database.

    The custom type can optionally include a [string](https://linx.software/plugins/BuiltIn/Types/String/)
    field named "id" which will be populated by the document's unique
    [id attribute](#idattribute).

- #### Return options

    - *First row*  
        The function will return the first document returned by the query. If
        no data is returned by the query, an error will be reported.
    - *First row, else empty row*  
        The function will return the first document returned by the query. If
        no data is returned by the query, the function will return a row
        containing default values.
    - *List of rows*  
        The function will return all documents in one list. The list can then
        be used later in the process without having to execute the query
        again.
    - *Row by row*  
        The function will automatically return one document at a time. You
        will see a "ForEachRow" loop icon as a child of this function.
        Any function you attach to the results will be inside of the
        loop. This is recommended whenever you expect to retrieve
        multiple items, but you don't need the complete list of items
        all at once.

ID Attributes{#idattribute}
-------------

Each document in a Mongo collection is uniquely identified by an Object
ID field called "\_id". When reading from the database, it is possible to retrieve this id by adding a [string](https://linx.software/plugins/BuiltIn/Types/String/) field named "id" or "\_id" to the custom type specified in the [return type](#ReturnType) property.

When using the object id as an element in the [query](#query)
proeprty, the [string](https://linx.software/plugins/BuiltIn/Types/String/) value will need to be converted back
into a mongo object id. For example, a document with id
"525bc19331eec126ecdcf199" can be retrieved using the query:

        { 
            _id: ObjectId("525bc19331eec126ecdcf199")
        }

<hr>
Links
-------------

- [Connection string format](http://docs.mongodb.org/manual/reference/connection-string/)
- [Map-Reduce](https://docs.mongodb.com/manual/reference/method/db.collection.mapReduce/)
- [Query documents](http://docs.mongodb.org/manual/tutorial/query-documents/)
- [Object Ids](http://docs.mongodb.org/manual/reference/object-id/)  
