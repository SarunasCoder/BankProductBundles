using BankProductBundles.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace BankProductBundles.Controllers
{
		/// <summary> 
		/// This class exposes actions with bank products
		/// </summary>
    public class ProductsController : ApiController
    {

			/// <summary>
			/// This method adds product to user's bundle
			/// </summary>
			/// <remarks>
			/// This method uses DB stored procedure <note type="important">stp_AddProduct</note> 
			/// to add a product for a User in DB. And then returns selected and not selected products.
			/// </remarks>
			/// <param name="userId">User Id</param>
			/// <param name="productId">Product Id</param>
			/// <returns>A HttpResponseMessage with a HttpStatusCode and user's selected and not selected products</returns>
			public HttpResponseMessage PostProduct(int userId, int productId)
			{
				XmlDocument retXml = new XmlDocument();

				DBLib db = new DBLib();
				db.AddProcedure("stp_AddProduct");
				db.AddParameter("@UserID", SqlDbType.Int, userId);
				db.AddParameter("@ProductID", SqlDbType.Int, productId);
				retXml = db.GetResultAsXmlDocument();

				var response = Request.CreateResponse<XmlDocument>(HttpStatusCode.Created, retXml);

				return response;
			}

			/// <summary>
			/// This method removes product from user's bundle
			/// </summary>
			/// <remarks>
			/// This method uses DB stored procedure <note type="important">stp_RemoveProduct</note> 
			/// to remove a product for a User in DB. And then returns selected and not selected products.
			/// </remarks>
			/// <param name="userId">User Id</param>
			/// <param name="productId">Product Id</param>
			/// <returns>A HttpResponseMessage with a HttpStatusCode and user's selected and not selected products</returns>
			public HttpResponseMessage DeleteProduct(int userId, int productId)
			{
				XmlDocument retXml = new XmlDocument();

				DBLib db = new DBLib();
				db.AddProcedure("stp_RemoveProduct");
				db.AddParameter("@UserID", SqlDbType.Int, userId);
				db.AddParameter("@ProductID", SqlDbType.Int, productId);
				retXml = db.GetResultAsXmlDocument();

				var response = Request.CreateResponse<XmlDocument>(HttpStatusCode.OK, retXml);
			
				return response;
			}

    }
}
