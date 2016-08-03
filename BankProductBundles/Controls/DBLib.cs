using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace BankProductBundles.Controls
{
	/// <summary> 
	/// This helper class exposes actions with local DB
	/// </summary>
	public class DBLib
	{

		private SqlConnection scon = new SqlConnection(ConfigurationManager.ConnectionStrings["DB"].ConnectionString);
		private SqlCommand cmd = new SqlCommand();

		/// <summary>
		/// Defines stored procedure to execute
		/// </summary>
		/// <param name="procName">The stored procedure name to execute</param>
		public void AddProcedure(string procName)
		{
			cmd = new SqlCommand(procName, scon);
			cmd.CommandType = CommandType.StoredProcedure;
		}

		/// <summary>
		/// Defines stored procedure's parameter
		/// </summary>
		/// <param name="paramName">Parameter name</param>
		/// <param name="dbType">Parameter DB data type</param>
		/// <param name="paramValue">Parameter value</param>
		public void AddParameter(string paramName, SqlDbType dbType, object paramValue)
		{
			SqlParameter param = new SqlParameter(paramName, dbType);
			param.Direction = ParameterDirection.Input;
			param.Value = paramValue;
			cmd.Parameters.Add(param);
		}

		/// <summary>
		/// Gets sotred procedure result in Xml
		/// </summary>
		/// <returns>DB result from stored procedure in XmlDocument</returns>
		public XmlDocument GetResultAsXmlDocument()
		{
			scon.Open();
			XmlDocument retXml = new XmlDocument();
			XmlReader xmlReader = cmd.ExecuteXmlReader();
			retXml.Load(xmlReader);
			scon.Close();

			return retXml;
		}

	}
}