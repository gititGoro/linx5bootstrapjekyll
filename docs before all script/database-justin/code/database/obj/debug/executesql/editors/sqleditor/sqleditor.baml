   M S B A M L   `   `   `  ����\  XTwenty57.Linx.Components.Database, Version=1.3.0.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:Twenty57.Linx.Components.Database.UI.Controls-Twenty57.Linx.Components.Database.UI.Controls  5      U QGongSolutions.Wpf.DragDrop, Version=0.1.4.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:GongSolutions.Wpf.DragDrop;assembly=GongSolutions.Wpf.DragDropGongSolutions.Wpf.DragDrop 5      �Vclr-namespace:Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModelHTwenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel  5      U QTwenty57.Linx.Plugin.UI, Version=16.0.1.226, Culture=neutral, PublicKeyToken=nullrNclr-namespace:Twenty57.Linx.Plugin.UI.Windows;assembly=Twenty57.Linx.Plugin.UITwenty57.Linx.Plugin.UI.Windows 5	      rNclr-namespace:Twenty57.Linx.Plugin.UI.Helpers;assembly=Twenty57.Linx.Plugin.UITwenty57.Linx.Plugin.UI.Helpers 5
      ] YICSharpCode.AvalonEdit, Version=5.0.3.0, Culture=neutral, PublicKeyToken=9cc39be672370310oLclr-namespace:ICSharpCode.AvalonEdit.Editing;assembly=ICSharpCode.AvalonEditICSharpCode.AvalonEdit.Editing 5      �Vclr-namespace:Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModelHTwenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel  5      q=clr-namespace:Twenty57.Linx.Components.Database.UI.Converters/Twenty57.Linx.Components.Database.UI.Converters  5      �Tclr-namespace:Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.HelpersFTwenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers  5      �Uclr-namespace:Twenty57.Linx.Plugin.UI.Editors.Static;assembly=Twenty57.Linx.Plugin.UI&Twenty57.Linx.Plugin.UI.Editors.Static 5      N    HTwenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.SQLEditor   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      VmvvmTextEditor;clr-namespace:Twenty57.Linx.Components.Database.UI.Controls     5      [ddLclr-namespace:GongSolutions.Wpf.DragDrop;assembly=GongSolutions.Wpf.DragDrop     5      hmodelVclr-namespace:Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel     5      cpluginUINclr-namespace:Twenty57.Linx.Plugin.UI.Windows;assembly=Twenty57.Linx.Plugin.UI     5	      jpluginUIHelpersNclr-namespace:Twenty57.Linx.Plugin.UI.Helpers;assembly=Twenty57.Linx.Plugin.UI     5
      c
avaloneditLclr-namespace:ICSharpCode.AvalonEdit.Editing;assembly=ICSharpCode.AvalonEdit     5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      blocalVclr-namespace:Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel  5      Puiconverters=clr-namespace:Twenty57.Linx.Components.Database.UI.Converters  5      bhelpersTclr-namespace:Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers  5      kpluginUIEditorsUclr-namespace:Twenty57.Linx.Plugin.UI.Editors.Static;assembly=Twenty57.Linx.Plugin.UI  5      J	 FWpfAnimatedGif, Version=1.4.14.0, Culture=neutral, PublicKeyToken=null,gif"http://wpfanimatedgif.codeplex.com 	 5        � ShowInTaskbar  .   � Title$	 SQL��5       � WindowStartupLocation$ CenterOwner=�5      $	��800��5      $	��800��6   $	��500��5      $	��450��6      DialogWindowStyle#��[  5      2  ,Twenty57.Linx.Plugin.UI.Helpers.DialogCloser   DialogResult 5      �� *DialogResult+ +� FocusedElement 5      ��  �� ElementName$ 	sqlEditor�� � 	Resources 5        TabGreen �� Color  	BackGreen  SoftGrey �� 
TargetType  BaseTabStyle	 �� Property  DialogWindowBackgroundBrush
 �� Value  EditorTestStyle �� BasedOn  BaseSqlTestStyle  VariablesObjectsStyle '	 #VariablesObjectHeaderTextBlockStyle �� Triggers �� Binding �� RelativeSource �� AncestorType �� Value 
 SQLTestTabItemStyle �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName  BaseVariablesObjectsStyle  VariablesTabItemStyle  ObjectTabItemStyle  expressionButtonStyle  DialogButtonStyle   booleanToVisibilityConverterI   CTwenty57.Linx.Components.Database.UI.Converters.GridLengthConverter  gridLengthConverterl   fTwenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers.GridSplitterVisibilityConverter # gridSplitterVisibilityConverters   mTwenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers.TestSqlPaneScrollViewerHeightConverter * &testSqlPaneScrollViewerHeightConverter_   YTwenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers.MouseWheelBehavior  ignoreMouseWheelBehavior��%L  &	       &	 !     &	 B     &	 c     7  &	 �     7  7  &	     &	 �    7  7  &		 !    &	
 �    7  7  &	 J    &	 �    7  7  7  7  7  &	 7
    7  7  7  7  7  &	 �    7  &	 �    &	     &	     &	 *    &	 =    �� 5      $ #2EB398��6%   �� 5       $ #AFDBD2��6&   �� 5!      $ #808080��6%   �� 5#      " ��6
   .�5$      �� 	 � ��8
   6"   5%      �� 5&      " ��6
   8   .�5'      �� 	 � ��$
 0,2,0,0q�6'   �� 5(      	 � ��8
  6#   5)      �� 5+      " ��6
   .�5,      �� 	 � ��$
 60��6   �� 5-      	 � ��$
 25��6   5.      �� 50      " ��6
   8   .�51      �� 	 � ��$
 5,10,5,0q�6   �� 52      	 � ��$
 0,2,0,0q�6'   �� 53      	 � ��8
  6#   54      �� 56      " ��6
    57      �� 58       6   �� *
IsSelected+ �� $ TabItemL� True5:      .a�5;      �� 	 � ��$

 Bold"�6$   5<      5>      �� 5@      " ��6
   8   .�5A      �� 	 � ��
 5B      �� 5C      " ��6   .R�5D      � .+�5E   	   �� $��Border��6   $��0,0,0,2q�6   
�������._�5F   
   �� $��0,0,0,3q�6   
�������
�������._�5G      �� $ ContentSite��6   $��Center=�61   $��Center=�5H      $��Header��5I      $��	12,2,12,2q�6#   ��. 5K      5L      5M   
   5N   	    5O      P� 5P   	    �@��
IsSelected . . �5Q   
   �� $ Border��6   8
  	 � ��6<   �� 5R   
   $ Border��6   	 � ��$
 2,2,2,0q�6@   �� 5S   
   $ Border��6   	 � ��$
 	-2,0,0,-2q�67   5T   
   P� 5U   	    �@��
IsSelected .  . �5V   
   �� $ Border��6   	 � ��

 �����6<   �� 5W   
   $ Border��6   	 � ��$
 0,0,0,1q�6@   �� 5X   
   $ Border��6   	 � ��$
 0,0,0,0q�67   5Y   
   5[      5]      5^      �� 5`      " ��6
   .�5a      �� 	 � ��$
 95��6   �� 5b      	 � ��$
 25��6   5c      �� 5e      " ��6
   8   .�5f      �� 	 � ��
 5g      �� 5h      " ��6   .R�5i      � .+�5j   	   �� $��Border��6   
