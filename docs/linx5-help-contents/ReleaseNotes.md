---
layout: docs
title: ReleaseNotes
description: Release Notes
group: linx5-help-contents
toc: true
redirect_from: docs/linx5-help-contents/releasenotes
---
# Release Notes for Linx
<a id="5_13_9671_0"></a>
## 5.13.9671
- Remove MSMQ dependency.
- Show Process tab on single click.
- Add XML to Import Custom Type.
- Fix very long property names breaks field editor.
- Update default dialog window styles.
- Update Linx Plugin library.
- Add Plugin window, remove Functions and Types window.
- Update styling.
- Fix faulty selection behaviour for duplicate names.
- Add Test to Expression Editor.
- Fix expression copy/paste bug between two Linx processes.
- Fix expression bug where reference start name is a C# keyword.
- Fix deploy hangs when server not available.
- Add checks for XSS and CSRF.
- Fix cut/paste onto child item gives object reference error.
- Block webuser access to the server website.
- Fix Process copied between Designers does not resolve parameter mapping.
- Fix server crash when cannot find custom binding certificate.
- Update LinxServer styling.
- Improve Output window when logs of > 100 lines are written.
- Remove Run.
- Allow plugins to expose Types.
- Change ExpressionEditor and Type dropdowns to show Types contained in plugins.
- Fix Linx Designer unstable when trying to copy large project.
- Fix LinxDesigner closes on cancel.
- Fix Server crash when removing solution.
- Fix download links for logs.
- Change empty Event wording.
- Fix LinxServer blank setting shows line.
- Add ExpressionEditor shows hint when no help available.
- Speed up image loading and transformation.
- Change Debugger default dates and output window wording.

