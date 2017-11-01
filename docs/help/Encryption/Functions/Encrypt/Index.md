Encrypt
=======

Encrypts text or binary data.

Properties
----------

-  #### Input type

    Either *Text* (default) or *Binary*.

    If *Text* is selected, the data property will accept a string type,
    and the "Input encoding" property will be shown.  
     If *Binary* is selected the data property will accept a list of
    bytes or Base64 encoded string.

-  #### Input encoding

    *Default*, *ANSI*, *ASCII*, *EBCDIC*, *Mac*, *OEM*, *Unicode*,
    *UTF7*, *UTF8*.

    This property will appear only when *Text* is selected for the Input
    type property, and controls which codepage to use when converting
    the data to encrypt to an array of bytes.

-  #### Data

    The data to encrypt. The type of the value must correspond to the
    option selected for the Input type property.

-  #### Algorithm

    *AES* (default), or *RSA*.

    The encryption algorithm to use.

-  #### Use passphrase

    Allows the AES encryption key and initialisation vector to be
    specified manually or generated from a passphrase and salt.

-  #### Passphrase

    Passphrase used to generate AES key. Only shown if "Use passphrase"
    property is checked.

-  #### Passphrase salt

    Passphrase salt, optional. Only shown if "Use passphrase" property
    is checked.

-  #### Key

    AES encryption key. Only shown if "Use passphrase" property is
    unchecked.

-  #### IV

    AES encryption initialisation vector. Only shown if "Use passphrase"
    property is unchecked.

- #### Load key from file

    Allows RSA key to be specified manually or loaded from a certificate
    file.

- #### Certificate file path

    Path to the certificate file containing RSA key. Only visible if
    "Load key from file" is checked.

- #### Certificate password

    The password protecting the certificate file. Only visible if "Load
    key from file" is checked.

- #### Exponent

    The public exponent of the RSA public key. Only visible if "Load key
    from file" is unchecked.

- #### Modulus

    The modulus function of the RSA public key. Only visible if "Load
    key from file" is unchecked.

Links
-----

AES encryption algorithm:
<http://en.wikipedia.org/wiki/Advanced_Encryption_Standard>

RSA encryption algorithm:
<http://en.wikipedia.org/wiki/RSA_(cryptosystem)>
