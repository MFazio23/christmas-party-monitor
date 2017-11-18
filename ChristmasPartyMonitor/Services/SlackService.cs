using ChristmasPartyMonitor.Models;
using ChristmasPartyMonitor.Utilities;
using Newtonsoft.Json;
using RestSharp;

namespace ChristmasPartyMonitor.Services
{
    public class SlackService
    {
        private readonly IRestClient _client;

        public SlackService()
        {
            this._client = new RestClient(Config.SlackMessageBaseUrl);
            _client.AddDefaultHeader("Accept", "application/json");
            _client.AddDefaultHeader("Content-Type", "application/json");
        }

        public SlackMessage SendSlackMessage(string message)
        {
            var request = new RestRequest(Config.SlackMessageEndpointUrl)
            {
                Method = Method.POST,
                RequestFormat = DataFormat.Json
            };

            var body = new SlackMessage
            {
                Text = message,
                Username = Config.SlackMessageUsername,
                IconUrl = Config.SlackMessageIconUrl,
                Markdown = true,
                Channel = Config.SlackMessageChannel
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);

            this._client.Post(request);

            return body;
        }
    }
}