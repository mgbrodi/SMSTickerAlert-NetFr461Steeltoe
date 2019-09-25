# SMSTickerAlert

SMS Ticker Alert is a .NET Framework4.6.1 application that leverages Steeltoe. The original project is a .NETFramework 4.5.2 taht lives here https://github.com/mgbrodi/SMSTickerAlert-NetFramework452.

It contains three projects:

- ModelsCommons -> a dll providing Utilities and Models shared between projects. The DB approach is Code First and in the file named Models/TickerDatabaseInit.cs the3re is a preconfigured list of tickers, change it to add or remove tickers
- SMSTickerAlert -> a WebForm application that allows to list all the tickers in the system and to configure alerts. An alerts is defined based on a selected ticker, high and a low threshold and a mobile number. Ony one alert is allowed per mobile number. Any subsequent add with the same number will result in an update of teh record already present.
- TickerUpdater -> a Console Application (acting as a windows service) that on predefined intervals downolads the latest value for the tickers defined in the DB form http://marketwatch.com/. After the update is complete it checks if there are text messages to be sent based on thresholds defined in each alert. Once an alert has been processed (failure or success) it is removed from the DB.

In order to run the applicationyou will need to:

Set up a SQLDB and collect credentials.

Set up an account from https://www.twilio.com/. Once connected to twilio create a project and go to the Dashboard, follow the Settings link to verify aone or more mobile numbers to send Text messages to. While on teh page to verify the mobile number click on the Manage Numbers link on the left side to collect the number that is used to configure the SMSFrom property. Go back to teh Dashboard and collect Account and Token information.

Create a git repository to hold the configuration for TickerUpdater, here is an example:
https://github.com/mgbrodi/Config/blob/master/TickerUpdaterSteeltoe.properties

To configure the system for CloudFoundry:
Connect to CloudFoundry and select the org and space.
Build the SMSTickerAlert solution.

Go to the TickerUpdater Directory.
- cf create-service p-config-server standard myConfigServer -c TickerUpdater/config-server.json
- cf cups TickerSQLConnection -p '"pw","uid","uri"'
- cf cups Twilio -p '"SMSKeyToken","SMSAccount","SMSFrom"'
- cf push -c C:\Users\vcap\app\TickerUpdater.exe --no-start
- cf bind-service TickerUpdaterSteeltoe myConfigServer
- cf bind-service TickerUpdaterSteeltoe TickerSQLConnection
- cf bind-service TickerUpdaterSteeltoe Twilio
- cf start TickerUpdaterSteeltoe
Checks the logs to see that the tickers are worked on.

Publish the SMSTickerAlert Project.
Go to the SMSTickerAlert directory.
- cf push --no-start
- cf bind-service TickerUpdaterSteeltoe TickerSQLConnection
- cf start SMSTickerAlertSteeltoe

Connect to the application page and set up an alarm.

To configure the system to run locally: 
Use the appsettings.local.json changing the credentials and connection information for configserver, Twilio and the DB in the proper variables. The appsettings.local.json is in the rood of the applications and shoudl be copied in TickerUpdater and SMSTickeAlert before building and publishing.


Always start the Console application first to populate/update the DB with the latest values for the tickers, leave it running and start the Webform.
