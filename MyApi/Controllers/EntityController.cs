using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
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


        List<Contact> contacts = new List<Contact>();

        
        


        public EntityController()
        {
           if (ConnectToCRm())
            {
               // GetAllContacts();
                GetAllCases();
               

            }
            else
            {
                GetError();
            }
           

        }

        private DataCollection<Entity> GetAllCases()
        {
             QueryExpression query = new QueryExpression();

            //Query on reated entity records
            query.EntityName = "incident";

            //Retrieve the all attributes of the related record
            query.ColumnSet = new ColumnSet(true);

            //create the relationship object
            Relationship relationship = new Relationship();

            //add the condition where you can retrieve only the account related active contacts
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, "Active"));

            // name of relationship between account & contact
            relationship.SchemaName = "incident_customer_accounts";

            //create relationshipQueryCollection Object
            RelationshipQueryCollection relatedEntity = new RelationshipQueryCollection();

            //Add the your relation and query to the RelationshipQueryCollection
            relatedEntity.Add(relationship, query);

            //create the retrieve request object
            RetrieveRequest request = new RetrieveRequest();

            //add the relatedentities query
            request.RelatedEntitiesQuery = relatedEntity;

            //set column to  and the condition for the account 
            request.ColumnSet = new ColumnSet("accountid");
            var id = Guid.Parse("dbdd0b93-4a1b-4848-b83a-39352f6b2e7a");
            request.Target = new EntityReference { Id = id, LogicalName = "account" };

            RetrieveResponse response = (RetrieveResponse)service.Execute(request);
           return  ((DataCollection<Relationship, EntityCollection>)(((RelatedEntityCollection)(response.Entity.RelatedEntities))))[new Relationship("incident_customer_accounts")].Entities;
        }

        private void GetError()
        {
           
        }

        public Boolean ConnectToCRm()
        {
            try
            {

                var connectionString = @" AuthType = Office365;              
                Url = https://orgcde5f393.crm4.dynamics.com;
                    Username=Saif.Hazemi@isamm.u-manouba.tn;
                    Password=g'lmaram9782536A";
                CrmServiceClient conn = new CrmServiceClient(connectionString);
                 service = (IOrganizationService)conn.
                  OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;
                //var query = new QueryExpression("contact") { ColumnSet = new ColumnSet("lastname", "emailaddress1", "telephone1", "msdyn_orgchangestatus") };

                //EntityCollection eccontact = service.RetrieveMultiple(query);


                //for (int i = 0; i < eccontact.Entities.Count; i++)
                //{
                //    Contact contact = new Contact();
                //    contact.full_name = (string)eccontact.Entities[i].Attributes["lastname"];
                //    contact.Email = (string)eccontact.Entities[i].Attributes["emailaddress1"];
                //    contact.Business_phone = (string)eccontact.Entities[i].Attributes["telephone1"];
                //    contact.password=(string)eccontact.Entities[i].Attributes["msdyn_orgchangestatus"];
                    
                        
                //    contacts.Add(contact);

                    

                //}



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


        public DataCollection<Entity> Get() {
            QueryExpression query = new QueryExpression();

            //Query on reated entity records
            query.EntityName = "incident";

            //Retrieve the all attributes of the related record
            query.ColumnSet = new ColumnSet("title","incidentid");

            //create the relationship object
            Relationship relationship = new Relationship();

            //add the condition where you can retrieve only the account related active contacts
            query.Criteria = new FilterExpression();
            query.Criteria.AddCondition(new ConditionExpression("statecode", ConditionOperator.Equal, "Active"));

            // name of relationship between account & contact
            relationship.SchemaName = "incident_customer_accounts";

            //create relationshipQueryCollection Object
            RelationshipQueryCollection relatedEntity = new RelationshipQueryCollection();

            //Add the your relation and query to the RelationshipQueryCollection
            relatedEntity.Add(relationship, query);

            //create the retrieve request object
            RetrieveRequest request = new RetrieveRequest();

            //add the relatedentities query
            request.RelatedEntitiesQuery = relatedEntity;

            //set column to  and the condition for the account 
            request.ColumnSet = new ColumnSet("accountid");
            var id = Guid.Parse("dbdd0b93-4a1b-4848-b83a-39352f6b2e7a");
            request.Target = new EntityReference { Id = id, LogicalName = "account" };

            RetrieveResponse response = (RetrieveResponse)service.Execute(request);
            return ((DataCollection<Relationship, EntityCollection>)(((RelatedEntityCollection)(response.Entity.RelatedEntities))))[new Relationship("incident_customer_accounts")].Entities;



        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public Boolean addCase()
        {
            Entity newCase = new Entity("incident");
            newCase["title"] ="test" ;
           // newCase["customerid"] = Guid.Parse("dbdd0b93-4a1b-4848-b83a-39352f6b2e7a");
           // newCase["subjectid"] = "test subj";
            newCase["ownerid"] = Guid.Parse("288c5327-d484-ec11-8d21-000d3ab5003d");
            
            if (service.Create(newCase) !=  null)
            {
                return true;

            }
            else
            {
                return false;
            }


      
           

        }

        private EntityCollection GetAllContacts()
        {
          
            
            var result = new List<Contact>();
            EntityCollection resp;


            do
            {
                var query = new QueryExpression("contact") { ColumnSet = new ColumnSet("fullname", "emailaddress1", "telephone1", "new_password") };
                resp = service.RetrieveMultiple(query);


                for (int i = 0; i < resp.Entities.Count; i++)
                {
                    Contact contact = new Contact();
                    if (resp.Entities[i].Contains("fullname"))
                    {
                        contact.full_name = (string)resp.Entities[i].Attributes["fullname"];

                    }
                    if (resp.Entities[i].Contains("new_password"))
                    {
                            contact.password = (string)resp.Entities[i].Attributes["new_password"];
                    }
                    if (resp.Entities[i].Contains("emailaddress1"))
                    {
                        contact.Email = (string)resp.Entities[i].Attributes["emailaddress1"];
                    }
                    if (resp.Entities[i].Contains("telephone1"))
                    {
                        contact.Business_phone = (string)resp.Entities[i].Attributes["telephone1"];
                    }

                  
                 
                 


                    contacts.Add(contact);



                }

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