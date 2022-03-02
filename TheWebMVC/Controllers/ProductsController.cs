using APIMessages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TheWebMVC.Models;

namespace TheWebMVC.Controllers
{
    public class ProductsController : Controller
    {
        [HttpPost]
        public ActionResult Index(Contact contact)
        {
            var records = GetEntitiesFromApi();
            foreach(var elemet in records)
            {
                if (elemet.Email == contact.Email && elemet.password == contact.password)
                {
                    return RedirectToAction("Dashbord");
                }
                
              
                
            }
            return View();


        }
        public ActionResult Index1()
        {
            
            return View();
        }
        public ActionResult affiche()
        {
            var products = GetEntitiesFromApi();

            
            return View(products);
        }

        public ActionResult Dashbord()
        {

            return View();
        }

        private List<Contact> GetEntitiesFromApi()
        {
            try
            {
                var resultList = new List<Contact>();
                var Client = new HttpClient();

                var getDataTAsk = Client.GetAsync("https://localhost:44306/api/Entity/get")
                    .ContinueWith(Response =>
                    {
                        var result = Response.Result;
                        if(result.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var readResult = result.Content.ReadAsAsync<List<Contact>>();
                            readResult.Wait();
                            resultList = readResult.Result;
                        }
                    });
                getDataTAsk.Wait();
                return resultList;  
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
