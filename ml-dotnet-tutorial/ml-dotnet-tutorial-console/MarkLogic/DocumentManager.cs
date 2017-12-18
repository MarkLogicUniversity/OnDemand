using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace MarkLogic.REST
{
    [Serializable()]
	public class DocumentIOException : System.Exception
	{
		public DocumentIOException() : base() { }
		public DocumentIOException(string message) : base(message) { }
	}

    public interface DocumentManager
    {
        HttpClient GetRESTClient();
        void SetConnection(HttpClient _client, string _host, string _port);

        GenericDocument Read(string uri);
		string Write(string uri, GenericDocument content);

		Task<GenericDocument> ReadAsync(string uri);
		Task<string> WriteAsync(string uri, GenericDocument content);
	}

	public interface JsonDocumentManager
	{
		HttpClient GetRESTClient();
		void SetConnection(HttpClient _client, string _host, string _port);
		
        JObject Read(string uri);
		string Write(string uri, JObject content);
	}

	public interface XmlDocumentManager
	{
		HttpClient GetRESTClient();
		void SetConnection(HttpClient _client, string _host, string _port);
		
        XmlDocument Read(string uri);
		string Write(string uri, XmlDocument content);
	}

    // GenericDocument
    // Object to describe any type of textual content to 
    //  send to or get from a MarkLogic database. Content'
    //  could be text, XML or JSON data.
	public class GenericDocument
	{
		string m_content = string.Empty;
		string m_mimetype = string.Empty;

		public string GetContent()
		{
			return m_content;
		}

		public void SetContent(string _content)
		{
			m_content = _content;
		}

		public string GetMimetype()
		{
			return m_mimetype;
		}

		public void SetMimetype(string _mimetype)
		{
			m_mimetype = _mimetype;
		}
	}
}
