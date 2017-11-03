namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel
{
	public class SimpleNodeViewModel : NodeViewModel
	{
		public SimpleNodeViewModel(string text, NodeViewModel parent = null)
			: this(text, false, parent)
		{ }

		public SimpleNodeViewModel(string text, bool allowDrag, NodeViewModel parent = null)
			: base(parent)
		{
			Text = text;
			AllowDrag = AllowDrag;
		}
	}
}
