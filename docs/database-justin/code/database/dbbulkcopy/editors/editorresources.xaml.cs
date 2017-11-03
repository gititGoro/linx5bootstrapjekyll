using System.Windows;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Editors
{
	public partial class EditorResources
	{
		private static EditorResources instance = null;

		private EditorResources()
		{
			InitializeComponent();
		}

		public static DataTemplate TableChooserInlineEditorTemplate
		{
			get { return Instance["TableChooserInlineEditorTemplate"] as DataTemplate; }
		}

		private static EditorResources Instance
		{
			get
			{
				if (instance == null)
					instance = new EditorResources();
				return instance;
			}
		}
	}
}
