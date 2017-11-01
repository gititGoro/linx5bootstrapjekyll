XMLReader
=========

XMLReader reads an [XML](http://en.wikipedia.org/wiki/XML)-formatted string from a
file or from a field and parses it into an object structure.

Properties
----------

-  #### XML source{#properties-xmlSource}

    Controls where the function reads the input XML from, and can be one
    of the following values:

    1.  File  
        Reads from the XML file defined in the [XML
        file](#properties-xmlFile) property;
    2.  String  
        Reads from an XML formatted string defined in the [XML
        string](#properties-xmlString) property.
<p>
-  #### XML file{#properties-xmlFile}

    The path to an XML file which will be read by the function.  
     This property is visible only when [XML
    source](#properties-xmlSource) is set to "File".

-  #### XML string{#properties-xmlString}

    The XML formatted string to read.  
     This property is visible only when [XML
    source](#properties-xmlSource) is set to "String".

-  #### Schema

    The XML schema describing the structure of the XML document. The
    schema will be used to build the object structure that the function
    will provide in its output.  
    The Schema editor can be used to derive a schema from a sample XML
    document.

Links
-----

[Wikipedia XML](http://en.wikipedia.org/wiki/XML)

[Sample XML
File](http://msdn.microsoft.com/en-us/library/ms762271(v=vs.85).aspx)

[Wikipedia XML schema](http://en.wikipedia.org/wiki/XML_schema)

[Wikipedia list of XML
schemas](http://en.wikipedia.org/wiki/List_of_XML_schemas)
