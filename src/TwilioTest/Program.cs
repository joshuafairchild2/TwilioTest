using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TwilioTest
{
    public class Message
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
        public string Status { get; set; }
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            //RestClient client = new RestClient("https://api.twilio.com/2010-04-01");

            //RestRequest request = new RestRequest("Accounts/AC080c816880862e69f9f835767148ff3c/Messages", Method.POST);

            //request.AddParameter("To", "+13607212885");
            //request.AddParameter("From", "+19718035253");
            //request.AddParameter("Body", "Hello worms");

            //client.Authenticator = new HttpBasicAuthenticator("AC080c816880862e69f9f835767148ff3c", "1ed03f4c5d05bdbf9c3c06bd00cbc2a0");

            //client.ExecuteAsync(request, response =>
            //{
            //    Console.WriteLine(response);
            //});

            //Console.ReadLine();

            RestClient client = new RestClient("https://api.twilio.com/2010-04-01");

            RestRequest request = new RestRequest("Accounts/AC080c816880862e69f9f835767148ff3c/Messages.json", Method.GET);

            client.Authenticator = new HttpBasicAuthenticator("AC080c816880862e69f9f835767148ff3c", "1ed03f4c5d05bdbf9c3c06bd00cbc2a0");

            RestResponse response = new RestResponse();

            Task.Run(async () =>
            {
                response = await GetResponseContentAsync(client, request) as RestResponse;
            }).Wait();

            JObject jsonReponse = JsonConvert.DeserializeObject<JObject>(response.Content);

            List<Message> messageList = JsonConvert.DeserializeObject<List<Message>>(jsonReponse["messages"].ToString());

            foreach (Message message in messageList)
            {
                Console.WriteLine("To: {0}", message.To);
                Console.WriteLine("From: {0}", message.From);
                Console.WriteLine("Body: {0}", message.Body);
                Console.WriteLine("Status: {0}", message.Status);
                Console.WriteLine("");
            }

            Console.ReadLine();
        }

        public static Task<IRestResponse> GetResponseContentAsync(RestClient _client, RestRequest _request)
        {
            TaskCompletionSource<IRestResponse> tcs = new TaskCompletionSource<IRestResponse>();
            _client.ExecuteAsync(_request, response =>
            {
                tcs.SetResult(response);
            });
            return tcs.Task;
        } 
    }
}
