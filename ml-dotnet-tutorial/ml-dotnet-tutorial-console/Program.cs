// add in MarkLogic C# api
using MarkLogic.REST;
using MimeTypes;
using System;
using System.Configuration;
using System.IO;


// Only used for asynchronous tasks.
// not used in this example.
//using System.Threading.Tasks;

// Used for XML support.
//  not used in this example.
//using System.Xml;

/*
 * Example application that uses MarkLogic's REST api to read, write and search
 * documents in MarkLogic's example database of "Documents". Other databases could
 * be used simply by using a port number of a MarkLogic REST instance along with
 * a user with sufficient permissions to read and update the desired database.
 * 
 * Please see the README.md test file for more information about the classes used
 * in this example and their usage of the MarkLogic REST api.
 * 
 * For more information on MarkLogic's REST api, please see the documentation at:
 *      http://docs.marklogic.com/REST
 */

namespace mldotnettutorialconsole
{
    class MainClass
    {
		public static void Main(string[] args)
        {
            Console.WriteLine("MarkLogic C# API Demo.");
            Console.WriteLine("Begin Read, Write and Search Tests.");

			/* Read the MarkLogic property settings from 
			 *  the app.config file.
			 * host - Name or IP address of MarkLogic Server
			 * port - port number of the REST Instance Application Server
			 * username - user with at least rest-writer role
			 * password - password for the user
			 * realm - by default, MarkLogic uses the string "public"
			 */
            var host = ConfigurationManager.AppSettings["host"];
			var port = ConfigurationManager.AppSettings["port"];
			var username = ConfigurationManager.AppSettings["username"];
			var password = ConfigurationManager.AppSettings["password"];
			var realm = ConfigurationManager.AppSettings["realm"];

			// Create a DatabaseClient object. These objects represent
			//  long-lived connections to MarkLogic databases.
			DatabaseClient dbClient = DatabaseClientFactory.NewClient(host, port, username, password, realm, AuthType.Digest);

            var cmd = string.Empty;
            do
            {
                PrintMenu();
                cmd = Console.ReadLine();
                cmd = cmd.ToLower();

                switch (cmd)
                {
                    case "1":
                        WriteToDatabase(dbClient);
                        break;
                    case "2":
                        ReadFromDatabase(dbClient);
                        break;
                    case "3":
                        SearchDatabase(dbClient);
                        break;
                    case "4":
                        PrintHelp();
                        break;
                    default:
                        break;
                }

            } while (cmd != "exit");

			// Tell the DatabaseClient we are done with it
			//  and release any connections and resources
			dbClient.Release();
		}

        /*
         * Accept a Document ID (URI) from the console. Read the desired
         * document from the Documents database in MarkLogic. Display the
         * returned document plus write the document to the current working
         * directory.
         */
        static void ReadFromDatabase(DatabaseClient dbClient)
        {
            // Get the desired document's URI from the console.
            Console.Write("Document URI: ");
			var uri = Console.ReadLine();

            // The document content will be saved to the filename specified.
            //  If not filename is specified, the filename part of the URI 
            //  is used as the filename.

            string myDocumentPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string defaultFname = string.Empty;
            if (uri.StartsWith("/") || uri.StartsWith("\\"))
            {
                defaultFname = myDocumentPath + uri;
            } else {
                defaultFname = myDocumentPath + "/" + uri;
            }
            defaultFname = defaultFname.Replace("\\", "/");
            Console.Write("Save as (ENTER for "+ defaultFname + "): ");
            string filename = Console.ReadLine();
            if (filename.Length == 0)
            {
                filename = defaultFname;
            }

            // Create a DocumentManager to act as our interface for
            //  reading and writing to/from the database.
            DocumentManager mgr = dbClient.NewDocumentManager();

            // Use the DocumentManager object to read a document 
            // with the uri of "/doc1.xml" from the "Documents" database.
            string mimetype = string.Empty;
            string content = string.Empty;

            // Use the GenericDocument class to read a non-binary document
            //  from the "Documents" database. This example code reads text,
            //  JSON and XML data but binary is not implemented.
            GenericDocument doc = mgr.Read(uri);
            mimetype = doc.GetMimetype();
            content = doc.GetContent();

            // Display the retuned document's mime type and content.
			Console.WriteLine(" ");
			Console.WriteLine("---Document Saved-------------");
            Console.WriteLine("---Document Content from DB---");
            Console.Write("Mime type: ");
            Console.WriteLine(mimetype);
            Console.Write("Content: ");
            Console.WriteLine(content);

			// Write the returned content to the specified file. 
			//File.WriteAllText(filename, content);
			FileInfo file = new FileInfo(filename);
			file.Directory.Create(); // If the directory already exists, this method does nothing.
			File.WriteAllText(file.FullName, content);

        }

