using System;
using System.ComponentModel;
using System.IO;
using Twenty57.Linx.Components.File.Converters;

namespace Twenty57.Linx.Components.File
{
	public static class DirectoryWatchShared
	{
		public const string BufferSizePropertyName = "Buffer size";
		public const string FilterPropertyName = "Filter";
		public const string IncludeSubdirectoriesPropertyName = "Include subdirectories";
		public const string NotifyFilterPropertyName = "Notify filter";
		public const string PathPropertyName = "Path";
		public const string WatchForChangesPropertyName = "Watch for changes";
		public const string WatchForCreationPropertyName = "Watch for creation";
		public const string WatchForDeletionsPropertyName = "Watch for deletions";
		public const string WatchForRenamingPropertyName = "Watch for renaming";

		public const string FullPathOutputName = "FullPath";
		public const string NameOutputName = "Name";
		public const string OldFullPathOutputName = "OldFullPath";
		public const string OldNameOutputName = "OldName";

		public const string ChangedEventEventName = "ChangedEvent";
		public const string CreatedEventEventName = "CreatedEvent";
		public const string DeletedEventEventName = "DeletedEvent";
		public const string ErrorEventEventName = "ErrorEvent";
		public const string RenamedEventEventName = "RenamedEvent";
	}

	[Serializable, TypeConverter(typeof(SerializableExpandableObjectConverter))]
	public class NotifyFilter : ICloneable
	{
		public NotifyFilter()
		{
			Attributes = false;
			CreationTime = false;
			DirectoryName = false;
			FileName = true;
			LastAccess = false;
			LastWrite = false;
			Security = false;
			Size = false;
		}

		[Description("The attributes of the file or folder.")]
		public bool Attributes { get; set; }

		[DisplayName("Creation time")]
		[Description("The time the file or folder was created.")]
		public bool CreationTime { get; set; }

		[DisplayName("Directory name")]
		[Description("The name of the directory.")]
		public bool DirectoryName { get; set; }

		[DisplayName("File name")]
		[Description("The name of the file")]
		public bool FileName { get; set; }

		[DisplayName("Last access")]
		[Description("The date the file of folder was last opened.")]
		public bool LastAccess { get; set; }

		[DisplayName("Last write")]
		[Description("The date the file or folder last had anything written to it.")]
		public bool LastWrite { get; set; }

		[Description("The security settings of the file or folder.")]
		public bool Security { get; set; }

		[Description("The size of the file or folder.")]
		public bool Size { get; set; }

		public override string ToString()
		{
			return "(Complex Value)";
		}

		public object Clone()
		{
			NotifyFilter n = new NotifyFilter();
			n.Attributes = Attributes;
			n.CreationTime = CreationTime;
			n.DirectoryName = DirectoryName;
			n.FileName = FileName;
			n.LastAccess = LastAccess;
			n.LastWrite = LastWrite;
			n.Security = Security;
			n.Size = Size;
			return n;
		}

		public NotifyFilters CalcFilter()
		{
			NotifyFilters nf = 0;
			if (Attributes)
				nf = nf | NotifyFilters.Attributes;
			if (CreationTime)
				nf = nf | NotifyFilters.CreationTime;
			if (DirectoryName)
				nf = nf | NotifyFilters.DirectoryName;
			if (FileName)
				nf = nf | NotifyFilters.FileName;
			if (LastAccess)
				nf = nf | NotifyFilters.LastAccess;
			if (LastWrite)
				nf = nf | NotifyFilters.LastWrite;
			if (Security)
				nf = nf | NotifyFilters.Security;
			if (Size)
				nf = nf | NotifyFilters.Size;

			return nf;
		}

		public string ConvertToString()
		{
			return
				Attributes.ToString()
				+ "|" + CreationTime.ToString()
				+ "|" + DirectoryName.ToString()
				+ "|" + FileName.ToString()
				+ "|" + LastAccess.ToString()
				+ "|" + LastWrite.ToString()
				+ "|" + Security.ToString()
				+ "|" + Size.ToString();
		}

		public static NotifyFilter ConvertFromString(string notifyFilter)
		{
			string[] values = notifyFilter.Split('|');
			if (values.Length != 8)
				throw new Exception("NotifyFilter could not be parsed from the string [" + notifyFilter + "]. An incorrect number of values where found in the string.");

			Func<int, bool> Parse = (index) => Boolean.Parse(String.IsNullOrEmpty(values[index]) ? "False" : values[index]);

			return new NotifyFilter
			{
				Attributes = Parse(0),
				CreationTime = Parse(1),
				DirectoryName = Parse(2),
				FileName = Parse(3),
				LastAccess = Parse(4),
				LastWrite = Parse(5),
				Security = Parse(6),
				Size = Parse(7)
			};
		}
	}
}