﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Twenty57.Linx.Components.File.TextFileRead.Editors.TextFileReadFieldsEditor.ViewModel;

namespace Twenty57.Linx.Components.File.TextFileRead.Editors.TextFileReadFieldsEditor
{
	public partial class TextFileReadFieldsEditor
	{
		private const string DataGridColumnKeyFormat = "ReadFieldsEditor_DataGridColumn_{0}";

		private FieldsEditorViewModel context;

		private DataGrid DataGridFields { get; set; }
		protected override bool PersistLayout
		{
			get
			{
				return true;
			}
		}

		public static bool Display(FieldsEditorViewModel context, Window owner = null)
		{
			TextFileReadFieldsEditor window = new TextFileReadFieldsEditor(context);
			window.Owner = owner ?? Application.Current.MainWindow;
			return window.ShowDialog() ?? false;
		}

		protected override void ApplyAdditionalLayoutInfo(Dictionary<string, object> layoutInfo)
		{
			DataGridLength[] originalColumnWidths = this.DataGridFields.Columns.Select(column => column.Width).ToArray();

			try
			{
				var dataGridLengthConverter = new DataGridLengthConverter();
				for (int index = 0; index < this.DataGridFields.Columns.Count; index++)
				{
					var dataGridColumnKey = string.Format(DataGridColumnKeyFormat, index);
					if (!layoutInfo.ContainsKey(dataGridColumnKey))
						continue;

					this.DataGridFields.Columns[index].Width = (DataGridLength)dataGridLengthConverter.ConvertFromString((string)layoutInfo[dataGridColumnKey]);
				}
			}
			catch
			{
				for (int index = 0; index < this.DataGridFields.Columns.Count; index++)
				{
					this.DataGridFields.Columns[index].Width = originalColumnWidths[index];
				}
			}
		}

		protected override Dictionary<string, object> FetchAdditionalLayoutInfo()
		{
			var additionalLayoutInfo = new Dictionary<string, object>();
			var dataGridLengthConverter = new DataGridLengthConverter();
			Regex widthSpecialCharacterMatch = new Regex(@"[^0-9.]");

			for (int index = 0; index < this.DataGridFields.Columns.Count; index++)
			{
				DataGridColumn dataGridColumn = this.DataGridFields.Columns[index];
				string dataGridColumnWidthStr = dataGridLengthConverter.ConvertToString(dataGridColumn.Width);

				if (widthSpecialCharacterMatch.IsMatch(dataGridColumnWidthStr))
				{
					additionalLayoutInfo.Add(string.Format(DataGridColumnKeyFormat, index), dataGridLengthConverter.ConvertToString(dataGridColumn.Width));
				}
				else
				{
					additionalLayoutInfo.Add(string.Format(DataGridColumnKeyFormat, index), Convert.ToString(dataGridColumn.ActualWidth));
				}
			}

			return additionalLayoutInfo;
		}

		public TextFileReadFieldsEditor(FieldsEditorViewModel context)
		{
			InitializeComponent();
			DataContext = context;
			this.context = context;
			SetupDataGrid();
			AddHandler(Validation.ErrorEvent, new RoutedEventHandler(OnErrorEvent));
		}

		private void OnErrorEvent(object sender, RoutedEventArgs e)
		{
			var ea = (ValidationErrorEventArgs)e;
			if (ea.Action == ValidationErrorEventAction.Added)
				context.ErrorCount++;
			if (ea.Action == ValidationErrorEventAction.Removed)
				context.ErrorCount--;
		}

		private void DelimiterChanged(object sender, RoutedEventArgs e)
		{
			txtOtherDelimiter.GetBindingExpression(TextBox.TextProperty).UpdateSource();
		}

		private void FileTypeChanged(object sender, RoutedEventArgs e)
		{
			SetDelimiterEnabled();
		}

