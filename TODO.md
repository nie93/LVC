# TODO List for ShadowSys Solution

* (O_o):  They closed my IT request ticket...
* (=. =): Still waiting for IT guys to fix the filesystem...
* (#_#):  Get up late... Good job, my curtain!
* (-O-)~: Yawning...

Zhijie Nie, 2017-06-26

## Program Development
* (C#) Need to remodify `RunPythonCmd()` under `PythonScript` class, given `OperationMode` is 
obselete now since `SysConfigFrame` class is removed.
* (C#) Re-configure a `PreviousFrame`
* (C#) Construct a `EcaConnector` class
* [**HOLD**] (C#) Use `GrafanaAdapters.cs` to create data connection to DashBoard
* (C#) Refine `SysConfigFrame(object)` class
* [**Maybe**] (C#) Create ***InputAdapter*** & ***OutputAdapter*** class
* [**ALWAYS**] More practices on Git & Github
* [**ALWAYS**] Refine the codes at your best


## Documentation
* Documentation for ShadowSys


## Coding Improvements
* Learn the usage of ***rawkeyValuePair*** and ***Dictionary*** (C#)
* Learn how to debug between C# and Python using Visual Studio
* Learn Python class object
* Learn Python coding style (pylint)


## Need to Knows
* FILTER Expressions

| ColumnName        | DataType  | Examples  |
| ----------------: | :-------- | :-------  |
|Adder	            |double     |           |
|AlternateTag	    |string     |           |
|Company	        |string     |           |
|Description	    |string     | Test Device ABB-521 Frequency |
|Device	            |string     |           |
|DeviceID	        |int        |           |
|EngineeringUnits	|string     |           |
|FramesPerSecond	|int        |           |
|ID	                |string     | PPA:42    |
|Internal	        |int        |           |
|Latitude	        |Decimal    |           |
|Longitude	        |Decimal    |           |
|Multiplier	        |double     |           |
|Phase	            |string     |           |
|PhasorID	        |int        |           |
|PhasorType	        |string     |           |
|PointTag	        |string     | TVA_TESTDEVICE:ABBF |
|Protocol	        |string     |           |
|ProtocolType	    |string     |           |
|SignalID	        |Guid       | 856ba8e9-4c54-11e7-85eb-c85b76348f0a |
|SignalReference	|string     | TESTDEVICE-FQ |
|SignalType	        |string     | FREQ      |
|SourceNodeID	    |Guid       |           |
|Subscribed	        |int        |           |
|UpdatedOn	        |DateTime   |           |


* TestHarness is just a tool for developing an analytic - the actual end product will be an 
**installable Windows service**.


### Reserved URLs for GPA

| Function                          | openPDC        | openECA        | openHistorian  |
| --------------------------------: | :------------- | :------------- | :------------- | 
| dataPublisherPort                 | `http://localhost:6165/` | `http://localhost:6190/` | |
| RemoteStatusServerConnection      | `http://localhost:8500/` | `http://localhost:8525/` | |
| AlarmServiceUrl                   | `http://+:5018/` | `http://+:5021/` | 
| Alarm data web-service            | `http://+:5018/` | `http://+:5023/` | 
| Statistics metadata web-service   | `http://+:6051/` | `http://+:6061/` | 
| Statistics data web-service       | `http://+:6052/` | `http://+:6062/` | 
| Historian metadata web-service    | `http://+:6151/` | `http://+:6161/` | 
| Historian data web-service        | `http://+:6152/` | `http://+:6162/` | 
| (Grafana) Statistics Interface    | `http://+:6352/api/grafana/` | `http://+:6362/api/grafana/` | `http://+:6356/api/grafana/` |
| (Grafana) Archive Interface       | `http://+:6452/api/grafana/` | `http://+:6462/api/grafana/` | 


## Issues
* (PSS/E) Model too large, applied DVP planning model contains 70,000+ buses

* (openECA) Overwrite `Server/*.csv` AddNewLine real-time test (See if openECA) 

Results: Even though `Server/*.csv` file updated (admin access required), the CsvAdapter has to be 
initialized in openECA Manager


## ePHASORsim Contingencies
* Purchased license (1,000 buses)
* localhost license (10,000 buses) expires on 2017-06-30
* RT-LAB counts 3-phase system 3 nodes per bus


## Completed Tasks
* (openECA) Testing Output csvAdapter to archieve measurements (which is initialized 
everytime when run the program)
* (Python) Implement Operation Mode (0: STAT, 1: LVC, 2: SCAL)
* (openECA) Update to version 1.0.5.0
* (C#) Generate the LVC Analytic and create a new repository
* (C#) Test LVC analytic on new version openECA
* (C#) Create Outputs and display them on the MainWindow
* (C#) Refine LVC codes
* (C#) Create new repository for ***LVCinShadowSys***
* (C#) Try to another project to ***LVCinShadowSys.sln***
* [**Failed**] (C#) Connect LVC's inputs with ShadowSys' outputs by mapping files
* [**Failed**] (SIEGate) Create a loop to get openECA re-subscribed (Able to subscribe to direct 
device only, see connection diagram)
* (C#) ECAApiDemo
* (C#) Call LVCTestHarness from LVCinShadowSysTestHarness
* (C#) Ritchie's reply: 

There should an `App.config` file for the Service and ServiceConsole projects, these will need to 
reference a unique port. Look in the `ServiceHost.Designer.cs` file for this line:


```C#
this.m_remotingServer.ConfigurationString = "port=[REMOTE_CONSOLE_PORT]; interface=0.0.0.0";
```

This should match the following line in the `ServiceClient.Designer.cs` in the console project:
```C#
this.m_remotingClient.ConnectionString = "server=localhost:[REMOTE_CONSOLE_PORT]; interface=0.0.0.0";
```

These port numbers need to be unique per service instance

So `LVCService` port number should be different from `LVCinShadowSysService`

* (C#) Feeding data of `PPA:9` to `PPA:12` using `DataSubcriber`

<!--
[![](files/openH2_icon.png)![openHistorian](files/openHistorian2_Logo2016.png)](https://github.com/GridProtectionAlliance/openHistorian "openHistorian")-->
