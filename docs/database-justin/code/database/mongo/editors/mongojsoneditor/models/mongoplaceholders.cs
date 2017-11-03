namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models
{
	public class QueryPlaceholder : PlaceholderSegment
	{
		public QueryPlaceholder()
		{
		}

		public QueryPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}

		public override bool CanInsert(Template template)
		{
			return template.Category.StartsWith("Query Selectors") 
				|| template.Category.Contains("Samples") 
				|| template is MongoDatabaseObjectTemplate;
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template is MongoDatabaseObjectTemplate)
				return TransformTemplate("", new FieldNamePlaceholder(serialiser).TransformTemplate(template), " : [[Value]Value]]");
			return TransformTemplate("", template, "");
		}

		public override DocumentSegment Clone()
		{
			return Clone(new QueryPlaceholder(StartOffset, Text, serialiser));
		}
	}

	public class QueryArrayPlaceholder : QueryPlaceholder
	{
		public QueryArrayPlaceholder()
		{
		}

		public QueryArrayPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template.Category.StartsWith("Query Selectors"))
				return TransformTemplate("", template, ",\n\t[[queryarray]Additional query selectors can be inserted here]]");
			if (template is MongoDatabaseObjectTemplate)
				return TransformTemplate("", new FieldNamePlaceholder(serialiser).TransformTemplate(template),
					" : [[Value]Value]],\n\t[[queryarray]Additional query selectors...]] ");
			return TransformTemplate("", template, "");
		}

		public override DocumentSegment Clone()
		{
			return Clone(new QueryArrayPlaceholder(StartOffset, Text, serialiser));
		}
	}

	public class ValuePlaceholder : PlaceholderSegment
	{
		public ValuePlaceholder()
		{
		}

		public ValuePlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}

		public override bool CanInsert(Template template)
		{
			return false;
		}

		public override DocumentSegment Clone()
		{
			return Clone(new ValuePlaceholder(StartOffset, Text, serialiser));
		}
	}

	public class ValueArrayPlaceholder : PlaceholderSegment
	{
		public ValueArrayPlaceholder()
		{
		}

		public ValueArrayPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			return TransformTemplate("", template, ", [[value]Another Value ...]]");
		}

		public override DocumentSegment Clone()
		{
			return Clone(new ValuePlaceholder(StartOffset, Text, serialiser));
		}
	}

	public class FieldNamePlaceholder : PlaceholderSegment
	{
		public FieldNamePlaceholder()
		{
		}

		public FieldNamePlaceholder(SegmentedDocumentSerialiser segmentedDocumentSerialiser)
		{
			serialiser = segmentedDocumentSerialiser;
		}

		public FieldNamePlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}

		public override bool CanInsert(Template template)
		{
			return template is MongoDatabaseObjectTemplate;
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template is MongoDatabaseObjectTemplate)
				if (template.TemplateText.Contains("."))
					return TransformTemplate("\"", template, "\"");
			return TransformTemplate("", template, "");
		}

		public override DocumentSegment Clone()
		{
			return Clone(new FieldNamePlaceholder(StartOffset, Text, serialiser));
		}
	}

	public class AggregationPipeLineOperatorPlaceHolder : PlaceholderSegment
	{
		public AggregationPipeLineOperatorPlaceHolder()
		{
		}

		public AggregationPipeLineOperatorPlaceHolder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}

		public override bool CanInsert(Template template)
		{
			return template.Category.StartsWith("Aggregation.Pipeline Operators")
				|| template.Category.Contains("Samples")
				;
		}

		public override DocumentSegment Clone()
		{
			return Clone(new AggregationPipeLineOperatorPlaceHolder(StartOffset, Text, serialiser));
		}
	}

	public class AggregationPipeLineOperatorArrayPlaceholder : PlaceholderSegment
	{
		public AggregationPipeLineOperatorArrayPlaceholder()
		{
		}

		public AggregationPipeLineOperatorArrayPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}

		public override bool CanInsert(Template template)
		{
			return template.Category.StartsWith("Aggregation.Pipeline Operators")
					||template.Category.Contains("Samples") ;
		}

		public override DocumentSegment Clone()
		{
			return Clone(new AggregationPipeLineOperatorArrayPlaceholder(StartOffset, Text, serialiser));
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template.Category.Contains("Samples"))
				return base.TransformTemplate(template);
			return TransformTemplate("{ ", template,
				" },\n[[pipelineOperatorArray]Additional pipeline operators can be added here]]");
		}
	}


	public class NameValueArrayPlaceholder : FieldNamePlaceholder
	{
		public NameValueArrayPlaceholder()
		{
		}

		public NameValueArrayPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}


		public override DocumentSegment Clone()
		{
			return Clone(new NameValueArrayPlaceholder(StartOffset, Text, serialiser));
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template is MongoDatabaseObjectTemplate)
				return TransformTemplate("", new FieldNamePlaceholder(serialiser).TransformTemplate(template),
					" : [[value]Value]], \n[[namevaluearray]...]]");
			return TransformTemplate("", template, "");
		}
	}


	public class UpdateOperatorPlaceholder : FieldNamePlaceholder
	{
		public UpdateOperatorPlaceholder()
		{
		}

		public UpdateOperatorPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}


		public override DocumentSegment Clone()
		{
			return Clone(new UpdateOperatorArrayPlaceholder(StartOffset, Text, serialiser));
		}

		public override bool CanInsert(Template template)
		{
			return template is MongoDatabaseObjectTemplate
			       || template.Category.StartsWith("Update Operators")
			       || template.Category.Contains("Samples");
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template is MongoDatabaseObjectTemplate)
				return TransformTemplate("$set : { ", new FieldNamePlaceholder(serialiser).TransformTemplate(template),
					",  : [[value]Value]] }");
			return TransformTemplate("", template, "");
		}
	}

	public class UpdateOperatorArrayPlaceholder : UpdateOperatorPlaceholder
	{
		public UpdateOperatorArrayPlaceholder()
		{
		}

		public UpdateOperatorArrayPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}


		public override DocumentSegment Clone()
		{
			return Clone(new UpdateOperatorArrayPlaceholder(StartOffset, Text, serialiser));
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template is MongoDatabaseObjectTemplate || template.Category.StartsWith("Update Operators"))
				return TransformTemplate("", base.TransformTemplate(template),
					", \n\t[[updateOperatorArray]Additional update operators can be inserted here]]");
			return TransformTemplate("", template, "");
		}
	}


	public class GroupOperator : FieldNamePlaceholder
	{
		public GroupOperator()
		{
		}

		public GroupOperator(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}


		public override DocumentSegment Clone()
		{
			return Clone(new GroupOperator(StartOffset, Text, serialiser));
		}

		public override bool CanInsert(Template template)
		{
			return template is MongoDatabaseObjectTemplate 
				|| template.Category.StartsWith("Aggregation.Group Operators")
				|| template.Category.Contains("Samples");
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template is MongoDatabaseObjectTemplate)
				return TransformTemplate("[[value]New field name]] : { $first : { \"$", template, "\" }}");
			return TransformTemplate("", template, "");
		}
	}

	public class GroupOperatorArray : GroupOperator
	{
		public GroupOperatorArray()
		{
		}

		public GroupOperatorArray(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}


		public override DocumentSegment Clone()
		{
			return Clone(new GroupOperatorArray(StartOffset, Text, serialiser));
		}

		public override bool CanInsert(Template template)
		{
			return template is MongoDatabaseObjectTemplate 
				|| template.Category.StartsWith("Aggregation.Group Operators")
				|| template.Category.Contains("Samples");
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			return TransformTemplate("", base.TransformTemplate(template),
				",\n[[groupoperatorarray]Additional group operators...]]");
		}
	}


	public class ExpressionPlaceholder : PlaceholderSegment
	{
		public ExpressionPlaceholder()
		{
		}

		public ExpressionPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}


		public override DocumentSegment Clone()
		{
			return Clone(new ExpressionPlaceholder(StartOffset, Text, serialiser));
		}

		public override bool CanInsert(Template template)
		{
			return template is MongoDatabaseObjectTemplate
			       || (template.Category.StartsWith("Aggregation.Expression Operators") &&
			           !template.Category.StartsWith("Aggregation.Group Operators")
			           ||template.Category.Contains("Samples")
				       );
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template is MongoDatabaseObjectTemplate)
				return TransformTemplate("\"$", template, "\"");
			return TransformTemplate("{ ", base.TransformTemplate(template), " }");
		}
	}

	public class NameExpressionArrayPlaceholder : FieldNamePlaceholder
	{
		public NameExpressionArrayPlaceholder()
		{
		}

		public NameExpressionArrayPlaceholder(int offset, string text, SegmentedDocumentSerialiser serialiser)
			: base(offset, text, serialiser)
		{
		}


		public override DocumentSegment Clone()
		{
			return Clone(new NameExpressionArrayPlaceholder(StartOffset, Text, serialiser));
		}

		public override bool CanInsert(Template template)
		{
			return template is MongoDatabaseObjectTemplate
			       || (template.Category.StartsWith("Aggregation.Expression Operators") &&
			           !template.Category.StartsWith("Aggregation.Group Operators")
			           ||template.Category.Contains("Samples")
				       );
		}

		public override SegmentedDocument TransformTemplate(Template template)
		{
			if (template is MongoDatabaseObjectTemplate)
				return TransformTemplate("[[Value]New Field Name]] : \"$", template,
					"\",\n[[NameExpressionArray]Additional fields...]]");
			return TransformTemplate("[[Value]New Field Name]] : { ", template,
				" },\n[[NameExpressionArray]Additional fields...]]");
		}
	}
}