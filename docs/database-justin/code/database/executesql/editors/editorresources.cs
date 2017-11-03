using System.Windows;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors
{
	public partial class EditorResources
	{
		private static EditorResources instance;

		private EditorResources()
		{
			InitializeComponent();
		}

		public static DataTemplate ResultTypeInlineEditorTemplate
		{
			get { return Instance["ResultTypeInlineEditorTemplate"] as DataTemplate; }
		}

		private static EditorResources Instance
		{
			get
			{
				if (null == instance)
					instance = new EditorResources();

				return instance;
			}
		}
	}
}
