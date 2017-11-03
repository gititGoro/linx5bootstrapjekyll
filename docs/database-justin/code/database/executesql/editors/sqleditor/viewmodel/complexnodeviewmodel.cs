
namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel
{
	public class ComplexNodeViewModel : NodeViewModel
	{
		public ComplexNodeViewModel(NodeViewModel parent = null)
			: base(null)
		{ }

		public ComplexNodeViewModel(string text, NodeViewModel parent = null, bool contextMenuVisible = false)
			: this(parent)
		{
			Text = text;
			ContextMenuVisible = contextMenuVisible;
		}

		public bool ContextMenuVisible { get; }
	}
}