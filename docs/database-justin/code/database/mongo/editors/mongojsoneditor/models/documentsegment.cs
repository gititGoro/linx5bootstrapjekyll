using ICSharpCode.AvalonEdit.Document;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models
{
	public class DocumentSegment : TextSegment
	{
		public DocumentSegment()
		{
		}

		public DocumentSegment(int offset, string text)
		{
			StartOffset = offset;
			Text = TransformTextOnLoad(text);
			Length = Text.Length;
			EndOffset = StartOffset + Length;
		}

		public DocumentSegment(int offset, int length)
		{
			StartOffset = offset;
			Text = "";
			Length = length;
			EndOffset = StartOffset + Length;
		}

		public string Text { get; set; }

		public virtual string TransformTextOnLoad(string inputString)
		{
			return inputString;
		}

		public virtual DocumentSegment Clone()
		{
			return Clone(new DocumentSegment());
		}


		internal virtual void OnReplace(ref int offset, ref int replacementLength, ref string text)
		{
		}

		internal virtual bool StripOnSave()
		{
			return true;
		}

		protected virtual DocumentSegment Clone(DocumentSegment segment)
		{
			segment.EndOffset = EndOffset;
			segment.StartOffset = StartOffset;
			segment.Length = Length;
			segment.Text = Text;
			return segment;
		}
	}

	public class PlaceholderSegment : DocumentSegment
	{
		protected SegmentedDocumentSerialiser serialiser;


		public PlaceholderSegment()
		{
		}

		public PlaceholderSegment(int offset, string text, SegmentedDocumentSerialiser segmentedDocumentSerialiser)
			: base(offset, text)
		{
			StartOffset += 1;
			EndOffset -= 2;
			Length = EndOffset - StartOffset;
			serialiser = segmentedDocumentSerialiser;
		}


		public override string TransformTextOnLoad(string inputString)
		{
			return "/*" + inputString + "*/";
		}

		public virtual SegmentedDocument TransformTemplate(Template template)
		{
			return new SegmentedDocument(template.TemplateDocument);
		}

		public virtual bool CanInsert(Template template)
		{
			return true;
		}

		public override DocumentSegment Clone()
		{
			return Clone(new PlaceholderSegment(StartOffset, Text, serialiser));
		}

		internal override void OnReplace(ref int offset, ref int replacementLength, ref string text)
		{
			if (Length == 0) return;
			if (offset > StartOffset - 1)
			{
				replacementLength = replacementLength + (offset - StartOffset);
				offset = StartOffset - 1;
			}

			if (replacementLength + offset < EndOffset + 1)
				replacementLength = EndOffset - offset + 1;

			if (offset < 0)
			{
				replacementLength += offset - 1;
				offset = 0;
			}

			Length = 0;
		}

		protected SegmentedDocument TransformTemplate(string prefix, Template template, string suffix)
		{
			var doc = new SegmentedDocument(new TextDocument(""));
			doc.Replace(0, 0, serialiser.Deserialise(prefix));
			doc.Replace(doc.TextLength, 0, template.TemplateDocument);
			doc.Replace(doc.TextLength, 0, serialiser.Deserialise(suffix));
			return doc;
		}

		protected SegmentedDocument TransformTemplate(string prefix, SegmentedDocument doc, string suffix)
		{
			doc.Replace(0, 0, serialiser.Deserialise(prefix));
			doc.Replace(doc.TextLength, 0, serialiser.Deserialise(suffix));
			return doc;
		}
	}
}