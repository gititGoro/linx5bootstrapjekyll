IfElse
======

IfElse allows for testing of conditions. The if statement provides a
separate path of execution when an "if" clause evaluates to *TRUE*.

Properties
----------

-  #### Conditions

    A set of conditions to evaluate.

-  #### Stop when true

    When *UNCHECKED*, the conditions will be evaluated separately as a
    series of *IF* statements. When *CHECKED*, the conditions will be
    combined with *IfElse* statements.

-  #### Show else

    When *CHECKED*, the statement will include an *ELSE* condition for
    you to define. When *UNCHECKED* there will be no *ELSE* condition.

Examples
--------

#### 'Stop when true' is *CHECKED*
<pre>
if (condition1) {
	//your logic  
} elseif (condition2) {
	//your logic  
}
</pre>
#### 'Stop when true' is *UNCHECKED*
<pre>
if (condition1) {
	//your logic  
}

if (condition2) {
	//your logic  
}
</pre>