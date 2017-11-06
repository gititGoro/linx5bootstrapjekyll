using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers
{
	// http://stackoverflow.com/questions/2189053/disable-mouse-wheel-on-itemscontrol-in-wpf
	/// <summary>
	/// Captures and forwards MouseWheel events so that a nested control does not
	/// prevent an outer scrollable control from scrolling downwards.
	/// </summary>
	public sealed class MouseWheelBehavior : Behavior<UIElement>
	{
		protected override void OnAttached()
		{
			base.OnAttached();
			AssociatedObject.PreviewMouseWheel += AssociatedObject_PreviewMouseWheel;
		}

		protected override void OnDetaching()
		{
			AssociatedObject.PreviewMouseWheel -= AssociatedObject_PreviewMouseWheel;
			base.OnDetaching();
		}

		private void AssociatedObject_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (e.Delta > 0)
			{
				ScrollViewer scrollViewer = GetScrollViewer(AssociatedObject);
				if ((scrollViewer != null) && (scrollViewer.VerticalOffset > 0))
					return;
			}

			var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
			e2.RoutedEvent = UIElement.MouseWheelEvent;

			AssociatedObject.RaiseEvent(e2);
			e.Handled = e2.Handled;
		}

		// http://blog.kervinramen.com/2010/10/wpf-datagrid-controlling-scrollbar.html
		private static ScrollViewer GetScrollViewer(DependencyObject o)
		{
			for (int i=0; i<VisualTreeHelper.GetChildrenCount(o); i++)
			{
				var child = VisualTreeHelper.GetChild(o, i);
				if ((child != null) && (child is ScrollViewer))
					return child as ScrollViewer;
				ScrollViewer sub = GetScrollViewer(child);
				if (sub != null)
					return sub;
			}
			return null;
		}
	}
}
