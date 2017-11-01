SSRSRenderReport
================

Executes and exports a report on Reporting Services.

Properties
----------

-  #### Authentication

	The domain, user name and password used to authenticate against Reporting Services.

-  #### File path

	The file path where the report will be created. 

-  #### Output type

	The type of report to generate. Can be xls, xlsx, pdf, doc, docx, csv, html, mhtml, xml or tiff.

-  #### Reporting server
	- **Parameters**:  
		Semi colon delimited string of report parameters. Use Reporting Services to see the actual report parameters. e.g. StartDate='1 jan 2006';EndDate='1 dec 2006';CompanyId=1
	- **Service URL**:  
		Reporting Server URL e.g. http://localhost/reportserver/reportservice.asmx or http://localhost/reportserver/reportexecution2005.asmx
	- **Report name**:  
		The Report Name includes folder names. e.g. /My Folder/My Report
	- **Timeout**:  
		Timeout in milliseconds
	- **Service type**:  
		The Reporting services type, i.e. RS2008, RS2012 or RS2014.  
  
-  #### Web proxy
	- **Proxy name**:  
		The name or ip address of the proxy server.
	- **Port**:  
		The port number of the proxy server.
	- **Bypass on local**:  
		True to bypass the proxy if the Reporting Server is on localhost.











