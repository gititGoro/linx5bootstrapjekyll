using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel
{
	public class StoredProcedureNodeViewModel : ComplexNodeViewModel
	{
		public StoredProcedureNodeViewModel(DatabaseAssistant assistant, IDbConnection connection, string procedureName, NodeViewModel parent)
			: base(procedureName, parent)
		{
			AllowDrag = true;

			var parameterNames = GetParameterNames(assistant.GetStoredProcedureParameters(connection, procedureName));
			ConfigureParameters(procedureName, parameterNames);
		}

		public StoredProcedureNodeViewModel(ADOX.Procedure procedure, ADODB.Connection connection, NodeViewModel parent)
			: base(parent)
		{
			if (null == procedure)
				throw new ArgumentNullException("procedure");
			if (null == connection)
				throw new ArgumentNullException("connection");

			string procedureName = procedure.Name;
			Regex matchSPName = new Regex(@"(.*);\d+");
			Match match = matchSPName.Match(procedureName);
			if (match.Success && match.Groups.Count == 2)
				procedureName = match.Groups[1].Value;

			AllowDrag = true;
			Text = procedureName;

			List<string> paremeterNames = new List<string>();
			ADODB.Command command = new ADODB.CommandClass();
			command.ActiveConnection = connection;
			command.CommandType = ADODB.CommandTypeEnum.adCmdStoredProc;
			command.CommandText = procedureName;
			command.Parameters.Refresh();

			var parameterNames = GetParameterNames(command.Parameters);
			ConfigureParameters(procedureName, parameterNames);
		}

		private void ConfigureParameters(string procedureName, List<string> parameterNames)
		{
			foreach (var parameterName in parameterNames)
			{
				SimpleNodeViewModel parameterViewModel = new ParameterNodeViewModel(parameterName, parameterName, this);
				Children.Add(parameterViewModel);
			}

			SimpleDragText = String.Format("{0}", procedureName);
			ExtendedDragText = String.Format("EXECUTE RETURN_VALUE = {0}{1}{2}", procedureName, (parameterNames.Count > 0) ? Environment.NewLine : String.Empty,
				String.Join(String.Format(",{0}", Environment.NewLine), parameterNames.Select(item => "\t" + item).ToArray()));
		}

		private List<string> GetParameterNames(ADODB.Parameters parameters)
		{
			var parameterNames = new List<string>();
			try
			{
				foreach (ADODB.Parameter parameter in parameters)
				{
					if (parameter.Direction == ADODB.ParameterDirectionEnum.adParamInputOutput || parameter.Direction == ADODB.ParameterDirectionEnum.adParamOutput)
						parameterNames.Add(CreateParameter(parameter.Name, true));
					else if (parameter.Direction != ADODB.ParameterDirectionEnum.adParamReturnValue)
						parameterNames.Add(CreateParameter(parameter.Name, false));
				}
			}
			catch { }

			return parameterNames;
		}

		private List<string> GetParameterNames(IEnumerable<DatabaseModel.ProcedureParameter> parameters)
		{
			var parameterNames = new List<string>();
			foreach (DatabaseModel.ProcedureParameter parameter in parameters)
			{
				if (parameter.Direction == DatabaseModel.ParameterDirection.InOut || parameter.Direction == DatabaseModel.ParameterDirection.Out)
					parameterNames.Add(CreateParameter(parameter.Name, true));
				else if (parameter.Direction != DatabaseModel.ParameterDirection.ReturnValue)
					parameterNames.Add(CreateParameter(parameter.Name, false));
			}

			return parameterNames;
		}

		private static string CreateParameter(string parameterName, bool isOutput)
		{
			return string.Format("{0} = VALUE{1}", parameterName, (isOutput) ? " OUTPUT" : string.Empty);
		}
	}
}
