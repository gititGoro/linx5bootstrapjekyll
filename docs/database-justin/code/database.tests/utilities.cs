using System.Configuration;
using System.Text.RegularExpressions;

namespace Twenty57.Linx.Components.Database.Tests
{
	public class Utilities
	{
		public static string OleDbConnectionString
		{
			get { return ConfigurationManager.AppSettings["OleDbConnectionString"]; }
		}

		public static string SqlConnectionString
		{
			get
			{
				Regex r = new Regex(
					@"Data Source=[^;\s]*|Initial Catalog=[^;\s]*|Integrated Security=[^;\s]*|User ID=[^;\s]*|Password=[^;\s]*|Pooling=[^;\s]*",
					RegexOptions.IgnoreCase);
				Match m = r.Match(OleDbConnectionString);
				string s = "";
				while (m.Success)
				{
					s = s + m.Value + ";";
					m = m.NextMatch();
				}
				return s;
			}
		}

		public static string OracleConnectionString
		{
			get { return ConfigurationManager.AppSettings["OracleConnectionString"]; }
		}

		public static string OdbcConnectionString
		{
			get { return ConfigurationManager.AppSettings["OdbcConnectionString"]; }
		}
	}
}