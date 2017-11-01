MongoDBRead
===========

MongoDBRead performs read operations on a Mongo database.

<span class="recommendation">Use the 'loop results' function of this
function for large datasets!</span>

Properties
----------

-  #### Aggregation Pipeline

    An optional JSON formatted array, containing the [aggregation
    pipeline](#aggregationPipelineDetails) used to perform aggregation
    operations on documents returned from the database.

-  #### Collection

    The collection to read from.

-  #### Connection string

    A [connection string](#connectionString) to your database.

-  #### Criteria

    A JSON formatted [query object](#queryDetails) which specifies the
    selection criteria needed to be matched by a document in order to be
    returned from the collection. If no criteria is specified, all
    documents in the collection will be read.

-  #### Loop results

    1.  Checked  
         The function will automatically return one item at a time. You
        will see a "ForEachObject" loop icon as a child of this
        function. Any function you attach to the results will be inside
        of the loop. This is recommended for all operations where you
        don't need the complete list of items all at once.
    2.  Unchecked  
         The function will return all items in one table. You can then
        use that table anywhere in the process as a table. You can also
        pass it to a loop function later and loop through the records
        then.
<p>
-  #### Return type

    The [custom type](~/Support/BuiltIn/Types/CustomType/) that will be used to store the
    results from the database. The names of the fields in the custom
    type should match the field names of the documents returned from the
    Mongo database.

    The custom type can optionally include a [string](~/Support/BuiltIn/Types/String/)
    field named "id" which will be populated by the document's unique
    [id attribute](#idattribute).

Aggregation Pipeline
--------------------

The aggregation pipeline should be specified as a JSON formatted array
of [aggregation operation](#aggregationOperations) objects.

For example, if we have a collection of "People" documents with
attributes "age" and "name", the aggregation pipeline:<pre>
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
</pre>will sort the documents in the collection by age, and group the results
into a single document with attributes "total" (containing the total
number of records in the collection) and "eldest" (the "name" attribute
of the document with the greatest "age").

More information on Mongo's aggregation pipeline and operations can be
found in Mongo's [aggregation documentation.](#aggregationPipeline)

When used in conjunction with the [criteria](#criteria) property,
results from the database are first filtered to match the criteria,
before being sent to the aggregation pipeline.

Query Object
------------

The [criteria](#criteria) field should be specified as a JSON
representation of a mongo query document. The format required is
documented in Mongo's [query document documentation](#queryDocument).

For example, if we select from collection of documents with attributes
"quantity" and "price", the criteria:<pre>
{ 
    $or: [
            { quantity: { $gt: 100 } },
            { price: { $lt: 9.95 } }
        ]
}
</pre>will return all documents in the collection with fields "quantity"
greater than 100 or "price" less than 9.95.

ID Attributes
-------------

Each document in a Mongo collection is uniquely identified by an [Object
ID](#links.mongoId) field called "\_id". When reading from the database,
it is possible to retrieve this id by adding a [string](~/Support/BuiltIn/Types/String/)
field named "id" or "\_id" to the custom type specified in the [return
type](#ReturnType) property.

When using the object id as an element in the [criteria](#criteria)
field, the [string](~/Support/BuiltIn/Types/String/) value will need to be converted back
into a mongo object id. For example, a document with id
"525bc19331eec126ecdcf199" can be retrieved using the
[criteria](#criteria):

            { 
                _id: ObjectId("525bc19331eec126ecdcf199")
            }
        

#### Links

Connection string format:
<http://docs.mongodb.org/manual/reference/connection-string/>  
 Aggregation pipeline:
<http://docs.mongodb.org/manual/core/aggregation-pipeline/>  
 Aggregation operators:
<http://docs.mongodb.org/manual/reference/operator/aggregation/#aggregation-pipeline-operator-reference>  
 Query documents:
<http://docs.mongodb.org/manual/tutorial/query-documents/>  
 Object Ids: <http://docs.mongodb.org/manual/reference/object-id/>  

