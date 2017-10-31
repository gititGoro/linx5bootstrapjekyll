TryCatch
========

The TryCatch function allows processes to handle errors. Use this function when you expect errors and want to handle them.

Properties
----------

-  #### Options

    You must define a 'Try' and you can opt to have a 'Catch', or 'Finally' or both when using this function.

    *'Try'* is a required path for the TryCatch function. The process
    will always *try* and execute some logic.

    *'Catch'* is optional and does not have to be used. If no *catch* is
    configured, the logic will be tried, but any error generated in the
    'Try' will be ignored. The 'Catch' logic needs to handle the error.
    You may want to log this to a database or file.

    *'Finally'* is optional and does not have to be used. *'Finally'*
    contains logic that will always execute, regardless of whether the
    'Try' or the 'Catch' executed. If you are, for example, creating some files and
    are trying to read these, you may want to clear the folder of the
    files again when you are done with them, regardless of whether you could read them or not. The
    logic to remove these files or back them up to another folder would fit well into 'Finally'.


