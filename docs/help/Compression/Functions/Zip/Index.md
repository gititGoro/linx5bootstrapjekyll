# Zip

Adds files to a zip archive.

## Properties

- ### Archive
The file path to the zip archive folder that will be created/used to store the given files.

- ### Archive does not exist
Determines whether the zip archive should be created or an exception thrown if the zip archive does not exist.
	- *Create File* creates a zip archive for the given file path.
	- *Throw Exception* stops the process and returns an error.

- ### Archive exists
If the zip archive exists you can opt to
	- **Append data:** Opens the archive and adds the given files to it.
	- **Increment file name:** Will add a number to the zip archive's name to create a unique zip archive. This is a sequential number starting at 1.
	- **Overwrite file:** Will replace the content of the zip archive with the new files.
	- **Throw exception:** Stops the process and returns an error.

- ### Files to compress
A file path string, or a JSON array containing multiple file path strings. All files will be added to the zip archive.

## Output
If the process is successful then the files will be stored in a zip archive and its file path returned as the output.

## Video
[Linx 5 Zip and Unzip ](https://www.youtube.com/watch?v=pMYJoSWFUhk)