using System.ComponentModel;
using System.Windows.Controls;
using Twenty57.Linx.Components.Database.UI.Controls;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser
{
	public partial class TableInLineEditor
	{
		public TableInLineEditor()
		{
			InitializeComponent();
			if (!DesignerProperties.GetIsInDesignMode(this))
				ConfigureStringEditor();
		}

		private void ConfigureStringEditor()
		{
			TextBox editor = (new StringEditor()).Editor;
			editor.IsReadOnly = true;
			mainPanel.Children.Add(editor);
		}
	}
}