������ ._�5k   
   �� $��1,1,0,0q�6   8�� ._�5l      �� $ ContentSite��6   $��Center=�61   $��Center=�5m      $��Header��5n      $��	12,2,12,2q�6#   ��. 5p      5q      5r   
   5s   	    5t      P� 5u   	    �@��
IsSelected . . �5v   
   �� $ Border��6   8
  	 � ��6;   �� 5w   
   $ Border��6   8
  	 � ��6<   �� 5x   
   $ Border��6   	 � ��$
 0,0,0,2q�6@   �� 5y   
   $ Border��6   	 � ��$
 
0,0,0,-1.5q�67   5z   
   P� 5{   	    �@��
IsSelected .  . �5|   
   �� $ Border��6   8
  	 � ��6<   �� 5}   
   $ Border��6   	 � ��$
 0,0,0,1q�6@   �� 5~   
   $ Border��6   	 � ��$
 
0,0,0,-1.5q�67   5   
   5�      5�      5�      �� 5�      " ��6
   8   .�5�      �� 	 � ��
 5�      �� 5�      " ��6   .R�5�      � .+�5�   	   �� $��Border��6   
������ ._�5�   
   �� $��0,1,1,0q�6   8�� ._�5�      �� $ ContentSite��6   $��Center=�61   $��Center=�5�      $��Header��5�      $��	12,2,12,2q�6#   ��. 5�      5�      5�   
   5�   	    5�      P� 5�   	    �@��
