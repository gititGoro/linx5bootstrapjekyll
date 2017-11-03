using System.Data;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.BeginTransaction
{
	public interface ITransactionDesignerData
	{
		ConnectionType ConnectionType { get; }
		string ConnectionString { get; }
		IsolationLevel IsolationLevel { get; }
	}
}
