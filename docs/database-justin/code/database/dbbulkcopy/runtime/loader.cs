using System;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Runtime
{
	public class Loader : IDisposable
	{
		private DbBulkCopierX bulkCopier;
		private Func<dynamic, object[]> makeRow;

		public Loader(DbBulkCopierX bulkCopier, Func<dynamic, object[]> makeRow)
		{
			this.bulkCopier = bulkCopier;
			this.makeRow = makeRow;
		}

		public dynamic Write
		{
			set 
			{
				bulkCopier.AcceptRow(makeRow(value));
			}
		}

		public void Dispose()
		{
			bulkCopier.Dispose();
		}
	}
}
