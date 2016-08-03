using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using BankProductBundles.Controls;
using BankProductBundles.Models;
using System.Xml.Linq;

namespace BankProductBundles.Controllers
{
		/// <summary> 
		/// This class exposes actions with bank product bundles
		/// </summary>
    public class BundlesController : ApiController
    {

			/// <summary>
			/// This method from provided user information offers the best product bundle for him
			/// </summary>
			/// <remarks>
			/// By provided user information, method uses DB stored procedure <note type="important">stp_GetBundle</note> 
			/// to save user's information and best bundle for him. And then returns bundle's products.
			/// </remarks>
			/// <param name="age">User age</param>
			/// <param name="student">Is user a student information (1-yes, 0-no)</param>
			/// <param name="income">User income</param>
			/// <returns>A HttpResponseMessage with a HttpStatusCode, the best bundle's products and all other products</returns>
			public HttpResponseMessage PostBundle(int age, int student, int income)
			{
				XmlDocument retXml = new XmlDocument();

				DBLib db = new DBLib();
				db.AddProcedure("stp_GetBundle");
				db.AddParameter("@Age", SqlDbType.Int, age);
				db.AddParameter("@Student", SqlDbType.Bit, student);
				db.AddParameter("@Income", SqlDbType.Int, income);
				retXml = db.GetResultAsXmlDocument();

				var response = Request.CreateResponse<XmlDocument>(HttpStatusCode.Created, retXml);
				
				return response;
			}

			/// <summary>
			/// This method confirms if user's selected products are valid for him
			/// </summary>
			/// <remarks>
			/// By provided user Id, method uses DB stored procedure <note type="important">stp_GetUserInfo</note> 
			/// to get information
			/// about the user and his selected products. And then confirm if all selected products are
			/// valid for him.
			/// </remarks>
			/// <param name="id">The Id of the user, whose selected products must be confirmed</param>
			/// <exception cref="Exception">
			/// This is thrown if the UserID node in retXml parameter is null.
			/// </exception>
			/// <returns>A HttpResponseMessage with a HttpStatusCode and bundle confirmation message</returns>
			public HttpResponseMessage GetBundleConfirm(int id)
			{
				try
				{
					string message = "Bundle CONFIRMED";
					HttpStatusCode respCode = HttpStatusCode.OK;

					XmlDocument retXml = new XmlDocument();

					DBLib db = new DBLib();
					db.AddProcedure("stp_GetUserInfo");
					db.AddParameter("@UserID", SqlDbType.Int, id);
					retXml = db.GetResultAsXmlDocument();

					if (retXml.SelectSingleNode("Root/UserID") == null)
						throw new Exception("User not found");

					//Fill user data
					int age = int.Parse(retXml.SelectSingleNode("Root/Age").InnerText);
					int student = int.Parse(retXml.SelectSingleNode("Root/Student").InnerText);
					int income = int.Parse(retXml.SelectSingleNode("Root/Income").InnerText);

					//All products
					Product[] products = new Product[] 
					{ 
						new Product { Id = 1, Name = "Current Account", Type = ProdType.Account, AgeFrom = 17, AgeTo = 999, Student = -1, Income = 1}, 
						new Product { Id = 2, Name = "Current Account Plus", Type = ProdType.Account, AgeFrom = 17, AgeTo = 999, Student = -1, Income = 40001 }, 
						new Product { Id = 3, Name = "Junior Saver Account", Type = ProdType.Account, AgeFrom = 0, AgeTo = 17, Student = -1, Income = 0 },
						new Product { Id = 4, Name = "Student Account", Type = ProdType.Account, AgeFrom = 17, AgeTo = 999, Student = 1, Income = 0 },
						new Product { Id = 5, Name = "Debit Card", Type = ProdType.Card, AgeFrom = 0, AgeTo = 999, Student = -1, Income = 0 }, 
						new Product { Id = 6, Name = "Credit Card", Type = ProdType.Card, AgeFrom = 18, AgeTo = 999, Student = -1, Income = 12001 }, 
						new Product { Id = 7, Name = "Gold Credit Card", Type = ProdType.Card, AgeFrom = 18, AgeTo = 999, Student = -1, Income = 40001 } 
					};

					//Filter only user products
					var userProducts = products.Where(p => XDocument.Load(new XmlNodeReader(retXml)).Descendants("Id").Select(element => element.Value).ToArray().Contains(p.Id.ToString()));

					//Check if at least one product selected
					if (userProducts.Count() == 0)
					{
						message = "Zero products selected";
						respCode = HttpStatusCode.BadRequest;
					}

					//Check if there are more than one acount product
					if (userProducts.Count(p => p.Type == ProdType.Account) > 1)
					{
						message = "You can have only one Account type product";
						respCode = HttpStatusCode.BadRequest;
					}
					else
					{
						//Check each product validity
						foreach (var product in userProducts)
						{
							//Debit card
							if (product.Id == 5)
							{
								//No Current Account, Current Account Plus or Student Account selected
								if (userProducts.Count(p => new string[] { "1", "2", "3" }.Contains(p.Id.ToString())) == 0)
								{
									message = "No Current Account, Current Account Plus or Student Account selected";
									respCode = HttpStatusCode.BadRequest;
									break;
								}
							}
							//Not debit card
							else
							{
								if (!(income >= product.Income && age >= product.AgeFrom && age <= product.AgeTo && (product.Student == -1 || student == product.Student)))
								{
									message = "You are not valid for product - " + product.Name;
									respCode = HttpStatusCode.BadRequest;
									break;
								}
							}
						}
					}
			
					var response = Request.CreateResponse<string>(respCode, message);
					
					return response;
				}
				catch (Exception e)
				{
					var response = Request.CreateResponse<string>(HttpStatusCode.NotFound, e.Message);
					
					return response;
				}
			}

    }
}
