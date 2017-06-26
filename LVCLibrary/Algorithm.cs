using System;
using System.Text;
using ECAClientFramework;
using ECAClientUtilities.API;
using LVC.Model.LVCData;

namespace LVC
{
    public static class Algorithm
    {
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
            SystemSettings.LagTime = 10;
            SystemSettings.LeadTime = 1;
        }

        internal static Output Execute(Inputs inputData, _InputsMeta inputMeta)
        {
            Output output = Output.CreateNew();

            try
            {
                // TODO: Implement your algorithm here...
                // You can also write messages to the main window:

                output.OutputData.IntOutput = (Int16)DateTime.Now.Millisecond;

                #region [ Write openECA Client Windows Message ]
                StringBuilder _message = new StringBuilder();
                _message.Append($"\n ================= LVC_test Analytics ================");
                _message.Append($"\n                 Run Time:  {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                _message.Append($"\n -----------------------------------------------------");
                _message.Append($"\n                  TapVTx4:  {inputData.TapVTx4}");
                _message.Append($"\n                  TapVTx5:  {inputData.TapVTx5}");
                _message.Append($"\n              CapBkrVCap1:  {inputData.CapBkrVCap1}");
                _message.Append($"\n              CapBkrVCap2:  {inputData.CapBkrVCap2}");
                _message.Append($"\n              BusBkrVCap1:  {inputData.BusBkrVCap1}");
                _message.Append($"\n              BusBkrVCap2:  {inputData.BusBkrVCap2}");
                _message.Append($"\n                VoltsVTx4:  {inputData.VoltsVTx4:0.000} Volts");
                _message.Append($"\n                VoltsVTx5:  {inputData.VoltsVTx5:0.000} Volts");
                _message.Append($"\n               LocKvVCap1:  {inputData.LocKvVCap1:0.000} Volts");
                _message.Append($"\n               LocKvVCap2:  {inputData.LocKvVCap2:0.000} Volts");
                _message.Append($"\n                   MwVTx4:  {inputData.MwVTx4:0.000} MW");
                _message.Append($"\n                  MvrVTx4:  {inputData.MvrVTx4:0.000} MVAR");
                _message.Append($"\n                   MwVTx5:  {inputData.MwVTx5:0.000} MW");
                _message.Append($"\n                  MvrVTx5:  {inputData.MvrVTx5:0.000} MVAR");
                _message.Append($"\n                     G1Mw:  {inputData.G1Mw:0.000} MW");
                _message.Append($"\n                    G1Mvr:  {inputData.G1Mvr:0.000} MVAR");
                _message.Append($"\n                     G2Mw:  {inputData.G2Mw:0.000} MW");
                _message.Append($"\n                    G2Mvr:  {inputData.G2Mvr:0.000} MVAR");
                _message.Append($"\n =====================================================");
                _message.Append($"\n            FakeIntOutput:  {output.OutputData.IntOutput:0.000}");
                MainWindow.WriteMessage(_message.ToString());

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
