using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.File.TextFileRead.Editors.TextFileReadFieldsEditor.ViewModel
{
	public class FieldsEditorViewModel : INotifyPropertyChanged
	{
		public static DelimiterType CurrentDelimiterType = DelimiterType.Comma;
		public static ObservableCollection<WrapperAfield> FieldList { get; set; }

		private FileType textFileType;
		private DelimiterType delimiter;
		private TextQualifierType textQualifier;
		private int skipHeaderLines = 0;
		private int skipFooterLines = 0;
		private string otherDelimiter = "";
		private List<FieldType> typeList;
		private bool? dialogResult;
		private bool canInfer;
		private bool canClearFields;
		private string sampleFileName;
		private string sampleFileText;
		private ICommand saveCommand;
		private ICommand loadSample;
		private ICommand clearFields;
		private DataView sampleFormattedData;
		private TextFileReaderFields Fields;

		public TextFileReaderFields UpdatedFields { get; private set; }
		public int ErrorCount { get; set; }

		public FieldsEditorViewModel(TextFileReaderFields fields)
		{
			TypeList = new List<FieldType>
			{
				new FieldType(typeof(bool)),
				new FieldType(typeof(byte)),
				new FieldType(typeof(DateTime)),
				new FieldType(typeof(decimal)),
				new FieldType(typeof(double)),
				new FieldType(typeof(int)),
				new FieldType(typeof(string)),
			};
			Fields = fields;
			TextFileType = fields.TextFileType;
			Delimiter = fields.Delimiter;
			OtherDelimiter = fields.OtherDelimiter;
			TextQualifier = fields.TextQualifier;
			ErrorCount = 0;
			LoadFieldList();
			FieldList.CollectionChanged += FieldList_CollectionChanged;
			CanClearFields = (FieldList != null && FieldList.Count > 0);
			this.sampleFormattedData = new DataView();
			if (TextFileType == FileType.Delimited)
				CanInfer = !(FieldList != null && FieldList.Count > 0);
			else
				CanInfer = false;
		}

		void FieldList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			CanClearFields = (FieldList.Count > 0);
			if (TextFileType == FileType.Delimited)
				CanInfer = (FieldList.Count <= 0);

			if (e.Action == NotifyCollectionChangedAction.Add)
				foreach (WrapperAfield wrapperField in e.NewItems)
				{
					wrapperField.PropertyChanged += wrapperField_PropertyChanged;
					if (wrapperField.ViewModelHandle == null)
						wrapperField.ViewModelHandle = this;
				}
			if (e.Action == NotifyCollectionChangedAction.Remove)
				foreach (WrapperAfield wrapperField in e.OldItems)
					wrapperField.PropertyChanged -= wrapperField_PropertyChanged;
			LoadSampleFormatted();
		}

		public DataView SampleFormattedData
		{
			get { return this.sampleFormattedData; }
			set
			{
				this.sampleFormattedData = value;
				OnPropertyChanged();
			}
		}

		public List<FieldType> TypeList
		{
			get { return this.typeList; }
			set
			{
				if (this.typeList != value)
				{
					this.typeList = value;
					OnPropertyChanged();
				}
			}
		}

		public IEnumerable<string> TypeNameList
		{
			get
			{
				return TypeList.Select(type => type.Name);
			}
		}

		public int SkipFooterLines
		{
			get { return this.skipFooterLines; }
			set
			{
				if (this.skipFooterLines != value)
				{
					this.skipFooterLines = value;
					LoadSampleFormatted();
					OnPropertyChanged();
				}
			}
		}

		public int SkipHeaderLines
		{
			get { return this.skipHeaderLines; }
			set
			{
				if (this.skipHeaderLines != value)
				{
					this.skipHeaderLines = value;
					LoadSampleFormatted();
					OnPropertyChanged();
				}
			}
		}

		public TextQualifierType TextQualifier
		{
			get { return this.textQualifier; }
			set
			{
				if (this.textQualifier != value)
				{
					this.textQualifier = value;
					LoadSampleFormatted();
					OnPropertyChanged();
				}
			}
		}

		public FileType TextFileType
		{
			get { return this.textFileType; }
			set
			{
				if (this.textFileType != value)
				{
					this.textFileType = value;
					if (this.textFileType == FileType.FixedLength)
						CanInfer = false;
					else if (this.textFileType == FileType.Delimited)
						CanInfer = (FieldList == null || FieldList.Count == 0);
					LoadSampleFormatted();
					OnPropertyChanged();
				}
			}
		}

		public DelimiterType Delimiter
		{
			get { return this.delimiter; }
			set
			{
				if (this.delimiter != value)
				{
					this.delimiter = value;
					CurrentDelimiterType = value;
					if (value != DelimiterType.Other)
						OtherDelimiter = "";
					if (value != DelimiterType.Other || !String.IsNullOrEmpty(this.otherDelimiter))
						LoadSampleFormatted();
					OnPropertyChanged();
				}
			}
		}

		public string OtherDelimiter
		{
			get { return this.otherDelimiter; }
			set
			{
				if (this.otherDelimiter != value)
				{
					this.otherDelimiter = value;
					if (this.otherDelimiter.Length != 0)
					{
						LoadSampleFormatted();
						Delimiter = DelimiterType.Other;
					}
					OnPropertyChanged();
				}
			}
		}

		public bool? DialogResult
		{
			get { return this.dialogResult; }
			set
			{
				if (this.dialogResult != value)
				{
					this.dialogResult = value;
					OnPropertyChanged();
				}
			}
		}

		public bool CanInfer
		{
			get { return this.canInfer; }
			set
			{
				if (this.canInfer != value)
				{
					this.canInfer = value;
					OnPropertyChanged();
				}
			}
		}

		public bool CanClearFields
		{
			get { return this.canClearFields; }
			set
			{
				if (this.canClearFields != value)
				{
					this.canClearFields = value;
					OnPropertyChanged();
				}
			}
		}

		public string SampleFileName
		{
			get { return this.sampleFileName; }
			set
			{
				if (this.sampleFileName != value)
				{
					this.sampleFileName = value;
					OnPropertyChanged();
				}
			}
		}

		public string SampleFileText
		{
			get { return this.sampleFileText; }
			set
			{
				if (this.sampleFileText != value)
				{
					this.sampleFileText = value;
					OnPropertyChanged();
				}
			}
		}

		public ICommand SaveCommand
		{
			get
			{
				if (null == saveCommand)
					saveCommand = new DelegateCommand(SaveAndCloseWindow);
				return saveCommand;
			}
		}

		public ICommand LoadSample
		{
			get
			{
				if (null == loadSample)
					loadSample = new DelegateCommand(LoadTextFile);
				return loadSample;
			}
		}

		public ICommand ClearFields
		{
			get
			{
				if (null == clearFields)
					clearFields = new DelegateCommand(ClearAllFields);
				return clearFields;
			}
		}

		private void ClearAllFields()
		{
			FieldList.Clear();
			ErrorCount = 0;
			if (FileType.Delimited == TextFileType)
				CanInfer = true;
			SampleFormattedData = new DataView();
		}

		private void LoadTextFile()
		{
			if (!OpenFile() || SampleFileName == null || SampleFileName.Trim() == String.Empty)
				return;

			SampleFileText = String.Empty;
			SampleFormattedData = new DataView();

			if (!System.IO.File.Exists(SampleFileName))
			{
				MessageBox.Show("Could not find specified file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				SampleFileName = String.Empty;
				return;
			}

			if (CanInfer)
			{
				CanInfer = false;
				InferFromSampleFile();
				LoadSampleText();
				LoadSampleFormatted();
			}
			else
			{
				LoadSampleText();
				LoadSampleFormatted();
			}
		}

		private void InferFromSampleFile()
		{
			var sniffer = CSVSniffer.Sniff(SampleFileName, Convert.ToInt32(SkipHeaderLines));
			if (sniffer.Delimiter.HasValue)
			{
				SetDelimiter(sniffer.Delimiter.Value);
				SetTextQualifier(sniffer.TextQualifier);
				this.SkipHeaderLines = sniffer.HeaderLines;
				SetFieldList(sniffer.ColumnHeaders);
			}
			else
			{
				MessageBox.Show("Could not infer delimiter, text qualifier and column headers from file", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
			}
		}

		private void SetDelimiter(char delimiter)
		{
			switch (delimiter)
			{
				case ',':
					{
						OtherDelimiter = ",";
						Delimiter = DelimiterType.Comma;
						break;
					}
				case '\t':
					{
						OtherDelimiter = "\t";
						Delimiter = DelimiterType.Tab;
						break;
					}
				default:
					{
						Delimiter = DelimiterType.Other;
						OtherDelimiter = delimiter.ToString();
						break;
					}
			}
		}

		private void SetTextQualifier(char? textQualifier)
		{
			if (textQualifier.HasValue)
			{
				switch (textQualifier.Value)
				{
					case '"':
						{
							TextQualifier = TextQualifierType.DoubleQuotes;
							break;
						}
					case '\'':
						{
							TextQualifier = TextQualifierType.SingleQuotes;
							break;
						}
					default:
						{
							throw new Exception("TextQualifier " + textQualifier.Value + " not expected.");
						}
				}
			}
			else
			{
				TextQualifier = TextQualifierType.None;
			}
		}

		private void SetFieldList(string[] headers)
		{
			FieldList.Clear();
			foreach (var columnHeaderName in headers)
			{
				var aField = new WrapperAfield() { Name = columnHeaderName, ViewModelHandle = this };
				FieldList.Add(aField);
			}
		}

		public void LoadSampleFormatted()
		{
			if (SampleFileName == null || SampleFileName.Trim() == String.Empty || FieldList == null || FieldList.Count == 0)
				return;

			string line = String.Empty;
			int counter = 0;
			var sampleData = new DataTable();
			string lengthList = "";

			foreach (var field in FieldList)
				lengthList += field.Length + ",";

			if (lengthList != String.Empty)
				lengthList = lengthList.Substring(0, lengthList.Length - 1);

			using (var fileStream = new FileStream(SampleFileName, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
			using (var streamReader = new StreamReader(fileStream))
			{
				bool columnsAdded = false;
				if (FieldList.Count > 0)
				{
					foreach (WrapperAfield header in FieldList)
						sampleData.Columns.Add(header.Name);
					columnsAdded = true;
				}

				FieldParser fieldParser = new FieldParser(TextFileType, Delimiter, TextQualifier, OtherDelimiter, lengthList);
				int skipCounter = 0;
				while ((line = streamReader.ReadLine()) != null && counter < 50)
				{
					if (skipCounter < SkipHeaderLines)
					{
						skipCounter++;
						counter++;
						continue;
					}

					string[] values = fieldParser.GetValues(line);
					if (!columnsAdded && sampleData.Columns.Count == 0)
					{
						int columnCounter = 1;
						Array.ForEach(values, _ => sampleData.Columns.Add("Column" + columnCounter++));
					}

					sampleData.Rows.Add(values.Take(sampleData.Columns.Count).ToArray());
					counter++;
				}
			}

			if (counter < 50 && sampleData.Rows.Count > SkipFooterLines)
				for (int idx = 0; idx < SkipFooterLines; idx++)
					sampleData.Rows.RemoveAt(sampleData.Rows.Count - 1);

			SampleFormattedData = sampleData.DefaultView;
		}

		private void LoadSampleText()
		{
			using (var fileStream = new FileStream(SampleFileName, FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
			using (var streamReader = new StreamReader(fileStream))
			{
				StringBuilder sb = new StringBuilder();
				string line = String.Empty;
				int counter = 0;
				while ((line = streamReader.ReadLine()) != null && counter < 50)
				{
					sb.AppendLine(line);
					counter++;
				}

				SampleFileText = sb.ToString();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		private void SaveAndCloseWindow()
		{
			if (ErrorCount > 0)
			{
				MessageBox.Show("Please correct validation errors in field list", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			UpdatedFields = new TextFileReaderFields();
			UpdatedFields.TextFileType = TextFileType;
			UpdatedFields.Delimiter = Delimiter;
			UpdatedFields.TextQualifier = TextQualifier;
			UpdatedFields.OtherDelimiter = OtherDelimiter;
			UpdatedFields.FieldList = CopyFieldList();
			DialogResult = true;
		}

		private bool OpenFile()
		{
			OpenFileDialog filePathDialog = new OpenFileDialog
			{
				CheckPathExists = false,
				CheckFileExists = false,
				Title = "Select file"
			};

			if (filePathDialog.ShowDialog() ?? false)
			{
				SampleFileName = filePathDialog.FileName;
				return true;
			}
			return false;
		}

		private AfieldCollection CopyFieldList()
		{
			AfieldCollection newFieldList = new AfieldCollection();
			foreach (var wrapperField in FieldList)
				newFieldList.Add(wrapperField.ToAfield());
			return newFieldList;
		}

		private void LoadFieldList()
		{
			FieldList = new ObservableCollection<WrapperAfield>();
			foreach (var field in Fields.FieldList)
				FieldList.Add(new WrapperAfield(field, this));
		}

		private void wrapperField_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Name")
				LoadSampleFormatted();
		}

		public class WrapperAfield : INotifyPropertyChanged
		{
			private string name;
			private bool skip;
			private FieldType type;
			private string format;
			private int length;
			public FieldsEditorViewModel ViewModelHandle { get; set; }

			public WrapperAfield()
			{
				Name = String.Empty;
				Skip = false;
				Type = new FieldType(typeof(string));
				Format = String.Empty;
				Length = 0;
				ViewModelHandle = null;
			}

			public WrapperAfield(Afield afield, FieldsEditorViewModel viewModelHandle)
			{
				Name = afield.Name;
				Skip = afield.Skip;
				Type = afield.Type;
				Format = afield.Format;
				Length = afield.Length;
				ViewModelHandle = viewModelHandle;
			}

			public Afield ToAfield()
			{
				return new Afield
				{
					Name = this.Name,
					Skip = this.Skip,
					Type = this.Type,
					Format = this.Format,
					Length = this.Length
				};
			}

			public string Name
			{
				get { return this.name; }
				set
				{
					if (this.name != value)
					{
						this.name = value;
						if (ViewModelHandle != null)
							ViewModelHandle.LoadSampleFormatted();
						OnPropertyChanged();
					}
				}
			}

			public bool Skip
			{
				get { return this.skip; }
				set
				{
					if (this.skip != value)
					{
						this.skip = value;
						OnPropertyChanged();
					}
				}
			}

			public FieldType Type
			{
				get { return this.type; }
				set
				{
					if (this.type != value)
					{
						this.type = value;
						OnPropertyChanged();
					}
				}
			}

			public string TypeName
			{
				get { return Type.Name; }
				set
				{
					Type = ViewModelHandle.TypeList.Single(type => type.Name == value);
				}
			}

			public string Format
			{
				get { return this.format; }
				set
				{
					if (this.format != value)
					{
						this.format = value;
						OnPropertyChanged();
					}
				}
			}

			public int Length
			{
				get { return this.length; }
				set
				{
					if (this.length != value)
					{
						this.length = value;
						if (ViewModelHandle != null)
							ViewModelHandle.LoadSampleFormatted();
						OnPropertyChanged();
					}
				}
			}

			public event PropertyChangedEventHandler PropertyChanged;

			private void OnPropertyChanged([CallerMemberName] string propertyName = null)
			{
				if (PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
				}
			}

		}
	}

	public class EnumToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return parameter.Equals(value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (((bool)value) == true)
			{
				return parameter;
			}
			else
			{
				return DependencyProperty.UnsetValue;
			}
		}
	}

	public class PositiveNumericRule : ValidationRule
	{
		public string fieldName { get; set; }

		public PositiveNumericRule()
		{
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			long longVal;
			if (!long.TryParse((string)value, out longVal) || longVal < 0)
				return new ValidationResult(false, "Only positive numeric values allowed: " + fieldName);
			return new ValidationResult(true, null);
		}
	}

	public class SingleCharRule : ValidationRule
	{
		public string fieldName { get; set; }

		public SingleCharRule()
		{
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (((string)value).Length > 1)
				return new ValidationResult(false, "Only single character allowed: " + fieldName);
			return new ValidationResult(true, null);
		}
	}

	public class OtherCharNotEmptyRule : ValidationRule
	{
		public OtherCharNotEmptyRule()
		{
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (DelimiterType.Other == FieldsEditorViewModel.CurrentDelimiterType && (value == null || string.IsNullOrEmpty((string)value)))
				return new ValidationResult(false, "Please specify Other Delimiter");
			return new ValidationResult(true, null);
		}
	}

	public class ValidFieldNameRule : ValidationRule
	{
		public ValidFieldNameRule()
		{
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			try
			{
				Names.IsNameValid(value as string, true);
				return new ValidationResult(true, null);
			}
			catch (Exception exc)
			{
				return new ValidationResult(false, exc.Message);
			}
		}
	}

	public class UniqueFieldNameRule : ValidationRule
	{
		public UniqueFieldNameRule()
		{
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			foreach (var field in FieldsEditorViewModel.FieldList)
				if (field.Name.ToLower() == ((string)value).ToLower())
					return new ValidationResult(false, "Please enter a unique field name");
			return new ValidationResult(true, null);
		}
	}
}
