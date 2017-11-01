---
layout: docs
title: GenerateHash
description: GenerateHash
group: cryptography
feature: Functions
component: GenerateHash
toc: true
redirect_from: docs/cryptography/Functions/GenerateHash/index
---
GenerateHash
============

GenerateHash computes a Base64-encoded hash for the input data using the specified
hashing algorithm.

Properties
----------

-  #### Data

    The input data for which the hash will be calculated.

-  #### Hash algorithm

    The algorithm to use when computing the hash. Possible values are:
    MD5, RIPEMD160, SHA1, SHA256, SHA384 or SHA512.

Output
------

-  A string-formatted hexadecimal number that contains the computed hash.
