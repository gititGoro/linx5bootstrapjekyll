using System.ComponentModel;

namespace Twenty57.Linx.Components.Database.BeginTransaction
{
	public static class BeginTransactionShared
	{
		public const string ConnectionTypePropertyName = "Connection type\x200B";
		public const string SqlServerIsolationLevelPropertyName = "Isolation level";
		public const string OracleIsolationLevelPropertyName = "Isolation level\x200B";
		public const string OleDbIsolationLevelPropertyName = "Isolation level\x200B\x200B";
		public const string OdbcIsolationLevelPropertyName = "Isolation level\x200B\x200B\x200B";

		public const string ExecutionPathName = "Transaction";
	}


	public enum SqlServerIsolationLevel
	{
		[Description("Read committed")]
		ReadCommitted,
		[Description("Read uncommitted")]
		ReadUncommitted,
		[Description("Repeatable read")]
		RepeatableRead,
		Serializable
	}

	public enum OracleIsolationLevel
	{
		[Description("Read committed")]
		ReadCommitted,
		Serializable
	}

	public enum OleDbIsolationLevel
	{
		Chaos,
		[Description("Read committed")]
		ReadCommitted,
		[Description("Read uncommitted")]
		ReadUncommitted,
		[Description("Repeatable read")]
		RepeatableRead,
		Serializable
	}

	public enum OdbcIsolationLevel
	{
		[Description("Read committed")]
		ReadCommitted,
		[Description("Read uncommitted")]
		ReadUncommitted,
		[Description("Repeatable read")]
		RepeatableRead,
		Serializable
	}
}