		private void SetDelimiterEnabled()
		{
			if (this.context.TextFileType == FileType.Delimited)
			{
				HeaderDelimiters.IsEnabled = true;
				RBComma.IsEnabled = true;
				RBTab.IsEnabled = true;
				RBOther.IsEnabled = true;
				txtOtherDelimiter.IsEnabled = true;
				DataGridFields.Columns[1].Visibility = Visibility.Hidden;
			}
			else
			{
				HeaderDelimiters.IsEnabled = false;
				RBComma.IsEnabled = false;
				RBTab.IsEnabled = false;
				RBOther.IsEnabled = false;
				txtOtherDelimiter.IsEnabled = false;
				DataGridFields.Columns[1].Visibility = Visibility.Visible;
			}
		}

		private void formattedDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
		{
			string existingHeader = e.Column.Header as string;
			if (null == existingHeader)
				return;

			e.Column.Header = existingHeader.Replace("_", "__");
		}


		// http://wpf.codeplex.com/wikipage?title=Single-Click%20Editing
		private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			DataGridCell cell = sender as DataGridCell;
			if (cell != null && !(cell.Column is DataGridComboBoxColumn) && !cell.IsEditing && !cell.IsReadOnly)
			{
				if (!cell.IsFocused)
				{
					cell.Focus();
				}
				DataGrid dataGrid = FindVisualParent<DataGrid>(cell);
				if (dataGrid != null)
				{
					if (dataGrid.SelectionUnit != DataGridSelectionUnit.FullRow)
					{
						if (!cell.IsSelected)
							cell.IsSelected = true;
					}
					else
					{
						DataGridRow row = FindVisualParent<DataGridRow>(cell);
						if (row != null && !row.IsSelected)
						{
							row.IsSelected = true;
						}
					}
				}
			}
		}

		private static T FindVisualParent<T>(UIElement element) where T : UIElement
		{
			UIElement parent = element;
			while (parent != null)
			{
				T correctlyTyped = parent as T;
				if (correctlyTyped != null)
				{
					return correctlyTyped;
				}

				parent = VisualTreeHelper.GetParent(parent) as UIElement;
			}
			return null;
		}

		private void DataGridFields_InitializingNewItem(object sender, InitializingNewItemEventArgs e)
		{
			var existingNames = FieldsEditorViewModel.FieldList.Select(f => f.Name).Where(f => !string.IsNullOrEmpty(f)).ToList();
			var counter = 1;

			foreach (var existingField in FieldsEditorViewModel.FieldList.ToList())
			{
				if (!string.IsNullOrEmpty(existingField.Name))
					continue;

				string newName;
				do
				{
					newName = "Column" + counter++;
				} while (existingNames.Contains(newName, StringComparer.InvariantCultureIgnoreCase));

				existingField.Name = newName;
				existingNames.Add(newName);
			}
		}

		private void DataGridCell_GotKeyboardFocus(object sender, RoutedEventArgs e)
		{
			var cell = (sender as DataGridCell);
			if ((cell != null) && !(cell.Column is DataGridComboBoxColumn))
			{
				this.DataGridFields.ScrollIntoView(cell);
				this.DataGridFields.BeginEdit();
			}
		}

		private void DataGridFields_Loaded(object sender, RoutedEventArgs e)
		{
			DataGrid dataGrid = sender as DataGrid;
			dataGrid.CanUserAddRows = true;
			dataGrid.SelectedIndex = 0;
			dataGrid.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
		}

