namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel
{
	public class SQLParameterModel
	{
		public SQLParameterModel(string parameterName, string initialValue)
		{
			Name = parameterName;
			Value = initialValue;
		}

		public string Name { get; private set; }

		public string Value { get; set; }

		public double NameLabelWidth { get; set; }
	}
}
