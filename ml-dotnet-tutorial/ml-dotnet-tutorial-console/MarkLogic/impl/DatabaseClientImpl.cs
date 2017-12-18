using System;
using System.Net;
using System.Net.Http;

using MarkLogic.impl;

namespace MarkLogic.REST
{
    public class DatabaseClientImpl : DatabaseClient
    {
		string m_Host { get; set; }
		string m_Port { get; set; }
		CredentialCache m_credCache = new CredentialCache();
		HttpClientHandler m_clientHandler = new HttpClientHandler();
		HttpClient m_httpClient;

		/* SetConnection() - set the connection information, creating an initialized HttpClient
         * for this object. The DatabaseClient.Release() method should be called when this object
         * is no longer needed to release any resources consumed by the HttpClient object.
         */
		public void SetConnection(string _host, string _port, string _username, string _password, string _domain, AuthType _authType)
		{
			m_Host = _host;
			m_Port = _port;
			string mlHost = string.Format("http://{0}:{1}", _host, _port);

			switch (_authType)
			{
				case AuthType.Basic:
					this.m_credCache.Add(new Uri(mlHost), "Basic", new NetworkCredential(_username, _password));
					this.m_clientHandler.Credentials = this.m_credCache;
					this.m_clientHandler.PreAuthenticate = true;
					this.m_httpClient = new HttpClient(m_clientHandler);
					break;
				case AuthType.Digest:
					this.m_credCache.Add(new Uri(mlHost), "Digest", new NetworkCredential(_username, _password, _domain));
					this.m_clientHandler.Credentials = this.m_credCache;
					this.m_clientHandler.PreAuthenticate = true;
					this.m_httpClient = new HttpClient(m_clientHandler);
					break;
				default:
					this.m_httpClient = new HttpClient();
					break;
			}
		}

		/* 
         * NewDocumentManger() - create a new DocumentManager class to read or write documents from/to MarkLogic.
         */
		public DocumentManager NewDocumentManager()
		{
			DocumentManager docMgr = new DocumentManagerImpl();
			docMgr.SetConnection(m_httpClient, m_Host, m_Port);

			return docMgr;
		}

		/* 
         * NewXmlDocumentManger() - create a new DocumentManager class to read or write documents from/to MarkLogic.
         */
		public XmlDocumentManager NewXmlDocumentManager()
		{
			XmlDocumentManager docMgr = new XmlDocumentManagerImpl();
			docMgr.SetConnection(m_httpClient, m_Host, m_Port);

			return docMgr;
		}

		/* 
         * NewJsonDocumentManger() - create a new DocumentManager class to read or write documents from/to MarkLogic.
         */
		public JsonDocumentManager NewJsonDocumentManager()
		{
			JsonDocumentManager docMgr = new JsonDocumentManagerImpl();
			docMgr.SetConnection(m_httpClient, m_Host, m_Port);

			return docMgr;
		}

		/* 
         * NewQueryManger() - create a new DocumentManager class to read or write documents from/to MarkLogic.
         */
		public QueryManager NewQueryManager()
		{
			QueryManager qMgr = new QueryManagerImpl();
			qMgr.SetConnection(m_httpClient, m_Host, m_Port);

			return qMgr;
		}

		/*
         * Disppose of the HttpClient object used by this DatabaseClient object and release resources
         * the HttpClient object consumes.
         */
		public void Release()
		{
			m_httpClient.Dispose();
			return;
		}
	}
}