<a id="5_11_720_9240"></a>
## 5.11.720.9240
- Fix installing plugin fails when moving to another window.
- Reduce installation size of Nuget packages.
- ExpressionEditor remembers size and layout.
- Show editor for complex compiled types.
- Allow undefined values in fields editor.
- Cater for Nullable<> when adding optional items.
- Display enum values in fields editor.
- Change functions and types help icon and show on hover only.
- LinxServer: Improve service start button UI.
- Fix LinxServer UI connectivity when server is stopped and started.
- Improve tab order in debug values window.
- Improve help section of properties window.
- Improve LinxServer service start and stop notifications.
- Improve toolbar button interactions.
- Expand dropdowns to full width of content.
- Show debug window on error if not visible.
- Update LinxDesigner menu.
- Improve packages window.
- Fix undo in settings window.
- Change ordering of settings and properties in dropdowns.
- PropertyGrid tab name shows type name.
- Default deploy window to selected server.
- Installer now removes Linx 5 directory within AppData when uninstalling.
- Resize tabbed window borders.
- Improve default variable name for custom type.
- Allow parsing of nullable simple types and enums from JSON.
- Don't show read-only properties in custom type value editor.
- Show warning when we auto change contents of field.
- Persist size and position of custom editors.
- Don't show process output where no output is defined.
- LinxServer: Disable plugins page buttons when nothing is selected.
- LinxServer: Add scrollbar to solution menu when it gets cut off.
- Fix items selected from dynamic dropdown does not use typeconverter.
- Add additional checks for convert to byte[].
<a id="5_10_637_9045"></a>
## 5.10.637.9045
- Remove person icon on LinxServer.
- Improve accuracy of analytics logging.
- Update to .NET Core 1.1.
- Update NuGet packages.
- Create CustomTypes from JSON Schema.
- Fix generated dll name clashes with installed dll.
- Show content structure of List in Output window.
- Fix LinxServer service start error message overlays service name.
- Fix postback error for large error messages.
- Speed up metrics retrieval.
- Set max metrics database size.
- Fix loss of metrics in queue when host is closed.
- Fix instance of List does not refresh when underlying CustomType is changed.
- Encrypt passwords in LinxDesigner.config.
- Show LinxServer logs in descending order.
- Fix wrapping of long Setting values in LinxServer.
- Allow loading of multiple versions of strong-named dll.
- Update Linx icons.
- Allow assignment to IEnumerable<> properties.
- Fix double click required to add Service from toolbar.
- Remove "Are you sure" popups in Linx Designer.
- Fix IfElse editor does not recognize True or False values.
- Improve Intellisense stability.
- Fix proxy error when clicking 'Start using Linx'.
- Display underlying type icon for Nullable<> types.
- Fix installing plugin fails when moving to another window.
<a id="5_10_582_8916"></a>
## 5.10.582.8916
- LinxServer: Improved several UI elements.
- LinxServer: Show  resource use.
- LinxServer: Fix datetimes on the RunProcess dialog.
- LinxDesigner: Show type names in Output window and Property dropdowns.
- LinxServer: Update plugins page.
- LinxDesigner: Treat xml the same as json in the SetValue editor.
- LinxServer: Add restart message to Settings.
- LinxServer: Add system notifications.
- LinxServer: Log dates in UTC.
- LinxServer: Show url for process exposed as a web service.
- LinxDesigner: Do not display setting values in property grid.
- LinxServer: Improve ssl cipher.
- Fix errors related to plugin updates.
- LinxDesigner: Improve several UI elements.
- LinxServer: Add download system version specification.
- Support nullable parameters and output from plugins.
- Fix semver minor version check when querying packages on disk.
- LinxServer: Log when port is in use.
- LinxDesigner: Fix copy paste reference bug.
- LinxServer: Improve ssl security algorithms.
- LinxServer: Add registration pages.
- LinxDesigner: Add registration screens.
- LinxServer: Auto refresh pages.
- LinxServer: Show licensing info in bottom bar.
- LinxServer: Only show notifications for actions started by the user.
- LinxServer: Fix search "see all results".
- LinxServer: Fix notifications in Edge.
- LinxDesigner: Slow down scroll speed in package window.
- LinxServer: Fix after installation plugins page shows nothing.
- LinxServer: Fix error when uploading a solution to the server.
- Update string.Split() to take string param.
- Fix compiler error when using setting in if condition.
- Make displayname of generic types in output more user-friendly.
- Fix MSMQ install on Windows 7.
<a id="5_9_513_8765"></a>
## 5.9.513.8765
- LinxServer: Fix plugins page links to release notes.
- Analytics not submitted during install.
- Fix deploy from LinxDesigner shows error even if successful.
- Fix calling process as REST service without input parameters should use default values.
<a id="5_7_481_8722"></a>
## 5.7.481.8722
- Fix GetTypeReference bug on DesignerContext for complex expressions.
- Update plugin.
- Allow CSharpExpressions on custom types and lists.
- Check expression result type to determine conversion logic in compiler.
- Fix implicit operator bug.
- Allow xml values in UI.
- Only allow JSON or XML string values for complex types.
- Fix WCF error handling between host and service.
- Fix Settings index error.
- Display Cut/Copy options on Folder item.
- Determine expressions return type, automatic type conversion.
- Fix compiler error when double-clicking on a lsoz file. The working directory was not correct.
- Add List functions to ExpressionEditor.
- Fix reference scope bug when moving function in same process.
- Update LinxServer with new UI and metrics.
- Allow for simple values in list parameter.
- Compile in temp dir.
- Show empty list in debug values.
- Fix plugin loading bug for mismatched third-party libraries (e.g. Json.Net).
- Only use compatible NuGet dependencies when querying NuGet package information.
- Fix signed service not starting.
- Update UnknownType icon.
- Updated Installer Grey Button styles (for installer, close and uninstall).
- Changed the button styles of disabled primary buttons in Linx Designer to be greyed out (no background).
- Upgrade plugins to enable namespaces changes.
<a id="5_6_352_8421"></a>
## 5.6.352.8421
- Update NuGet packages.
- Designer style changes.
- Change tab order of Save and Cancel.
- Fix compiler error for embedded try-catch.
- Remove input parameter results in compile exception.
- Remove "Prefer 32-bit" flag from project.
- Change PropertyGrid tab order.
- Windows menu is incorrect after floating window.
- Install MSMQ with LinxServer.
- Change name selection when adding a Function.
- Complete border around property windows.
- Change border of editor windows to those used by property windows.
- Remove shadow around window.
- Fix setting conversion error in designer context.
- Change ServiceEvent to never throw exceptions.
- Increase CheckForUpdates timeout.
- Exception on Run window has incorrect layout.
- Inline solution name edit do not follow file naming conventions.
- Update plugin.
- Debug of DirectoryWatch.CreatedEvent throws exception.
- Change color of debug toolbar.
- Ctrl-F to focus on Search box.
- Object reference error when deleting single setting that is referenced.
- Add Delete and Find References context menu on Settings.
- Property window blank when click in empty Process.
<a id="5_5_305_8308"></a>
## 5.5.305.8308
- Add Linx Service dependency on KeyIso Service to avoid start-up timeout.
- Add parameter value logging for external project processes.
- Improve logging of parameters.
- Add list type validation for ForEach.
- Add scrollbar to error section on Run UI.
- Fix intermittent rollback of solutions issue on Linx Server.
- Improve property grid tabbing behavior.
- Improve logging of strings.
- Fix DoWhile conditions affected by logging.
- Update code-signing certificate in installer.
<a id="5_4_287_8256"></a>
## 5.4.287.8256
- Fix SetValue exception when setting compiled type.
- Fix LinxServer comment file exception when upgrading format.
<a id="5_3_282_8246"></a>
## 5.3.282.8246
- Fix read license file and installation file without locking.
- Fix display applicable icon for launch editor.
- Fix close file stream when checking for Linx 4 solution.
- Throw error for invalid conversions in JSONExpression, e.g. "Five" to int.
- Throw error for invalid conversions in XMLExpression, e.g. "Five" to int.
- Allow exception to bubble up if call in event fails.
- Fix deployment of solution with special characters (e.g. &) in name.
- Do not overwrite NLog configuration if loaded from config.
- Improve Linx Server logging.
- Add Search.
- Fix drop-down nullable-typed properties.
- Add logging for AddToList.
- Make server usernames case-insensitive.
- Do not allow empty username or password.
- Add Find References.
- Add logging for AddToList.
- Add logging for ClearList.
- Add logging for ForEach.
- Add logging for SetValue.
- Add logging for ThrowException.
- Add logging for DoWhile.
- Add logging for IfElse.
- Add logging for TryCatch.
- Add logging for RunProcess.
- Fix scroll bar color.
- Fix debug value bug for expanded children with same name.
- Redraw flow diagram after specified delay.
- Changed Debug related analytics.
- Fix right-click below last instance shows incorrect menu.
- Fix SetValue dropdown after selecting StringBuilder type.
- Add Output window.
- Fix dropdown option tree style.
- Fix resource selector dropdown style.
- Update plugin to 12.0.0.
- Update icons for AddToList and ClearList.
- Change tab order: Process Input and Output Fields editor.
- Change tab order: CustomType.Value Set Fields editor.
- Change tab order: CustomType Edit Fields editor.
- Change CustomType tab order.
- Throw detailed exception when plugins can't update.
- Requery update information for plugins after application update.
- Fix only copy settings file on rollback if it exists.
- Fix cannot uninstall deprecated plugins.
- Update welcome screen.
- Debug window layout saved.
- Change Designer tabs.
- Replace window dock menu with float/dock toggle.
- Fix AddToList not picking up list changes. 
- Make properties in JSON and XML case-insensitive.
- Fix SolutionHost failing after Service exception.
<a id="5_3_175_7937"></a>
## 5.3.175.7937
- Improve LinxServer start-up sequence.
- Improve readability of long Function names and Help border.
- Fix copy paste with keyboard on Execution Paths.
- Sort Projects alphabetically on server view.
- Add clear all and copy options on debug output screen.
- Fix Process copy throws away all input and output values.
- Change server logging level to Info.
- Change command logging level to Trace.
- Fix exception when parsing invalid Setting value.
- Fix metrics logged multiple times after close and open of Solution.
- Fix Settings referring to Settings do not resolve when overridden on server.
<a id="5_3_153_7886"></a>
## 5.3.153.7886
- Rename packages to plugins in installer text.
- Add Settings menu and form.
- Add "Plugin update repository" setting.
- Always escape JSON in expressions.
- Improve error message on broken package searchpaths.
- Don't show minimize on dialog windows.
- Fix multiple file upload bug on LinxServer.
- Improve LinxServer load of solutions during start-up.
<a id="5_3_136_7843"></a>
## 5.3.136.7843
- Fix null reference exception.
- Add ToInt64 extension methods on decimal and double.
- Debugger shows incorrect values after recursive call.
- Update Linx plugin.
- Call UpdateToLastestVersion method when loading function and service data.
- Add new updating mechanism to functions.
- Add update functionality for PassAsReference properties.
- Fixed reference not found error.
- Remove binding errors on ProcessControl.
- Do not override setting values from debugger.
- Use distinct assemblies to speed up compilation.
- Scroll current debug target into view.
- Plugin manager should tell us when server not contactable.
- Updates from Linx Server UI.
- JsonIgnore non-persisted properties in SolutionRuntimeInfo.
- Fix JSON/XML expression formatting in collection editor.
- Add custom type validation on property value.
- Fix version binding.
- Allow multiline Input debug values.
- Fix DummyItem displayed when debugging.
- Fix type reference change in SetValue.
- Improve logging infrastructure.
- Output log.
- Debug Output: Add Trace information.
- Fix app.config.
- Fixed deadlock when Stopping debugger.
- Fix Paste after copy not taking selected function into account.
- Linx Server RunProcess window: Fix AppState is not defined error.
- Fix Property dropdown: Dropdown was empty for property of type IEnumerable<byte>.
- Debugger: Dispose faster.
- LinxServer: multi-line parameter input and output when running a process.
- Return empty server package list if internet connection fails.
- Store: Better error message when function data update fails during solution load.
- Fix BindingExpression error "System.Windows.Data Error: 8 : Cannot save value from target back to source.".
- Add ToBytesFromBase64 as extension method on String.
- Tell the user when loading a Linx 4 file.
- Add ToBase64 as extension method on List<byte>.
- Fix BindingExpression errors "Cannot find source for binding with reference 'ElementName=expressionEditor'" and "BindingExpression path error: 'Children' property not found on 'object'".
- Fix property reference bug.
- Fix "ValueIsValid" binding error.
- Fix SetValue bug - loses value when target is removed.
- Change display of CustomTypes in property window.
- Fix list of custom type breaks when custom type is renamed.
- Change display of CustomTypes in dropdowns.
- Allow the use of CustomTypes across projects.
- Debugger: Do not display items that do not have debug values.
- Debug Values: Show dates in local culture.
- Double-click on Linx 4 file will launch Linx 4.
- Designer: Add licence page.
- Fixed EmptyFunction discarded property value-type info.
- Designer: Loading a Solution without the required Plugins installed should tell the user.
- Don't allow saving the solution when plugins are missing.
- Change upgrade detection in installer.
- Fix immutable functiondata and servicedata does not add version.
- Log full exception text in logger.
- Lazy load recently used functions.
<a id="5_3_56_7501"></a>
## 5.3.56.7501
- Fix object reference exception when validating null types.
<a id="5_3_54_7497"></a>
## 5.3.54.7497
- Fix loading of legacy solutions
<a id="5_3_51_7491"></a>
## 5.3.51.7491
- Fix bug: LinxServer: Process output failed to show.
- Fix bug: Embedded custom type compiler error.
- Show Plugin Manager with Updates selected.
- Add ReferencesChanged method for applicable components.
- Limit objects available in property dropdown to cater for functions like BeginTransaction and the functions that hooks onto its output.
- Update plugin.
- LinxServer: Do not recompile when Settings change.
- Service properties can reference Settings.
- Fix bug: Type-literal compile error.
- Fix bug: Match service by Id rather than Name.
- LinxServer: SolutionHost recompiles after plugin updates.
- Fix bug: Check for null WPF listview item.
- Fix bug: Move IsExpanded property to base to prevent binding errors on treeview.
- LinxServer: Delete next version folder if it fails to create. Do not fail on non-existent settings file.
- LinxServer: Handle null activeServices array.
- Fix bug: DummyDomainItem shown in properties when nothing selected.
- Only add resolve paths for valid components.
- Fix bug: Object reference error on Expression Editor intellisense for invalid properties.
- Fix colour in Expression Editor.
- Fix bug: Recursive RunProcess crashes when debugging.
- Fix bug: Octal conversion error in debug statistics.
- Fix bug: Change Guid.ToString format to fix object reference error.
- Fix bug: Double slashes in help links.
- Show validation error when CustomType deleted.
- Update help and release notes links to point to new site.
<a id="5_3_28_7405"></a>
## 5.3.28.7405
- Replacing the word "Packages" with "Plugins".
- Changed Installer to not update Service account when upgrading.
- Designer: Add links to community site.
- Designer: Add Edit menu item.
- Allow case-insensitive upload of solution files.
- Input Fields Editor: Show context in heading.
- Fix bug: Custom types cause a conversion error when calling a Process in another Project.
- Display dynamic date fields using user culture.
- Allow copy of debug values.
- Fix bug: Paste triggers GetDataInScope.
- Remove conversion code for old Linx 5 beta versions.
- Update Linx Plugin API.
- Update various infrastructure libraries.
- Only update references that have changed.
- Check for compiled type when getting default value.
- Fix bug: Input or Output values of a copied Process lose their parents.
- Fix bug: Changing the name of a Function with an Execution Path do not fix the downstream references.
- CustomTypes incorrectly specified by broken Plugin.
- Fix bug: RunProcess reference mapping after move.
- Fix bug: DateTime culture conversion bug when DateTime is expression.
- Fix bug: Load solution without installed function corrupts data.
<a id="5_3_5_7330"></a>
## 5.3.5.7330
- Change REST response property name from Value to Values
- Fix bug: Add null check in process designer
<a id="5_3_3_7324"></a>
## 5.3.3.7324
- Fix bug: Disable cut/copy/paste in debug mode
- Fix bug: Display statistics when opening process designer in debug mode 
<a id="5_3_1_7318"></a>
## 5.3.1.7318
- UI changes
- Update Linx plugin
- Force a restart after doing plugin updates in the designer
- Change designer and server to be culture invariant
- Display function names in compilation errors
- Change service start-up behaviour to better handle solutions with broken services
- Fix bug: Double-click on lsoz file does not open file in designer
- Fix bug: Compiling process that is not open in designer gives object reference error
- Fix bug: Property grid does not display items in alphabetical order when that option is selected
- Fix bug: Character expression '"' was interpreted as the start of a string in expression editor
- Fix bug: Instance of custom type editor throws exception when not all properties are filled in
- Fix bug: Collapsed docked windows do not open on hover
- Fix bug: Server statistics incorrectly calculated when multiple threads are running
- Fix bug: Custom type property not parsed correctly when used in expression
- Fix bug: Validation index error for custom validations
- Fix bug: Copy paste of child items with references gives error
- Fix bug: Project paste not working
- Fix bug: ForEach output does not change when List type changes
- Various other enhancements and bug fixes
<a id="5_0_680_6896"></a>
## 5.0.680.6896
