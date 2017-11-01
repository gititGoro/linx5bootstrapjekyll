TextFileRead
============

<span class="recommendation">Use this function to read delimited files,
such as CSV files.</span>

The TextFileRead function reads the contents of text files. It can return delimited
file contents field-by-field.

Properties
----------

-  #### File path

    The path to the file to be read.

-  #### Codepage

    Code page is another term for character encoding. It consists of a
    table of values that describes the character set for a particular
    language.

    The default Codepage is the codepage used in your operating system.

-  #### Read type

    Three options are available for reading the file contents.

    *Line by line* returns each line contents in a loop. If you have
    defined fields, each field is returned by the function and can be
    accessed in the loop.

    *List of lines* returns the entire file, but as a list. Each list
    member represents one line from the file. If you use a
    [ForEach](~/Support/BuiltIn/Functions/ForEach/) function to loop the list, you can access all
    fields separately in the loop.

    *Complete* reads the entire file and returns it as one string.

-  #### Fields

    This property allows for importing of delimited files. You can
    define the fields manually or by importing a sample file.

    If you define fields, the function can return each field separately.

-  #### Skip header lines

    The number of header lines to ignore in the file.

-  #### Skip footer lines

    The number of footer lines to ignore in the file.

Video
-----
[Linx 5: Text File Read and Write](https://www.youtube.com/watch?v=6ZUXNNL8wYg)


Links
-----

<http://en.wikipedia.org/wiki/Code_page>

<http://msdn.microsoft.com/en-us/goglobal/bb964653.aspx>
