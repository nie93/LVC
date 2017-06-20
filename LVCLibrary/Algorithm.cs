using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ECAClientFramework;
using ECAClientUtilities.API;
using ECACommonUtilities;
using ECACommonUtilities.Model;
using LVC.Model.LVCData;
using LVC.Adapters;
using LVC.VcControlDevice;
using GSF.Configuration;
using GSF.TimeSeries;
using GSF.TimeSeries.Transport;

namespace LVC
{
    public static class Algorithm
    {
        static VoltVarControllerAdapter vca;
        static VoltVarController PreviousFrame;
        
        static Algorithm()
        {
            /*
             * Testing Files Configurations
             * test1.xml    -   Verify if the controller can RAISE both transformers' taps when voltages on both buses are lower than the limit (VLLIM = 114.5kV)
             * test2.xml    -   Verify if the controller is still able to operate (VLLIM = 114.5kV), when the other transformer's tap has reached the highest tap position (16)
             * test3.xml    -   Verify if the controller can switch ON the capacitor bank when the voltage in Pamplin substation reach the lower limit (Clov = 113.5kV)
             * test4.xml    -   Verify if the controller can switch OFF the capacitor bank when the voltage in Crewe substation reach the higher limit (Chiv = 119.7kV)
            */
            string configurationPathName = (@"C:\Users\niezj\Documents\dom\LVC\Data\Configurations_test1.xml");
            vca = new VoltVarControllerAdapter();
            vca.ConfigurationPathName = configurationPathName;
            vca.Initialize();
            PreviousFrame = new VoltVarController();
            PreviousFrame = VoltVarController.DeserializeFromXml(configurationPathName);
        }

        public static Hub API { get; set; }

        internal class Output
        {
            public Outputs OutputData = new Outputs();
            public _OutputsMeta OutputMeta = new _OutputsMeta();
            public static Func<Output> CreateNew { get; set; } = () => new Output();
        }

        public static void UpdateSystemSettings()
        {
            SystemSettings.InputMapping = "InputMapping";
            SystemSettings.OutputMapping = "OutputMapping";
            SystemSettings.ConnectionString = @"server=localhost:6190; interface=0.0.0.0";
            SystemSettings.FramesPerSecond = 30;
            SystemSettings.LagTime = 3;
            SystemSettings.LeadTime = 1;
        }

