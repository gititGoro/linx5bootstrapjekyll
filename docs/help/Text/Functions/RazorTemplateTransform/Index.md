RazorTemplateTransform
======================

Razor is a template markup syntax based on the C\# programming language.The RazorTemplateTransform function allows for creating HTML from templates.
It is used by ASP.NET MVC.  

Properties
----------

-  #### Encoding

    Describes what to do with text written to the template from the
    model.

    1.  ##### Raw

        Output the text as-is.

    2.  ##### Html

        Encode the text to html.

-  #### Model

    The data that is accessed by the template. Use @Model in the
    template to access the properties of the structured object
    referenced in this property.

-  #### Template

    The Razor template string.

Links
-----

[Wikipedia
Razor](http://en.wikipedia.org/wiki/ASP.NET_Razor_view_engine)

["Razor syntax quick
reference"](http://haacked.com/archive/2011/01/06/razor-syntax-quick-reference.aspx)
