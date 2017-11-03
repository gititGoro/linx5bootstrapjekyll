using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Converters
{
	internal class TreeViewItemStyleConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			var editor = values[1] as MongoJsonEditorViewModel;
			var item = values[0] as TemplateTreeItemViewModel;
			var enabledBrush = values[3] as Brush;
			var disabledBrush = values[4] as Brush;

			if (editor != null && item != null)
			{
				if (!EnableItem(item, editor))
				{
					return disabledBrush;
				}
			}
			return enabledBrush;
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			return null;
		}

		private bool EnableItem(TemplateTreeItemViewModel treeItem, MongoJsonEditorViewModel editor)
		{
			if (treeItem.Template != null)
			{
				return editor.CanInsert(treeItem.Template);
			}
			foreach (TemplateTreeItemViewModel child in treeItem.Children)
				if (EnableItem(child, editor)) return true;
			return false;
		}
	}
}