# ml-dotnet-tutorial
This is an example of creating a .NET API wrapper for MarkLogic's REST Client API. The example covers document creation, update and string search capabilities.

The code is written using Microsoft's Visual Studio Community 2017 edition. It has been tested on the Mac and Windows versions of Visual Studio Community.

For JSON documents, the project uses Newtonsoft's Json.NET open-source .NET assembly. The license for Json.NET is MIT's and you can download the assembly directly within Visual Studio. The assembly has been included in the project's `packages` directory for your convenience.

## Requirements

The following are required to test this example code.

* Visual Studio Community Edition 2017, Mac or Windows
* MarkLogic Version 9
* A test MarkLogic database. The example uses MarkLogic's `Documents` database but this can be configured.
* A REST instance application server for the test database. The example uses MarkLogic's `App Services` application server on port 8000 but this can be configured.
* A MarkLogic administrator account or one with sufficient permissions. See **Settings in app.config** below.

## Setup

The following instructions assume MarkLogic is installed on your local computer, functioning properly and you have a MarkLogic login with the `admin` role.

### Clone the Microsoft Visual Studio project

1. Clone the **ml-dotnet-tutorial** Microsoft Visual Studio project from GitHub. After cloning, the `ml-dotnet-tutorial.xml` workspace file will also be downloaded in the same directory. 
2. Open a browser to <http://localhost:8000>, Query Console.
3. On the right-hand side of Query Console, click the `Workspace` dropdown arrow.
4. From the menu, select `Import Workspace...`.
5. Click the **Choose File** button.
6. Browse to directory with the `ml-dotnet-tutorial.xml` file.
7. Select the `ml-dotnet-tutorial.xml` file then click the **Open** button.
8. Click the **Import** button to import the new Workspace into Query Console.  

### Setting MarkLogic Configurations Settings

For testing, the .NET API example code uses content stored in the MarkLogic `Documents` database. This is controlled by an XML configuration file in the `ml-dotnet-tutorial` project, called `app.config`.

#### Settings in `app.config`  

* host - Defaults to `"localhost"`. The host name or IP address of the computer running MarkLogic.
* port - Defaults to `"8000"`. The port number of a REST instance application server for your testing database. The default value uses MarkLogic's `App Service` application server and `Documents` database.
* username - No default value. MarkLogic username with sufficient permissions and privileges to insert new documents, read them and update them. If you are testing on a local computer, use your `admin` username.
* password - No default value. The password for the username.
* realm - Defaults to `public`. This is the realm value used for user passwords in the MarkLogic Security database.

### Insert Test Documents

There are 2 tabs in the workspace. The first tab inserts documents for testing the .NET API project code into the `Documents` MarkLogic database. It inserts a mix of XML, JSON and Text documents and puts them all into a `dotnet` collection. The second tab delete all the documents in the `dotnet` collection, cleaning up the test data from your Documents database.

## Testing the Example Code

Once you have the project built, run the project in **Debug** mode. A console or terminal window will appear with a simple text menu.