IsSelected . . �5�   
   �� $ Border��6   8
  	 � ��6;   �� 5�   
   $ Border��6   8
  	 � ��6<   �� 5�   
   $ Border��6   	 � ��$
 0,0,0,2q�6@   �� 5�   
   $ Border��6   	 � ��$
 
0,0,0,-1.5q�67   5�   
   P� 5�   	    �@��
IsSelected .  . �5�   
   �� $ Border��6   8
  	 � ��6<   �� 5�   
   $ Border��6   	 � ��$
 0,0,0,1q�6@   �� 5�   
   $ Border��6   	 � ��$
 
0,0,0,-1.5q�67   5�   
   5�      5�      5�      �� 5�      " ��6
   8   .�5�      �� 	 � ��$	
 120��6   �� 5�      	 � ��$
 20��6   �� 5�      	 � ��$
 INSERT E_XPRESSION��6    5�      �� 5�       6   �� *SQLIndexInExpression+ True6;   .a�5�      �� 	 � ��$
 EDIT E_XPRESSION��6!   6<   5�      5�      �� 5�      6!     5�      6%     5�      6,     5�      63     5�      6   5�      .��5�      �  � RowDefinitions 5�      �� 5�      $��* �6   �� 5�      $
��Auto �6   .+�5�      � -   5        $��mainGrid��5�      $��0��6	    � ColumnDefinitions 5�      �� 5�      $��2* �6   $	��280��6"    5�      �� 5�      $��5 �6   �� 5�      $��* �6   $��80��6   �� 5�      $
��Auto �6   .+�5�      ]� $��2��6   $��1��5�   
   ��.  $	��Top=�5�       $��0 0 0 5q�68    K� ZIndex$
 1000��5�   
   .+�5�      �� $��Right=�6   #��[ 5�   	   ��5�   	   �� *ExpressionCommand+y�5�   	   �� *SQLTabSelected+  VisibilityConverter �� 	Converter# [ 5�      �� 5�      $��0q�6   
�������$��1��5�   	   ._�5�      �� -   5         ��Name$ editorTestTabControl��5�      
�������#��[ 5�   l    �� SelectedIndex 6   �� *EditorTestTabSelectedIndex+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�.@�5�      �� $	��SQL��6   #��[
 6   
�������.��5�      � -   5         �Name$ sqlGrid��5�       5�   	   �� 5�   
   $
��571* �6   �� 5�   
   $��3* �6   $��3��6&   �� 5�   
   $	��204��6   $
��204* �6+    5�   	   �� 5�   
   $��* �6   .+�5�   	   B   <Twenty57.Linx.Components.Database.UI.Controls.MvvmTextEditor  -   5           Name$  	sqlEditor��5�   '   $��0��6:     TitlebarActive#��[ 6J   $��Stretch=�6w   $��Consolas)�5�   
   $
��Auto=�5�   
   $
��Auto=�5�   
   )  #GongSolutions.Wpf.DragDrop.DragDrop!   IsDropTarget! . "   BoundCaretOffset" 5�   
   �� *
