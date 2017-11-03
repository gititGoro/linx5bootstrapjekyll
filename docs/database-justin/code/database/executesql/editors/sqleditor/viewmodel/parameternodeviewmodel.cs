namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel
{
	public class ParameterNodeViewModel : SimpleNodeViewModel
	{
		public ParameterNodeViewModel(string text, string dragText, NodeViewModel parent)
			: base(text, parent)
		{
			AllowDrag = true;
			SimpleDragText = dragText;
			ExtendedDragText = dragText;
		}
	}
}
