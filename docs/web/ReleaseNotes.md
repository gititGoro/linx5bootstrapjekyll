---
layout: docs
title: ReleaseNotes
description: Release Notes
group: web
toc: true
redirect_from: docs/web/releasenotes
---
# Release Notes for Web plugin
<a id="1_11_162_521"></a>
## 1.11.162.521
- Update Linx Plugin library.
- Update Nancy library.
- Update NJsonSchema library.
- Fix CallSOAPWebService array of string bug.
- Add support for all security protocols in CallSOAPWebService.
- Expose types when CallRESTWebService output is a list.
- Fix object missing properties when parsing swagger.
- Soap function Settings Editor
- Fix RESTWebService method names not based on operationid.
- Fix bug where generated type name gets changed for uniqueness when not required.

<a id="1_10_145_474"></a>
## 1.10.145.474
- Update Linx plugin;
<a id="1_9_144_470"></a>
## 1.9.144.470
- Allow setting of cookie in CallRestWebService.
- Fix CallSOAPWebService invalidate cached wsdl.
- Allow streaming calls with CallSOAPWebService.
<a id="1_8_137_455"></a>
## 1.8.137.455
- Fix CallRestWebService compile error caused by hidden property.
<a id="1_7_135_450"></a>
## 1.7.135.450
- CallRestWebService: Only url encode querystring values.
- Fix custom headers not sent to server.
<a id="1_6_132_443"></a>
## 1.6.132.443
- Update NuGet packages.
- RestWebService: Improve exception messages.
- CallRestWebService: Fix compiler error when enumerator cannot advance.
- RestWebService: Fixed missing required parameters causing file download.
- SOAP functions: Fix flattened type error when drilling down more than one level.
<a id="1_6_127_433"></a>
## 1.6.127.433
- Default ExpectContinue header to false and fix adding of request headers.
- Fix out and ref parameters in CallSOAPService.
- SoapService RequiredParametersBehavior error.
- Allow https on RESTService.
- Support nillable parameters and output on CallSOAPWebService.
- Fix wsdl returned from SOAPService not the same as used to design service.
- Update Linx plugin
- Update NuGet packages
<a id="1_5_114_403"></a>
## 1.5.114.403
- Improve wsdl importing.
<a id="1_4_111_394"></a>
## 1.4.111.394
- Improve error handling when CallSOAPWebService expects a list of strings as input parameter.
- Fix changing method name in CallSOAPWebService does not refresh method properties.
- Improve wsdl importing.
- Update NuGet packages
- Update Linx plugin
- Fix CallSOAPWebservice not importing wsdl.
- Update UI styles.
- Update plugin utilities.
<a id="1_3_94_352"></a>
## 1.3.94.352
- Put sample in SoapWebService.EndpointUrl property.
- Put sample in RESTWebService.BaseUri property.
- Fix object reference error for empty body parameters.
<a id="1_2_91_344"></a>
## 1.2.91.344
- Change tab order of Save and Cancel.
- Fix CallSOAPWebService to unwrap TargetInvocationException.
- Fix SOAPService and RESTService to relay process exceptions.
- Fix SOAPService. Calling method with "password" parameter does not show input parameter.
- Enable https calls for old webservices.
- Update Linx Plugin.
<a id="1_1_82_322"></a>
## 1.1.82.322
- Update Linx Plugin.
- Changed tab order on controls.
- Add Settings property to CallSOAPWebService.
- Add Settings property to RESTWebService.
- Add Settings property to CallRESTWebService.
- Change the Response section of CallRESTWebService.
- RESTService: Swagger parser enhancements.
- SOAPService WsdlLocationEditor: Add '?wsdl' to the URI for .asmx or .scv services.
- RestWebService: Show swagger spec and allow user to edit it.
- CallSOAPWebService: Improve parameter logging.
- Support streams in SOAP responses.
<a id="1_0_63_222"></a>
## 1.0.63.222
- Fix when output is CustomType and CustomType is updated, compilation breaks
<a id="1_0_61_218"></a>
## 1.0.61.218
- Update Linx plugin.
- Fixed Message Contract methods generated.
<a id="1_0_54_204"></a>
## 1.0.54.204
- Update Linx plugin
- RESTWebService properties accept Settings.
- SOAPWebService properties accept Settings.
<a id="1_0_48_192"></a>
## 1.0.48.192
- Update Linx plugin
- RESTService Swagger import: Generate each definition type only once.
<a id="1_0_46_187"></a>
## 1.0.46.187
- Update Linx plugin
- UI Changes
- Updated DynamicSoap.dll (Support non-bodywrapped message contract methods)
- CallSOAPWebService method editor: Automatically add missing '?wsdl' for .asmx and .svc 
- CallSOAPWebService: Fixed error when selecting method
<a id="1_0_35_157"></a>
## 1.0.35.157
- Add CallRESTWebService function
- Add CallSOAPWebService function
- Add RESTWebService Service
- Add SOAPWebService Service
