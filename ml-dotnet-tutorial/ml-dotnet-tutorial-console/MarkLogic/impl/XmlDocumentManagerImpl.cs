using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;

using MarkLogic.REST;

namespace MarkLogic.impl
{
	public class XmlDocumentManagerImpl : XmlDocumentManager
	{
		HttpClient m_httpClient; //HttpClient object for communicating. Created by DatabaseClient and passed into this object.

		string m_host;
		string m_port;
		static readonly string m_cliientRestDocService = "/v1/documents"; //MarkLogic REST API service for document CRUD.

		public HttpClient GetRESTClient() { return m_httpClient; }

		public void SetConnection(HttpClient _client, string _host, string _port)
		{
			this.m_httpClient = _client;
			this.m_host = _host;
			this.m_port = _port;
		}

        public XmlDocument Read(string uri)
		{
			XmlDocument doc = new XmlDocument();
			string result = String.Empty;

			string url = SetURL(uri);
			Uri requestUri = new Uri(url);

			HttpResponseMessage response = GetRESTClient().GetAsync(requestUri).Result;  // Blocking call!  

			if (response.IsSuccessStatusCode)
			{
				result = response.Content.ReadAsStringAsync().Result;   // Blocking call!
				doc.LoadXml(result);
				return doc;
			}
			else
			{
				result = string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
				Console.WriteLine(result);
				throw new DocumentIOException(result);
			}
		}

		public string Write(string uri, XmlDocument content)
		{
			HttpResponseMessage response;

			string url = SetURL(uri);
			Uri requestUri = new Uri(url);

			var mediaType = "application/xml";
			var doc = content.InnerXml;
			HttpContent httpBody = new StringContent(doc);
			httpBody.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
			response = GetRESTClient().PutAsync(requestUri, httpBody).Result;

			// either this - or check the status to retrieve more information
			response.EnsureSuccessStatusCode();
			// get the rest/content of the response in a synchronous way
			var result = response.Content.ReadAsStringAsync().Result;

			if (response.IsSuccessStatusCode)
			{
				return result;
			}
			else
			{
				result = string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
				Console.WriteLine(result);
				throw new DocumentIOException(result);
			}
		}
		
        public string SetURL(string uri)
		{
			string url = string.Format("http://{0}:{1}{2}/?uri={3}", m_host, m_port, m_cliientRestDocService, uri);
			return url;
		}
    }
}
