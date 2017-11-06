using System;
using System.Xml.Serialization;

namespace Twenty57.Linx.Components.MongoDB.Editors.MongoJsonEditor.Models
{
	[Serializable]
	public class Template
	{
		public Template(string name)
		{
			Name = name;
			TemplateText = name;
		}

		public Template(string name, string templateText)
		{
			Name = name;
			TemplateText = templateText;
		}

		public Template()
		{
			Name = "";
			TemplateText = "";
		}

		public string Name { get; set; }
		public string Category { get; set; }
		public string TemplateText { get; set; }
		public string Description { get; set; }

		[XmlIgnore]
		internal SegmentedDocumentSerialiser Serialiser { get; set; }

		[XmlIgnore]
		public SegmentedDocument TemplateDocument { get { return Serialiser.Deserialise(TemplateText); } }


		public virtual string Path
		{
			get { return Category; }
			set { throw new NotImplementedException(); }
		}
	}

	public class MongoDatabaseObjectTemplate : Template
	{
		public override string Path { get; set; }
	}
}