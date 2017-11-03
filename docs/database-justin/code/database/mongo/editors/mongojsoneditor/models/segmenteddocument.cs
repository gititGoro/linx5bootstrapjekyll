using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models
{
	public class SegmentedDocument
	{
		private readonly TextDocument textDocument;
		public event EventHandler<DocumentChangeEventArgs> Changed;
		private bool InAutomaticChange;
		private DocumentChangeEventArgs lastChange;
		private ReadOnlyCollection<DocumentSegment> changedSegments;

		public SegmentedDocument(TextDocument textDocument)
		{
			this.textDocument = textDocument;
			Segments = new TextSegmentCollection<DocumentSegment>(textDocument);
			this.textDocument.Changing += onChanging;
			this.textDocument.UpdateFinished += OnUpdateEnd;
			ReadOnlySectionProvider = new TextSegmentReadOnlySectionProvider<TextSegment>(textDocument);
		}

		public SegmentedDocument(SegmentedDocument templateText)
		{
			textDocument = new TextDocument("");
			Segments = new TextSegmentCollection<DocumentSegment>(textDocument);
			textDocument.Changing += onChanging;
			textDocument.UpdateFinished += OnUpdateEnd;
			Load(templateText);
		}

		public TextSegmentCollection<DocumentSegment> Segments { get; set; }

		public string Text
		{
			get { return textDocument.Text; }
		}

		public TextSegmentReadOnlySectionProvider<TextSegment> ReadOnlySectionProvider { get; set; }

		public int TextLength
		{
			get { return textDocument.TextLength; }
		}


		public void InsertTemplate(int offset, Template template, out int insertionEndIndex)
		{
			var placeholder =
				Segments.FindSegmentsContaining(offset).Where(v => v is PlaceholderSegment).FirstOrDefault() as PlaceholderSegment;

			SegmentedDocument document = template.TemplateDocument;
			if (placeholder != null)
			{
				document = placeholder.TransformTemplate(template);
			}

			int replacmentLength = 0;
			Replace(ref offset, ref replacmentLength, document);
			insertionEndIndex = offset + document.TextLength;
		}

		public void Replace(int offset, int replacementLength, SegmentedDocument text)
		{
			int offsetRef = offset;
			int replacementLengthRef = replacementLength;
			Replace(ref offsetRef, ref replacementLengthRef, text);
		}

		public void Replace(ref int offset, ref int replacementLength, SegmentedDocument text)
		{
			InAutomaticChange = true;

			var affectedSegments = new ReadOnlyCollection<DocumentSegment>(new List<DocumentSegment>());

			affectedSegments = Segments.FindOverlappingSegments(offset, replacementLength);

			string textString = text.Text;

			Replace(affectedSegments, ref offset, ref replacementLength, ref textString);

			foreach (DocumentSegment segment in text.Segments)
			{
				DocumentSegment newsegment = segment.Clone();
				newsegment.StartOffset = newsegment.StartOffset + offset;
				newsegment.EndOffset = newsegment.StartOffset + newsegment.Length;
				Segments.Add(newsegment);
			}
			InAutomaticChange = false;
			lastChange = null;
		}

		public void Replace(int offset, int length, string text)
		{
			textDocument.Replace(offset, length, text);
		}

		internal void StripEditingSections()
		{
			foreach (DocumentSegment segment in Segments)
			{
				InAutomaticChange = true;
				if (segment.StripOnSave())
				{
					textDocument.Replace(segment.StartOffset - 1, segment.Length + 2, "");
				}
				InAutomaticChange = false;
			}
		}

		internal void Load(string outputString, IEnumerable<DocumentSegment> segments = null)
		{
			textDocument.Text = outputString;
			Segments.Clear();
			if (segments != null)
				foreach (DocumentSegment segment in segments)
					Segments.Add(segment);

			textDocument.UndoStack.ClearAll();
		}

		internal void Load(SegmentedDocument document)
		{
			Load(document.Text, document.Segments.Select(v => v.Clone()));
		}

		internal bool CanInsert(int offset, Template template)
		{
			var placeholder =
				Segments.FindSegmentsContaining(offset).Where(v => v is PlaceholderSegment).FirstOrDefault() as PlaceholderSegment;
			if (placeholder != null)
				return placeholder.CanInsert(template);
			return true;
		}

		private void onChanging(object sender, DocumentChangeEventArgs e)
		{
			if (!InAutomaticChange)
			{
				changedSegments = Segments.FindOverlappingSegments(e.Offset, e.RemovalLength);
				lastChange = e;
			}
		}

		private void OnUpdateEnd(object sender, EventArgs e)
		{
			if (!InAutomaticChange && lastChange != null && changedSegments.Any())
			{
				InAutomaticChange = true;
				ApplyChanges(lastChange, changedSegments);
				InAutomaticChange = false;
				Changed(sender, null);
			}
		}

		private void ApplyChanges(DocumentChangeEventArgs lastChange, ReadOnlyCollection<DocumentSegment> changedSegments)
		{
			if (textDocument.TextLength == 0)
			{
				Segments.Clear();
				return;
			}		
			if (lastChange == null) return;
			if (textDocument.IsInUpdate) return;
			List<DocumentSegment> affectedSegments = changedSegments.Where(v => v is PlaceholderSegment).ToList();

			foreach (var segment in affectedSegments.Where(v=>v.Length==0).ToList())
			{
				Segments.Remove(segment);
				affectedSegments.Remove(segment);
			}

			if (!affectedSegments.Any()) return;
			if (!textDocument.UndoStack.CanUndo) return;
			try
			{
				textDocument.UndoStack.Undo();
			}
			catch (Exception)
			{
				return;
			}

			int offset = lastChange.Offset;
			int legnth = lastChange.RemovalLength;
			string text = lastChange.InsertedText.Text;
			Replace(affectedSegments, ref offset, ref legnth, ref text);
		}

		private void Replace(IEnumerable<DocumentSegment> affectedSegments, ref int offset, ref int replacementLength,
			ref string text)
		{
			foreach (DocumentSegment segment in affectedSegments)
			{
				segment.OnReplace(ref offset, ref replacementLength, ref text);
				if (segment.Length == 0)
					Segments.Remove(segment);
			}
			offset = Math.Min(textDocument.TextLength, offset);
			replacementLength = Math.Min(textDocument.TextLength - offset, replacementLength);
			textDocument.Replace(offset, replacementLength, text);
		}
	}
}