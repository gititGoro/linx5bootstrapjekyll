using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twenty57.Linx.Components.File
{
	[Serializable]
	public class ChangedOutputValues
	{
		public string FullPath { get; set; }
		public string Name { get; set; }
	}

	[Serializable]
	public class OldChangedOutputValues : ChangedOutputValues
	{
		public string OldFullPath { get; set; }
		public string OldName { get; set; }
	}
}