using System.Windows.Controls;
using System.Windows.Input;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.Helpers
{
	/// <summary>
	/// Handles the MouseWheel event only when the scroller can scroll in the associated direction.
	/// </summary>
	public class TestSqlPaneScrollViewer : ScrollViewer
	{
		protected override void OnMouseWheel(MouseWheelEventArgs e)
		{
			if (((e.Delta > 0) && (VerticalOffset > 0)) || ((e.Delta < 0) && (VerticalOffset < ExtentHeight - ViewportHeight)))
				base.OnMouseWheel(e);
		}
	}
}
