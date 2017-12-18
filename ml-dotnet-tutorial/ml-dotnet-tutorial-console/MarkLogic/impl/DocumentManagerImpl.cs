using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using MarkLogic.REST;

namespace MarkLogic.impl
{
    public class DocumentManagerImpl : DocumentManager
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

        public GenericDocument Read(string uri)
        {
            string result = string.Empty;

            string url = SetURL(uri);
            Uri requestUri = new Uri(url);

            HttpResponseMessage response = m_httpClient.GetAsync(requestUri).Result;  // Blocking call!  
            if (response.IsSuccessStatusCode)
            {
                result = response.Content.ReadAsStringAsync().Result;   // Blocking call!
                var mimetype = response.Content.Headers.ContentType.MediaType;
                GenericDocument content = new GenericDocument();
                content.SetMimetype(mimetype);
                content.SetContent(result);

                return content;
            }
            else
            {
                result = string.Format("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                Console.WriteLine(result);
                throw new DocumentIOException(result);
            }
        }

        public string Write(string uri, GenericDocument content)
        {
            HttpResponseMessage response;

            string url = SetURL(uri);
            Uri requestUri = new Uri(url);

            var mediaType = content.GetMimetype();
            HttpContent httpBody = new StringContent(content.GetContent());
            httpBody.Headers.ContentType = new MediaTypeHeaderValue(content.GetMimetype());
            response = m_httpClient.PutAsync(requestUri, httpBody).Result;

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

        public async Task<GenericDocument> ReadAsync(string uri)
        {
            string url = SetURL(uri);
            Uri requestUri = new Uri(url);
            using (var r = await m_httpClient.GetAsync(requestUri))
            {
                string result = await r.Content.ReadAsStringAsync();
                var mimetype = r.Content.Headers.ContentType.MediaType;

                GenericDocument content = new GenericDocument();
                content.SetMimetype(mimetype);
                content.SetContent(result);

                return content;
            }
        }

        public async Task<string> WriteAsync(string uri, GenericDocument content)
        {
            string url = SetURL(uri);
            Uri requestUri = new Uri(url);

            HttpContent httpBody = new StringContent(content.GetContent());
            httpBody.Headers.ContentType = new MediaTypeHeaderValue(content.GetMimetype());

            using (var r = await m_httpClient.PutAsync(requestUri, httpBody))
            {
                string result = await r.Content.ReadAsStringAsync();
                return result;
            }
        }

        public string SetURL(string uri)
        {
            string url = string.Format("http://{0}:{1}{2}/?uri={3}&collection=dotnet", m_host, m_port, m_cliientRestDocService, uri);
            return url;
        }
    }
}
