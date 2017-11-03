using Microsoft.Win32;

namespace Twenty57.Linx.Components.Database.Tests.Helpers
{
	public static class OdbcHelpers
	{
		public static void CreateSqlServerDSN(string name, string description, string sqlServerDriverPath, string server, string databaseName, string defaultUser)
		{
			RegistryKey odbcKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\ODBC\ODBC.INI\" + name);
			try
			{
				odbcKey.SetValue("Description", description);
				odbcKey.SetValue("Driver", sqlServerDriverPath);
				odbcKey.SetValue("Server", server);
				odbcKey.SetValue("Database", databaseName);
				odbcKey.SetValue("LastUser", defaultUser);
			}
			finally
			{
				odbcKey.Close();
			}

			RegistryKey odbcDirectoryKey = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources");
			try
			{
				odbcDirectoryKey.SetValue(name, "SQL Server");
			}
			finally
			{
				odbcDirectoryKey.Close();
			}
		}

		public static void RemoveSqlServerDSN(string name)
		{
			Registry.LocalMachine.DeleteSubKey(@"SOFTWARE\ODBC\ODBC.INI\" + name);
			RegistryKey odbcDirectoryKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\ODBC\ODBC.INI\ODBC Data Sources", true);
			try
			{
				odbcDirectoryKey.DeleteValue(name);
			}
			finally
			{
				odbcDirectoryKey.Close();
			}
		}
	}
}
