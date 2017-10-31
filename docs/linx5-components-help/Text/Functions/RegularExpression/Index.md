Regular Expression
=================

A Regular Expression is a sequence of characters that forms a search
pattern. Regular Expressions are very similar across all programming
languages.

Properties
----------

-  #### Action

    The function will always return the *index* and the *length* of
    found matches.

    *First match* only returns the information for the first match
    found.

    *All matches* returns a list of all matches found.

    *Replace* will use the 'Replace with' field to replace content
    matching the pattern.

    *Split* will split the string at the matched pattern. The matched
    pattern is not returned.

    If you are, for example, looking for a comma to split a string (1,2)
    by, then a list containing two rows is returned. The comma will not
    be returned.

-  #### Expression

    The search pattern to apply to the text. See some
    [examples](http://regexlib.com/)

-  #### Input string

    The string to apply the pattern to.

-  #### Options

    A set of standard Regular Expression options to pick from. Here is a
    [detailed
    description](http://msdn.microsoft.com/en-us/library/yd1hzczs.aspx)
    of these options.

-  #### Replace with

    A string to replace matched content with.

-  #### Loop results

    A flag to indicate if lists returned by the function should be
    looped or provided as a 'list object'.

Links
-----

<http://en.wikipedia.org/wiki/Regular_expression>

<http://www.regular-expressions.info/reference.html>

<http://regexlib.com/CheatSheet.aspx>

<http://gskinner.com/RegExr/>