        /*
         * Accept a Document path and filename from the console. 
         * Also, accept the desired document's URI from the console. 
         * Write the document to the "Documents" database in MarkLogic.
         */
        static void WriteToDatabase(DatabaseClient dbClient)
		{
            // Get the desired document to write to the MarkLogic
            //  "Documents" database.
            Console.Write("Document path and filename to store: ");
			string filename = Console.ReadLine();
			if (filename.Length == 0)
			{
				Console.WriteLine(("No file specified."));
				return;
			}
			string content = File.ReadAllText(filename);

            // Get a Document URI. If the URI does not already
            //  exist in the "Documents" database, the document
            //  is inserted as a new document. If the URI does exist,
            //  the existing document in the "Documents" database is 
            //  updated.
            Console.Write("Document URI: ");
			var uri = Console.ReadLine();
            if (uri.Length == 0)
            {
                Console.WriteLine(("You must supply a document URI to save a document to the MarkLogic database."));
                return;
            }

			// Create a DocumentManager to act as our interface for
			//  reading and writing to/from the database.
			DocumentManager mgr = dbClient.NewDocumentManager();

            // Create a GenericDocument object to write the
            //  content from the desired file to the MarkLogic
            //  database. The connection from the Database Client
            //  is used to write to the database.
            GenericDocument doc = new GenericDocument();
            // set the mime type.
            string mimetype = MimeTypeMap.GetMimeType(Path.GetExtension(filename));
            Console.WriteLine(Path.GetExtension(filename));
            Console.WriteLine(mimetype);

            doc.SetMimetype(mimetype);
            // set the contents from the file that was read.
			doc.SetContent(content);

            // write the document to the database with the
            //  specified URI.
			var results = mgr.Write(uri, doc);

			Console.WriteLine(" ");
			Console.WriteLine("--------------------------------");
			Console.Write("Write results: ");
			Console.WriteLine(results);
		}

        /*
         * Accept a text search string from the console. 
         * Search the "Documents" database in MarkLogic and
         * return a Search Result object.
         */
        static void SearchDatabase(DatabaseClient dbClient)
		{
			Console.Write("Enter a search term: ");
			var query = Console.ReadLine();
			if (query.Length == 0)
			{
				Console.WriteLine(("You must enter a search term to search the MarkLogic database."));
				return;
			}

			// Create a QueryManager to act as our interface for
			//  searching the database.
			QueryManager mgr = dbClient.NewQueryManager();

			// Search results are retuned in a SearchResult object.
            // 
			SearchResult searchResult = mgr.Search(query);
			
            Console.WriteLine(" ");
            Console.WriteLine("--------------------------------");
			Console.Write("Search total: ");
			Console.WriteLine(searchResult.GetTotalResults()); // total results found
			Console.Write("Search results per page: ");
			Console.WriteLine(searchResult.GetPageLength()); // number of results per page
			Console.Write("Starting at item: ");
			Console.WriteLine(searchResult.GetStart()); // starting result number
			Console.WriteLine("Search Results:");

            // Each search result is returned in a MatchDocSummary object.
            //  GetMatchResults() returns a list of these, if any.
            foreach (MatchDocSummary result in searchResult.GetMatchResults())
            {
				Console.WriteLine(" ");
				Console.WriteLine("---Result " + result.GetIndex() + "---------");

                Console.Write("URI: ");
                Console.WriteLine(result.GetUri());

				Console.Write("Relevance Score: ");
				Console.WriteLine(result.GetScore());

				Console.Write("Mimetype: ");
                Console.WriteLine(result.GetMimetype());

				Console.Write("Text: ");
                Console.WriteLine(result.GetFirstSnippetText());

            }

			// return all search results as a string
			// string results = searchResult.ToString();
        
        }
		
        static void PrintMenu()
        {
			Console.WriteLine(" ");
			Console.WriteLine("|-----------------------------------------------------|");
            Console.WriteLine("| Select from the following options then press ENTER. |");
            Console.WriteLine("|-----------------------------------------------------|");
            Console.WriteLine("  1. Load document.");
            Console.WriteLine("  2. Read document.");
            Console.WriteLine("  3. Search a term.");
			Console.WriteLine("  4. Help.");
			Console.WriteLine("  Type 'exit' to quit.");
			Console.Write("==> ");
		}

		static void PrintHelp()
		{
			Console.WriteLine(" ");
            Console.WriteLine("  1. Load - Type 1 then ENTER to load an XML, JSON or Text file to the MarkLogic Documents database.");
			Console.WriteLine("  2. Read - Type 2 then ENTER to read an XML, JSON or Text file from the MarkLogic Documents database.");
			Console.WriteLine("  3. Enter a search term then press ENTER to search the MarkLogic Documents database. Results are returned as text snippets followed by the entire MarkLogic search response.");
		}
    }
}
