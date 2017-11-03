using System;
using System.IO;
using System.Reflection;

namespace Twenty57.Linx.Components.File.Tests.Helpers
{
	internal static class ResourceHelpers
	{
		public static void WriteResourceToFile(string resourceName, string filePath)
		{
			using (Stream input = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
			using (Stream output = System.IO.File.Create(filePath))
			{
				input.CopyTo(output);
			}
		}

		internal static void WriteResourceToFile(string v, Assembly assembly, string templateFilePath)
		{
			throw new NotImplementedException();
		}
	}
}
