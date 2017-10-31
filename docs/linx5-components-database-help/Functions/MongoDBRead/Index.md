MongoDBRead
===========

MongoDBRead performs read operations on a Mongo database.

<span class="recommendation">Use 'Row by row' under 'Return options' for large datasets.</span>

Properties
----------

-  #### Collection

    The collection to read from.

-  #### Connection string

    A [connection string](http://docs.mongodb.org/manual/reference/connection-string/) to your database.

- #### Fields
  
  Specifies which fields to return. Format is

        { field1: value, field2: value ... }

  where field = the field name and value = 1 to include and 0 to exclude. 
  The parameter contains either include or exclude specifications, not both, 
  unless the exclude is for the _id field.

- #### Limit
  
    An integer indicating the maximum number of documents to return.
  
- #### Pipelines

    Available when Operation = Aggregate. 
    
    The aggregation pipelines should be specified as a JSON formatted array of aggregation objects.

    For example, if we have a collection of "People" documents with
    attributes "age" and "name", the aggregation pipeline:

        [
            { 
                $sort: { age: -1 } 
            },
            { 
                $group: {
                    total: { $sum: 1 }, 
                    eldest: { $first: "$name"} 
                }
            },
        ]

    will sort the documents in the collection by age, and group the results
    into a single document with attributes "total" (containing the total
    number of records in the collection) and "eldest" (the "name" attribute
    of the document with the greatest "age").

    More information on MongoDb's aggregation pipeline and operations can be
    found [here](http://docs.mongodb.org/manual/reference/operator/aggregation/#aggregation-pipeline-operator-reference).

    An editor is provided to add database fields and variables to the aggregation pipeline.

-  #### Query{#criteria}

    A MongoDb query document. For example, if we select from a collection of documents with attributes
    "quantity" and "price", the criteria:

        { 
            $or: [
                    { quantity: { $gt: 100 } },
                    { price: { $lt: 9.95 } }
                ]
        }

    will return all documents in the collection with fields "quantity"
    greater than 100 or "price" less than 9.95.
    
    More information can be found [here](http://docs.mongodb.org/manual/tutorial/query-documents/).

- #### Skip

    An integer indicating how many documents to skip before returning results.

- #### Sort
  
    Specifies the order in which the query returns matching documents. Format is

        { field1: value, field2: value ... }

  where field = the field name and value = 1 to sort ascending and -1 to sort descending. 

- #### Operation
  
  - *Find*
    Selects documents in a collection.

  - *Aggregate*
    Calculates aggregate values for the data in a collection.

-  #### Output type

    The [custom type](https://linx.software/plugins/BuiltIn/Types/CustomType/) that will be used to store the
    results from the database. The names of the fields in the custom
    type should match the field names of the documents returned from MongoDb.

    The custom type can optionally include a [string](https://linx.software/plugins/BuiltIn/Types/String/)
    field named "id" which will be populated by the document's unique
    [id attribute](#idattribute).

- #### Return options

    - *First row*  
        The function will return the first document returned by the query. If
        no data is returned by the query, an error will be reported.
    - *First row, else empty row*  
        The function will return the first document returned by the query. If
        no data is returned by the query, the function will return a document
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

When using the object id as an element in the Query
property, the [string](https://linx.software/plugins/BuiltIn/Types/String/) value will need to be converted back
into a mongo object id. For example, a document with id
"525bc19331eec126ecdcf199" can be retrieved using the
query:

    { 
        _id: ObjectId("525bc19331eec126ecdcf199")
    }

<hr>
Links
-------------

- [Connection string format](http://docs.mongodb.org/manual/reference/connection-string/)
- [Aggregation pipeline](http://docs.mongodb.org/manual/core/aggregation-pipeline/)
- [Aggregation operators](http://docs.mongodb.org/manual/reference/operator/aggregation/#aggregation-pipeline-operator-reference)
- [Query documents](http://docs.mongodb.org/manual/tutorial/query-documents/)
- [Object Ids](http://docs.mongodb.org/manual/reference/object-id/)  

