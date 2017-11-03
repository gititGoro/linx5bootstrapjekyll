using System;
using System.Data;
using System.Linq;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel
{
	public class TableNodeViewModel : ComplexNodeViewModel
	{
		private ConnectionType connectionType;
		private string connectionString;
		private bool childrenLoaded;
		private ICommand generateSelectCommand;
		private ICommand generateInsertCommand;
		private ICommand generateUpdateCommand;
		private Action<string> setSql;
		private DatabaseModel.Column[] cachedTableColumns;
		private string cachedTableColumnsError;
		private SqlGenerator sqlGenerator = null;

		public TableNodeViewModel(ConnectionType connectionType, string connectionString, string tableName, NodeViewModel parent, Action<string> setSql, bool contextMenuVisible)
			: base(tableName, parent, contextMenuVisible)
		{
			this.connectionType = connectionType;
			this.connectionString = connectionString;
			this.setSql = setSql;
			AllowDrag = true;
			this.sqlGenerator = new SqlGenerator(tableName, connectionType);

			SimpleDragText = sqlGenerator.TableName;
			ExtendedDragText = sqlGenerator.GenerateSelectCommand(ColumnNames);

			AddMockChild();
		}

		public override bool IsExpanded
		{
			get { return base.IsExpanded; }
			set
			{
				if (base.IsExpanded == value)
					return;

				if (!base.IsExpanded && !this.childrenLoaded)
					LoadAllChildren();

				base.IsExpanded = value;
			}
		}

		public ICommand GenerateSelectCommand
		{
			get
			{
				if (this.generateSelectCommand == null)
					this.generateSelectCommand = new DelegateCommand(ExecuteGenerateSelectCommand);

				return this.generateSelectCommand;
			}
		}

		public ICommand GenerateInsertCommand
		{
			get
			{
				if (this.generateInsertCommand == null)
					this.generateInsertCommand = new DelegateCommand(ExecuteGenerateInsertCommand);

				return this.generateInsertCommand;
			}
		}

		public ICommand GenerateUpdateCommand
		{
			get
			{
				if (this.generateUpdateCommand == null)
					this.generateUpdateCommand = new DelegateCommand(ExecuteGenerateUpdateCommand);

				return this.generateUpdateCommand;
			}
		}

		private string[] ColumnNames
		{
			get
			{
				DatabaseModel.Column[] columns;
				string error;
				return (GetCachedTableColumns(out columns, out error)) ? columns.Select(c => c.Name).ToArray() : new string[0];
			}
		}

		private void ExecuteGenerateSelectCommand()
		{
			string sql = sqlGenerator.GenerateSelectCommand(ColumnNames);
			this.setSql?.Invoke(sql.ToString());
		}

		private void ExecuteGenerateInsertCommand()
		{
			string sql = sqlGenerator.GenerateInsertCommand(ColumnNames);
			this.setSql?.Invoke(sql.ToString());
		}

		private void ExecuteGenerateUpdateCommand()
		{
			string sql = sqlGenerator.GenerateUpdateCommand(ColumnNames);
			this.setSql?.Invoke(sql.ToString());
		}

		private void LoadAllChildren()
		{
			Children.Clear();

			DatabaseModel.Column[] columns;
			string error;

			if (GetCachedTableColumns(out columns, out error))
			{
				foreach (var column in columns)
				{
					string dragText = $"{sqlGenerator.Delimiter}{column.Name}{sqlGenerator.Delimiter}";
					SimpleNodeViewModel columnViewModel = new ParameterNodeViewModel(column.Name + " [" + column.DataType.ToString() + "]", dragText, this);
					Children.Add(columnViewModel);
				}
			}
			else
				Children.Add(new SimpleNodeViewModel(error, false, this));

			this.childrenLoaded = true;
		}

		private void AddMockChild()
		{
			IsExpanded = false;
			this.childrenLoaded = false;
			Children.Add(new ParameterNodeViewModel("Loading...", string.Empty, this));
		}

		private bool GetCachedTableColumns(out DatabaseModel.Column[] columns, out string error)
		{
			if (this.cachedTableColumns == null)
			{
				try
				{
					var assistant = DatabaseAssistant.GetDatabaseAssistant(this.connectionType);
					using (IDbConnection connection = assistant.CreateConnection(this.connectionString))
					{
						connection.Open();
						this.cachedTableColumns = assistant.GetTableColumns(connection, Text).ToArray();
					}
				}
				catch (Exception exception)
				{
					this.cachedTableColumns = new DatabaseModel.Column[0];
					this.cachedTableColumnsError = exception.Message;
				}
			}

			columns = this.cachedTableColumns;
			error = this.cachedTableColumnsError;
			return string.IsNullOrEmpty(this.cachedTableColumnsError);
		}
	}
}
