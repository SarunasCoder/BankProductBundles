using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankProductBundles.Controllers;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System.Xml;
using System.Configuration;
using System.IO;

namespace BankProductBundlesTests
{
	/// <summary> 
	/// This class exposes tests for ProductsController class
	/// </summary>
	[TestClass]
	public class ProductsControllerTests
	{

		/// <summary> 
		/// In constructor path to local DB is set
		/// </summary>
		public ProductsControllerTests()
		{
			var dataDirectory = ConfigurationManager.AppSettings["DataDirectory"];
			var absoluteDataDirectory = Path.GetFullPath(dataDirectory);
			AppDomain.CurrentDomain.SetData("DataDirectory", absoluteDataDirectory);
		}

		/// <summary> 
		/// This method tests if PostProduct method adds product successfully
		/// </summary>
		[TestMethod]
		public void PostProduct_ProductAdded()
		{
			HttpStatusCode expected = HttpStatusCode.Created;

			var products = new ProductsController();
			products.Request = new HttpRequestMessage();
			products.Configuration = new HttpConfiguration();
			int prodId = 1;

			HttpResponseMessage actual = products.PostProduct(3, prodId);

			XmlDocument resXml = new XmlDocument();
			Assert.IsTrue(actual.TryGetContentValue<XmlDocument>(out resXml));
			Assert.IsNotNull(resXml);
			Assert.IsTrue(resXml.SelectSingleNode(String.Format("Root/UserProducts/UserProduct[Id='{0}']", prodId.ToString())) != null);
			Assert.AreEqual(expected, actual.StatusCode);
		}

		/// <summary> 
		/// This method tests if DeleteProduct method removes product successfully
		/// </summary>
		[TestMethod]
		public void DeleteProduct_ProductDeleted()
		{
			HttpStatusCode expected = HttpStatusCode.OK;

			var products = new ProductsController();
			products.Request = new HttpRequestMessage();
			products.Configuration = new HttpConfiguration();
			int prodId = 1;

			HttpResponseMessage actual = products.DeleteProduct(3, prodId);

			XmlDocument resXml = new XmlDocument();
			Assert.IsTrue(actual.TryGetContentValue<XmlDocument>(out resXml));
			Assert.IsNotNull(resXml);
			Assert.IsTrue(resXml.SelectSingleNode(String.Format("Root/UserProducts/UserProduct[Id='{0}']", prodId.ToString())) == null);
			Assert.AreEqual(expected, actual.StatusCode);
		}

	}
}
