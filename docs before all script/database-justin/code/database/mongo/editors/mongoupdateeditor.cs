using System.Collections.Generic;
using ICSharpCode.AvalonEdit.Document;
using Twenty57.Linx.Components.Database.Mongo.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Components.MongoDB;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models;
using Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;
using System.Windows.Media;
using System.Windows;

namespace Twenty57.Linx.Components.Database.Mongo.Editors
{
	public class MongoUpdateEditor : IPropertyEditor
	{
		public string Name { get { return "Mongo Update Editor"; } }
		public string IconResourcePath { get { return null; } }
		public DataTemplate InlineTemplate { get { return null; } }

		public void EditValue(Property property, object designer)
		{

			var serialiser = new SegmentedDocumentSerialiser();
			var viewModel =
				new MongoJsonEditorViewModel(MongoDBWriteShared.Names.UpdateOperation, designer as MongoDBComponent, property.Value.ToString(),
					new List<ITemplateSource>
					{
						new MongoDatabaseTemplateSource(designer as MongoDBComponent, serialiser),
						new FilteredTemplateSource(
							new EmbeddedFileTemplateSource("Mongo Operators",
								"Twenty57.Linx.Components.Database.Resources.MongoTemplates.xml", serialiser),
							Template => Template.Category.StartsWith("Query Selectors") 
								|| Template.Category.StartsWith("Update Operators")
								|| Template.Category.Contains("Update Samples") )
					}, "", serialiser,
					(designer as FunctionDesigner).Context
					);

			viewModel.WatermarkText = "Write a mongo update operation here, or drag snippets from the 'Mongo Operators' tab below.";
			viewModel.InitialiseDocument(new TextDocument());

			if (Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.MongoJsonEditor.Display(viewModel, (FunctionDesigner)designer))
				property.Value = viewModel.SegmentedDocument.Text;
		}
	}
}