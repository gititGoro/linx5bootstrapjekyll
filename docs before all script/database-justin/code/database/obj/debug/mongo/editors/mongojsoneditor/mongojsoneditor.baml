   M S B A M L   `   `   `  ����U  QGongSolutions.Wpf.DragDrop, Version=0.1.4.0, Culture=neutral, PublicKeyToken=nullkLclr-namespace:GongSolutions.Wpf.DragDrop;assembly=GongSolutions.Wpf.DragDropGongSolutions.Wpf.DragDrop  5   
   ] YICSharpCode.AvalonEdit, Version=5.0.3.0, Culture=neutral, PublicKeyToken=9cc39be672370310oLclr-namespace:ICSharpCode.AvalonEdit.Editing;assembly=ICSharpCode.AvalonEditICSharpCode.AvalonEdit.Editing 5   
   U QTwenty57.Linx.Plugin.UI, Version=16.0.1.226, Culture=neutral, PublicKeyToken=nullrNclr-namespace:Twenty57.Linx.Plugin.UI.Windows;assembly=Twenty57.Linx.Plugin.UITwenty57.Linx.Plugin.UI.Windows 5   
   rNclr-namespace:Twenty57.Linx.Plugin.UI.Helpers;assembly=Twenty57.Linx.Plugin.UITwenty57.Linx.Plugin.UI.Helpers 5	   
   \ XTwenty57.Linx.Components.Database, Version=1.3.0.0, Culture=neutral, PublicKeyToken=nullm;clr-namespace:Twenty57.Linx.Components.Database.UI.Controls-Twenty57.Linx.Components.Database.UI.Controls 5
   
   �Fclr-namespace:Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor8Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor 5   
   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      �Uclr-namespace:Twenty57.Linx.Plugin.UI.Editors.Static;assembly=Twenty57.Linx.Plugin.UI&Twenty57.Linx.Plugin.UI.Editors.Static 5   
   N   HTwenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.MongoJsonEditor   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   
   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   
   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   
   @d2http://schemas.microsoft.com/expression/blend/2008     5   
   [ddLclr-namespace:GongSolutions.Wpf.DragDrop;assembly=GongSolutions.Wpf.DragDrop     5   
   c
avaloneditLclr-namespace:ICSharpCode.AvalonEdit.Editing;assembly=ICSharpCode.AvalonEdit     5   
   ]uiNclr-namespace:Twenty57.Linx.Plugin.UI.Windows;assembly=Twenty57.Linx.Plugin.UI     5   
   chelpers1Nclr-namespace:Twenty57.Linx.Plugin.UI.Helpers;assembly=Twenty57.Linx.Plugin.UI     5	   
   Pcontrols;clr-namespace:Twenty57.Linx.Components.Database.UI.Controls     5
   
   bmongoJsonEditorFclr-namespace:Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor     5   
   9system&clr-namespace:System;assembly=mscorlib     5      qpluginUIEditorsUclr-namespace:Twenty57.Linx.Plugin.UI.Editors.Static;assembly=Twenty57.Linx.Plugin.UI     5   
     � ShowInTaskbar  .   � Title$ 
Mongo JSON��5   
    � WindowStartupLocation$ CenterOwner=�5   
   $	��800��5   
   $	��800��6   $	��450��5   
   $	��450��6      DialogWindowStyle#��[  5   
   2  ,Twenty57.Linx.Plugin.UI.Helpers.DialogCloser   DialogResult 5   
   �� *DialogResult+ +� FocusedElement 5   
   ��  �� ElementName$ 	sqlEditor�� � 	Resources 5        VisibilityConverter �� 
TargetType  DialogButtonStyle �� BasedOn  expressionButtonStyle	 �� Property
 �� Value �� Triggers �� Binding �� Value   booleanToVisibilityConverter��%�  &	       &	      7  &	 n    �� 5      6!   �� 5      " ��6
   8   .�5      �� 	 � ��$
 20��6   �� 5      	 � ��$	
 120��6   �� 5      	 � ��$	
 0 0q�6   �� 5      	 � ��$
 INSERT E_XPRESSION��6    5       �� 5!       6   �� *CaretInExpression+ True68   .a�5"      �� 	 � ��$
 EDIT E_XPRESSION��6!   6<   5#      5%      �� 5'      6!   5)      .��5+      �  � RowDefinitions 5,      �� 5-      $��* �6   �� 5.      $
��Auto �6   .+�51      � -   5        $��mongoJsonEditorGrid��51   	   $��0��6$    � ColumnDefinitions 52      �� 53      $
��1.5* �6   $	��210��6$   �� 54      $
��Auto �6   �� 55      $��2* �6   $	��280��6"   �� 56      $
��Auto �6   �� 57      $��1* �6   $	��180��6"    59      �� 5:      $��30 �6   �� 5;      $��* �6   $��80��6   �� 5<      $
��Auto �6   �� 5=      $
��Auto �6   $��40��6"   .+�5@      ]� $��0��6   $��0��5A   
   ��.  $��Stretch=�5C   
   $��0 0 0 5q�6&   .+�5D      �� $
��Left=�6   $��Database Fields��5E      $��5q�6"     ForegroundDarkBrush#��[ 6-   5F      �� 5H      $��1��6   $��0��5I      $��4��5J      $��Center=�5K      $��Stretch=�5L       �� ShowsPreview . $��3��5N      ]� 5P      $��2��6   $��0��5Q   
   ��.  $��Stretch=�5R       $��0 0 0 5q�6<   .+�5S      �� $��Right=�6   #��[ 5T   	   ��5U   	   �� *ExpressionCommand+5V      �� 5X      $��0��6   $��1��6   $��3��6)   $��1q�6:     TitlebarActive#��[ 6N   ._�5Y      �� -   5         ��Name$ templateTabs��5Y   !   $��0��6   6!   5Z      �� 5[      $��1q�6   
�������$��2��5]   	   $��3��5^      $��1��5_   	   ._�5`      �� -   5        $��
EditCanvas��5`      .+�5a      �� -   5        $��	Watermark��5a      $��Italic$�6     ForegroundDisabledBrush#��[ 61   ��6g   �� *WatermarkText+6�   B  <Twenty57.Linx.Components.Database.UI.Controls.MvvmTextEditor  5b      -   5          Name$ 
textEditor��5b        ForegroundLightBrush#��[ 5e      $��1��5f   	   $��Consolas)�5g   	   $
��Auto=�5h   	   $
��Auto=�5i   	   )   #GongSolutions.Wpf.DragDrop.DragDrop   IsDropTarget . #��[ 5m   	   $��
0,0,-0.4,3q�5n   	   ��5c      �� *ActualWidth+$ 
EditCanvas����5d      �� *ActualHeight+$ 
EditCanvas��   BoundCaretOffset 5j   	   �� *
CaretIndex+ �� Mode$ TwoWay=�   DropHandler 5l   
   �� '  !ICSharpCode.AvalonEdit.TextEditor   Text. 5o       5p      -  'ICSharpCode.AvalonEdit.Editing.TextArea��%T   '        �� 5q   	   "  6   .�5r   
   �� 	 � ��

 �   �6'   5s   
   5t   	   . 6+   5u      5v      5w      �� 5y      $��3��6   $��0��6"   $��4��6/   $��Center=�5z      $��Stretch=�5{       . $��3��5}      5~      �� 5      $��4��6   $��1��6   $��3��6)   $��1q�6:   #��[ 6N   ._�5�      <  6Twenty57.Linx.Plugin.UI.Editors.Static.VariablesEditor     Designer 6&   �� *
Designer+$ OneWay=� � DataContext 6Q   �� 5�      5�      ]� 5�      $��1��6   $��Stretch=�6    $	  DialogWindowButtonDockPanelStyle#��[	 69   ��.  .+�5�      �� $��_CANCEL��6    �� IsCancel . $��Right=�6.   $��2��6E   �� 5�       
 DialogPrimaryButtonStyle#��[
 6   $	��_OK��6>   $��0 0 10 0q�6w   $��Right=�6�   $��1��6�   ��6L   �� *	OKCommand+$ OneWay=�6�   5�      5�      5�      