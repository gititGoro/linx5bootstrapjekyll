XMLPoke
=======

The XMLPoke function writes to one or more XML nodes in an XML Document.

Properties
----------

-  #### XML source{#properties-xmlSource}

    XML source controls where the function reads the input XML from, and
    can be one of the following values:

    1.  File  
        Reads from the XML file defined in the [XML
        file](#properties-xmlFile) property;
    2.  String  
        Reads from an XML formatted string defined in the [XML
        string](#properties-xmlString) property.
<p>
-  #### XML file{#properties-xmlFile}

    The path to an XML file which will be read by the function.  
     This property is only visible if [XML
    source](#properties-xmlSource) is set to "File".

-  #### XML string{#properties-xmlString}

    The XML formatted string to read.  
     This property is only visible if [XML
    source](#properties-xmlSource) is set to "String".

-  #### XPath{#properties-xPath}

    XPath is a string conforming to the [XML path
    language](#links-xPath) format. It is used to select which nodes are
    read by the function.

-  #### Namespaces{#properties-namespaces}

    An optional string defining any [XML
    namespaces](#links-xmlNamespaces) for prefixes used in the
    [XPath](#properties-xPath) property.

    Namespaces should be defined in the format [prefix]="[URI]".
    Multiple definitions should be separated by a space.

-  #### Return XML

    If "Return XML" is set, the updated XML string will be provided as
    the function's output.

-  #### Update File

    If "Update File" is set, the changes made by this function will be
    written back into the [XML file](#properties-xmlFile).  
     This option is available only when [XML
    source](#properties-xmlSource) is set to "File".

Links
-----

[Wikipedia: XML](http://en.wikipedia.org/wiki/XML)

<a id="links-xPath" />[Wikipedia: XML Path Language](http://en.wikipedia.org/wiki/XPath)

<a id="links-xmlNamespaces" />[Wikipedia: XML Namespaces](http://en.wikipedia.org/wiki/XML_Namespace)