		private void SetupDataGrid()
		{
			DataGridFields = new DataGrid()
			{
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
				CanUserDeleteRows = true,
				IsSynchronizedWithCurrentItem = true,
				CanUserAddRows = false,
				CanUserResizeRows = false,
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,
				AutoGenerateColumns = false,
				IsManipulationEnabled = true,
				Margin = new Thickness(10, 5, 0, 5)
			};

			Binding binding = new Binding();
			binding.Path = new PropertyPath("FieldList");
			binding.Mode = BindingMode.TwoWay;
			BindingOperations.SetBinding(DataGridFields, DataGrid.ItemsSourceProperty, binding);
			DataGridFields.InitializingNewItem += DataGridFields_InitializingNewItem;
			DataGridFields.Loaded += DataGridFields_Loaded;
			DataGridFields.PreviewKeyDown += DataGrid_PreviewKeyDown;

			DataGridFields.Resources.Add(typeof(DataGridRow), (Style)FindResource("DataGridDataRowValidationStyle"));
			DataGridFields.RowStyle = (Style)FindResource("DataGridRowStyle");

			#region NameColumn
			DataGridTextColumn nameColumn = new DataGridTextColumn() { Header = "Name", MinWidth = 200 };
			nameColumn.Width = new DataGridLength(200, DataGridLengthUnitType.Star);
			nameColumn.Foreground = (Brush)FindResource("ForegroundDarkBrush");
			binding = new Binding() { Path = new PropertyPath("Name"), Mode = BindingMode.TwoWay, NotifyOnValidationError = true };
			binding.ValidationRules.Add(new ValidFieldNameRule());
			binding.ValidationRules.Add(new UniqueFieldNameRule());
			nameColumn.Binding = binding;
			DataGridFields.Columns.Add(nameColumn);
			#endregion

			#region LengthColumn
			DataGridTextColumn lengthColumn = new DataGridTextColumn() { Header = "Length", MinWidth = 30 };
			lengthColumn.Width = new DataGridLength(30, DataGridLengthUnitType.Star);
			lengthColumn.Foreground = (Brush)FindResource("ForegroundDarkBrush");
			binding = new Binding() { Path = new PropertyPath("Length"), Mode = BindingMode.TwoWay, NotifyOnValidationError = true, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
			binding.ValidationRules.Add(new PositiveNumericRule() { fieldName = "FieldLength" });
			lengthColumn.Binding = binding;
			DataGridFields.Columns.Add(lengthColumn);
			#endregion

			#region TypeColumn
			DataGridComboBoxColumn typeColumn = new DataGridComboBoxColumn() { Header = "Type", MinWidth = 50 };
			typeColumn.Width = new DataGridLength(50, DataGridLengthUnitType.Star);
			typeColumn.ElementStyle = (Style)FindResource("DataGridComboBoxColumnElementStyle");
			typeColumn.EditingElementStyle = (Style)FindResource("DataGridComboBoxColumnEditingElementStyle");
			binding = new Binding() { Path = new PropertyPath("TypeName"), Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
			typeColumn.SelectedValueBinding = binding;
			DataGridFields.Columns.Add(typeColumn);
			#endregion

			#region InputFormatColumn
			DataGridTextColumn inputFormatColumn = new DataGridTextColumn() { Header = "Input Format", MinWidth = 200 };
			inputFormatColumn.Width = new DataGridLength(200, DataGridLengthUnitType.Star);
			binding = new Binding() { Path = new PropertyPath("Format") };
			inputFormatColumn.Foreground = (Brush)FindResource("ForegroundDarkBrush");
			inputFormatColumn.Binding = binding;
			DataGridFields.Columns.Add(inputFormatColumn);
			#endregion

			#region SkipColumn
			DataGridCheckBoxColumn skipColumn = new DataGridCheckBoxColumn() { Header = "Skip", Width = new DataGridLength(0, DataGridLengthUnitType.Auto) };
			binding = new Binding() { Path = new PropertyPath("Format") };
			skipColumn.Binding = binding;
			DataGridFields.Columns.Add(skipColumn);
			#endregion

			#region CrossColumn
			DataGridTemplateColumn crossColumn = new DataGridTemplateColumn() { IsReadOnly = true, Width = new DataGridLength(0, DataGridLengthUnitType.Auto) };
			crossColumn.CellTemplate = (DataTemplate)FindResource("CrossCellDataTemplate");
			DataGridFields.Columns.Add(crossColumn);
			#endregion

			ContainerGrid.Children.Add(DataGridFields);
			DataGridFields.SetValue(Grid.RowProperty, 3);
			SetDelimiterEnabled();
		}

		private void DataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				DataGrid dataGrid = sender as DataGrid;
				dataGrid.CurrentColumn = dataGrid.Columns[0];
				dataGrid.Focus();
				dataGrid.BeginEdit();
			}
		}

		private void RefreshGrid(object sender, RoutedEventArgs e)
		{
			ContainerGrid.Children.Remove(this.DataGridFields);
			SetupDataGrid();
		}
	}
}
