---
layout: docs
title: getbrandingthemes
description: getbrandingthemes
group: functions
feature: brandingtheme
component: getbrandingthemes
toc: true
---
GetBrandingThemes
============

Use this function to retrieve one or many branding themes.

Properties
----------

- #### Authentication
[Authentication](../../../Common/Authentication/Index.md) values used to connect to the Xero server.
- #### Branding theme id / name
You can retrieve an individual record by specifying the BrandingThemeID.
- #### Modified after
A UTC timestamp (yyyy-mm-ddThh:mm:ss). Only branding themes created since this timestamp will be returned e.g. 2009-11-12T00:00:00.
- #### Where
Restrict the returned items using the specified criteria. See [Where filter](../../../Common/Filters/Where/Index.md).
- #### Order by
Return items in a specific order. See [Order by filter](../../../Common/Filters/OrderBy/Index.md).
- #### Return options
Return Options: Select how the data is to be returned. Possible values are:
  * Only the first item.
  * The first item or an empty item. 
  * All the items one by one.
  * All of the items at once.


Output
-----
#### [Return type](#return-options) of branding themes
A list of branding themes or a single branding theme depending on the selected [Return option](#return-options).

Links
-----

https://developer.xero.com/documentation/api/branding-themes
