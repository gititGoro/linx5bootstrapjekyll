---
layout: docs
title: PublishWebService
description: PublishWebService
group: linx5main
feature: Samples
component: PublishWebService
toc: true
redirect_from: docs/linx5main/Samples/PublishWebService/index
---
Publish Web Service
===================

This sample publishes web services to retrieve data from the Northwind database. You can get sample Northwind databases [here](https://code.google.com/p/northwindextended/downloads/list).

This Linx Solution uses the **Database**, **Utilities**, and **Web** plugins.

#### To run the sample

1. Open the PublishWebService solution with Linx Designer.
1. Click on the Settings button.
1. On the Settings tab change the values of the Settings to suit your environment (if you need help with the database connection string, have a look at this help topic). Be aware that using Windows Authentication in your connection string might not work when the solution runs in LinxServer because LinxServer does not run in the desktop user's security context except if you specifically change it.
1. Click the Deploy button.
1. Select the server. If no server is configured
	1. Click Add Server.
	1. Fill in the properties:
		1. Server Configuration Name = local
		1. Server URL = https://localhost:8022 (default port if you did not provide one during install)
		1. User = [user name] (admin is the default user installed)
		1. Password = [your password] (provided during install or changed since)
	1. Click Save.
1. Select the server.
1. Click Deploy.
1. When deployment is finished click the server name in the notification message.
1. The browser should now be on the server admin page.
1. Click on PublishWebService under Solutions.
1. Select the Projects tab.
1. Both Processes should be visible with Run links next to them.
1. The full web service url is also displayed on the right hand side.
1. Click Run to test it or copy the Url to another browser tab, change the parameters to suitable values and submit. Remember to check 'Wait for finish' when using Run or else it will execute asynchronously and you will not see the result.

If you have any problems please contact us at linx@digiata.com.

#### Download
[PublishWebService.zip](PublishWebService.zip)

#### Video
[Linx 5 PublishWebService](https://www.youtube.com/watch?v=mtp-On-h5L0)
