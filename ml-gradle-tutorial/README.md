# README for the ml-gradle tutorial
This README file contains tested environment information and troubleshooting tips for MarkLogic University's On Demand video, "**Introduction to ml-gradle**".

## Getting Started
To do the hands-on portion of the **ml-gradle** video, you'll need the following.

1. Download and install MarkLogic from <http://developer.marklogic.com/products> .
2. Download the zip archived course material.
3. Extract the zip archive. 
4. Follow along with the ml-gradle tutorial video.
5. You will also need the following.
	1. Gradle
	2. Java
	3. Docker
	4. ml-gradle  
	The requirements for the above are listed in the **ml-gradle** tutorial.

## Tested Environments
The **ml-gradle-tutorial** project files have been tested on Macintosh and Windows computers. The following list the tested environments for each platform. Any known incompatibilities are also noted.

### Windows
* OS Version: Windows 10 Professional (64-bit)
* Gradle version: 3.5.1
* Java version: 1.8.0_141
* Docker version: 17.06.0-ce (Community Edition)

> **Notes**  
> 
>	* The Docker container Server architecture must be set to `linux/amd64` rather than `windows/amd64`. This is usually the default setting for Docker on Windows. To toggle between Linux and Windows containers, right-click on the Docker tray icon.

### Macintosh
* OS Version: macOS Sierra (10.12.6)
* Gradle version: 3.5.1
* Java version: 1.8.0_101
* Docker version 17.03.0-ce (Community Edition)

> **Notes**  
> 
>	* Docker version 17.06.0-ce (Community Edition) was also tested but caused failures when the Docker Compose tool ran. The previous Docker for Mac version 17.03.0-ce had no such errors. At this time, we cannot support Docker for Mac 17.06.0-ce.

## Troubleshooting
If the Docker Compose tool will build the necessary MarkLogic containers, likely the rest of the hands-on tutorial will also be successful.

###Test the Docker Compose tool
1. Open a command prompt in Windows or Terminal in Macintosh.
2. Change to the directory where you downloaded the `mlu-gradle-tutorial` main project directory.
3. Change to the `docker` sub-directory.
4. Type `docker-compose up -d` then press the ENTER key.
5. Three Docker containers should successfully build with Docker output similar to the following:

```
	Creating mls1.local  
	Creating mls2.local  
	Creating mls3.local  
```
If there are any errors creating these 3 containers, check to see if you are using a supported Docker version. The supported Docker versions are listed in the **Tested Environments** section above.

###Cleanup After an Error
If an error occurs during a task, you might want to just the 3 Docker containers then try again.

To remove the cluster that was created, even if not all 3 Docker containers were created:  

1. In a command line or terminal prompt, make sure you are in the main project directory, `ml-gradle-tutorial` . 
2. Type `gradle docker:tearDwonCluster` then press the ENTER key.

Any created Docker artifact (containers, sub-networks, etc.) will be removed.

