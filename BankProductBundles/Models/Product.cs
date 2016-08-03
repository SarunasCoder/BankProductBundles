using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankProductBundles.Models
{
	/// <summary>
	/// This model class defines Product structure
	/// </summary>
	public class Product
	{

		/// <summary>
		/// Product Id
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Product name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Product type
		/// </summary>
		public ProdType Type { get; set; }

		/// <summary>
		/// Product's min age
		/// </summary>
		public int AgeFrom { get; set; }

		/// <summary>
		/// Product's max age
		/// </summary>
		public int AgeTo { get; set; }

		/// <summary>
		/// Product for student
		/// </summary>
		/// <value>Value meanings: 1-only for students, 0-only for non students,
		/// -1-for students and non students</value>
		public int Student { get; set; }

		/// <summary>
		/// Product's min income
		/// </summary>
		public int Income { get; set; }
	}

	/// <summary>
	/// Type of the Product
	/// </summary>
	public enum ProdType
	{
		/// <summary>
		/// Account type of the Product
		/// </summary>
		Account = 1,
		/// <summary>
		/// Card type of the Product
		/// </summary>
		Card = 2
	}
}