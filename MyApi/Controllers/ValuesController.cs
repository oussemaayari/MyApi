using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using MyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MyApi.Controllers
{
    public class ValuesController : ApiController
    {
        IOrganizationService service;


       
        List<Contact> contacts = new List<Contact>();
        public ValuesController()
        {
            if (ConnectToCRm())
            {


            }
            else
            {
                GetError();
            }
        }

        private void GetError()
        {
            throw new NotImplementedException();
        }

        private bool ConnectToCRm()
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
                var query = new QueryExpression("contact") { ColumnSet = new ColumnSet("fullname", "emailaddress1", "telephone1") };

                EntityCollection eccontact = service.RetrieveMultiple(query);


                for (int i = 0; i < eccontact.Entities.Count; i++)
                {
                    Contact contact = new Contact();
                    contact.full_name = (string)eccontact.Entities[i].Attributes["fullname"];
                    contact.Email = (string)eccontact.Entities[i].Attributes["emailaddress1"];
                    contact.Business_phone = (string)eccontact.Entities[i].Attributes["telephone1"];
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

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(Guid id)
        {
            service.Delete("conatct", id);
         
        }
    }
}
