---
layout: docs
title: WriteLargeFile
description: WriteLargeFile
group: linx5main
feature: Samples
component: WriteLargeFile
toc: true
redirect_from: docs/linx5main/Samples/WriteLargeFile/index
---
Write large file
================

In order to introduce the functionality of the FileOpen function, this example gives two processes to demonstrate how to use the function and how much of an advantage the FileOpen function gives you in your file usage times, especially for large files.

The first process **FileOpen** uses the FileOpen function to get a handle on the file, and then the repeated operations are performed on the file.

The second process **NormalFileWrite** does exactly the same repeats, but instead of getting the handle, each write is made directly to the file.

You can change the **RepeatCounter** in each process to compare the access speeds.

#### To run the sample

1. Open the FileOpenWrite solution with Linx Designer.
1. Click on the Settings button.
1. On the Settings tab change the value of DBConnectionString to the connection string for the NorthWind Database on your computer.
1. On the Settings tab change the value of FilePathForFileOpen to a valid path on your computer.
1. On the Settings tab change the value of FilePathForNormalFile to a valid path on your computer.
1. Open the first Process called FileOpen and click on Run & Debug, run the process and take note of the total time.
1. Open the second Process called NormalFileWrite and click on Run & Debug, run the process and take note of the total time.
1. Compare the times of the Processes.

#### Download
[WriteLargeFile.zip](WriteLargeFile.zip)
