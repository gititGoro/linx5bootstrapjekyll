using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor
{
	public partial class MongoJsonEditor
	{
		private const string GridColumnKeyFormat = "MongoJsonEditor_GridColumn_{0}";

		private MongoJsonEditor(MongoJsonEditorViewModel viewModel, FunctionDesigner context)
			: this()
		{
			base.DataContext = viewModel;
			viewModel.Designer = context;
			base.Title = viewModel.Title;
			foreach (var source in viewModel.TemplateSources)
			{
				var treeView = new TemplateTreeView(source);
				templateTabs.Items.Add(treeView);
			}

			viewModel.InitialiseDocument(this.textEditor.Document);
			Watermark.Visibility = String.IsNullOrEmpty(textEditor.Text) ? Visibility.Visible : Visibility.Hidden;
			textEditor.TextChanged += (sender, e) => HideWatermark();
			textEditor.GotFocus += (sender, e) => HideWatermark();
			textEditor.LostFocus += (sender, e) => HideWatermark();
		}

		private void HideWatermark()
		{
			Watermark.Visibility =
				(String.IsNullOrEmpty(textEditor.Text) &&
				!textEditor.IsKeyboardFocusWithin) ? Visibility.Visible : Visibility.Hidden;
		}

		private MongoJsonEditor()
		{
			InitializeComponent();
			ApplySQLHighlighting();
		}

		protected override bool PersistLayout
		{
			get
			{
				return true;
			}
		}

		public static bool Display(MongoJsonEditorViewModel viewModel, FunctionDesigner context)
		{
			MongoJsonEditor editor = new MongoJsonEditor(viewModel, context);
			editor.Owner = Application.Current.MainWindow;
			return editor.ShowDialog() ?? false;
		}
		private void resultsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			string existingHeader = e.Column.Header as string;
			if (null == existingHeader)
				return;

			e.Column.Header = existingHeader.Replace("_", "__");
		}

		protected override void ApplyAdditionalLayoutInfo(Dictionary<string, object> layoutInfo)
		{
			GridLength[] originalColumnWidths = this.mongoJsonEditorGrid.ColumnDefinitions.Select(cd => cd.Width).ToArray();

			try
			{
				for (int index = 0; index < this.mongoJsonEditorGrid.ColumnDefinitions.Count; index++)
				{
					var gridColumnKey = string.Format(GridColumnKeyFormat, index);
					if (layoutInfo.ContainsKey(gridColumnKey))
						this.mongoJsonEditorGrid.ColumnDefinitions[index].Width = (GridLength)layoutInfo[gridColumnKey];
				}
			}
			catch
			{
				for (int index = 0; index < this.mongoJsonEditorGrid.ColumnDefinitions.Count; index++)
				{
					this.mongoJsonEditorGrid.ColumnDefinitions[index].Width = originalColumnWidths[index];
				}
			}
		}

		protected override Dictionary<string, object> FetchAdditionalLayoutInfo()
		{
			var additionalLayoutInfo = new Dictionary<string, object>();

			for (int index = 0; index < this.mongoJsonEditorGrid.ColumnDefinitions.Count; index++)
			{
				ColumnDefinition column = this.mongoJsonEditorGrid.ColumnDefinitions[index];
				additionalLayoutInfo.Add(string.Format(GridColumnKeyFormat, index), column.Width);
			}

			return additionalLayoutInfo;
		}

		private void ApplySQLHighlighting()
		{
			using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Twenty57.Linx.Components.Database.Resources.MongoJsonHighlightingDefinition.xshd"))
			{
				using (XmlTextReader reader = new XmlTextReader(resourceStream))
				{
					this.textEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
				}
			}
		}



		internal void OnDoubleClickTemplate(TemplateTreeItemViewModel templateTreeItemViewModel)
		{
			if (templateTreeItemViewModel == null) return;
			if (templateTreeItemViewModel.Template == null) return;
			(DataContext as MongoJsonEditorViewModel).InsertTemplate(templateTreeItemViewModel.Template);
		}
	}
}
