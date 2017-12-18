using System;
using System.Collections.Generic;
using System.Xml;

using MarkLogic.REST;

namespace MarkLogic.impl
{
	public class SearchResultImpl : SearchResult
	{
		string m_rawSearchResults = System.String.Empty;
		Format m_format = Format.XML;
		long m_totalResults = 0;
		List<MatchDocSummary> m_docSummaries;
		long m_start = 0;
		long m_pageLength = 0;

        string m_ResponseStatus;

		public void WithFormat(Format format)
		{
			this.m_format = format;
		}

		public long GetStart()
		{
			return this.m_start;
		}

		public void SetStart(long _start)
		{
			this.m_start = _start;
		}

		public long GetPageLength()
		{
			return this.m_pageLength;
		}

		public void SetPageLength(long _length)
		{
			this.m_pageLength = _length;
		}

		public long GetTotalResults()
		{
			return this.m_totalResults;
		}

		public void SetTotalResults(long _total)
		{
			this.m_totalResults = _total;
		}

        public List<MatchDocSummary> GetMatchResults()
		{
			return this.m_docSummaries;
		}

		public void SetMatchResults(string rawSearchResults)
		{
			// Save the raw string returned from the "v1/search" 
            //  REST api call.
			SetRawSearchResults(rawSearchResults);

			// Create an XmlDocument object from the unparsed
			//  search response.
			// Currently this assumes the search response was
			//  returned as an XML string.
			XmlDocument results = new XmlDocument();
			try
			{
				results.LoadXml(rawSearchResults);
			}
			catch (XmlException e)
			{
				Console.WriteLine("SearchResult class parse error. Message={0} XML source={1}", e.Message, rawSearchResults);
				throw;
			}

            // Get the Search results metadata from the root element of
            //  'search:search'. The metadata is in attributes of the root.
			XmlElement root = results.DocumentElement;
			SetTotalResults(long.Parse(root.GetAttribute("total")));
			SetStart(long.Parse(root.GetAttribute("start")));
			SetPageLength(long.Parse(root.GetAttribute("page-length")));

            // Get each Search result and create a new MatchDocSummary object
            //  from each and add each to a List.
            m_docSummaries = new List<MatchDocSummary>();

			var nsmgr = new XmlNamespaceManager(results.NameTable);
			nsmgr.AddNamespace("search", "http://marklogic.com/appservices/search");
			//var nodes = results.SelectNodes("//search:result", nsmgr);
            foreach (XmlNode node in results.SelectNodes("//search:result", nsmgr))
			{
				Console.WriteLine("{0}: {1}", node.Name, node.InnerText);
                var resultMatch = new MatchDocumentSummaryImpl();
                var uri = node.Attributes["uri"].Value;
                resultMatch.SetUri(uri);

				var index = node.Attributes["index"].Value;
                resultMatch.SetIndex(long.Parse(index));

				var path = node.Attributes["path"].Value;
				resultMatch.SetPath(path);

				var score = node.Attributes["score"].Value;
				resultMatch.SetScore(int.Parse(score));

				var fitness = node.Attributes["fitness"].Value;
				resultMatch.SetFitness(double.Parse(fitness));

				var confidence = node.Attributes["confidence"].Value;
				resultMatch.SetConfidence(double.Parse(confidence));

				var mimetype = node.Attributes["mimetype"].Value;
				resultMatch.SetMimetype(mimetype);

				var format = node.Attributes["format"].Value;
				resultMatch.SetFormat(format);

                var snippets = node.InnerXml;
                resultMatch.SetSnippetText(snippets);
                GetMatchResults().Add(resultMatch);
			}
		}

		// return the search result as a string
		override public string ToString()
		{
			return this.m_rawSearchResults;
		}

		public void SetRawSearchResults(string rawSearchResults)
        {
            m_rawSearchResults = rawSearchResults;
        }

        public string GetResponseStatus()
        {
            return m_ResponseStatus;
        }

        public void SetResponseStatus(string _responseStatus)
        {
            m_ResponseStatus = _responseStatus;
        }
	}

    public class MatchDocumentSummaryImpl : MatchDocSummary
	{
        double m_confidence;
        double m_fitness;
        int m_score;
        string m_mimetype;
        Format m_format;
        long m_index;
        string m_path;
        string m_uri;
        string m_snippet; 

        public double GetConfidence()
        {
            return this.m_confidence;
        }

		public void SetConfidence(double _confidence)
		{
			this.m_confidence = _confidence;
		}

		public double GetFitness()
        {
            return this.m_fitness;
        }

		public void SetFitness(double _fitness)
		{
            this.m_fitness = _fitness;
		}

		public int GetScore()
        {
            return this.m_score;
        }

		public void SetScore(int _score)
		{
			this.m_score = _score;
		}

		public long GetIndex()
		{
			return this.m_index;
		}

		public void SetIndex(long _index)
		{
			this.m_index = _index;
		}

		public string GetMimetype()
        {
            return this.m_mimetype;    
        }

		public void SetMimetype(string _mimetype)
		{
			this.m_mimetype = _mimetype;
		}

		public Format GetFormat()
        {
            return this.m_format;
        }

        public void SetFormat(string _format)
        {
            switch (_format)
            {
                case "xml":
                    this.m_format = Format.XML;
                    break;
                case "json":
                    this.m_format = Format.JSON;
                    break;
                case "text":
                    this.m_format = Format.TEXT;
                    break;
                default:
                   this.m_format = Format.XML;
                    break;
            }
		}

		public string GetPath()
        {
            return this.m_path;
        }

		public void SetPath(string _path)
		{
			this.m_path = _path;
		}

		public string GetUri()
        {
            return this.m_uri;
        }

		public void SetUri(string _uri)
		{
			this.m_uri = _uri;
		}

		public string GetFirstSnippetText()
        {
            return this.m_snippet;
        }

		public void SetSnippetText(string _snippet)
		{
			this.m_snippet = _snippet;
		}
	}

	public class QueryDefinitionImpl
	{

	}

	public class StringQueryDefinitionImpl : QueryDefinitionImpl
	{
		string QueryString { get; set; }

		public void SetCriteria(string query)
		{
			this.QueryString = query;
		}
	}
}
