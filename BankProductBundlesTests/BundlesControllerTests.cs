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
	/// This class exposes tests for BundlesController class
	/// </summary>
	[TestClass]
	public class BundlesControllerTests
	{

		/// <summary> 
		/// In constructor path to local DB is set
		/// </summary>
		public BundlesControllerTests()
		{
			var dataDirectory = ConfigurationManager.AppSettings["DataDirectory"];
			var absoluteDataDirectory = Path.GetFullPath(dataDirectory);
			AppDomain.CurrentDomain.SetData("DataDirectory", absoluteDataDirectory);
		}

		/// <summary> 
		/// This method tests if PostBundle method creates and returns bundle to a user
		/// </summary>
		[TestMethod]
		public void PostBundle_CreatesBundle()
		{
			HttpStatusCode expected = HttpStatusCode.Created;

			var bundles = new BundlesController();
			bundles.Request = new HttpRequestMessage();
			bundles.Configuration = new HttpConfiguration();

			HttpResponseMessage actual = bundles.PostBundle(18, 0, 10000);

			XmlDocument resXml = new XmlDocument();
			Assert.IsTrue(actual.TryGetContentValue<XmlDocument>(out resXml));
			Assert.IsNotNull(resXml);
			Assert.IsTrue(resXml.SelectSingleNode("Root/UserID") != null);
			Assert.AreEqual(expected, actual.StatusCode);
		}

		/// <summary> 
		/// This method tests if GetBundleConfirm method returns the right
		/// StatusCode if input is 0
		/// </summary>
		[TestMethod]
		public void GetBundleConfirm_ZeroUserAsInput()
		{
			HttpStatusCode expected = HttpStatusCode.NotFound;

			var bundles = new BundlesController();
			bundles.Request = new HttpRequestMessage();
			bundles.Configuration = new HttpConfiguration();

			HttpResponseMessage actual = bundles.GetBundleConfirm(0);

			Assert.AreEqual(expected, actual.StatusCode);
		}

		/// <summary> 
		/// This method tests if GetBundleConfirm method returns the right StatusCode if 
		/// bundle is not valid for a user 
		/// </summary>
		[TestMethod]
		public void GetBundleConfirm_BadUserBundle()
		{
			HttpStatusCode expected = HttpStatusCode.BadRequest;

			var bundles = new BundlesController();
			bundles.Request = new HttpRequestMessage();
			bundles.Configuration = new HttpConfiguration();

			HttpResponseMessage actual = bundles.GetBundleConfirm(2);

			Assert.AreEqual(expected, actual.StatusCode);
		}

		/// <summary> 
		/// This method tests if GetBundleConfirm method returns the right StatusCode if 
		/// bundle is valid for a user 
		/// </summary>
		[TestMethod]
		public void GetBundleConfirm_GoodUserBundle()
		{
			HttpStatusCode expected = HttpStatusCode.OK;

			var bundles = new BundlesController();
			bundles.Request = new HttpRequestMessage();
			bundles.Configuration = new HttpConfiguration();

			HttpResponseMessage actual = bundles.GetBundleConfirm(1);

			Assert.AreEqual(expected, actual.StatusCode);
		}

		
	}
}
