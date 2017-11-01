ReadPDF
=======

ReadPDF reads text, signatures and form data from Pdf documents.

<span class="recommendation"> All passwords entered into this function's
properties will be visible in plain text in the Linx solution file and
in the compiled .Net assembly. </span>

Properties
----------

-  #### PDF filename

    Path to the PDF file to read.

-  #### Authentication type

    Pdf files can be protected using a password or a certificate, this
    field indicates which type of authentication to attempt when loading
    the PDF.

    1.  None  
        The document is unprotected and can be opened immediately.
    2.  Password  
        The document is protected by a password.
    3.  Certificate  
        The document is protected by a certificate.
<p>
-  #### PDF password

    Password required to access the PDF file. (Visible when using
    "Password" authentication type.)

-  #### Certificate source

    Source to load the certificate from. (Visible when using
    "Certificate" authentication type.)

    1.  File  
        Load a certificate from a .pfx file.
    2.  Store  
        Load a certificate from the windows certificate store.
<p>
-  #### Certificate file path

    Path to a .pfx file containing a certificate. (Visible when using
    "File" as the certificate source.)

-  #### Certificate file password (Optional)

    The password required to open the .pfx file. (Visible when using
    "File" as the certificate source.)

-  #### Stored certificate

    Certificate loaded from the windows certificate store. (Visible when
    using "Store" as the certificate source.)

-  #### Read text

    Reads the document text, and returns it an output parameter named
    "Text".

-  #### Split text

    Controls how the document text is split. (Visible when "Read text"
    is checked.)

    1.  Never  
        Text is never split, all text in the document is returned as a
        single string value.
    2.  Page  
        Text is split per page, and returned in a list or strings with
        one entry per page.
<p>
- #### Read form data

    Reads any form data preset in the document.

- #### Return form data as

    Controls how the form data is returned.

    1.  Custom type  
        Form data is used to populate an existing custom type.
    2.  Infer type from a sample PDF  
        Return type is constructed based on a sample PDF document.
    3.  List  
        Form data is returned as a list of entries.
<p>
- #### Form data type

    The type used to hold the document's form data. (Visible when
    returning form data as a custom type.)

- #### Sample PDF

    A sample PDF containing the empty form.

- #### Read signatures

    Reads any signatures present in the document.

Output
------

#### Text  
The document text. Either a list or a string depending on the value of
"Split text" property.

#### FormData  
Contents of the document form. Either an object or a list of values
depending on the value of "Read form data as".

#### Signatures

The signatures present in the document.

-  IsSigned  
    True if the document contains one or more signatures.
-  LatestSignature  
     The most recent signature in the document.
    1.  SignedBy  
        Name of the person who placed the signature.
    2.  SignedAt  
        Location where the document was signed.
    3.  Reason  
        Reason for signing.
    4.  SignedOn  
        Date of signing
    5.  Unmodified  
        True if the signed revision has not been altered.
    6.  SignedRevision  
        The revision number signed.
    7.  IsLatestRevision  
        No further revisions have been made since signing.


