using System.Windows;

namespace Twenty57.Linx.Components.File.TextFileRead.Editors
{
	public partial class EditorResources
	{
		private static EditorResources instance;

		private EditorResources()
		{
			InitializeComponent();
		}

		public static DataTemplate TextFileReadFieldsInlineEditorTemplate
		{
			get { return Instance["TextFileReadFieldsInlineEditorTemplate"] as DataTemplate; }
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
