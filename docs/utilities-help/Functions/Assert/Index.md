Assert
======

Allows adding run-time checks to assert the validity of predicates that
should hold true in a process.  
 Whenever the condition specified in an Assert fails, an error will
occur to indicate the fact and the process will halt.

Properties
----------

-  #### Assertion

    The type of assertion to evaluate.

-  #### Message

    The message to include in the error when the assertion fails.

*See the Assertion list below for additional properties that apply
    to specific types of assertions.*

Assertions
----------

-  #### Are equal

    Verifies that the two values specified are equal.

    ##### Properties

    1.  Expected - The expected value.
    2.  Actual - The actual value.
    3.  Tolerance - Optional. In case the values are numeric, this
        specifies the maximum acceptable difference between the expected
        and actual values.

-  #### Are not equal

    Verifies that the two items specified hold different values.

    ##### Properties

    1.  Expected - The expected value.
    2.  Actual - The actual value.

-  #### Contains

    Asserts that the specified item is contained in the given list.

    ##### Properties

    1.  Item - The item to look for in the list.
    2.  List - The list to search in for the specified item.

-  #### Greater

    Verifies that the first value is greater than the second value.

    ##### Properties

    1.  X - The first value, expected to be greater.
    2.  Y - The second value, expected to be less than the first.

-  #### Greater or equal

    Verifies that the first value is greater than or equal to the second
    value.

    ##### Properties

    1.  X - The first value, expected to be greater or equal.
    2.  Y - The second value, expected to be less than or equal to the
        first.

-  #### Less

    Verifies that the first value is less than the second value.

    ##### Properties

    1.  X - The first value, expected to be less.
    2.  Y - The second value, expected to be greater than the first.

-  #### Less or equal

    Verifies that the first value is less than or equal to the second
    value.

    ##### Properties

    1.  X - The first value, expected to be less than or equal.
    2.  Y - The second value, expected to be greater than or equal to
        the first.

-  #### Is true

    Asserts that a condition is true.

    ##### Properties

    1.  Condition - The condition to evaluate.

-  #### Is false

    Asserts that a condition is false.

    ##### Properties

    1.  Condition - The condition to evaluate.

- #### Is null

    Verifies that the item provided holds a null-value.

    ##### Properties

    1.  Item - The item to test for null.

- #### Is not null

    Verifies that the item provided does hold a value and is not null.

    ##### Properties

    1.  Item - The item to test for null.

- #### Is empty

    Asserts that a list or string is empty.

    ##### Properties

    1.  Item - The list or string to test for.

- #### Is not empty

    Asserts that a list or string is not empty.

    ##### Properties

    1.  Item - The list or string to test for.

- #### Fail

    Fails unconditionally. Use to verify that a specific part of a
    process never executes.

Links
-----

[Wikipedia: Assertion (software
development)](http://en.wikipedia.org/wiki/Assertion_%28software_development%29)
