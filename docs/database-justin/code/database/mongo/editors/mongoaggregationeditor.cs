using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;
using Twenty57.Linx.Components.Database.Mongo.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Components.Database.Mongo.MongoDBRead;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;
using System.Windows.Media;
using System.Windows;

namespace Twenty57.Linx.Components.Database.Mongo.Editors
{
	public class MongoAggregationEditor : IPropertyEditor
	{
		public string Name { get { return "Mongo Update Editor"; } }
		public string IconResourcePath { get { return null; } }
		public DataTemplate InlineTemplate { get { return null; } }

		public void EditValue(Property property, object designer)
		{

			var serialiser = new SegmentedDocumentSerialiser();
			var viewModel =
				new MongoJsonEditorViewModel(MongoDBReadShared.Names.AggregationPipeline, designer as MongoDBComponent, property.Value.ToString(),
					new List<ITemplateSource>
					{
						new MongoDatabaseTemplateSource(designer as MongoDBComponent, serialiser),
						new FilteredTemplateSource(
								new EmbeddedFileTemplateSource("Mongo Operators", "Twenty57.Linx.Components.Database.Resources.MongoTemplates.xml", serialiser),
							(Template)=> Template.Category.StartsWith("Query Selectors") 
								|| Template.Category.StartsWith("Aggregation")
								|| Template.Category.Contains("Aggregation Samples"))
					}, "", serialiser,
					(designer as FunctionDesigner).Context
					);

			viewModel.InitialiseDocument(new TextDocument());
			viewModel.WatermarkText = "Write a mongo aggregation pipeline here, or drag snippets from the 'Mongo Operators' tab below.";
			if (Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.MongoJsonEditor.Display(viewModel,(FunctionDesigner)designer))
				property.Value = viewModel.SegmentedDocument.Text;
		}
	}
}