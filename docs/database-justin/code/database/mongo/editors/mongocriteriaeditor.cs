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
	public class MongoCriteriaEditor : IPropertyEditor
	{
		public string Name { get { return "Mongo Update Editor"; } }
		public string IconResourcePath { get { return null; } }
		public DataTemplate InlineTemplate { get { return null; } }

		public void EditValue(Property property, object designer)
		{
			var serialiser = new SegmentedDocumentSerialiser();
			string propertyValue = GetPropertyValue(property);
			var viewModel =
				new MongoJsonEditorViewModel(property.Id, designer as MongoDBComponent, propertyValue,
					new List<ITemplateSource>
					{
						new MongoDatabaseTemplateSource(designer as MongoDBComponent, serialiser),
						new FilteredTemplateSource(
								new EmbeddedFileTemplateSource("Mongo Operators", "Twenty57.Linx.Components.Database.Resources.MongoTemplates.xml", serialiser),
							(Template)=>Template.Category.StartsWith("Query Selectors")
								||Template.Category.StartsWith("Query Samples")

							)
					}, "", serialiser,
					(designer as FunctionDesigner).Context
					);
			viewModel.WatermarkText = "Write a mongo query operation here, or  drag snippets from the 'Mongo Operators' tab below. ";

			viewModel.InitialiseDocument(new TextDocument());
			if (MongoDB.Editors.MongoJsonEditor.MongoJsonEditor.Display(viewModel, ((FunctionDesigner)designer)))
				property.Value = viewModel.SegmentedDocument.Text;
		}

		private static string GetPropertyValue(Property property)
		{
			var propertyValue = property.Value as string ?? string.Empty;
			if (!string.IsNullOrEmpty(propertyValue))
				return propertyValue;

			if (property.Id == MongoDBMapReduce.MongoDBMapReduceShared.Names.Map)
				return @"function Map(){

}";
			else if (property.Id == MongoDBMapReduce.MongoDBMapReduceShared.Names.Reduce)
				return @"function Reduce(key, values){

}";
			else if (property.Id == MongoDBMapReduce.MongoDBMapReduceShared.Names.Finalize)
				return @"function Finalize(key, reduced){

}";
			else
				return string.Empty;
		}
	}
}