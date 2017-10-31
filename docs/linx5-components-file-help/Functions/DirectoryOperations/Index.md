DirectoryOperations
==============

This function supports copying, moving, creating, and deleting directories (folders),
as well as determining if a directory exists.

Properties
----------

-  #### Action {#action-property}

    The action to perform on the source directory. The available actions are *Copy*, *Move*, *Create*, 
    *Delete*, and *Directory exists*.

-  #### Source directory

    Visible when [Action](#action-property) is either *Copy* or *Move*. Indicates the full path of the directory 
    on which to perform the action.

-  #### Target directory {#target-directory-property}

    Visible when [Action](#action-property) is either *Copy* or *Move*. Indicates the full path to the destination 
    directory.

-  #### Directory

    Visible when [Action](#action-property) is *Create*, *Delete*, or *Directory exists*. Indicates the full 
    path of the directory on which to perform the action.

-  #### Directory exists {#directory-exists-property}

    What to do when the [Target directory](#target-directory-property) already exists.

    When the selected [Action](#action-property) is *Create* or *Delete*, the following options are available:

    - *Overwrite directory* replaces the entire contents of the target directory with the contents of the 
    source directory.

    - *Merge directory* retains all existing sub-directories and files in the target directory. Files that 
    also exist in the source directory will replace those in the target directory based on the 
    [Replace existing files](#replace-existing-files-property) property.

    - *Do nothing* keeps the target directory unchanged.
    
    When the selected [Action](#action-property) is *Create*, the following options are available in case 
    the directory already exists:

    - *Do nothing* performs no action.
    
    - *Increment folder name* will add a number to the end of the folder name to make the name unique.
    This is a sequential number starting with 1.
    
    - *Clear* will delete the entire contents of the directory. 
    
    - *Throw exception* will stop the process and return an error.

- #### Replace existing files {#replace-existing-files-property}

    Visible when the [Action](#action-property) is *Copy* or *Move*, and the 
    [Directory exists](#directory-exists-property) option selected is *Merge directory*. Selects if any file 
    already in the destination directory that is also in the source directory should be replaced by the file in 
    the source directory. If this option is unchecked, all existing files in the target directory will be kept
    unchanged. 

Output
------

If [Action](#action-property) is *Create*, the function returns the name of the folder that was created.

If [Action](#action-property) is *Directory exists*, the return value is a boolean value indicating if the 
directory exists.
 