        internal static Output Execute(Inputs inputData, _InputsMeta inputMeta)
        {
            Output output = Output.CreateNew();

            #region [ Environment Settings ]
            string MainFolderPath = (@"C:\Users\niezj\Documents\dom\LVC\");
            string DataFolderPath = Path.Combine(MainFolderPath, @"Data");
            string LogFolderPath = Path.Combine(MainFolderPath, @"Log");

            string[] LogFileNameList = Directory.GetFiles(LogFolderPath, "CtrlDecisionLog_*.xml");
            string LogFileName = $"CtrlDecisionLog_{LogFileNameList.Length + 1:000}_{DateTime.UtcNow:yyyyMMdd_HHmmssfff}.xml";

            string LogFilePath = Path.Combine(LogFolderPath, LogFileName);

            bool EnableVoltageControlsMessageOutput = false;
            #endregion

            #region [ Subscribe MetaData from openECA ]
            StringBuilder _subscriberMessage = new StringBuilder();
        
            DataSubscriber dataSubscriber = new DataSubscriber()
            {
                ConnectionString = SystemSettings.ConnectionString
            };


            // Attach to ConnectionEstablished and MetadataReceived events
            // so we can do our work once we are connected to the ECA server
            dataSubscriber.ConnectionEstablished += (sender, arg) =>
            {
                // Create meta signal that represents our output measurement
                MetaSignal metaSignal = new MetaSignal()
                {
                    AnalyticProjectName = "TestProject",
                    AnalyticInstanceName = "TestInstance",
                    SignalType = "VPHM",
                    PointTag = "LVC_TEST:VPHM",
                    Description = "Test for Local Voltage Controller"
                };

                // Send meta signal to server to ask it to create our output measurement
                string message = new ConnectionStringParser<SettingAttribute>().ComposeConnectionString(metaSignal);
                dataSubscriber.SendServerCommand((ServerCommand)ECAServerCommand.MetaSignal, message);

                // Ask for metadata from server so we can find our new output measurement
                dataSubscriber.SendServerCommand(ServerCommand.MetaDataRefresh);
            };

            dataSubscriber.MetaDataReceived += (sender, arg) =>
            {
                DataSet metadata = arg.Argument;

                // Subscribe to all frequency meausurements that were taken on June 3
                dataSubscriber.Subscribe(new UnsynchronizedSubscriptionInfo(false)
                {
                    FilterExpression = "FILTER ActiveMeasurements WHERE (ID='PPA:9' OR ID='PPA:10' OR ID='PPA:11' OR ID='PPA:12') AND SignalType='VPHM' ORDER BY ID",
                    ProcessingInterval = 0
                });
            };
            
            dataSubscriber.NewMeasurements += (sender, arg) =>
            {
                ICollection<IMeasurement> measurements = arg.Argument;
                // Process measurements here
                List<IMeasurement> measurementList = measurements.ToList();     // Convert Measurement Collection to List
                _subscriberMessage.AppendLine($"# of measurement: {measurements.Count}");
                _subscriberMessage.AppendLine($"       Timestamp: {measurementList[0].Timestamp:yyyy-MM-dd HH:mm:ss:fff}");

                foreach (IMeasurement meas in measurementList)
                {
                    _subscriberMessage.AppendLine($"             Key: {meas.Key} \t| Value: {meas.Value:0.0000}");
                }


                MainWindow.WriteMessage(_subscriberMessage.ToString());
            };

            // Initialize and start the subscriber so we can
            // create an output measurement and request metadata
            dataSubscriber.Initialize();
            dataSubscriber.Start();
            #endregion

            // Extract inputData from openECA then Call SubRoutine
            vca.GetData(inputData, inputMeta, PreviousFrame);

            // Logging control decision to *.xml files
            vca.SerializeToXml(LogFilePath);

            // Store Current vca.InputFrame to previous.InputFrame for the next InputFrame 
            PreviousFrame = vca.InputFrame;

            try
            {
                // TODO: Implement your algorithm here...
                // You can also write messages to the main window:

                #region [ Write openECA Client Windows Message]
                if (EnableVoltageControlsMessageOutput)
                {
                    StringBuilder _message = new StringBuilder();
                    _message.AppendLine($" =================== LVC Analytics ===================");
                    _message.AppendLine($"                     Run Time:  {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

                    _message.AppendLine($" ------------- Control Decision Message ------------- ");
                    _message.AppendLine($" {vca.LogMessage.Replace("|", "\n\t")}");

                    _message.AppendLine($" -------------- Contrlled Transformers -------------- ");
                    foreach (VcTransformer Transformer in vca.InputFrame.ControlTransformers)
                    {
                        _message.AppendLine($"  Transformer    DeviceId:  {Transformer.DeviceId}");
                        _message.AppendLine($"                 LtcCtlId:  {Transformer.LtcCtlId}");
                        _message.AppendLine($"                     TapV:  {Transformer.TapV}");
                        _message.AppendLine($"                   VoltsV:  {Transformer.VoltsV}");
                    }

                    _message.AppendLine($" ------------ Controlled Capacitor Banks ------------ ");
                    foreach (VcCapacitorBank CapBank in vca.InputFrame.ControlCapacitorBanks)
                    {
                        _message.AppendLine($"  CapacitorBank  CapBkrId:  {CapBank.CapBkrId}");
                        _message.AppendLine($"                  CapBkrV:  {CapBank.CapBkrV}");
                        _message.AppendLine($"                   LockvV:  {CapBank.LockvV}");
                        _message.AppendLine($"                     Clov:  {CapBank.Clov}");
                        _message.AppendLine($"                     Chiv:  {CapBank.Chiv}");
                    }

                    _message.AppendLine($" =====================================================");

                    MainWindow.WriteMessage(_message.ToString());
                }
                System.Threading.Thread.Sleep(5000);
                #endregion

            }
            catch (Exception ex)
            {
                // Display exceptions to the main window
                MainWindow.WriteError(new InvalidOperationException($"Algorithm exception: {ex.Message}", ex));
            }

            return output;
        }
    }
}
