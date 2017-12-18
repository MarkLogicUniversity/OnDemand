using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarkLogic.REST
{
    // QueryManager
    // Object to do searches in MarkLogic.
    // Queries return a SearchResult object for the
    //  search results.
    public interface QueryManager
    {
        HttpClient GetRESTClient();
        void SetConnection(HttpClient _client, string _host, string _port);
        string SearchRaw(string query, long start = 1, long pageLength = 10, string format = "xml");
        SearchResult Search(string query, long start = 1, long pageLength = 10, string format = "xml");
        Task<string> SearchRawAsync(string query, long start = 1, long pageLength = 10, string format = "xml");
    }

	// SearchResult
	// The SearchResult object contains a List of 
    //  MatchDocSummary objects describing each result found and
    //  metadata, such as total results found, starting number and number of
	//  items per paged results.
	public interface SearchResult
	{
		void WithFormat(Format format);

        long GetStart();
        void SetStart(long _start);

        long GetPageLength();
        void SetPageLength(long _pageLength);

        long GetTotalResults();

        // return the search result as a string
        string ToString();

        List<MatchDocSummary> GetMatchResults();

        void SetMatchResults(string rawSearchResults);

		string GetResponseStatus();
	}

    // MatchDocSummary
    // Represents the content found in searches.
    //  Each document result from searches has a
    //  MatchDocSummary object.
    public interface MatchDocSummary
    {
        double GetConfidence();
        double GetFitness();
        int GetScore();
        long GetIndex();
        string GetMimetype();
        Format GetFormat();
        string GetPath();
        string GetUri();
        string GetFirstSnippetText();
    }
}