SQLIndex+$ TwoWay=�#   	BoundText# 5�   
   �� *SQL+$ PropertyChanged=�$ TwoWay=�$   DropHandler$ 5�   *   �� '  !ICSharpCode.AvalonEdit.TextEditor%   Text.% 6O    5�   
   -	  'ICSharpCode.AvalonEdit.Editing.TextArea��%T   '	        �� 5�      " 	 6   .�5�      �� 	 � ��

 �   �6)   5�      5�      .% 63   5�   
   �� 5�   	   $��1��6   $��	0,-2,0,-2q�5�      $��Center=�5�      $��Stretch=�5�      & �� ShowsPreview& . ���#B3B3B3$��1,0,0,0q�5�      $��3��5�      �� 5�   	   $��1q�6   $��
0,-2,-2,-2q�6$   $��3��68   #��[ 6H   ���#BBBBBB$��2��6�   ._�5�   
   �� ' �� TabStripPlacement$	' Top=�6   #��[ 60    5�   	   �� *SQLSelectedTabIndex+$ TwoWay=�$ PropertyChanged=�.@�5�      �� #��[ 6   ��5�      �� 5�      $��	Variables��6   #��[	 6(   .��5�      �� #��[ 6   $��3q�6N   $��-3q�6b   ._�5�      <
  6Twenty57.Linx.Plugin.UI.Editors.Static.VariablesEditor
  $��0q�6p   ( 
  Designer( 6-   �� *
Designer+$ OneWay=�) � DataContext) 6X   �� 6p   5�      5�      �� 5�      #��[ 6   $��0q�6?   ��5�      �� 5      $��Objects��6   #��[	 6&   .��5     �� #��[ 6   $��3q�6N   $��-3q�6b   ._�5     �  5     �� 5     $	��85* �6   �� 5     $	��23* �6   �� 5     $
��230* �6   .+�5	     �� #��[ 6   $��3��6S   .+�5
     ��   AppIcon_Loading" 	 WpfAnimatedGif.ImageBehavior*   AnimatedSource#* [ 6   $��20��6Y   $��20��6e   $��5q�6�   $��0��6�   y�6p   �� *LoadingDatabaseObjects+# [ 6�   5     R� 5     $��0,0,0,0q�6   +   IsDragSource+ . $��3��6�   ��6)   �� *DatabaseObjects+y�6Q   �� *DatabaseObjectsLoaded+# [ ,   DragHandler, 6�   ��  5     - �� DataTypec   ]Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel.ComplexNodeViewModel. �� ItemsSource/ �� Orientation0 � ContextMenu1 m� Commandb   \Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel.SimpleNodeViewModel��%�  (��       5  )   *, +)7  (��     5     *, +)�� 5     "-  6)   . 6X   �� *
Children+.R�5     �� $/ 
Horizontal=�6   .+�5     �� ��6   �� *Text+0 5     �� 5     y�6    �� *ContextMenuVisible+$ OneWay=�8   .@�5     m� $��Generate Select��6   1 67   �� *GenerateSelectCommand+$ OneWay=�m� 5     $��Generate Insert��6   1 67   �� *GenerateInsertCommand+$ OneWay=�m� 5     $��Generate Update��6   1 67   �� *GenerateUpdateCommand+$ OneWay=�5     5     5     5     �� 5     "-  6   .R�5     �� $/ 
Horizontal=�6   $��0,1,2,1q�65   .+�5     �� ��6   �� *Text+5     5     5      ��5!     �� 5"     " Q�6   .�5#     �� 	 �@Q�
IsExpanded
 6.   �� *
IsExpanded+$ TwoWay=��� 5$     	 �@Q�
IsSelected
 6.   �� *
IsSelected+$ TwoWay=�5%     5'     5(     5)     5*     5+     5,  
   5-  	   5.     �� 5/     $
