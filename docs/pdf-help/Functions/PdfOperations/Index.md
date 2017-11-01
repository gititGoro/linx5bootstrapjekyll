PDFOperations
=============

PDFOperations modifies a PDF document

<span class="recommendation"> All passwords entered into this function's
properties will be visible in plain text in the Linx solution file and
in the compiled .Net assembly. </span>

Properties
----------

-  #### Operation

    The operation to perform on the document.

    1.  Fill form  
        Enters data into form fields in the document.
    2.  Change protection  
        Adds or removes protection to a document.
    3.  Split  
        Splits document pages into separate PDF files.
    4.  Concatenate  
        Joins pages from separate PDF files into a single document.
    5.  Add watermark  
        Superimposes a single page document onto another document.
    6.  Sign  
        Digitally sign a document.
<p>
### Input PDF

The follow properties specify the PDF document to be loaded and modified
by the operation:

-  #### Input PDF filename

    Path to the PDF file.

-  #### Input authentication type

    PDF files can be protected using a password or a certificate, this
    field indicates which type of authentication to attempt when loading
    the PDF.

    1.  None  
        The document is unprotected and can be opened immediately.
    2.  Password  
        The document is protected by a password.
    3.  Certificate  
        The document is protected by a certificate.

    <span class="recommendation"> Certificate protected files are not
    supported by the "Fill form", "Sign", "Add watermark" and
    "Concatenate" operations.  
     If you need to use the operations on a certificate protected
    document, first remove the certificate protection using the "Change
    protection" operation. </span> <span class="recommendation"> The
    certificate must include the private key in order to be able to load
    the document. </span>

-  #### Input PDF password

    Password required to access the PDF file.

-  #### Input certificate source

    Source to load the certificate from. (Visible when using
    "Certificate" authentication type.)

    1.  File  
        Load a certificate from a .pfx file.
    2.  Store  
        Load a certificate from the windows certificate store.
<p>
-  #### Input certificate file path

    Path to a .pfx file containing a certificate.

-  #### Input certificate file password

    Password needed to open the certificate file.

-  #### Input certificate

    Certificate loaded from the windows certificate store. (Visible when
    using "Store" as the certificate source.)

### Output PDF

The follow properties specify options for saving the PDF document after
it has been modified.

-  #### Output PDF filename

    Path to the PDF file to write to.

-  #### Output PDF protection

    Protection used to secure the output PDF.

    1.  None  
        No protection
    2.  Password  
        The PDF will ask for a password before opening
    3.  Certificate  
        The PDF will only be readable by the owner of the specified
        certificate
