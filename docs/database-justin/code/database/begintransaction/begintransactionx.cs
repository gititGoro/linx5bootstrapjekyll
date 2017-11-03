using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.BeginTransaction
{
	public class BeginTransactionX : IEnumerable<Transaction>
	{
		public BeginTransactionX(ConnectionType connectionType, string connectionString, IsolationLevel isolationLevel)
		{
			Transaction = new Transaction(connectionType, connectionString, isolationLevel);
		}

		public Transaction Transaction { get; private set; }

		public IEnumerator<Transaction> GetEnumerator()
		{
			return new SingleEnumerator<Transaction>(Transaction);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		private class SingleEnumerator<T> : IEnumerator<T> where T : IDisposable, IDone
		{
			private T item;
			private bool done = false;

			public SingleEnumerator(T item)
			{
				this.item = item;
			}

			public void Reset()
			{
				done = false;
			}

			public bool MoveNext()
			{
				if (done)
				{
					item.SetIsDone();
					return false;
				}
				done = true;
				return true;
			}

			public T Current
			{
				get { return item; }
			}

			object IEnumerator.Current
			{
				get { return item; }
			}

			public void Dispose()
			{
				item.Dispose();
			}
		}
	}
}
