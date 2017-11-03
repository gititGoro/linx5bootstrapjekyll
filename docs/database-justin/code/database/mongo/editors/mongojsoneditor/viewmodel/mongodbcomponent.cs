using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twenty57.Linx.Components.MongoDB;

namespace Twenty57.Linx.Components.Database.Mongo.Editors.MongoJsonEditor.ViewModel
{
	public interface MongoDBComponent
	{
		string Collection { get; }
		string ConnectionString { get; }
		List<MongoDatabaseObject> GetDatabaseObjects();
	}
}
