---
layout: docs
title: graphicsmagick
description: graphicsmagick
group: graphicsmagick
feature: functions
component: graphicsmagick
toc: true
---
GraphicsMagick
==============

The GraphicsMagick function provides an interface to the [GraphicsMagick
library](#GMLibrary) which allows for converting, manipulating and
reading image files.

Properties
----------

-  #### Operation

    The operation to perform.

    1.  [Compare](#Compare)  
        Analyzes the difference between two images.
    2.  [Composite](#Composite)  
        Creates a composite image by superimposing two input images.
    3.  [Convert](#Convert)  
        Converts images to a different format.
    4.  [Identify](#Identify)  
        Reads image metadata.
    5.  [Montage](#Montage)  
        Creates a montage image by tiling multiple input images.
    6.  [Transform](#Transform)  
        Modifies an existing image by applying one or more transform
        operations.

### Compare{#Compare}

The compare operation reports the difference between two images and
generates an output image highlighting the differences between the two.
For details of the values returned from this function, see [compare
operation output](#CompareOutput).

-  #### Reference image

    Path to the reference image which is used as a basis for the
    comparison.

-  #### Comparison image

    Path to the image to be compared against the reference image.

-  #### Comparison metric{#ErrorMetric}

    The function used to quantify the difference between the two images:

    1.  Mean absolute error
    2.  Mean square error
    3.  Peak absolute error
    4.  Peak signal-to-noise ratio
    5.  Root mean square error
<p>
-  #### Save annotated image

    When checked, a new image will be created, highlighting the
    differences between the reference and comparison images.

-  #### Highlight style

    The method used to highlight differences in the annotated image.

    1.  Assign  
        Assign the highlight colour to each differing pixel.
    2.  Threshold  
        Creates a black and white image, indicating the different areas.
    3.  Tint  
        Apply a tint to the different areas based on the highlight
        colour.
    4.  XOR  
        Mix the highlight colour and original colours using a bitwise
        XOR operation.
<p>
-  #### Highlight colour

    The colour used to highlight differences in the annotated image.

-  #### Output file

    Path to save the annotated image to.

-  #### Timeout

    Timeout in seconds.

### Composite{#Composite}

Creates a composite image by superimposing two input images.

-  #### Base image path

    Path to the base image.

-  #### Overlay image

    Path to the image to overlay on top of the base image.

-  #### Merge operation

    The merge operation to apply when compositing the two images.

    1.  Over  
         The output will be the union of the two image shapes, with
        opaque areas of overlay-image obscuring base-image in the region
        of overlap.
    2.  In  
         The output is simply overlay-image cut by the shape of
        base-image. None of the image data of base-image will be in the
        output.
    3.  Out  
         The output image is overlay-image with the shape of base-image
        cut out.
    4.  Atop  
         The output is the same shape as base-image, with overlay-image
        obscuring base-image where the image shapes overlap. Note this
        differs from over because the portion of overlay-image outside
        base-image's shape does not appear in the output.
    5.  Xor  
         The output is the image data from both overlay-image and
        base-image that is outside the overlap region. The overlap
        region will be blank.
    6.  Plus  
         The output is just the sum of the image data. Output values are
        cropped to MaxRGB (no overflow). This operation is independent
        of the matte channels.
    7.  Minus  
         The result of overlay-image - base-image, with underflow
        cropped to zero. The matte channel is ignored (set to opaque,
        full coverage).
    8.  Add  
         The result of overlay-image + base-image, with overflow
        wrapping around (mod MaxRGB+1).
    9.  Subtract  
         The result of overlay-image - base-image, with underflow
        wrapping around (mod MaxRGB+1). The add and subtract operators
        can be used to perform reversible transformations.
    10. Difference  
         The result of abs(overlay-image - base-image). This is useful
        for comparing two very similar images.
    11. Divide  
         The result of overlay-image / base-image. This is useful for
        improving the readability of text on unevenly illuminated photos
        (by using a gaussian blurred copy of overlay-image as
        base-image).
    12. Multiply  
         The result of overlay-image \* base-image. This is useful for
        the creation of drop-shadows.
    13. Bumpmap  
         The base-image shaded by overlay-image.
    14. Copy  
         The output image is base-image replaced with overlay-image.
        Here the matte information is ignored.
    15. Copy Red  
         The output image is the red channel in base-image replaced with
        the red channel in overlay-image. The other channels are copied
        untouched.
    16. Copy Green  
         The output image is the green channel in base-image replaced
        with the green channel in overlay-image. The other channels are
        copied untouched.
    17. Copy Blue  
         The output image is the blue channel in base-image replaced
        with the blue channel in overlay-image. The other channels are
        copied untouched.
    18. Copy Opacity  
         output image is the opacity channel in base-image replaced with
        the opacity channel in overlay-image. The other channels are
        copied untouched.
    19. Copy Cyan  
         The output image is the cyan channel in base-image replaced
        with the cyan channel in overlay-image. The other channels are
        copied untouched. Use of this operator requires that base-image
        be in CMYK(A) colorspace.
    20. Copy Magenta  
         output image is the magenta channel in base-image replaced with
        the magenta channel in overlay-image. The other channels are
        copied untouched. Use of this operator requires that base-image
        be in CMYK(A) colorspace.
    21. Copy Yellow  
         The output image is the yellow channel in base-image replaced
        with the yellow channel in overlay-image. The other channels are
        copied untouched. Use of this operator requires that base-image
        be in CMYK(A) colorspace.
    22. Copy Black  
         The output image is the black channel in base-image replaced
        with the black channel in overlay-image. The other channels are
        copied untouched. Use of this operator requires that base-image
        be in CMYK(A) colorspace. If overlay-image is not in CMYK space,
        then the overlay-image pixel intensities are used.
<p>
-  #### Background color

    The background color to use for the new image.

-  #### Mask image path

    Path to a black and white image to use as a mask to apply before
    composition.

-  #### Output file

    Path where the output image will be saved.

-  #### Timeout

    Timeout in seconds.

### Convert{#Convert}

Coverts an image to a different format.

-  #### Input file

    Image file to convert

-  #### Output file

    Path where the output image will be saved. The format of the output
    file is determined by the file extension.

-  #### Timeout

    Timeout in seconds.

### Identify{#Identify}

Returns image metadata in [identify output](#IdentifyOutput).

-  #### Input file

    Image file to read.

-  #### Timeout

    Timeout in seconds.

### Montage{#Montage}

Creates a montage image by tiling multiple input images.

-  #### Source images

    List of images files to add to the montage.

-  #### Concatenate

    Set to true to join images together without any extra spaces.

-  #### Compression

    The type of compression to apply to the output image.  Possible values are: None (default), Group4, LZW or RLE.

-  #### Output width

    Width of the output image to save (Pixels).

-  #### Output height

    Height of the output image to save (Pixels).

-  #### Columns

    Maximum number of columns to use when tiling the input images.

-  #### Rows

    Maximum number of rows to use when tiling the input images.

-  #### Border width

    Width of the border to draw around each input image.

-  #### Border height

    Height of the border to draw around each input image.

-  #### Border colour

    Colour of the border to draw around each input image.

-  #### Include image names

    Prints the names of each image onto the montage.

- #### Text colour

    Colour of the image name text.

- #### Draw frame

    Draw a 3D frame around each image.

- #### Frame width

    Width of the frame.

- #### Frame height

    Height of the frame.

- #### Inner bevel width

    Width of the frame inner bevel.

- #### Outer bevel width

    Width of the frame outer bevel.

- #### Frame colour

    Colour of the frame.

- #### Draw shadow

    Draw a shadow cast by the frame.

-  #### Timeout

    Timeout in seconds.

### Transform{#Transform}

-  #### Input file

    Image file to read.

-  #### Output file

    Path to save the transformed image to.

-  #### Transform

    The transform to apply to the image.

-  #### Timeout

    Timeout in seconds.

Output
------

### Compare Operation Output{#CompareOutput}

-  Difference Metric  
     The result of the [comparison metric](#ErrorMetric) for the entire
    image.
    1.  Absolute  
         The absolute value of the comparison metric result
    2.  Normalised  
        The same result normalized against image size and colour depth.

### Identify Operation Output{#IdentifyOutput}

The following operations are available from the transform editor:

-  Filename  
    The filename of the input image.
-  SizeInByes  
    The size of the image on disk (in bytes).
-  Width  
    The width of the image (in pixels).
-  Height  
    The height of the image (in pixels).

Transform Editor
----------------

The transform editor is opened by clicking the "Editor transform" button
when using the "Transform" operation.

-  Original image (A)  
    The preview image before applying any transform operations.
-  Preview image (B)  
    The preview image after applying transform operations.
-  Load preview image (C)  
    Loads a different image to use for the preview.
-  Preview selected operation only (D)  
    When this is checked, the image preview only applies the selected
    operation, as opposed to all active operations.
-  Active operations (E)  
    A list of all added operations is displayed here.
-  Operation arguments (F)  
    Shows the arguments required by the selected operation.
-  Add button (G)  
    Adds a new operation.
-  Delete button (H)  
    Deletes the selection operation.

Transform Operations
--------------------

-  Colour  
    Operations used to adjust the colour of the image.
    1.  RandomThreshold  
        Changes the value of individual pixels based on the intensity of
        each pixel compared to a random threshold. The result is a
        low-contrast, two color image.
    2.  FloodFill  
        Flood-fill texture across pixels that match the color of the
        target pixel and are neighbors of the target pixel. Uses current
        fuzz setting when determining color match.
    3.  Gamma  
        Gamma correct image.
    4.  Colorize  
        Colorize image with the specified color, using specified percent
        alpha for red, green, and blue quantums"
    5.  Segment  
        Segment (coalesce similar image components) by analyzing the
        histograms of the color components and identifying units that
        are homogeneous with the fuzzy c-means technique. Also uses
        QuantizeColorSpace and Verbose image attributes.
    6.  Threshold  
        Threshold image.
    7.  Transparent  
        Add alpha channel to image, setting pixels matching color to
        transparent.
    8.  Contrast  
        Contrast image (enhance intensity differences in image)
    9.  CycleColormap  
        Displaces an image's colormap by a given number of positions.
    10. Equalize  
        Applies a histogram equalization to the image.
    11. Evaluate  
        Apply an arithmetic or bitwise operator to the image pixel
        quantums.
    12. HaldClut  
        Apply a color lookup table (Hald CLUT) to the image.
    13. Level  
        Adjust the levels of the image by scaling the colors falling
        between specified white and black points to the full available
        quantum range.
    14. Modulate  
        Modulate percent hue, saturation, and brightness of an image.
    15. Negate  
        Negate colors in image.
    16. Normalize  
        Normalize image (increase contrast by normalizing the pixel
        values to span the full range of color values)
    17. Opaque  
        Changes any pixel that matches target with the color defined by
        fill.
    18. AdaptiveThreshold  
        Local adaptive threshold image.
        http://www.dai.ed.ac.uk/HIPR2/adpthrsh.htm"
    19. CDL  
        Applies the color decision list from the specified ASC CDL file.
<p>
-  Size  
    Operations which change the size of the image.
    1.  Sample  
        Resize image by using pixel sampling algorithm to the specified
        percentage.
    2.  Scale  
        Resize image by using simple ratio algorithm to the specified
        percentage.
    3.  Zoom  
        Zoom image to specified size.
    4.  Magnify  
        Double the size of the image.
    5.  Minify  
        Half the size of the image.
    6.  Shave  
        Shave pixels from image edges.
    7.  Trim  
        Trim edges that are the background color from the image.
    8.  Chop  
        Chop image (remove vertical and horizontal subregion of image)
    9.  Crop  
        Crop image (subregion of original image)
    10. ChopHorizontal  
        Chop image (remove horizontal subregion of image)
    11. ChopVertical  
        Chop image (remove horizontal subregion of image)
<p>
-  Image Format  
    Operations used to change image format specific options.
    1.  SetDefine  
        Sets a format-specific option.
    2.  AddProfile  
        Adds the specified colour profile to the image.
    3.  SetComment  
        Sets the comment text saved with the image.
    4.  SetQuality  
        Sets the quality factor used by when compressing the image.
    5.  RemoveProfile  
        Remove a named colour profile from the image.
    6.  RePage  
        Resets the page property of this image.
    7.  Strip  
        Strips an image of all profiles and comments.
    8.  ChromaBluePrimary  
        Sets the chromaticity blue primary point.
    9.  ChromaGreenPrimary  
        Sets the chromaticity green primary point.
    10. ChromaRedPrimary  
        Sets the chromaticity red primary point.
    11. ChromaWhitePrimary  
        Sets the chromaticity white primary point.
<p>
-  Special Effects  
    Operations for applying decorative effects to an image.
    1.  Composite  
        Compose an image onto another at specified offset using the
        specified algorithm.
    2.  Border  
        Border image (add border to image)
    3.  Raise  
        Raise image (lighten or darken the edges of an image to give a
        3-D raised effect)
    4.  Shade  
        Shade image using distant light source.
    5.  Solarize  
        Solarize image (similar to effect seen when exposing a
        photographic film to light during the development process)
    6.  Spread  
        Spread pixels randomly within image by specified amount.
    7.  Stegano  
        Add a digital watermark to the image (based on second image)
    8.  Stereo  
        Create an image which appears in stereo when viewed with
        red-blue glasses (Red image on left, blue on right)
    9.  Swirl  
        Swirl image (image pixels are rotated by degrees)
    10. Texture  
        Channel a texture on image background.
    11. Tile  
        Compose an image repeated across and down the image.
    12. Wave  
        Map image pixels to a sine wave.
    13. Edge  
        Edge image (highlight edges in image)
    14. Emboss  
        Emboss image (highlight edges with 3D effect)
    15. Frame  
        Frame image with the specified with, height, innerBevel and
        outerBevel.
    16. Implode  
        Implode image (special effect)
    17. Lower  
        Lower image (lighten or darken the edges of an image to give a
        3-D lowered effect)
    18. OilPaint  
        Oilpaint image (image looks like oil painting)
    19. Charcoal  
        Charcoal effect image (looks like charcoal sketch)
<p>
-  Noise  
    Operations for adding and removing image noise.
    1.  ReduceNoise  
        Reduce noise in image using a noise peak elimination filter.
    2.  Despeckle  
        Despeckle image (reduce speckle noise)
    3.  AddNoise  
        Add noise to the specified channel of the image with the
        specified noise type.
<p>
-  Transform  
    Operations for applying geometric transforms to an image.
    1.  Roll  
        Roll image (rolls image vertically and horizontally)
    2.  Rotate  
        Rotate image counter-clockwise by specified number of degrees.
    3.  Flip  
        Flip image (reflect each scanline in the vertical direction)
    4.  Flop  
        Flop image (reflect each scanline in the horizontal direction)
    5.  Shear  
        Shear image (create parallelogram by sliding image by X or Y
        axis)
<p>
-  Blur  
    Operations that apply a blur effect to the image.
    1.  Sharpen  
        Sharpen pixels in image.
    2.  Unsharpmask  
        Replace image with a sharpened version of the original image
        using the unsharp mask algorithm.
    3.  Enhance  
        Apply a filter to improve image quality.
    4.  GaussianBlur  
        Gaussian blur image.
    5.  MedianFilter  
        Filter image by replacing each pixel component with the median
        color in a circular neighborhood.
    6.  MotionBlur  
        Motion blur image with specified blur factor.
    7.  Blur  
        Blur image with specified blur factor and channel.
<p>
-  Annotate  
    Operations for adding text to an image.
    1.  TransformOrigin  
        Origin of coordinate system to use when annotating with text or
        drawing.
    2.  TransformReset  
        Reset transformation parameters to default.
    3.  TransformRotation  
        Rotation to use when annotating with text or drawing.
    4.  TransformScale  
        Scale to use when annotating with text or drawing.
    5.  TransformSkewX  
        Skew to use in X axis when annotating with text or drawing.
    6.  TransformSkewY  
        Skew to use in Y axis when annotating with text or drawing.
    7.  Annotate  
        Renders text to the image using any of the above transforms.
<p>
#### Links{#GMLibrary}

GraphicsMagick library <http://www.graphicsmagick.org/>
