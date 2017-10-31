using System;
using System.Collections.Generic;

namespace Stadium5Help.Models
{
	public class Page
	{
		private const string htmlStartDelimter = "|sHtml|";
		private const string htmlEndDelimeter = "|eHtml|";

		public string Name { get; private set; }
		public string DisplayName { get; private set; }
		public DescriptionPart[] Description { get; private set; }
		public string Href { get; private set; }
		public string Icon { get; private set; }

		public Page(string name, string displayName, string description, string section)
		{
			this.Name = name;
			this.DisplayName = displayName;
			this.Description = ParseDescription(description);
			this.Href = "~/" + section + "/" + name;
			this.Icon = "~/Images/Icons/" + name + ".png";
		}

		private DescriptionPart[] ParseDescription(string description)
		{
			List<DescriptionPart> parts = new List<DescriptionPart>();
			bool isHtml = false;

			foreach (var part in description.Split(new string[] { htmlStartDelimter, htmlEndDelimeter }, StringSplitOptions.None))
			{
				if (part.Length > 0)
					parts.Add(new DescriptionPart(part, isHtml));
				isHtml = !isHtml;
			}

			return parts.ToArray();
		}

		public class DescriptionPart
		{
			public DescriptionPart(string value, bool isHtml)
			{
				this.Value = value;
				this.IsHtml = isHtml;
			}

			public string Value { get; private set; }
			public bool IsHtml { get; private set; }
		}
	}
}