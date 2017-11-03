using System;
using System.Linq;
using System.Text;

namespace Twenty57.Linx.Components.Database.Common
{
	public class SqlGenerator
	{
		private string[] columnNames = null;
		private string tableName;
		private ConnectionType connectionType;

		public SqlGenerator(string tableName, ConnectionType connectionType)
		{
			this.connectionType = connectionType;
			this.Delimiter = connectionType == ConnectionType.Oracle ? "" : "\"";
			this.tableName = $"{Delimiter}{TrimSql(tableName)}{Delimiter}";
		}

		public string TableName
		{
			get
			{
				return this.tableName;
			}
		}

		public string Delimiter { get; private set; }

		public string GenerateSelectCommand(string[] columnNames = null)
		{
			this.columnNames = columnNames ?? this.columnNames;
			if (!this.columnNames.Any())
				return "";

			string command = "SELECT";
			string offset = GetCommandOffset(command);
			StringBuilder sql = new StringBuilder($"{command} {Delimiter}{TrimSql(columnNames.First())}{Delimiter}{Environment.NewLine}");
			foreach (var name in columnNames.Skip(1))
			{
				sql.AppendLine($"{offset},{Delimiter}{TrimSql(name)}{Delimiter}");
			}
			sql.AppendLine($"  FROM {tableName};");
			return sql.ToString();
		}

		public string GenerateInsertCommand(string[] columnNames)
		{
			this.columnNames = columnNames ?? this.columnNames;
			if (!this.columnNames.Any())
				return "";

			string command = "INSERT INTO";
			string offset = GetCommandOffset(command);
			StringBuilder sql = new StringBuilder($"{command} {this.tableName}{Environment.NewLine}");

			for (int columnIndex = 0; columnIndex < columnNames.Length; columnIndex++)
			{
				string lineEnd = columnNames.Length - columnIndex > 1 ? Environment.NewLine : ")" + Environment.NewLine;
				if (columnIndex == 0)
					sql.Append($"{offset}({Delimiter}{TrimSql(columnNames[columnIndex])}{Delimiter}{lineEnd}");
				else
					sql.Append($"{offset},{Delimiter}{TrimSql(columnNames[columnIndex])}{Delimiter}{lineEnd}");
			}

			sql.AppendLine($"     VALUES{Environment.NewLine}{offset}(");
			foreach (var name in columnNames)
			{
				sql.AppendLine($"{offset},");
			}
			sql.AppendLine($"{offset});");
			return sql.ToString();
		}

		public string GenerateUpdateCommand(string[] columnNames)
		{
			this.columnNames = columnNames ?? this.columnNames;
			if (!this.columnNames.Any())
				return "";

			string command = "UPDATE";
			string offset = GetCommandOffset(command);
			StringBuilder sql = new StringBuilder($"{command} {this.tableName}{Environment.NewLine}");
			sql.AppendLine($"SET    {Delimiter}{TrimSql(columnNames.First())}{Delimiter} =");
			foreach (var name in columnNames.Skip(1))
			{
				sql.AppendLine($"{offset},{Delimiter}{TrimSql(name)}{Delimiter} =");
			}
			sql.AppendLine(" WHERE");
			return sql.ToString();
		}

		private static string TrimSql(string name)
		{
			string qualifierTrimmed = name.Contains(".") ? name.Substring(name.IndexOf(".") + 1) : name;
			return qualifierTrimmed
				.Replace("[", string.Empty)
				.Replace("]", string.Empty)
				.Replace("\"", string.Empty)
				.Replace("'", string.Empty)
				.Replace("`", string.Empty);
		}

		private static string GetCommandOffset(string command)
		{
			return new string(' ', command.Length);
		}
	}
}
