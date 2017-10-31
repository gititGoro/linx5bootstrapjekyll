using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Stadium5Help.Models
{
	public class Section
	{
		public static Page[] Controls { get; private set; }
		public static Page[] PageScripts { get; private set; }
		public static Page[] ApplicationExplorer { get; private set; }
		public static Page[] Frameworks { get; private set; }
		public static Page[] HowItWorks { get; private set; }
        public static Page[] Releasenotes { get; private set; }

        static Section()
		{
			Controls = LoadSection("Controls");
			PageScripts = LoadSection("PageScripts");
			ApplicationExplorer = LoadSection("ApplicationExplorer");
			Frameworks = LoadSection("Frameworks");
			HowItWorks = LoadSection("HowItWorks");
			Releasenotes = LoadSection("Releasenotes").Reverse().ToArray();
        }

        private static Page[] LoadSection(string section)
		{
			System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(List<Item>));
			using (var file = File.OpenRead(HttpContext.Current.Server.MapPath("~/App_Data/" + section + ".xml")))
			{
				var list = (List<Item>)serializer.Deserialize(file);
				return list.OrderBy(x => x.Name).Select(x => new Page(x.Name, x.DisplayName, x.Description, section)).ToArray();
			}
		}
	}
}
