using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace MarkLogic.REST
{
	public enum AuthType
	{
		// Basic authentication.
		Basic,
		// Digest authentication.
		Digest,
		// Application authentication.
		Application,
		// External authentication.
		External,
	}

    // Expected data type and desired document format for search results
    public enum Format
    {
        XML,
        JSON,
        TEXT,
        BINARY,
    }

	public static class DatabaseClientFactory
	{
		public static DatabaseClient NewClient(string _host, string _port, string _username, string _password, string _realm, AuthType _authType)
		{
			if (_host == null)
			{
				throw new System.ArgumentException("host parameter cannot be null", _host);
			}
			else if (_port == null)
			{
				throw new System.ArgumentException("port parameter cannot be null", _port);
			}
			else if (_username == null)
			{
				throw new System.ArgumentException("username parameter cannot be null", _username);
			}
			else if (_password == null)
			{
				throw new System.ArgumentException("password parameter cannot be null", _password);
			}
			else if (_realm == null)
			{
				throw new System.ArgumentException("realm parameter cannot be null", _realm);
			}

			DatabaseClientImpl DbClient = new DatabaseClientImpl();
			DbClient.SetConnection(_host, _port, _username, _password, _realm, _authType);

			return DbClient;
		}
	}

	public interface DatabaseClient
	{
        /* SetConnection() - set the connection information, creating an initialized HttpClient
         * for this object. The DatabaseClient.Release() method should be called when this object
         * is no longer needed to release any resources consumed by the HttpClient object.
         */
        //void SetConnection(string _host, string _port, string _username, string _password, string _domain, AuthType _authType);

        /* 
         * NewDocumentManger() - create a new DocumentManager class to 
         * read or write documents from/to MarkLogic.
         */
        DocumentManager NewDocumentManager();

        /* 
         * NewXmlDocumentManger() - create a new DocumentManager class to 
         * read or write XML documents from/to MarkLogic.
         */
        XmlDocumentManager NewXmlDocumentManager();

        /* 
         * NewJsonDocumentManger() - create a new DocumentManager class to 
         * read or write JSON documents from/to MarkLogic.
         */
        JsonDocumentManager NewJsonDocumentManager();

        /* 
         * NewQueryManger() - create a new QueryManager class to 
         * search contents in a MarkLogic database.
         */
        QueryManager NewQueryManager();

        /*
         * Disppose of the HttpClient object used by this DatabaseClient object and release resources
         * the HttpClient object consumes.
         */
        void Release();
	}
}