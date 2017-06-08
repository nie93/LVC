using System;
using System.IO;
using System.Text;
using ECAClientFramework;
using ECAClientUtilities.API;
using LVC.Model.LVCData;
using LVC.Adapters;
using LVC.VcControlDevice;

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
                StringBuilder _message = new StringBuilder();
                _message.Append($"\n =================== LVC Analytics ===================");
                _message.Append($"\n                 Run Time:  {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");

                _message.Append($"\n ------------- Control Decision Message ------------- ");
                _message.Append($"\n {vca.LogMessage.Replace("|", "\n\t")}");

                _message.Append($"\n -------------- Contrlled Transformers -------------- ");
                foreach (VcTransformer Transformer in vca.InputFrame.ControlTransformers)
                {
                    _message.Append($"\n  Transformer    DeviceId:  {Transformer.DeviceId}");
                    _message.Append($"\n                 LtcCtlId:  {Transformer.LtcCtlId}");
                    _message.Append($"\n                     TapV:  {Transformer.TapV}");
                    _message.Append($"\n                   VoltsV:  {Transformer.VoltsV}");
                }

                _message.Append($"\n ------------ Controlled Capacitor Banks ------------ ");
                foreach (VcCapacitorBank CapBank in vca.InputFrame.ControlCapacitorBanks)
                {
                    _message.Append($"\n  CapacitorBank  CapBkrId:  {CapBank.CapBkrId}");
                    _message.Append($"\n                  CapBkrV:  {CapBank.CapBkrV}");
                    _message.Append($"\n                   LockvV:  {CapBank.LockvV}");
                    _message.Append($"\n                     Clov:  {CapBank.Clov}");
                    _message.Append($"\n                     Chiv:  {CapBank.Chiv}");                    
                }               

                _message.Append($"\n =====================================================");

                MainWindow.WriteMessage(_message.ToString());
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
