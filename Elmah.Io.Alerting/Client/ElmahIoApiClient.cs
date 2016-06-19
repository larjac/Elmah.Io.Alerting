using System;
using System.Collections.Generic;
using Elmah.Io.Alerting.Models;
using Elmah.Io.Client;
using Newtonsoft.Json;
using RestSharp;

namespace Elmah.Io.Alerting.Client
{
    class ElmahIoApiClient : IApiClient
    {
        private const string BaseUrlV2 =
            "https://elmah.io/api/v2";
        private const string BaseUrlV3 =
            "https://api.elmah.io/v3";
        private readonly IRestClient _restClient;
        public ElmahIoApiClient()
        {
            _restClient = new RestClient(BaseUrlV2);
        }

        public ElmahIoResponse GetMessages(string logId, FilterQuery query)
        {
            var extraQuery = "";
            if (query != null)
            {
                extraQuery = string.Format("&query={0}&from={1}&to={2}", query.Query, query.From, query.To);
            }
            var request = new RestRequest(string.Format("messages?logid={0}&pagesize=20" + extraQuery, logId), Method.GET);
            
            var result = _restClient.Execute<List<Message>>(request);
            var obj = JsonConvert.DeserializeObject<ElmahIoResponse>(result.Content);
            return obj;
        }
        public ElmahIoResponse GetLogName(string logId)
        {
            throw new NotImplementedException();
            //var request = new RestRequest(string.Format(_uri, logId), Method.GET);
            
            //var result = _restClient.Execute<List<Message>>(request);
            //var obj = JsonConvert.DeserializeObject<ElmahIoResponse>(result.Content);
            //return obj;
        }
    }

    public class ElmahIoResponse
    {
        public List<Message> Messages { get; set; }
        public int Total { get; set; }

    }
}
