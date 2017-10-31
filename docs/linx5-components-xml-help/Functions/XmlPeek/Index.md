XMLPeek
=======

The XMLPeek function allows one or more XML nodes to be read from an XML
document.

Properties
----------

-  #### XML Source{#properties-xmlSource}

    XML source controls where the function reads the input XML from, and
    must be one of the following values:

    1.  File  
        Reads from the XML file defined in the [XML
        File](#properties-xmlFile) property;
    2.  String  
        Reads from an XML formatted string defined in the [XML
        String](#properties-xmlString) property.
<p>
-  #### XML File{#properties-xmlFile}

    The path to an XML file which will be read by the function.  
     This property is only visible if [XML
    Source](#properties-xmlSource) is set to "File".

-  #### XML String{#properties-xmlString}

    The XML formatted string to read.  
     This property is only visible if [XML
    Source](#properties-xmlSource) is set to "String".

-  #### XPath{#properties-xPath}

    XPath is a string conforming to the [XML path
    language](http://en.wikipedia.org/wiki/XPath) format. It is used to
    select which nodes are read by the function.

-  #### Namespaces

    An optional string defining any [XML
    namespaces](http://en.wikipedia.org/wiki/XML_Namespace) for prefixes
    used in the [XPath](#properties-xPath) property.

    Namespaces should be defined in the format [prefix]="[URI]".
    Multiple definitions should be separated by a space.

-  #### Output

    Output controls how the nodes matched by Xpath are returned from the
    functions. Output can be set to one of the following values:

    1.  Single node  
        The function will provide as output the contents of the first
        matched node.  
         If more than one node is matched by XPath, only the first one
        is returned.  
         If no matching nodes are found, an exception is thrown.
    2.  Single node or empty  
        The function will provide as output the contents of the first
        matched node, if one was found.  
         If more than one node is matched by XPath, only the first one
        is returned.  
         If no matching nodes are found, the output will contain an
        empty string.
    3.  List of nodes  
        The function will produce a list containing the contents of all
        matching nodes.
    4.  Loop nodes  
        The function will return the contents of each matched node one
        item at a time. You will see a "ForEachNode" loop icon as a
        child of this function. Any function added to the loop will be
        able to access the contents of the node in an output variable
        named "Node".

Links
-----

[Wikipedia: XML](http://en.wikipedia.org/wiki/XML)

[Wikipedia: XML Path Language](http://en.wikipedia.org/wiki/XPath)

[Wikipedia: XML Namespaces](http://en.wikipedia.org/wiki/XML_Namespace)
