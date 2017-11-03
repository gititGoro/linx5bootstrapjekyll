using System;
using System.Collections.Generic;

namespace Twenty57.Linx.Components.MongoDB
{
	public class MongoDatabaseObject
	{
		public MongoDatabaseObject(string name, string pathPrefix)
		{
			Name = name;
			PathPrefix = pathPrefix;
		}

		public string Name { get; set; }

		public string Path
		{
			get { return !String.IsNullOrEmpty(PathPrefix) ? PathPrefix + "." + Name : Name; }
		}

		public string PathPrefix { get; set; }

		public bool ComplexType { get; set; }

		internal class EqualityComparer : IEqualityComparer<MongoDatabaseObject>
		{
			public bool Equals(MongoDatabaseObject g1, MongoDatabaseObject g2)
			{
				return g1.Path == g2.Path;
			}

			public int GetHashCode(MongoDatabaseObject obj)
			{
				return obj.Path.GetHashCode();
			}
		}
	}
}