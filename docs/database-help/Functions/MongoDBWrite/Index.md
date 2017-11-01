MongoDBWrite
============

MongoDBWrite performs write operations on a Mongo database.

Properties
----------

-  #### Collection

    The collection to write to.

-  #### Connection string

    A [connection string](http://docs.mongodb.org/manual/reference/connection-string/) to your database.

-  #### Operation

    The operation to perform.

    1.  <a id="operations-insert"></a>*Insert*  
        Inserts a single object (specified in the
        [data](#properties-data) property) into the database.
    2.  <a id="operations-update"></a>*Update*  
        Applies an [update operation](#properties-updateOperation) to
        any documents in the database that matches the
        [criteria](#properties-criteria) property.
    3.  <a id="operations-replace"></a>*Replace*  
        Replaces a single document in the database with the object in
        the [data](#properties-data) property. The document to be
        replaced can either be specified by the
        [criteria](#properties-criteria) property, or by the [ID
        attribute](#idattribute) of the replacement document.
    4.  <a id="operations-delete"></a>*Delete*  
        Deletes one or more documents in the database which match the
        [criteria](#properties-criteria) property
    5.  <a id="operations-deleteall"></a>*Delete All*  
        Deletes all records in the collection.


-  #### Data{#properties-data}

    The object to be written into the database. The object can be
    specified either as a JSON formatted string, or as a instance of a
    [custom type](https://linx.software/plugins/BuiltIn/Types/CustomType/).

    Required for [insert](#operations-insert),
    [update](#operations-update), and [replace](#operations-replace)
    operations.

-  #### Criteria{#properties-criteria}

    A MongoDb [query document](http://docs.mongodb.org/manual/tutorial/query-documents/) that 
    specifies the selection criteria to be matched by a document in order for
    it to have the write operation applied.

    For example, if we select from collection of documents with attributes
    "quantity" and "price", the criteria:
    
        { 
            $or: [
                    { quantity: { $gt: 100 } },
                    { price: { $lt: 9.95 } }
                ]
        }

    will match all documents in the collection with fields "quantity"
    greater than 100 or "price" less than 9.95.

    Required for [update](#operations-update),
    [replace](#operations-replace) and [delete](#operations-delete)
    operations.

    An editor is provided to add database fields and variables to the criteria.

-  #### Insert If Not Found

    Controls the action of [replace](#operations-replace) and
    [update](#operations-update) operations, when no documents are found
    matching the [criteria](#properties-criteria) property.

    1.  Checked  
         A new document will be created.
    2.  Unchecked  
         No action will be taken.

-  #### Update Operation{#properties-updateOperation}

    A JSON formatted object containing one or more [update
    operators](#links-updateOperators) specifying operations to be performed
    on the matched documents. For example:

        {
            $set: { updated: true },
            $inc: { count: 1 }
        }

    will set an "updated" field to "true' and increment a "count" field for
    all matched documents.

    An editor is provided to add database fields and variables to the update operation.

ID Attributes{#idattribute}
-------------

Each document in a MongoDb collection is uniquely identified by an [Object
ID](http://docs.mongodb.org/manual/reference/object-id/) field called "\_id". This field is automatically
created whenever a new document is inserted into the database, and
cannot be changed by [update](#operations-update) or
[replace](#operations-replace) operations.

When using the object id as an element in the [criteria](#criteria)
field, the [string](https://linx.software/plugins/BuiltIn/Types/String/) value will need to be converted back
to a Mongo object id. For example, a document with id
"525bc19331eec126ecdcf199" can be matched by using the
[criteria](#criteria):

    { 
        _id: ObjectId("525bc19331eec126ecdcf199")
    }     

When running a [replace](#operations-replace) operation without
specifying any criteria, the function will attempt to replace a document
in the database whose id matches a [string](https://linx.software/plugins/BuiltIn/Types/String/) field named
"id" or "\_id" in the replacement object. This allows objects to be read
from the database using the MongoDBRead function, altered and written
back to the database without requiring a replacement
[criteria](#properties-criteria).

<hr>
Links
-------------

- [Connection string format](http://docs.mongodb.org/manual/reference/connection-string/)
- [Aggregation pipeline](http://docs.mongodb.org/manual/core/aggregation-pipeline/)  
- [Aggregation operators](http://docs.mongodb.org/manual/reference/operator/aggregation/#aggregation-pipeline-operator-reference)
- [Query documents](http://docs.mongodb.org/manual/tutorial/query-documents/)  
- [Update Operators](http://docs.mongodb.org/manual/reference/operator/update/#id1)  
- [Mongo Object Ids](http://docs.mongodb.org/manual/reference/object-id/)  

