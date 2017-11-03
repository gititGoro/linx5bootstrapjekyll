using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml;
using Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor
{
	public partial class SQLEditor
	{
		private const string MainGridColumnKeyFormat = "SQLEditor_MainGridColumn_{0}";
		private const string SQLGridColumnKeyFormat = "SQLEditor_SQLGridColumn_{0}";
		private const string TestGridColumnKeyFormat = "SQLEditor_TestGridColumn_{0}";

		private SQLEditor(SQLViewModel viewModel)
			: this()
		{
			base.DataContext = viewModel;
		}

		private SQLEditor()
		{
			InitializeComponent();
			ApplySQLHighlighting();
		}

		protected override bool PersistLayout { get; } = true;

		public static bool Display(SQLViewModel viewModel)
		{
			SQLEditor editor = new SQLEditor(viewModel);
			editor.Owner = Application.Current.MainWindow;
			return editor.ShowDialog() ?? false;
		}

		protected override void ApplyAdditionalLayoutInfo(Dictionary<string, object> layoutInfo)
		{
			GridLength[] originalMainGridColumnWidths = this.mainGrid.ColumnDefinitions.Select(rd => rd.Width).ToArray();
			GridLength[] originalSqlGridColumnWidths = this.sqlGrid.ColumnDefinitions.Select(rd => rd.Width).ToArray();
			GridLength[] originalTestGridColumnWidths = this.testGrid.ColumnDefinitions.Select(rd => rd.Width).ToArray();

			try
			{
				var gridLengthConverter = new GridLengthConverter();
				for (int index = 0; index < this.mainGrid.ColumnDefinitions.Count; index++)
				{
					var mainGridColumnKey = string.Format(MainGridColumnKeyFormat, index);
					if (!layoutInfo.ContainsKey(mainGridColumnKey))
						continue;

					this.mainGrid.ColumnDefinitions[index].Width = (GridLength)gridLengthConverter.ConvertFromString((string)layoutInfo[mainGridColumnKey]);
				}

				for (int index = 0; index < this.sqlGrid.ColumnDefinitions.Count; index++)
				{
					var sqlGridColumnKey = string.Format(SQLGridColumnKeyFormat, index);
					if (!layoutInfo.ContainsKey(sqlGridColumnKey))
						continue;

					this.sqlGrid.ColumnDefinitions[index].Width = (GridLength)gridLengthConverter.ConvertFromString((string)layoutInfo[sqlGridColumnKey]);
				}

				for (int index = 0; index < this.testGrid.ColumnDefinitions.Count; index++)
				{
					var testGridColumnKey = string.Format(TestGridColumnKeyFormat, index);
					if (!layoutInfo.ContainsKey(testGridColumnKey))
						continue;

					this.testGrid.ColumnDefinitions[index].Width = (GridLength)gridLengthConverter.ConvertFromString((string)layoutInfo[testGridColumnKey]);
				}
			}
			catch
			{
				for (int index = 0; index < originalMainGridColumnWidths.Length; index++)
					this.mainGrid.ColumnDefinitions[index].Width = originalMainGridColumnWidths[index];

				for (int index = 0; index < originalSqlGridColumnWidths.Length; index++)
					this.sqlGrid.ColumnDefinitions[index].Width = originalSqlGridColumnWidths[index];

				for (int index = 0; index < originalTestGridColumnWidths.Length; index++)
					this.testGrid.ColumnDefinitions[index].Width = originalTestGridColumnWidths[index];
			}
		}

		protected override Dictionary<string, object> FetchAdditionalLayoutInfo()
		{
			var additionalLayoutInfo = new Dictionary<string, object>();
			FetchGridLayoutInfo(additionalLayoutInfo);
			return additionalLayoutInfo;
		}

		private void FetchGridLayoutInfo(Dictionary<string, object> layoutInfo)
		{
			var gridLengthConverter = new GridLengthConverter();
			for (int index = 0; index < this.mainGrid.ColumnDefinitions.Count; index++)
			{
				ColumnDefinition columnDefinition = this.mainGrid.ColumnDefinitions[index];
				layoutInfo.Add(string.Format(MainGridColumnKeyFormat, index), gridLengthConverter.ConvertToString(columnDefinition.Width));
			}

			for (int index = 0; index < this.sqlGrid.ColumnDefinitions.Count; index++)
			{
				ColumnDefinition columnDefinition = this.sqlGrid.ColumnDefinitions[index];
				layoutInfo.Add(string.Format(SQLGridColumnKeyFormat, index), gridLengthConverter.ConvertToString(columnDefinition.Width));
			}

			for (int index = 0; index < this.testGrid.ColumnDefinitions.Count; index++)
			{
				ColumnDefinition columnDefinition = this.testGrid.ColumnDefinitions[index];
				layoutInfo.Add(string.Format(TestGridColumnKeyFormat, index), gridLengthConverter.ConvertToString(columnDefinition.Width));
			}
		}

		private void SQLParameterNameLabel_Loaded(object sender, RoutedEventArgs e)
		{
			// http://stackoverflow.com/questions/9264398/how-to-calculate-wpf-textblock-width-for-its-known-font-size-and-characters
			TextBlock label = (TextBlock)sender;
			var formattedText = new FormattedText(label.Text, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(label.FontFamily, label.FontStyle, label.FontWeight, label.FontStretch), label.FontSize, label.Foreground);
			label.Width = formattedText.Width + 5;
		}

		private void GridSplitter_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			Grid parent = FindAncestor<Grid>(sender as GridSplitter);
			if (null == parent)
				return;

			if ((bool)e.NewValue)
			{
				parent.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
				parent.RowDefinitions[3].Height = new GridLength(2, GridUnitType.Star);
			}
			else
			{
				parent.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
				parent.RowDefinitions[3].Height = GridLength.Auto;
			}
		}

		private void resultsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			string existingHeader = e.Column.Header as string;
			if (null == existingHeader)
				return;

			e.Column.Header = existingHeader.Replace("_", "__");
		}

		private void ApplySQLHighlighting()
		{
			using (Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Twenty57.Linx.Components.Database.Resources.SQLHighlightingDefinition.xshd"))
			{
				using (XmlTextReader reader = new XmlTextReader(resourceStream))
				{
					this.sqlEditor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
				}
			}
		}

		public static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
		{
			do
			{
				if (current is T)
					return (T)current;

				current = VisualTreeHelper.GetParent(current);
			}
			while (current != null);

			return null;
		}
	}
}
