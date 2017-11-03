using GongSolutions.Wpf.DragDrop;
using ICSharpCode.AvalonEdit;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors.Static;
using Twenty57.Linx.Plugin.UI.Helpers;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel
{
	public class SQLViewModel : INotifyPropertyChanged, IDragSource, IDropTarget
	{
		private const int MaxTestResultCount = 50;

		private bool? dialogResult;
		private int sqlIndex;
		private ConnectionType connectionType;
		private string connectionString;
		private SqlStringHandler sqlStringHandler;
		private IDesignerContext designerContext;
		private bool sqlParametersDirty = true;
		private List<SQLParameterModel> sqlParameters = new List<SQLParameterModel>();
		private double sqlParameterNameColumnWidth;

		private DataView queryResults;
		private bool moreResults = false;
		private string queryError;
		private int sqlSelectedTabIndex;

		private ICommand refreshCommand;
		private ICommand expressionCommand;
		private ICommand executeSQLCommand;
		private ICommand okCommand;

		private bool refreshIsRunning;
		private bool executeSQLIsRunning;
		private bool loadingDatabaseObjects;
		private bool databaseObjectsLoaded;
		private int editorTestTabSelectedIndex;

		public SQLViewModel(ExecuteSQLDesigner executeSQLDesigner)
		{
			this.connectionType = executeSQLDesigner.ResolvedConnectionType;
			this.connectionString = executeSQLDesigner.ResolvedConnectionString;

			this.sqlStringHandler = new SqlStringHandler();
			this.sqlStringHandler.SqlString = executeSQLDesigner.ResolvedSqlStatementValue ?? string.Empty;

			this.sqlIndex = 0;
			this.designerContext = executeSQLDesigner.Context;
			Designer = executeSQLDesigner;

			this.queryResults = null;
			this.queryError = String.Empty;

			DatabaseObjects = new ObservableCollection<NodeViewModel>();

			this.refreshIsRunning = false;
			this.executeSQLIsRunning = false;
		}

		public FunctionDesigner Designer { get; private set; }

		public Action<string> SetSqlAction
		{
			get
			{
				return sql =>
				{
					if (string.IsNullOrWhiteSpace(SQL))
					{
						SQL = sql;
					}
					else
					{
						SQL = $"{SQL}{Environment.NewLine}{sql}";
					}
				};
			}
		}

		public int SQLSelectedTabIndex
		{
			get
			{
				return this.sqlSelectedTabIndex;
			}
			set
			{
				if (this.sqlSelectedTabIndex != value)
				{
					this.sqlSelectedTabIndex = value;
					if (this.sqlSelectedTabIndex == 1 && !DatabaseObjectsLoaded && CanExecuteRefreshCommand())
						ExecuteRefreshCommand();
					OnPropertyChanged();
				}
			}
		}

		public int EditorTestTabSelectedIndex
		{
			get { return this.editorTestTabSelectedIndex; }
			set
			{
				if (this.editorTestTabSelectedIndex != value)
				{
					this.editorTestTabSelectedIndex = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(SQLTabSelected));
				}
			}
		}

		public bool SQLTabSelected
		{
			get
			{
				return this.EditorTestTabSelectedIndex == 0;
			}
		}

		public string SQL
		{
			get { return this.sqlStringHandler.SqlString; }
			set
			{
				if (this.sqlStringHandler.SqlString != value)
				{
					this.sqlStringHandler.SqlString = value;
					sqlParametersDirty = true;
					OnPropertyChanged();
					OnPropertyChanged(nameof(CanExecuteSQL));
					OnPropertyChanged(nameof(SQLParameters));
				}
			}
		}

		public int SQLIndex
		{
			get { return this.sqlIndex; }
			set
			{
				if (this.sqlIndex != value)
				{
					this.sqlIndex = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(SQLIndexInExpression));
				}
			}
		}

		public bool SQLIndexInExpression
		{
			get
			{
				SQLExpression expression;
				return InSQLExpression(out expression);
			}
		}

		public bool? DialogResult
		{
			get { return this.dialogResult; }
			set
			{
				if (this.dialogResult != value)
				{
					this.dialogResult = value;
					OnPropertyChanged();
				}
			}
		}

		public List<SQLParameterModel> SQLParameters
		{
			get
			{
				if (sqlParametersDirty)
				{
					Dictionary<string, string> valueMap = sqlParameters.ToDictionary(p => p.Name, p => p.Value);
					sqlParameters = sqlStringHandler.DistinctExpressionTexts.Select(e => new SQLParameterModel(e, valueMap.ContainsKey(e) ? valueMap[e] : string.Empty)).ToList();
					sqlParametersDirty = false;
					Task.Factory.StartNew(() =>
					{
						Thread.Sleep(50);
						OnPropertyChanged(nameof(SQLParameterNameColumnWidth));
					});
					OnPropertyChanged(nameof(SQLParameterCount));
				}
				return sqlParameters;
			}
		}

		public int SQLParameterCount
		{
			get { return sqlParameters.Count; }
		}

		public double SQLParameterNameColumnWidth
		{
			get
			{
				if (SQLParameters.Count == 0)
					return 0;
				double width = SQLParameters.Max(p => p.NameLabelWidth);
				if (width != 0)
					sqlParameterNameColumnWidth = width + 5;
				return sqlParameterNameColumnWidth;
			}
		}

		public bool HasQueryResults
		{
			get { return null != QueryResults; }
		}

		public DataView QueryResults
		{
			get { return this.queryResults; }
			private set
			{
				this.queryResults = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ResultsCount));
				OnPropertyChanged(nameof(HasQueryResults));
				OnPropertyChanged(nameof(HasQueryError));
				OnPropertyChanged(nameof(HasQueryExecuted));
			}
		}

		public int ResultsCount
		{
			get { return queryResults == null ? 0 : queryResults.Count; }
		}

		public bool MoreResults
		{
			get { return moreResults; }
			set
			{
				if (this.moreResults != value)
				{
					this.moreResults = value;
					OnPropertyChanged();
				}
			}
		}

		public bool HasQueryError
		{
			get { return !String.IsNullOrEmpty(QueryError); }
		}

		public string QueryError
		{
			get { return this.queryError; }
			private set
			{
				if (value != this.queryError)
				{
					this.queryError = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(HasQueryResults));
					OnPropertyChanged(nameof(HasQueryError));
					OnPropertyChanged(nameof(HasQueryExecuted));
				}
			}
		}

		public bool HasQueryExecuted
		{
			get { return HasQueryResults || HasQueryError; }
		}

		public bool CanExecuteSQL
		{
			get { return CanExecuteExecuteSQLCommand(); }
		}

		public ObservableCollection<NodeViewModel> DatabaseObjects { get; private set; }

		private bool ExecuteSQLIsRunning
		{
			get { return executeSQLIsRunning; }
			set
			{
				executeSQLIsRunning = value;
				OnPropertyChanged(nameof(CanExecuteSQL));
			}
		}

		public ICommand RefreshCommand
		{
			get
			{
				if (null == this.refreshCommand)
					this.refreshCommand = new DelegateCommand(ExecuteRefreshCommand, CanExecuteRefreshCommand);

				return this.refreshCommand;
			}
		}

		public ICommand ExpressionCommand
		{
			get
			{
				if (null == this.expressionCommand)
					this.expressionCommand = new DelegateCommand(ExecuteExpressionCommand);

				return this.expressionCommand;
			}
		}

		public ICommand ExecuteSQLCommand
		{
			get
			{
				if (null == this.executeSQLCommand)
					this.executeSQLCommand = new DelegateCommand(ExecuteExecuteSQLCommand, CanExecuteExecuteSQLCommand);

				return this.executeSQLCommand;
			}
		}

		public ICommand OKCommand
		{
			get
			{
				if (null == this.okCommand)
					this.okCommand = new DelegateCommand(() => DialogResult = true);

				return this.okCommand;
			}
		}

		public void Dropped(IDropInfo dropInfo)
		{ }

		public void StartDrag(IDragInfo dragInfo)
		{
			dragInfo.Data = null;
			dragInfo.Effects = DragDropEffects.None;

			if (null == dragInfo.SourceItem)
				return;

			NodeViewModel draggedModel = dragInfo.SourceItem as NodeViewModel;
			if (null == draggedModel)
				throw new Exception("Dragged item is not of type NodeViewModel.");

			if (!draggedModel.AllowDrag)
				return;

			dragInfo.Data = draggedModel;
			dragInfo.Effects = DragDropEffects.Move;
		}

		public void DragLeave(IDropInfo dropInfo)
		{ }

		public void DragOver(IDropInfo dropInfo)
		{
			dropInfo.Effects = DragDropEffects.None;
			NodeViewModel draggedModel = dropInfo.Data as NodeViewModel;

			TextEditor editor = dropInfo.VisualTarget as TextEditor;
			if (dropInfo.Data != null && editor != null)
			{
				TextViewPosition? position = editor.GetPositionFromPoint(dropInfo.DropPosition);
				if (position.HasValue)
				{
					editor.CaretOffset = editor.Document.GetOffset(position.Value.Line, position.Value.Column);
					dropInfo.Effects = dropInfo.AllowedEffects;
				}

				if (position == null)
				{
					editor.CaretOffset = editor.Document.TextLength;
					dropInfo.Effects = dropInfo.AllowedEffects;
				}
				editor.Focus();
			}
		}

		public void Drop(IDropInfo dropInfo)
		{
			TextEditor editor = dropInfo.VisualTarget as TextEditor;
			string dragText = string.Empty;
			if (dropInfo.Data is NodeViewModel)
			{
				var draggedModel = dropInfo.Data as NodeViewModel;
				dragText = (string.IsNullOrEmpty(editor.Text.Trim())) ? draggedModel.GetExtendedDragText() : draggedModel.GetSimpleDragText();
			}
			else if (dropInfo.Data is string)
			{
				dragText = dropInfo.Data as string;

				if (dragText == VariablesEditor.ExpressionEditorText)
				{
					ExecuteExpressionCommand();
					dragText = string.Empty;
				}
				else
				{
					SQLExpression expression;
					if (!InSQLExpression(out expression))
						dragText = SqlStringHandler.CreateSqlExpression(dragText);
				}
			}

			var caretIndex = editor.CaretOffset;
			editor.Text = editor.Text.Insert(caretIndex, dragText);
			editor.CaretOffset = caretIndex + dragText.Length;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (this.PropertyChanged != null)
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		private void ExecuteRefreshCommand()
		{
			this.refreshIsRunning = true;
			LoadingDatabaseObjects = true;

			DatabaseObjects.Clear();
			DatabaseObjects.Add(new SimpleNodeViewModel("Retrieving information..."));

			Task.Factory.StartNew<ObservableCollection<NodeViewModel>>(GetDatabaseObjectsAsync)
				.ContinueWith((previousTask) =>
				{
					Application.Current.Dispatcher.Invoke(() =>
						{
							DatabaseObjects.Clear();
							foreach (NodeViewModel dbObject in previousTask.Result)
								DatabaseObjects.Add(dbObject);

							this.refreshIsRunning = false;
							LoadingDatabaseObjects = false;
						});
				});
		}

		private bool CanExecuteRefreshCommand()
		{
			return !(String.IsNullOrEmpty(this.connectionString) || this.refreshIsRunning);
		}

		private void ExecuteExpressionCommand()
		{
			SQLExpression expression;
			bool editExpression = InSQLExpression(out expression);
			string expressionText = editExpression ? '=' + expression.ExpressionText : String.Empty;

			IExpression modifiedValue;
			if (designerContext.EditExpression(expressionText, out modifiedValue))
			{
				string wrappedExpression = string.Empty;
				if (!modifiedValue.IsEmpty)
					wrappedExpression = SqlStringHandler.CreateSqlExpression(modifiedValue.GetExpression());

				if (editExpression)
				{
					Regex replaceRegex = new Regex(SqlStringHandler.expressionPattern);
					SQL = replaceRegex.Replace(SQL, wrappedExpression, 1, expression.StartIndex);
				}
				else
					SQL = SQL.Insert(SQLIndex, wrappedExpression);
			}
		}

		private void ExecuteExecuteSQLCommand()
		{
			ExecuteSQLIsRunning = true;

			Task.Factory.StartNew<DataView>(ExecuteSQL)
				.ContinueWith((previousTask) =>
				{
					Application.Current.Dispatcher.Invoke(() =>
						{
							if (previousTask.Status == TaskStatus.Faulted && null != previousTask.Exception)
							{
								string exceptionText = String.Empty;
								previousTask.Exception.Handle((exception) => { exceptionText += exception.Message; return true; });
								QueryError = exceptionText;
								QueryResults = null;
							}
							else
							{
								QueryError = String.Empty;
								QueryResults = previousTask.Result;
							}

							ExecuteSQLIsRunning = false;
						});
				});
		}

		//https://wpfanimatedgif.codeplex.com/documentation
		public bool LoadingDatabaseObjects
		{
			get { return this.loadingDatabaseObjects; }
			set
			{
				if (this.loadingDatabaseObjects != value)
				{
					this.loadingDatabaseObjects = value;
					this.DatabaseObjectsLoaded = !value;
					OnPropertyChanged();
				}
			}
		}

		public bool DatabaseObjectsLoaded
		{
			get
			{
				return this.databaseObjectsLoaded;
			}
			set
			{
				if (this.databaseObjectsLoaded != value)
				{
					this.databaseObjectsLoaded = value;
					OnPropertyChanged();
				}
			}
		}

		private DataView ExecuteSQL()
		{
			string[] expressionNames = this.sqlStringHandler.ExpressionTexts.Select(et => SqlStringHandler.CreateSqlExpression(et)).ToArray();
			string[] expressionValues = this.sqlStringHandler.ExpressionTexts.Select(et => SQLParameters.First(sp => sp.Name == et).Value).ToArray();

			string parameterizedSql = SqlStringHandler.GetParameterizedSql(this.sqlStringHandler.SqlString, expressionNames);
			using (var adapter = GetAdapter(this.connectionType, this.connectionString, parameterizedSql, expressionValues))
			{
				DataTable dataTable = new DataTable();
				adapter.Fill(0, MaxTestResultCount + 1, dataTable);
				if ((MoreResults = dataTable.Rows.Count == MaxTestResultCount + 1))
					dataTable.Rows.RemoveAt(dataTable.Rows.Count - 1);
				return dataTable.DefaultView;
			}
		}

		private bool CanExecuteExecuteSQLCommand()
		{
			return !(String.IsNullOrEmpty(this.connectionString) || String.IsNullOrEmpty(SQL) || ExecuteSQLIsRunning);
		}

		private ObservableCollection<NodeViewModel> GetDatabaseObjectsAsync()
		{
			switch (this.connectionType)
			{
				case ConnectionType.Oracle:
				case ConnectionType.SqlServer:
					return GetDatabaseAssistantObjectsAsync();
				default:
					return GetGenericDatabaseObjectsAsync();
			}
		}

		private ObservableCollection<NodeViewModel> GetDatabaseAssistantObjectsAsync()
		{
			ObservableCollection<NodeViewModel> databaseObjects = new ObservableCollection<NodeViewModel>();

			IDbConnection connection = null;
			try
			{
				var assistant = DatabaseAssistant.GetDatabaseAssistant(this.connectionType);
				connection = assistant.CreateConnection(this.connectionString);
				connection.Open();

				var tableContainer = new ComplexNodeViewModel("Tables");
				databaseObjects.Add(tableContainer);
				foreach (var tableName in assistant.GetTableNames(connection).OrderBy(t => t))
					tableContainer.Children.Add(new TableNodeViewModel(this.connectionType, this.connectionString, tableName, tableContainer, SetSqlAction, true));

				var viewContainer = new ComplexNodeViewModel("Views");
				databaseObjects.Add(viewContainer);
				foreach (var viewName in assistant.GetViewNames(connection).OrderBy(v => v))
					viewContainer.Children.Add(new TableNodeViewModel(this.connectionType, this.connectionString, viewName, viewContainer, null, false));

				if (this.connectionType != ConnectionType.Oracle)
				{
					var storedProcedureContainer = new ComplexNodeViewModel("Stored Procedures");
					databaseObjects.Add(storedProcedureContainer);

					foreach (var spName in assistant.GetStoredProcedureNames(connection).OrderBy(sp => sp))
						storedProcedureContainer.Children.Add(new StoredProcedureNodeViewModel(assistant, connection, spName, storedProcedureContainer));
				}
			}
			catch (Exception exception)
			{
				databaseObjects.Clear();
				databaseObjects.Add(new SimpleNodeViewModel(exception.Message));
			}
			finally
			{
				if (connection != null)
				{
					connection.Close();
					connection.Dispose();
				}
			}

			return databaseObjects;
		}

		private ObservableCollection<NodeViewModel> GetGenericDatabaseObjectsAsync()
		{
			ObservableCollection<NodeViewModel> databaseObjects = new ObservableCollection<NodeViewModel>();

			ADODB.Connection connection = null;
			try
			{
				connection = new ADODB.ConnectionClass();
				connection.Open(this.connectionString, "", "", (int)ADODB.ConnectOptionEnum.adConnectUnspecified);

				ADOX.Catalog catalog = new ADOX.CatalogClass();
				catalog.ActiveConnection = connection;

				var tableContainer = new ComplexNodeViewModel("Tables");
				var viewContainer = new ComplexNodeViewModel("Views");
				PopulateTablesAndViews(catalog, tableContainer, viewContainer);

				var storedProcedureContainer = new ComplexNodeViewModel("Stored Procedures");
				PopulateStoredProcedures(catalog, connection, storedProcedureContainer);

				databaseObjects.Add(tableContainer);
				databaseObjects.Add(viewContainer);
				databaseObjects.Add(storedProcedureContainer);
			}
			catch (Exception exception)
			{
				databaseObjects.Clear();
				databaseObjects.Add(new SimpleNodeViewModel(exception.Message));
			}
			finally
			{
				if (null != connection)
				{
					try
					{
						connection.Close();
					}
					catch { }
				}
			}

			return databaseObjects;
		}

		private bool InSQLExpression(out SQLExpression expression)
		{
			expression = this.sqlStringHandler.Expressions.FirstOrDefault(
				item => item.StartIndex <= SQLIndex && item.EndIndex >= SQLIndex);
			return (null != expression);
		}

		private void PopulateTablesAndViews(ADOX.Catalog catalog, NodeViewModel tableContainer, NodeViewModel viewContainer)
		{
			foreach (ADOX.Table table in catalog.Tables)
			{
				TableNodeViewModel viewModel = null;
				if (table.Type == "VIEW")
				{
					viewModel = new TableNodeViewModel(this.connectionType, this.connectionString, table.Name, viewContainer, null, false);
					viewContainer.Children.Add(viewModel);
				}
				else if (table.Type == "TABLE")
				{
					viewModel = new TableNodeViewModel(this.connectionType, this.connectionString, table.Name, tableContainer, SetSqlAction, true);
					tableContainer.Children.Add(viewModel);
				}
				else
					continue;
			}
		}

		private static void PopulateStoredProcedures(ADOX.Catalog catalog, ADODB.Connection connection, NodeViewModel storedProcedureContainer)
		{
			foreach (ADOX.Procedure procedure in catalog.Procedures)
			{
				storedProcedureContainer.Children.Add(new StoredProcedureNodeViewModel(procedure, connection, storedProcedureContainer));
			}
		}

		private static DbDataAdapter GetAdapter(ConnectionType connectionType, string connectionString, string sqlText, string[] parameterValues)
		{
			DbDataAdapter adapter = null;
			switch (connectionType)
			{
				case ConnectionType.SqlServer:
					adapter = new SqlDataAdapter();
					break;
				case ConnectionType.Odbc:
					adapter = new OdbcDataAdapter();
					break;
				case ConnectionType.OleDb:
					adapter = new OleDbDataAdapter();
					break;
				case ConnectionType.Oracle:
					adapter = new OracleDataAdapter();
					break;
			}

			adapter.SelectCommand = (DbCommand)DatabaseHelpers.CreateCommand(connectionType, connectionString, sqlText, parameterValues, 60);
			return adapter;
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
