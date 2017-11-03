using GongSolutions.Wpf.DragDrop;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.Mongo.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors.Static;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel
{
	public class TemplateEditorViewModel : INotifyPropertyChanged, IDragSource, IDropTarget
	{
		public List<ITemplateSource> TemplateSources;

		private bool? dialogResult;
		private ICommand okCommand;
		private ICommand expressionCommand;
		private int caretIndex;
		private IDesignerContext designerContext;

		public TemplateEditorViewModel(IDesignerContext designerContext)
		{
			this.designerContext = designerContext;
		}

		public SegmentedDocument SegmentedDocument { get; private set; }

		public string Text
		{
			get { return SegmentedDocument.Text; }
		}

		public string Title { get; set; }

		public ICommand OKCommand
		{
			get
			{
				if (null == okCommand)
					okCommand = new DelegateCommand(OnClose);

				return okCommand;
			}
		}

		public ICommand ExpressionCommand
		{
			get
			{
				if (null == expressionCommand)
					expressionCommand = new DelegateCommand(ExecuteExpressionCommand);

				return expressionCommand;
			}
		}


		public int CaretIndex
		{
			get { return caretIndex; }
			set
			{
				if (caretIndex != value)
				{
					caretIndex = value;
					OnPropertyChanged("CaretIndex");
					OnPropertyChanged("CaretInExpression");
					OnPropertyChanged("TemplateAtCaret");
				}
			}
		}


		public PlaceholderSegment PlaceholdereAtCaret
		{
			get
			{
				return SegmentedDocument.Segments.FindSegmentsContaining(caretIndex).FirstOrDefault(v => v is PlaceholderSegment) as PlaceholderSegment;
			}
		}


		public bool CaretInExpression
		{
			get
			{
				LinxExpression expression;
				return InLinxExpression(out expression);
			}
		}

		public bool? DialogResult
		{
			get { return dialogResult; }
			set
			{
				if (dialogResult != value)
				{
					dialogResult = value;
					OnPropertyChanged("DialogResult");
				}
			}
		}

		public TemplateEditorViewModel()
		{
		}

		public void Dropped(IDropInfo dropInfo)
		{
			if (dialogResult == null) dialogResult = true;
		}

		public void StartDrag(IDragInfo dragInfo)
		{
			dragInfo.Data = null;
			dragInfo.Effects = DragDropEffects.None;

			if (null == dragInfo.SourceItem)
				return;

			var draggedModel = dragInfo.SourceItem as TemplateTreeItemViewModel;

			if (null == draggedModel || !draggedModel.AllowDrag)
				return;

			dragInfo.Data = draggedModel;
			dragInfo.Effects = DragDropEffects.Move;
		}

		public void DragLeave(IDropInfo dropInfo)
		{
		}

		public void DragOver(IDropInfo dropInfo)
		{
			dropInfo.Effects = DragDropEffects.None;
			var draggedModel = dropInfo.Data as TemplateTreeItemViewModel;

			bool isStringDraggedModel = dropInfo.Data is string;

			var editor = dropInfo.VisualTarget as TextEditor;

			if ((draggedModel != null || isStringDraggedModel) && editor != null)
			{
				TextViewPosition? position = editor.GetPositionFromPoint(dropInfo.DropPosition);
				if (position.HasValue)
				{
					editor.CaretOffset = editor.Document.GetOffset(position.Value.Line, position.Value.Column);
					if (isStringDraggedModel)
						dropInfo.Effects = dropInfo.AllowedEffects;
				}
				else
				{
					editor.CaretOffset = editor.Document.TextLength;
				}

				if (isStringDraggedModel || SegmentedDocument.CanInsert(editor.CaretOffset, draggedModel.Template))
					dropInfo.Effects = dropInfo.AllowedEffects;

				editor.Focus();
			}
		}

		public void Drop(IDropInfo dropInfo)
		{
			var draggedModel = dropInfo.Data as TemplateTreeItemViewModel;
			bool isStringDraggedModel = dropInfo.Data is string;

			if (draggedModel == null && !isStringDraggedModel) return;
			if (!isStringDraggedModel && draggedModel.Template == null) return;

			if (dropInfo.Data is string)
			{
				string dragText = dropInfo.Data as string;

				if (dragText == VariablesEditor.ExpressionEditorText)
				{
					ExecuteExpressionCommand();
					dragText = string.Empty;
				}
				else
				{
					if (!this.CaretInExpression)
						dragText = SqlStringHandler.CreateSqlExpression(dragText);
					var editor = dropInfo.VisualTarget as TextEditor;
					var caretIndex = editor.CaretOffset;
					editor.Text = editor.Text.Insert(caretIndex, dragText);
					editor.CaretOffset = caretIndex + dragText.Length;
				}
			}
			else
				InsertTemplate(draggedModel.Template);
		}

		internal virtual void InitialiseDocument(TextDocument textDocument)
		{
			SegmentedDocument = new SegmentedDocument(textDocument);
			SegmentedDocument.Changed += (sender, e) =>
			{
				OnPropertyChanged("CaretIndex");
				OnPropertyChanged("CaretInExpression");
				OnPropertyChanged("TemplateAtCaret");
			};
		}

		public bool CanInsert(Template template)
		{
			return (PlaceholdereAtCaret == null || PlaceholdereAtCaret.CanInsert(template));
		}

		internal void InsertTemplate(Template template)
		{
			if (!CanInsert(template)) return;
			int newCaretIndex = 0;
			SegmentedDocument.InsertTemplate(caretIndex, template, out newCaretIndex);
			caretIndex = newCaretIndex;
		}

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void ExecuteExpressionCommand()
		{
			LinxExpression expression;
			bool editExpression = InLinxExpression(out expression);
			string expressionText = (editExpression) ? "=" + expression.ExpressionText : string.Empty;

			IExpression modifiedValue;
			if (designerContext.EditExpression(expressionText, out modifiedValue))
			{
				string wrappedExpression = string.Empty;
				if (!modifiedValue.IsEmpty)
					wrappedExpression = SqlStringHandler.CreateSqlExpression(modifiedValue.GetExpression());

				if (editExpression)
				{
					SegmentedDocument.Replace(expression.StartIndex, expression.EndIndex - expression.StartIndex + 1,
						wrappedExpression);
				}
				else
					SegmentedDocument.Replace(CaretIndex, 0, wrappedExpression);
			}
		}

		private bool InLinxExpression(out LinxExpression expression)
		{
			expression = LinxExpression.FindAll(Text).FirstOrDefault(
				item => item.StartIndex <= CaretIndex && item.EndIndex >= CaretIndex);
			return (null != expression);
		}

		private void OnClose()
		{
			SegmentedDocument.StripEditingSections();
			DialogResult = true;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}


	internal class LinxExpression : ICloneable
	{
		internal const string expressionWrapperPrefix = @"@{";
		internal const string expressionPattern = @"\@\{(.*?)\}";
		internal const string expressionWrapperSuffix = @"}";

		public LinxExpression(Match match, int nameIndex)
		{
			if (match.Groups.Count != 2)
				throw new Exception("Invalid match specified.");

			StartIndex = match.Index;
			EndIndex = match.Index + (match.Length - 1);
			MatchText = match.Groups[0].Value;
			ExpressionText = match.Groups[1].Value;
		}

		private LinxExpression(LinxExpression instance)
		{
			StartIndex = instance.StartIndex;
			EndIndex = instance.EndIndex;
			MatchText = instance.MatchText;
			ExpressionText = instance.ExpressionText;
		}

		public int StartIndex { get; private set; }
		public int EndIndex { get; private set; }
		public string MatchText { get; private set; }
		public string ExpressionText { get; private set; }

		public object Clone()
		{
			return new LinxExpression(this);
		}

		public static IEnumerable<LinxExpression> FindAll(string container)
		{
			var expressionRegex = new Regex(expressionPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			foreach (Match match in expressionRegex.Matches(container))
			{
				yield return new LinxExpression(match, match.Index);
			}
		}

		internal static string Wrap(string p)
		{
			return expressionWrapperPrefix + p + expressionWrapperSuffix;
		}
	}


	public class MongoJsonEditorViewModel : TemplateEditorViewModel
	{
		private readonly SegmentedDocumentSerialiser serialiser;
		private readonly string defaultText;
		private readonly string propertyValue;
		public FunctionDesigner Designer { get; set; }

		public MongoJsonEditorViewModel(string title, MongoDBComponent mongoComponent, string propertyValue,
			List<ITemplateSource> templateSources, string defaultText, SegmentedDocumentSerialiser serialiser, IDesignerContext designerContext)
			: base(designerContext)
		{
			Title = "MongoDB " + title;
			this.propertyValue = propertyValue;
			this.serialiser = serialiser;
			serialiser.AddConstructor("placeholder", (offset, text) => new PlaceholderSegment(offset, text, serialiser));
			serialiser.AddConstructor("query", (offset, text) => new QueryPlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("queryarray", (offset, text) => new QueryArrayPlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("value", (offset, text) => new ValuePlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("valueArray", (offset, text) => new ValueArrayPlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("field", (offset, text) => new FieldNamePlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("pipelineOperator",
				(offset, text) => new AggregationPipeLineOperatorPlaceHolder(offset, text, serialiser));
			serialiser.AddConstructor("pipelineOperatorArray",
				(offset, text) => new AggregationPipeLineOperatorArrayPlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("namevaluearray", (offset, text) => new NameValueArrayPlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("updateOperatorArray",
				(offset, text) => new UpdateOperatorArrayPlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("updateOperator", (offset, text) => new UpdateOperatorPlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("groupoperatorarray", (offset, text) => new GroupOperatorArray(offset, text, serialiser));
			serialiser.AddConstructor("expression", (offset, text) => new ExpressionPlaceholder(offset, text, serialiser));
			serialiser.AddConstructor("NameExpressionArray",
				(offset, text) => new NameExpressionArrayPlaceholder(offset, text, serialiser));


			TemplateSources = templateSources;
			this.defaultText = defaultText;
		}

		internal override void InitialiseDocument(TextDocument textDocument)
		{
			base.InitialiseDocument(textDocument);

			if (String.IsNullOrEmpty(propertyValue))
			{
				SegmentedDocument.Load(serialiser.Deserialise(defaultText));
			}
			else
			{
				SegmentedDocument.Load(propertyValue);
			}
		}

		private string watermarkText = "";

		public string WatermarkText
		{
			get { return watermarkText; }
			set
			{
				watermarkText = value;
				OnPropertyChanged("WatermarkText");
			}
		}
	}
}