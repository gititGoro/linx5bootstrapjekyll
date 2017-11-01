TextFileWrite
=============

The TextFileWrite function will save some content to a text file.

Properties
----------

-  #### File path

    The path to the file you wish to write to. You can also set this property to the FileHandle 
    of a [FileOpen](../FileOpen/) function when its 'Is text' option is checked.

-  #### Contents

    The data to be written to file.

-  #### Destination codepage

    Code page is another term for character encoding. It consists of a
    table of values that describes the character set for a particular
    language.

    The default Codepage is the codepage used in your operating system.

-  #### File does not exist

    You can opt to create the file or return an error if the file
    already exists.

-  #### File exists

    If the file already exists, you can opt to

    *Append data* will find the end of the file and add the data there.

    *Increment file name* will add a number to the filename to make that
    name unique. This is a sequential number starting with 1.

    *Overwrite file* will replace all content in the file with the new
    content.

    *Throw exception* will stop the process and return an error.


Video
-----
[Linx 5: Text File Read and Write](https://www.youtube.com/watch?v=6ZUXNNL8wYg)


Links
-----

[Wikipedia: Code page](http://en.wikipedia.org/wiki/Code_page)

[Joel on Character Sets](http://www.joelonsoftware.com/articles/Unicode.html)

[Go Global Developer Center: Code pages](http://msdn.microsoft.com/en-us/goglobal/bb964653.aspx)
