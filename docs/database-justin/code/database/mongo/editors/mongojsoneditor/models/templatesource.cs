using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Twenty57.Linx.Components.Database.Mongo.Editors.MongoJsonEditor.ViewModel;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models
{
	public interface ITemplateSource
	{
		string Error { get; }
		string Name { get; set; }
		bool CanRefresh { get; }
		bool RefreshDisabled { get; }
		List<Template> Load();
	}

	public class FilteredTemplateSource : ITemplateSource
	{
		private readonly ITemplateSource concreteSource;
		private readonly Func<Template, bool> filterFunction;

		public FilteredTemplateSource(ITemplateSource concreteSource, Func<Template, bool> filterFunction)
		{
			this.concreteSource = concreteSource;
			this.filterFunction = filterFunction;
		}

		public string Name
		{
			get { return concreteSource.Name; }
			set { concreteSource.Name = value; }
		}

		public bool CanRefresh
		{
			get { return concreteSource.CanRefresh; }
		}


		public bool RefreshDisabled
		{
			get { return concreteSource.RefreshDisabled; }
		}

		public string Error
		{
			get { return ""; }
		}

		public List<Template> Load()
		{
			return concreteSource.Load().Where(v => filterFunction(v)).ToList();
		}
	}


	public class MongoDatabaseTemplateSource : ITemplateSource
	{
		private readonly MongoDBComponent mongoComponent;
		private readonly SegmentedDocumentSerialiser serialiser;

		public MongoDatabaseTemplateSource(MongoDBComponent component, SegmentedDocumentSerialiser serialiser)
		{
			mongoComponent = component;
			Name = "Database Fields";
			this.serialiser = serialiser;
		}

		public bool CanRefresh
		{
			get { return true; }
		}

		public string Name { get; set; }


		public bool RefreshDisabled
		{
			get
			{
				return String.IsNullOrEmpty(mongoComponent.Collection) ||
				       String.IsNullOrEmpty(mongoComponent.ConnectionString);
			}
		}

		public string Error { get; set; }

		public List<Template> Load()
		{
			var templates = new List<Template>();
			Error = "";

			if (String.IsNullOrEmpty(mongoComponent.ConnectionString))
			{
				Error = "Fields cannot be retrieved because the Connection String has not been specified.";
				return new List<Template>();
			}

			if (String.IsNullOrEmpty(mongoComponent.Collection))
			{
				Error = "Fields cannot be retrieved because the Collection has not been specified.";
				return new List<Template>();
			}

			try
			{
				List<MongoDatabaseObject> objects = mongoComponent.GetDatabaseObjects();
				if (!objects.Any())
				{
					Error = "Fields cannot be retrieved because the Collection is empty";
					return new List<Template>();
				}

				foreach (MongoDatabaseObject mongoObject in objects)
				{
					var template = new MongoDatabaseObjectTemplate();

					template.Name = mongoObject.Name;
					template.TemplateText = mongoObject.Path;
					template.Category = mongoComponent.Collection;
					template.Path = mongoObject.PathPrefix;
					template.Serialiser = serialiser;
					templates.Add(template);
				}
			}
			catch (Exception e)
			{
				Error = Error = "Fields cannot be retrieved because we cannot connect to the database\n" + e.Message;
			}

			return templates;
		}
	}

	public class EmbeddedFileTemplateSource : ITemplateSource
	{
		private readonly string fileName;

		private XmlTemplateFile fileContent;
		private readonly SegmentedDocumentSerialiser serialiser;

		public EmbeddedFileTemplateSource(string name, string fileName, SegmentedDocumentSerialiser serialiser)
		{
			Name = name;
			this.fileName = fileName;
			this.serialiser = serialiser;
		}

		public IEnumerable<Template> Templates
		{
			get
			{
				if (fileContent == null) Load();
				return fileContent.Templates;
			}
		}


		public string Name { get; set; }

		public bool CanRefresh
		{
			get { return false; }
		}


		public bool RefreshDisabled
		{
			get { return false; }
		}

		public string Error
		{
			get { return ""; }
		}

		public List<Template> Load()
		{
			var serializer = new XmlSerializer(typeof (XmlTemplateFile));
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
			{
				fileContent = (serializer.Deserialize(stream) as XmlTemplateFile);
				foreach (Template template in fileContent.Templates)
					template.Serialiser = serialiser;
				return fileContent.Templates;
			}
		}
	}

	[Serializable]
	public class XmlTemplateFile
	{
		public string SourceName { get; set; }
		public List<Template> Templates { get; set; }
	}
}