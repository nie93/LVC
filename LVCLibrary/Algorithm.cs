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
            string configurationPathName = (@"C:\Users\niezj\Documents\dom\LVC\Data\Configurations.xml");
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
            SystemSettings.InputMapping = "LVCData_InputMapping";
            SystemSettings.OutputMapping = "LVCData_OutputMapping";
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

            bool EnableVoltageControlsMessageOutput = true;
            #endregion

            // Extract inputData from openECA then Call SubRoutine
            vca.GetData(inputData, inputMeta, PreviousFrame);
            MainWindow.WriteMessage("%%\t" + vca.LogMessage.Replace("|", "\n = "));

            // Logging control decision to *.xml files
            vca.SerializeToXml(LogFilePath);
            // Store Current vca.InputFrame to previous.InputFrame for the next InputFrame 
            PreviousFrame = vca.InputFrame;
            PreviousFrame.SerializeToXml(@"C:\Users\niezj\Documents\dom\LVC\test.xml");
            System.Threading.Thread.Sleep(1000);

            try
            {
                // TODO: Implement your algorithm here...
                // You can also write messages to the main window:

                output.OutputData.IntOutput = (Int16)DateTime.Now.Millisecond;

                #region [ Write openECA Client Windows Message ]

                if (EnableVoltageControlsMessageOutput)
                {
                    StringBuilder _message = new StringBuilder();
                    _message.Append($"\n =================== LVC Analytics ===================");
                    _message.Append($"\n                 Run Time:  {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                    _message.Append($"\n                  TapVTx4:  {vca.InputFrame.ControlTransformers[0].TapV}");
                    _message.Append($"\n                  TapVTx5:  {vca.InputFrame.ControlTransformers[1].TapV}");
                    _message.Append($"\n              CapBkrVCap1:  {vca.InputFrame.ControlCapacitorBanks[0].CapBkrV}");
                    _message.Append($"\n              CapBkrVCap2:  {vca.InputFrame.ControlCapacitorBanks[1].CapBkrV}");
                    _message.Append($"\n              BusBkrVCap1:  {vca.InputFrame.ControlCapacitorBanks[0].BusBkrV}");
                    _message.Append($"\n              BusBkrVCap2:  {vca.InputFrame.ControlCapacitorBanks[1].BusBkrV}");
                    _message.Append($"\n -----------------------------------------------------");
                    _message.Append($"\n                VoltsVTx4:  {vca.InputFrame.ControlTransformers[0].VoltsV:0.000} Volts");
                    _message.Append($"\n                VoltsVTx5:  {vca.InputFrame.ControlTransformers[1].VoltsV:0.000} Volts");
                    _message.Append($"\n               LocKvVCap1:  {vca.InputFrame.ControlCapacitorBanks[0].LockvV:0.000} Volts");
                    _message.Append($"\n               LocKvVCap2:  {vca.InputFrame.ControlCapacitorBanks[1].LockvV:0.000} Volts");
                    _message.Append($"\n                   MwVTx4:  {vca.InputFrame.ControlTransformers[0].MwV:0.000} MW");
                    _message.Append($"\n                  MvrVTx4:  {vca.InputFrame.ControlTransformers[0].MvrV:0.000} MVAR");
                    _message.Append($"\n                   MwVTx5:  {vca.InputFrame.ControlTransformers[1].MwV:0.000} MW");
                    _message.Append($"\n                  MvrVTx5:  {vca.InputFrame.ControlTransformers[1].MvrV:0.000} MVAR");
                    _message.Append($"\n                     G1Mw:  {vca.InputFrame.SubstationInformation.G1Mw:0.000} MW");
                    _message.Append($"\n                    G1Mvr:  {vca.InputFrame.SubstationInformation.G1Mvr:0.000} MVAR");
                    _message.Append($"\n                     G2Mw:  {vca.InputFrame.SubstationInformation.G2Mw:0.000} MW");
                    _message.Append($"\n                    G2Mvr:  {vca.InputFrame.SubstationInformation.G2Mvr:0.000} MVAR");
                    _message.Append($"\n -----------------------------------------------------");
                    _message.Append($"\n            FakeIntOutput:  {output.OutputData.IntOutput}");
                    _message.Append($"\n =====================================================");
                    MainWindow.WriteMessage(_message.ToString());
                }

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
