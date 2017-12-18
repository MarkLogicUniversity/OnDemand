using System;
using System.Net.Http;
using System.Threading.Tasks;
using MarkLogic.REST;

namespace MarkLogic.impl
{
	public class QueryManagerImpl : QueryManager
	{
		HttpClient m_httpClient; //HttpClient object for communicating. Created by DatabaseClient and passed into this object.

		string m_host;
		string m_port;
		static readonly string m_clientRestDocService = "/v1/search"; //MarkLogic REST API service for document search.

		public HttpClient GetRESTClient() 
        { 
            return m_httpClient; 
        }

		public void SetConnection(HttpClient _client, string _host, string _port)
		{
			this.m_httpClient = _client;
			this.m_host = _host;
			this.m_port = _port;
		}

		// Search MarkLogic and return the results as a raw string
		public string SearchRaw(string query, long start = 1, long pageLength = 10, string format = "xml")
		{
			string results;

			string url = SetURL(query, start, pageLength, format);
			Uri requestUri = new Uri(url);

			HttpResponseMessage response = GetRESTClient().GetAsync(requestUri).Result;  // Blocking call!  
			if (response.IsSuccessStatusCode)
			{
				var hdrs = response.Headers;
				var total = "0";

				var result = response.Content.ReadAsStringAsync().Result;   // Blocking call!

				results = string.Format("start={0} page={1} total={2} results={3}", start, pageLength, total, result);
				return results;
			}
			else
			{
				results = string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
				Console.WriteLine(results);
				return results;
			}
		}

		// Search MarkLogic and return the results as a SearchResult object
		public SearchResult Search(string query, long start = 1, long pageLength = 10, string format = "xml")
		{
			string result = "";

			SearchResultImpl results = new SearchResultImpl();

			string url = SetURL(query, start, pageLength, format);
			Uri requestUri = new Uri(url);

			HttpResponseMessage response = GetRESTClient().GetAsync(requestUri).Result;  // Blocking call!  
			if (response.IsSuccessStatusCode)
			{
				result = response.Content.ReadAsStringAsync().Result;   // Blocking call!

				results.SetMatchResults(result);
                results.SetResponseStatus(string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase));
				return (SearchResult)results;
			}
			else
			{
				result = string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
				Console.WriteLine(result);

				results.SetMatchResults(System.String.Empty);
                results.SetResponseStatus(result);
				return (SearchResult)results;
			}
		}

		public async Task<string> SearchRawAsync(string query, long start = 1, long pageLength = 10, string format = "xml")
		{
			string url = SetURL(query, start, pageLength, format);
			string result = System.String.Empty;

			Uri requestUri = new Uri(url);
			using (var r = await GetRESTClient().GetAsync(requestUri))
			{
				result = await r.Content.ReadAsStringAsync();
				return result;
			}
		}

		private string SetURL(string query, long start = 1, long pageLength = 10, string format = "xml")
		{
			string url = string.Format("http://{0}:{1}{2}?q={3}&start={4}&pageLength={5}&format={6}",
									   m_host, m_port, m_clientRestDocService,
									   query, start, pageLength, format);
			return url;
		}
	}
}
