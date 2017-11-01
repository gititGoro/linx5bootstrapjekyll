---
layout: docs
title: SetValue
description: SetValue
group: linx5main
feature: Functions
component: SetValue
toc: true
redirect_from: docs/linx5main/Functions/SetValue/index
---
---
layout: docs
title: SetValue
description: SetValue
group: linx5main
feature: Functions
component: SetValue
toc: true
redirect_from: docs/linx5main/Functions/SetValue/index
---
SetValue
========

The SetValue function allows for assigning a value to an object (e.g.
x=1). Use this function when you want to assign a value to an object
that you need to refer to further down in your process.

If you have an Integer type, you can use SetValue to count the number of
times a function looped by adding 1 to the Integer in the loop using the
SetValue function. You can also use SetValue to build up a string in
your process by adding characters to the String using SetValue or
replace the entire string with a new one.

You can also use a [custom type](https://linx.software/plugins/BuiltIn/Types/CustomType/) and assign an
entire object to that type, as long as the object looks like the type.

Properties
----------

-  #### Target

    The object you wish to assign the value to.

-  #### Source

    The value that you want to assign to the target.
