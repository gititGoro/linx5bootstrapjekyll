using System.Windows;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors
{
	public partial class EditorResources
	{
		private static EditorResources instance = null;

		private EditorResources()
		{
			InitializeComponent();
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

		public static DataTemplate ProcedureParametersInlineEditorTemplate
		{
			get { return Instance["ProcedureParametersInlineEditorTemplate"] as DataTemplate; }
		}

		public static DataTemplate DataSetInlineEditorTemplate
		{
			get { return Instance["DataSetInlineEditorTemplate"] as DataTemplate; }
		}
	}
}
