using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using RestSharp;

namespace RestSharpTestCase
{
    public class Contact
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
    }
    [TestClass]
    public class RestSharpTestCase
    {
        RestClient client;

        [TestInitialize]
        public void SetUp()
        {
            client = new RestClient("http://localhost:4000/");

        }

        private IRestResponse getEmployeeList()
        {
            //arrange
            RestRequest request = new RestRequest("/addressBookSyatem", Method.GET);

            //act
            IRestResponse response = client.Execute(request);
            return response;
        }
        [TestMethod]
        public void OnCallingGETApi_ReturnContactList()
        {
            IRestResponse response = getEmployeeList();

            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            List<Contact> dataResponse = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
            Assert.AreEqual(2, dataResponse.Count);

            foreach (Contact con in dataResponse)
            {
                Console.WriteLine("firstName : " + con.firstName + " lastName : " + con.lastName + " address : " + con.address
                    + " city : " + con.city + " state : " + con.state + " zip : " + con.zip + " phoneNumber : " + con.phoneNumber
                    + " email : " + con.email);
            }
        }

        [TestMethod]
        public void GivenContact_OnPost_ShouldReturnAddedContact()
        {
            //arrange 
            RestRequest request = new RestRequest("/addressBookSyatem", Method.POST);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("firstName", "Roshan");
            jObjectbody.Add("lastName", "Dube");
            jObjectbody.Add("address", "Pune");
            jObjectbody.Add("city", "Pune");
            jObjectbody.Add("state", "Maharastra");
            jObjectbody.Add("zip", "456010");
            jObjectbody.Add("phoneNumber", "6856961235");
            jObjectbody.Add("email", "ajay@gmail.com");

            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);

            //act
            IRestResponse response = client.Execute(request);
            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);
            Contact dataResponse = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual("Ajay", dataResponse.firstName);
            Assert.AreEqual("Dube", dataResponse.lastName);
            Assert.AreEqual("Pune", dataResponse.address);
            Assert.AreEqual("Pune", dataResponse.city);
            Assert.AreEqual("Maharastra", dataResponse.state);
            Assert.AreEqual("456010", dataResponse.zip);
            Assert.AreEqual("6856961235", dataResponse.phoneNumber);
            Assert.AreEqual("ajay@gmail.com", dataResponse.email);

        }

        [TestMethod]
        public void GivenContact_OnUpdate_ShouldreturnUpdatedcontact()
        {
            //arrange
            RestRequest request = new RestRequest("/addressBookSyatem/Riya", Method.PUT);
            JObject jObjectbody = new JObject();
            jObjectbody.Add("email", "riya@gmail.com");


            request.AddParameter("application/json", jObjectbody, ParameterType.RequestBody);
            //act
            var response = client.Execute(request);

            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Contact dataResponse = JsonConvert.DeserializeObject<Contact>(response.Content);
            Assert.AreEqual("riya@gmail.com", dataResponse.email);
            Console.WriteLine(response.Content);
        }

        [TestMethod]
        public void GivenContactName_OnDelete_ShouldReturnSuccessStatus()
        {
            //arrange
            RestRequest request = new RestRequest("/addressBookSyatem/Riya", Method.DELETE);

            //act
            IRestResponse response = client.Execute(request);

            //assert
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
            Console.WriteLine(response.Content);
        }
    }
}
