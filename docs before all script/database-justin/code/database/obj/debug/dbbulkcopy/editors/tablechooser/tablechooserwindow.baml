   M S B A M L   `   `   `  ����U  QTwenty57.Linx.Plugin.UI, Version=16.0.1.226, Culture=neutral, PublicKeyToken=nullrNclr-namespace:Twenty57.Linx.Plugin.UI.Windows;assembly=Twenty57.Linx.Plugin.UITwenty57.Linx.Plugin.UI.Windows  5      rNclr-namespace:Twenty57.Linx.Plugin.UI.Helpers;assembly=Twenty57.Linx.Plugin.UITwenty57.Linx.Plugin.UI.Helpers  5      \ XTwenty57.Linx.Components.Database, Version=1.3.0.0, Culture=neutral, PublicKeyToken=nullq=clr-namespace:Twenty57.Linx.Components.Database.UI.Converters/Twenty57.Linx.Components.Database.UI.Converters 5      �Mclr-namespace:Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl?Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl 5	      �Yclr-namespace:Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser.ViewModelKTwenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser.ViewModel 5
      �Oclr-namespace:Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooserATwenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser 5      Z   TTwenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser.TableChooserWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      cpluginUINclr-namespace:Twenty57.Linx.Plugin.UI.Windows;assembly=Twenty57.Linx.Plugin.UI     5      jpluginUIHelpersNclr-namespace:Twenty57.Linx.Plugin.UI.Helpers;assembly=Twenty57.Linx.Plugin.UI     5      T
converters=clr-namespace:Twenty57.Linx.Components.Database.UI.Converters     5      kconnectioncontrolMclr-namespace:Twenty57.Linx.Components.Database.UI.Controls.ConnectionControl     5	      klocalYclr-namespace:Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser.ViewModel     5
      bwindowOclr-namespace:Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser     5         DialogWindowStyle#��[  5      $	��440��5      $	��600��6   $	��380��5      $	��550��6     � Title$  Table Chooser��5       � ShowInTaskbar .   � WindowStartupLocation$ CenterOwner=�5      2   ,Twenty57.Linx.Plugin.UI.Helpers.DialogCloser   DialogResult 5      �� *DialogResult+ � 	Resources 5      R  LTwenty57.Linx.Components.Database.UI.Converters.IsBusyToMouseCursorConverter  MouseCursorConverter  BoolToVisibilityConverterY  STwenty57.Linx.Components.Database.UI.Converters.InverseBooleanToVisibilityConverter $  InverseBoolToVisibilityConverter �� 
TargetType  EnableDisableStyle �� Triggers P� Property P� Value	 �� Property  ForegroundDarkBrush
 �� Value  ScrollBarPlaceholderStyle �� Binding �� Path �� RelativeSource �� AncestorType �� Value �� 
TargetType J� SnapsToDevicePixels ��Name �� Content ��Name �� Triggers �� 
TargetName ]� 
Conditions �� Property �� Value  SelectedBackgroundBrush  ForegroundLightBrush 	 ForegroundDisabledBrush��%�  &	       &	      &	 &     &	 9     7  &	 �     '�� �    7  7  7  7 	   5      6,   �� 5      6!     5      63   �� 5      " ��6
    5      P� 5       � {� . . �5      �� 	 � ��8
   6$   5      P� 5       � {� .  . �5       �� 	 � ��

 �����6$   5!      5#      �� 5%      " ��6
    5&      �� 5'       6   �� $= 7ListBoxScrollViewer.ComputedVerticalScrollBarVisibility� �� $ window:TableChooserWindowL� Visible6�   .a�5(      �� 	 � ��$
 12��6   5)      5+      �� 5-      " ��6
   .�5.      �� 	 �@J�SnapsToDevicePixels
 . 6+   �� 5/      	 � ��8
   6"   �� 50      	 � ��
 51      �� 52      " ��6   .R�53      �� $��Border��6    . 
������ ._�54   	   �� .+�55   
   �� $ gridrowPresenter��6   # z��69   �� 56   
   $ contentPresenter��6   #��z��65   $y�	Collapsed=�6q   57   
   58   	    59      P� 5:   	    66   Z�  �@��ColumnCollection. �5;   
   �� $ contentPresenter��6   	 � y�$
 Visible=�6E   �� 5<   
   $ gridrowPresenter��6   	 � y�$
 	Collapsed=�6E   5=   
   ]� 5>   	    5?   
   �� 5@       �@��
IsSelected . 6+   �� 5A       �@J�	IsFocused . 6*   ./�5C   
   �� $ Border��6   8
  	 � ��6;   �� 5D   
   8
  	 � ��6'   5E   
   P� 5F   	    � {� .  . �5G   
   �� 8
  	 � ��6'   5H   
   5J      5L      5M      5N      .��5O      �  � RowDefinitions 5P      �� 5Q      $
��Auto �6   �� 5R      $
��Auto �6   �� 5S      $��* �6   �� 5T      $
��Auto �6   .+�5W      W  QTwenty57.Linx.Components.Database.UI.Controls.ConnectionControl.ConnectionControl  -   5          Name$ ConnectionControl��5W   5   $��0��6(   65   � 5Y      $��1��6	    5Z      �� 5[      $
��Auto �6   �� 5\      $
��Auto �6   �� 5]      $
��Auto �6    � ColumnDefinitions 5_      �� 5`      $
��Auto �6   �� 5a      $��* �6   .+�5d      �� $��0��6   $��0��6   ��6)   �� *LabelColumnWidth+ �� ElementName$ ConnectionControl�� �� Mode$ OneWay=��� 5e      $��0��6   $��1��6   $��5 5 5 0q�6,   
����  �y�6O   �� *HasConnectionString+ �� 	Converter# [ 1/Enter a valid connection string to list tables.6�   �� 5f      $��1��6   $��0��6   $��5 5 0 0q�6,   #��[ 6=   Table:6w   �� 5g      $��1��6   $��1��6   $��5 5 5 0q�6+     �� 
IsEditable  . ! �� Text! 5h      �� *	TableName+" �� SelectedIndex" 5i      �� *
TableIndex+$ OneWay=���5j      �� *Tables+$ OneWay=�# �� IsAsync# . $ � Cursor$ 5k      �� *IsLoadingTables+# [ �� 5l      $��2��6   $��0��6   $��2��6,   $��5 5 5 0q�6@   
���  ��$
��Wrap=�6b   y�6v   �� *HasErrorText+# [ ��6�   �� *	ErrorText+$ OneWay=�5m      ]� 5o      $��2��6   ��. {�6   �� *HasTableColumns+.+�5p      �� $	��Top=�6   $��0 5 0 0q�6!   $��35��62    
 BackgroundBrush#��[
 6>   ._�5q      �� $��5q�6   $��Center=�6   #��[ 66   $
