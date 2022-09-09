using AddressBookRestSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace AddressBookTestProject
{
    [TestClass]
    public class UnitTest1
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            //Initialize the base URL to execute requests made by the instance
            client = new RestClient("http://localhost:5000");
        }
        private RestResponse GetContactList()
        {
            //Arrange
            //Initialize the request object with proper method and URL
            RestRequest request = new RestRequest("/contacts/list", Method.Get);
            //Act
            // Execute the request
            RestResponse response = client.Execute(request);
            return response;
        }

        /// <summary>
        /// UC22
        /// Reads the entries from json server.
        /// </summary>
        [TestMethod]
        public void ReadEntriesFromJsonServer()
        {
            RestResponse response = GetContactList();
            // Check if the status code of response equals the default code for the method requested
            Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
            // Convert the response object to list of employees
            List<Contact> employeeList = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
            Assert.AreEqual(4, employeeList.Count);
            foreach (Contact contactObj in employeeList)
            {
                Console.WriteLine($"Id: {contactObj.Id}\tFullName: {contactObj.FirstName} {contactObj.LastName}\tPhoneNo: {contactObj.PhoneNumber}\tAddress: {contactObj.Address}\tCity: {contactObj.City}\tState: {contactObj.State}\tZip: {contactObj.Zip}\tEmail: {contactObj.Email}");
            }
        }

        /// <summary>
        ///Adding contacts to the address book JSON server and return the same
        /// </summary>
        [TestMethod]
        public void OnCallingPostAPIForAContactListWithMultipleContacts_ReturnContactObject()
        {
            // Arrange
            List<Contact> contactList = new List<Contact>();
            contactList.Add(new Contact { FirstName = "Ram", LastName = "Singh", PhoneNumber = "8376000000", Address = "Aukrdi", City = "Pune", State = "Maharasta", Zip = "666777", Email = "ram@gmail.com" });
            contactList.Add(new Contact { FirstName = "Karan", LastName = "Saha", PhoneNumber = "8376123456", Address = "kota", City = "Kota", State = "Rajsthan", Zip = "555444", Email = "karan@gmail.com" });
            contactList.Add(new Contact { FirstName = "Mohan", LastName = "Saha", PhoneNumber = "9912345678", Address = "nanital", City = "nanital", State = "Uttrakhand", Zip = "876900", Email = "mohan@gmail.com" });

            //Iterate the loop for each contact
            foreach (var contactobj in contactList)
            {
                //Initialize the request for POST to add new contact
                RestRequest request = new RestRequest("/contacts/list", Method.Post);
                request.RequestFormat = DataFormat.Json;
                //Added parameters to the request object such as the content-type and attaching the jsonObj with the request
                request.AddBody(contactobj);
                //Act
                RestResponse response = client.ExecuteAsync(request).Result;
                //Assert
                Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
                Contact contact = JsonConvert.DeserializeObject<Contact>(response.Content);
                Assert.AreEqual(contactobj.FirstName, contact.FirstName);
                Assert.AreEqual(contactobj.LastName, contact.LastName);
                Assert.AreEqual(contactobj.PhoneNumber, contact.PhoneNumber);
                Assert.AreEqual(contactobj.Address, contact.Address);
                Assert.AreEqual(contactobj.City, contact.City);
                Assert.AreEqual(contactobj.State, contact.State);
                Assert.AreEqual(contactobj.Zip, contact.Zip);
                Assert.AreEqual(contactobj.Email, contact.Email);

                Console.WriteLine(response.Content);
            }

        }
    }
}