<p>
-  #### Owner password

    Password to gain owner-level access to the PDF (Visible if the "PDF
    protection" property is set to "Password")

-  #### Certificate source

    Source to load the owner's certificate. (Visible if the "PDF
    protection" property is set to "Certificate")

    1.  File  
        Load the certificate from a .pfx file.
    2.  Store  
        Load the certificate from the windows key store.
<p>
-  #### Certificate file path

    Path to a .pfx file containing a certificate used to authenticate
    the owner.

-  #### Certificate file password

    Password used to decrypt the certificate file.

-  #### Certificate

    Certificate selected from the windows key store.

-  #### User permissions

    Permissions granted by user level access.

    1.  ViewOnly  
        User will only be able to view the document.
    2.  AllowDegradedPrinting  
        User will be able to print the document at reduced resolution.
    3.  AllowModifyContents   
        User will be able to modify the document contents.
    4.  AllowCopy  
        User will be able to copy-and-paste text from the document.
    5.  AllowModifyAnnotations  
        User will be able to modify document annotations.
    6.  AllowScreenreaders  
        User will be able to use text-to-speech screen readers to read
        the document text.
    7.  AllowFillIn  
        User will be allowed to complete forms in the document.
    8.  AllowAssembly  
         Allows pages from this document to be used in assembling new
        documents.
    9.  AllowPrinting  
        User will be allowed to print the document.
    10. FullAccess  
        User is granted unrestricted access to the document.
<p>
- #### User password

    Password required to gain user-level access to the PDF.  
     <span class="recommendation"> Leave this field blank, if you would
    like to allow user-level access without requiring a password.
    </span>

- #### User certificate source

    Source to load the user's certificate from.

    1.  File  
        Load the certificate from a .pfx file.
    2.  Store  
        Load the certificate from the windows key store.
<p>
- #### User certificate file path

    Path to a .pfx file containing a certificate used to authenticate
    the user.

- #### User certificate file password

    Password needed to decrypt the user's certificate.

- #### User certificate

    Certificate in the windows key store.

- #### Encryption

    Encryption method used to protect the PDF contents.

    1.  Standard encryption (40 bit)
    2.  Standard encryption (128 bit)
    3.  AES encryption (128 bit)
    4.  AES encryption (256 bit)

    <span class="recommendation"> AES encryption is stronger than
    standard (RC4) encryption, but is only supported by newer versions
    of Acrobat reader.  
     If the document needs to be read by any Acrobat versions prior to
    Acrobat X, standard encryption should be used. </span>

- #### Plaintext metadata

    Prevents encryption of the document metadata.

### Fill Form

Fill form takes data from a custom type and inserts it into the
document's form fields. Each property in the custom type needs to match
up with the name of the field in the document.

-  #### Form data

    The custom type instance containing the data to insert.

### Change Protection

The change protection operation provides a way to change the protection
settings on a PDF document without modifying the document contents. No
further options other than the input and output PDF settings are
required.

### Split

The split operation extract each page from the input PDF and saves them
as new files, using the output PDF settings. Each page is saved to a new
file, using the output PDF filename with a suffix added to indicate the
page number.

-  #### Loop results

    Creates an execution path to loop through the names of the new PDF
    files.

### Concatenate

Concatenate takes a list of PDF filenames, and concatenates them all
into a single document, in the order they were specified.  
 The concatenation operation does not loading protected PDF files, if
any of the input files require a password or certificate to open, the
protection should first be removed using the "Change protection"
operation.

-  #### PDF files

    List of PDF files to concatenate

### Add Watermark

Add watermark loads a single page PDF and superimposes that page onto
one or more pages in the input document.

-  #### Watermark PDF filename

    The path to a single-page PDF document to use as the watermark.

-  #### Pages

    The pages to add the watermark to. A dash can be used to specify a
    range of pages, and multiple ranges can be specified if they are
    seperated by a comma. For example "1-3, 5" will select pages 1,2,3
    and 5.  
     Leave this blank to add the watermark to all pages.

-  #### Position

    1.  Above original  
        Draws the watermark above the existing document contents.
    2.  Below original  
        Draws the watermark below the existing document contents.

-  

### Sign

The Sign operation adds a digital signature to the document.

-  #### Signed at

    Location where the signing took place.

-  #### Reason

    Reason for signing the document.

-  #### Signing certificate source

    1.  File  
        Load the certificate from a .pfx file.
    2.  Store  
        Load the certificate from the windows key store.
<p>
-  #### Signing certificate file path

    Path to a .pfx file containing the signing certificate.

-  #### Signing certificate file password

    Password needed to decrypt the certificate file.

-  #### Signing certificate

    Certificate in the windows keystore.

-  #### Signature placement

    Where to place the signature in the document

    1.  Hidden  
        Don't display a visible signature.
    2.  Form signature field  
        Insert the signature into an existing signature form field.
    3.  On page  
        Place a visible signature on a specific page in the document.
<p>
-  #### Position X (mm), Position Y (mm)

    The coordinates of the signature's position on the page.
    (Visible if signature placement is "On page")  
     The X and Y coordinates refer to the horizontal and vertical offset
    (in mm) of the bottom-left corner of the signature, relative to the
    bottom-left corner of the page.

-  #### Width (mm), Height (mm)

    This size of the signature box.

- #### Field name

    Form field name to place the signature in. (Visible when Signature
    placement is "Form signature field").

- #### Background image

    Path to an image to use as the background for the signature field.
    This can be left blank if no background image is needed.

- #### Page

    The page number on which the signature should be placed.


