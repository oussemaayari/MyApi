using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using MyApi.Data;
using MyApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;



namespace MyApi.Controllers
{
    public class EntityController : ApiController
    {
       
        IOrganizationService service;


        List<ProductEntity> products = new List<ProductEntity>();
        List<Contact> contacts = new List<Contact>();
        List<ProductEntity> products2 = new List<ProductEntity>()        
        {
            new ProductEntity()
            {
                Name = "samir",
                price = 20,
                ProductId = 1

            }
        };

        EntityCollection entities = new EntityCollection();
        


        public EntityController()
        {
           if (ConnectToCRm())
            {
                entities = GetAllContacts();
               

            }
            else
            {
                GetError();
            }
           

        }

        private void GetError()
        {
            products.Add(new ProductEntity()
            {
                Name = "error",
                price = 0,
                ProductId = 0

            });
        }

        public Boolean ConnectToCRm()
        {
            try
            {
                var connectionString = @"AuthType = Office365; 
                            Url =https://orgcacc7db5.crm4.dynamics.com;
                            Username=ayari@isamm.u-manouba.tn;
                            Password=XgtS?%RTNj";
                CrmServiceClient conn = new CrmServiceClient(connectionString);
                 service = (IOrganizationService)conn.
                  OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
                var query = new QueryExpression("contact") { ColumnSet = new ColumnSet("lastname", "emailaddress1", "telephone1") };

                EntityCollection eccontact = service.RetrieveMultiple(query);
                

                for (int i = 0; i < eccontact.Entities.Count; i++)
                {
                    Contact contact = new Contact();
                    contact.full_name = (string)eccontact.Entities[i].Attributes["lastname"];
                    contact.Email = (string)eccontact.Entities[i].Attributes["emailaddress1"];
                    contact.Business_phone = (string)eccontact.Entities[i].Attributes["telephone1"];
                    //contact.password = (string)eccontact.Entities[i].Attributes["new_password"];
                    contact.ContactID = (Guid)eccontact.Entities[i].Attributes["contactid"];
                        
                    contacts.Add(contact);

                    

                }



                return (conn != null && conn.IsReady);
               


            }
            catch (Exception)
            {

                throw;
            }
        }
        //public ActionResult error()
        //{
        //    return "Crm didnt COnnect";
        //} 

        //public ienumerable<contact> get()
        //{

        //    return contacts;
        //}


        public Contact Get(Guid id)
        {
            var contact = contacts.FirstOrDefault(c => c.ContactID == id);
            return contact;
        }


        public EntityCollection Get()
        {


            entities = GetAllContacts();


            return entities;

        }
        
        private EntityCollection GetAllContacts()
        {
          
            
            var result = new List<Contact>();
            EntityCollection resp;


            do
            {
                var query = new QueryExpression("contact") { ColumnSet = new ColumnSet("fullname", "emailaddress1", "telephone1","new_password") };



                resp = service.RetrieveMultiple(query);
               
            } while (resp.MoreRecords);
            return resp;
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Contact> delete(Guid id )
        {
            service.Delete("contact", id);
            
            return contacts;

        }
        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public IEnumerable<Contact> Update(Guid id)
        {
            Entity newRecord = new Entity("contact");
         //   newRecord.Id = id;
            newRecord = service.Retrieve("contact", id, new ColumnSet("lastname"));
            newRecord["lastname"] = "lastnametest";
            service.Update(newRecord);

            return contacts;

        }

      





        // GET: Products
        //public ActionResult Index()
        //{
        //    return View();
        //}
    }
}