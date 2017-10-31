Decimal
=======

Drag a decimal type onto the canvas and assign a decimal value to it. Use the SetValue to change the value further below in your process.

A decimal type can be populated with non-repeating decimal fractions like '0.3' or '-1.17'. It supports up to 29 significant digits and can represent values in excess of 7.9228 x
10^28. It is particularly suitable for calculations, such as financial, that require a large number of digits but cannot tolerate rounding
errors.

It holds signed 128-bit (16-byte) values representing 96-bit (12-byte)
integer numbers scaled by a variable power of 10. The scaling factor
specifies the number of digits to the right of the decimal point; it
ranges from 0 through 28. With a scale of 0 (no decimal places), the
largest possible value is +/-79,228,162,514,264,337,593,543,950,335
(+/-7.9228162514264337593543950335E+28). With 28 decimal places, the
largest value is +/-7.9228162514264337593543950335, and the smallest
nonzero value is +/-0.0000000000000000000000000001 (+/-1E-28).

<hr>
#### Videos
[Linx 5: Types and Set Values](https://www.youtube.com/watch?v=HfRFVry8gp0)