��Bold"�6b   	Columns6}   5r      � 5t      $	��Top=�6
   $��5 5 5 0q�6   $��30��60    5u      �� 5v      $��2* �6   �� 5w      $��2* �6   �� 5x      $��* �6   �� 5y      $��* �6   �� 5z      $��* �6   �� 5{      $��* �6   �� 5|      $
��Auto �6   .+�5      �� -   5        % ��Name$% 
NameHeader��5      $��0��6!   $��1q�61   #��[
 6<   ._�5�      �� #��[ 6   $��Center=�6=   $��Center=�6Z   Name6{   5�      �� 5�      -   5        $% DataTypeHeader��5�      $��1��6%   $��1q�65   #��[
 6@   ._�5�      �� #��[ 6   $��Center=�6=   $��Center=�6Z   Type6{   5�      �� 5�      -   5        $% PrecisionHeader��5�      $��2��6&   $��1q�66   #��[
 6A   ._�5�      �� #��[ 6   $��Center=�6=   $��Center=�6Z   	Precision6�   5�      �� 5�      -   5        $% ScaleHeader��5�      $��3��6"   $��1q�62   #��[
 6=   ._�5�      �� #��[ 6   $��Center=�6=   $��Center=�6Z   Scale6|   5�      �� 5�      -   5        $% LengthHeader��5�      $��4��6#   $��1q�63   #��[
 6>   ._�5�      �� #��[ 6   $��Center=�6=   $��Center=�6Z   Length6}   5�      �� 5�      -   5        $% NullableHeader��5�      $��5��6%   $��1q�65   #��[
 6@   ._�5�      �� #��[ 6   $��Center=�6=   $��Center=�6Z   
Nullable6   5�      �� 5�      $��6��6   #��[ 6   5�      �� 5�      -   5        & ��Name$& ColumnsListView��5�      $	��Top=�6'   $��5 0 5 5q�6<     WindowBackgroundBrush#��[ 6M   $
��Auto=�6�   $��Disabled=�6�   ��5�      �� *TableColumns+ 5�      ' �� DataTypeb  \Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser.ViewModel.TableColumnModel��%�  (��       5�      *, +)7  7  7  7  7  7  �� 6   "'  6   .R�5�      � $��25��6    5�   	   �� 5�   
   $��2* �6   �� 5�   
   $��2* �6   �� 5�   
   $��* �6   �� 5�   
   $��* �6   �� 5�   
   $��* �6   �� 5�   
   $��* �6   .+�5�   	   �� $��0��6   $��1 0 1 0q�6    ���	#30D0D0D0��6H   �� $ 
NameHeader��$ ActualWidth�._�5�   
   �� $��2 0 2 0q�6   8��  $��Center=�6W   $��CharacterEllipsis=�6r   ��6�   �� *Name+5�   
   �� 5�   	   $��1��6   $��1 0 1 0q�6    ���	#30D0D0D0��6H   �� $ DataTypeHeader��$ ActualWidth�._�5�   
   �� $��2 0 2 0q�6   8�� $��Center=�6W   $��CharacterEllipsis=�6r   $��Courier New)�6�   ��6�   �� *