��Test��6   #��[
 6   .��50     d   ^Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers.TestSqlPaneScrollViewer  -   5        2  Name$2 testSqlPane��50  (   $��4��6=   #��[ 6M   $
��Auto=�6z   .��51  	   � -   5        $ testGrid��51      52  
   �� 53     $��* �6    55  
   �� 56     $
��100* �6   $	��150��6)   �� 57     $��3 �6   �� 58     $
��400* �6   $	��150��6)   .+�5;  
   �� $��0��6     ForegroundLightBrush#��[ 5=  
   $��Stretch=�5>  
   $��0q�5?  
   {�5<  
   �� *CanExecuteSQL+$ OneWay=�._�5@     �� -   5        3 ��Name$3 parameterScrollViewer��5@     $
��Auto=�67   $��0q�5B     $��Stretch=�6   ��5A     �� $ testSqlPane��4 �� Path$4 ActualHeight�# [ .��5C     �� $/ Vertical=�6   5 �� CanVerticallyScroll5 .  $��Stretch=�6J   .+�5D     �� $��5q�6     ForegroundDarkBrush#��[ 6"   
Test SQL6^   �� 5E     -   5        6 ��Name$6 SQLParametersList��5E     $��5q�61   ��6<   �� *SQLParameters+.  (System.Windows.Interactivity.Interaction7   	Behaviors7 5F       5G      5I     `   ZTwenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel.SQLParameterModel8 � ToolTip��%�  (��       5J     *, +)7  �� 6   "-  6   .R�5K     �  5L     �� 5M     $	��100��6#   $��* �62    5O     �� 5P     $��* �6    �� 5Q     $��* �6    .+�5S     �� -	   5        $��0��5S     $
��Left=�5U     $��CharacterEllipsis=�5V     ��62   �� *Name+$ OneTime=�8 5W     �� *Name+$ OneTime=���5X     �� $ parameterScrollViewer��$4 ActualWidth�8   �� 5Y     $��1��6   ��6&   �� *Value+5Z     5[     5\     5]     ]� 5^     ��.  #��[ 6-   .+�5_     �� $
��Left=�6   $��5q�6+   $��_EXECUTE SQL��66   $	��102��6M   $��20��6Y   ��6e   �� *ExecuteSQLCommand+$ OneWay=�5`     5a     5b     5c     �� 5d  
   $��1��6   $��0��5e     $��Center=�5f     $��Stretch=�5g     & . $��0q�5i     $��3��5j     �� 5k  
   -
   5        9 ��Name$9 resultsStackPanel��5k     $��2��60   y�5l  
   �� *HasQueryResults+# [ $ OneWay=�.+�5m     �� $��5q�6   $/ 
Horizontal=�6!   .+�5n     �� $
��Bold"�6   $��12(�6(   	Results6?   �� 5o     $��5 0 0 0q�6   $��Bottom=�6'   y�5p     �� *MoreResults+# [ $ OneWay=���5q     �� *ResultsCount+: �� StringFormat$: (first {0})��5r     &   System.Windows.Controls.DataGrid  5s     -   5        ;   AutoGenerateColumns; . <   
IsReadOnly< . =   CanUserSortColumns= .  ��5s     �� $ testSqlPane��$4 ActualHeight�# [ ��5w     �� *QueryResults+$ OneWay=�7 5y       5z     5|     5}     �� 5~  
   $��2��6   > �� TextWrapping$
> Wrap=�5     ? �� 
IsReadOnly? . $
��Auto=�5�     ��5�     �� *
QueryError+$ OneWay=�y�5�     �� *HasQueryError+# [ $ OneWay=�5�  
   5�  	   5�     5�     5�     5�     ]� 5�     $��1��6   $��Stretch=�6   ��.   $  DialogWindowButtonDockPanelStyle#��[ 6O   .+�5�     �� $��_CANCEL��6   @ �� IsCancel@ . $��Right=�6.   $��2��6E   �� 5�       DialogPrimaryButtonStyle#��[ 6   $	��_OK��6>   $��0 0 10 0q�6w   $��Right=�6�   $��1��6�   ��6L   �� *	OKCommand+$ OneWay=�6�   5�     5�     5�     