# Progressive Transformation with the Envelope Pattern
Here you'll find the supporting code and examples for the Progressive Transformation with the Envelope Pattern On Demand training tutorial.  Some basic familiarity with MarkLogic Server is assumed.

##Getting Started
<ol>
<li>Download MarkLogic:
  <ul>
    <li>http://developer.marklogic.com/
  </ul>
<li>Install, start, and initialize MarkLogic on your local machine:
  <ul>
    <li>http://docs.marklogic.com/guide/installation
  </ul>
</ol>
##Setup the Database for the Examples
<ol>
<li>With MarkLogic Server setup and running, there are no special database requirements. The tutorial uses the Documents database for the code examples. For Semantics, the "Triple Index" must be enabled on the Documents database.
<li>Enabling the Triple Index:
  <ol>
    <li>Open a browser (Firefox or Chrome is suggested) and go to the MarkLogic Administrative Interface, http://localhost:8001
    <li>Log in with your administration user
    <li>Select the "Documents" database in the list of databases
    <li>Scroll down the "Documents" configuration until you see the option, "triple index"
    <li>Set the "triple index" setting to "true", then click the ok button to save the changes
  </ol>
</ol>
##Progressive Transformation with the Envelope Pattern tutorial examples
<ol>
<li>Open Query Console:  http://localhost:8000
<li>Import the "envelope-pattern.xml" Query Console workspace from the https://github.com/MarkLogicUniversity/OnDemand/tree/master/envelope-pattern folder.  
<li>Follow along with the "Progressive Transformation with the Envelope Pattern" On Demand tutorial found here:  http://mlu.marklogic.com/ondemand/
</ol>
