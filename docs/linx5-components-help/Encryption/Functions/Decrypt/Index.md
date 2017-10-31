Decrypt
=======

Decrypts data encrypted by the "Encrypt" function.

Properties
----------

-  #### Data

    The data to decrypt. Can be either a base64-encoded string or a list
    of bytes.

-  #### Output as

    Either *Text* (default) or *Binary*.

    Return decrypted data as text or binary.

-  #### Output encoding

    *Default*, *ANSI*, *ASCII*, *EBCDIC*, *Mac*, *OEM*, *Unicode*,
    *UTF7*, *UTF8*.

    This property will appear only when *Text* is selected for the
    "Output as" property, and controls which codepage to use when
    converting the decrypted data to text.

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

    Passphrase salt (optional). Only shown if "Use passphrase" property
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

- #### Public exponent

    The public exponent function of the RSA public key. Only visible if
    "Load key from file" is unchecked.

- #### Private exponent

    The private exponent function of the RSA private key. Only visible
    if "Load key from file" is unchecked.

- #### Private P

    The private P function of the RSA private key. Only visible if "Load
    key from file" is unchecked.

- #### Private Q

    The private Q function of the RSA private key. Only visible if "Load
    key from file" is unchecked.

Links
-----

AES encryption algorithm:
<http://en.wikipedia.org/wiki/Advanced_Encryption_Standard>

RSA encryption algorithm:
<http://en.wikipedia.org/wiki/RSA_(cryptosystem)>