DataType+5�   
   �� 5�   	   $��2��6   $��1 0 1 0q�6    ���	#30D0D0D0��6H   �� $ PrecisionHeader��$ ActualWidth�._�5�   
   �� $��2 0 2 0q�6   8�� $��Center=�6W   $��CharacterEllipsis=�6r   ��6�   �� *	Precision+5�   
   �� 5�   	   $��3��6   $��1 0 1 0q�6    ���	#30D0D0D0��6H   �� $ ScaleHeader��$ ActualWidth�._�5�   
   �� $��2 0 2 0q�6   8�� $��Center=�6W   $��CharacterEllipsis=�6r   ��6�   �� *Scale+5�   
   �� 5�   	   $��4��6   $��1 0 1 0q�6    ���	#30D0D0D0��6H   �� $ LengthHeader��$ ActualWidth�._�5�   
   �� $��2 0 2 0q�6   8�� $��Center=�6W   $��CharacterEllipsis=�6r   ��6�   �� *Size+5�   
   �� 5�   	   $��5��6   $��1 0 1 0q�6    ���	#30D0D0D0��6H   �� $ NullableHeader��$ ActualWidth�._�5�   
   �� $��2 0 2 0q�6   8�� $��Center=�6W   $��CharacterEllipsis=�6r   ��6�   �� *
Nullable+5�   
   5�   	   5�      5�      5�      5�      ]� 5�      $��3��6   $��Stretch=�6   ��.   $  DialogWindowButtonDockPanelStyle#��[ 6O   .+�5�      �� $��_CANCEL��6   ( �� IsCancel( . $��Right=�6.   $��2��6E   �� 5�        DialogPrimaryButtonStyle#��[ 6   $	��_OK��6>   $��0 0 10 0q�6y   $��Right=�6�   $��1��6�   ��6L   �� *SaveCommand+$ OneWay=�6�   5�      5�      5�      