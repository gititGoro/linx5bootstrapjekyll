using System;
using System.Collections;
using System.Collections.Generic;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Runtime
{
	public static class SingleEnumerable
	{
		public static IEnumerable<T> Make<T>(T item) where T : IDisposable
		{
			return SingleEnumerable<T>.Make(item);
		}
	}


	internal class SingleEnumerable<T> : IEnumerable<T> where T : IDisposable
	{
		private T item;

		private SingleEnumerable(T item)
		{
			this.item = item;
		}

		public static SingleEnumerable<T> Make(T item)
		{
			return new SingleEnumerable<T>(item);
		}

		public IEnumerator<T> GetEnumerator()
		{
			return new SingleEnumerator<T>(item);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		private class SingleEnumerator<U> : IEnumerator<U> where U : IDisposable
		{
			private U item;
			private bool done = false;

			public SingleEnumerator(U item)
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
					return false;
				done = true;
				return true;
			}

			public U Current
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
