using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Twenty57.Linx.Components.Database.Common
{
	public class SqlStringHandler : ICloneable
	{
		internal const string expressionWrapperPrefix = @"@{";
		internal const string expressionWrapperSuffix = @"}";
		internal const string expressionPattern = @"\@\{(.*?)\}";
		internal const string sqlParameterPrefix = "@param";

		private string sql;
		private static readonly Regex expressionRegex;

		public SqlStringHandler()
		{
			this.sql = String.Empty;
			Expressions = new List<SQLExpression>();
		}

		static SqlStringHandler()
		{
			expressionRegex = new Regex(expressionPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Singleline);
		}

		public string SqlString
		{
			get
			{
				return this.sql;
			}
			set
			{
				this.sql = value;
				ParseSql();
			}
		}

		public List<SQLExpression> Expressions { get; private set; }

		public IEnumerable<string> ExpressionTexts
		{
			get { return Expressions.Select(e => e.ExpressionText); }
		}

		public IEnumerable<string> DistinctExpressionTexts
		{
			get { return ExpressionTexts.Distinct(); }
		}

		public object Clone()
		{
			SqlStringHandler clone = new SqlStringHandler();
			clone.sql = this.sql;
			foreach (var expression in Expressions)
				clone.Expressions.Add((SQLExpression)expression.Clone());

			return clone;
		}

		internal string GetExecutableDesignTimeSql()
		{
			return expressionRegex.Replace(SqlString, "'0'");
		}

		internal static SqlStringHandler GetSqlStringHandler(string sql)
		{
			var sqlStringHandler = new SqlStringHandler();
			sqlStringHandler.SqlString = sql;
			return sqlStringHandler;
		}

		internal static string CreateSqlExpression(string expression)
		{
			return String.Format("{0}{1}{2}", expressionWrapperPrefix, expression, expressionWrapperSuffix);
		}

		public static string GetParameterizedSql(string sql, string[] expressionIdentifiers)
		{
			for (int index = 0; index < expressionIdentifiers.Length; index++)
				sql = sql.Replace(expressionIdentifiers[index], sqlParameterPrefix + index);
			return sql;
		}

		private void ParseSql()
		{
			Expressions.Clear();
			if (this.sql.Length == 0)
				return;

			int expressionCounter = 0;
			foreach (Match match in expressionRegex.Matches(this.sql))
				Expressions.Add(new SQLExpression(match, ++expressionCounter));
		}
	}

	public class SQLExpression : ICloneable
	{
		public SQLExpression(Match match, int nameIndex)
		{
			if (match.Groups.Count != 2)
				throw new Exception("Invalid match specified.");

			StartIndex = match.Index;
			EndIndex = match.Index + (match.Length - 1);
			MatchText = match.Groups[0].Value;
			ExpressionText = match.Groups[1].Value;
		}

		private SQLExpression(SQLExpression instance)
		{
			StartIndex = instance.StartIndex;
			EndIndex = instance.EndIndex;
			MatchText = instance.MatchText;
			ExpressionText = instance.ExpressionText;
		}

		public int StartIndex { get; private set; }
		public int EndIndex { get; private set; }
		public string MatchText { get; private set; }
		public string ExpressionText { get; private set; }

		public object Clone()
		{
			return new SQLExpression(this);
		}
	}
}
