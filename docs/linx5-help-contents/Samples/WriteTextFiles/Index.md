Write CSV or Fixed Length File
====================

This sample contains functionality for writing CSV and fixed length files using expressions and Razor templates. 

The first process in this sample called "CommaDelimitedExpression" retrieves people data from a list and uses **expressions** defined in the Expression Editor to write the data to a comma-delimited file. 

The second process, called "CommaDelimitedUsingTemplate" uses a **Razor Template** to accomplish the same. It also writes a comma-separated file containing the customer data. 

The third process, called "FixedLengthExpression" uses **expressions** to generate data in a *'fixed length'* format. The data is also written to a file.

The fourth and last process, called "FixedLengthUsingTemplate" uses a **Razor template** to format the data in a *fixed length* format for writing to a file. 

#### To run the sample

1. Click on the Settings button and change the value of *FilePath* to a folder that exists on your computer.
2. Run any process in the sample project and visit the output folder to open the file you have written. 

#### Download
[WriteTextFiles.zip](WriteTextFiles.zip)
