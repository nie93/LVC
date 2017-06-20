//******************************************************************************************************
//  VoltVarControllerAdapter.cs
//
//  Copyright © 2016, Duotong Yang  All Rights Reserved.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  11/09/2016 - Duotong Yang
//       Generated original version of source code.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Collections;
using LVC.VcControlDevice;
using LVC.VcSubRoutines;
using ECAClientFramework;
using LVC.Model.LVCData;


namespace LVC.Adapters
{
    [Serializable()]
    public class VoltVarControllerAdapter
    {
        #region [ Private Members ]
        private VoltVarController m_inputFrame;
        private string m_configurationPathName;
        private string m_logMessage;
        #endregion

        #region[ Public Properties ] 

        [XmlIgnore()]
        public VoltVarController InputFrame
        {
            get
            {
                return m_inputFrame;
            }
            set
            {
                m_inputFrame = value;
            }
        }

        [XmlIgnore()]
        public string ConfigurationPathName
        {
            get
            {
                return m_configurationPathName;
            }
            set
            {
                m_configurationPathName = value;
            }
        }

        [XmlAttribute("LogMessage")]
        public string LogMessage
        {
            get
            {
                return m_logMessage;
            }
            set
            {
                m_logMessage = value;
            }
        }

        #endregion

        #region [ Private Methods  ]

        private void InitializeInputFrame()
        {

            m_inputFrame = new VoltVarController();
            
        }

        #endregion

        #region [ Public Methods ]

        public void Initialize()
        {
            m_inputFrame = VoltVarController.DeserializeFromXml(m_configurationPathName);
            m_logMessage = null;
        }

        public void GetData(Inputs inputsData, _InputsMeta inputsMeta, VoltVarController PreviousFrame)
        {
            #region [ Extract Inputs from openECA ]
            m_inputFrame.ControlTransformers[0].VoltsV = inputsData.VoltsVTx4;
            m_inputFrame.ControlTransformers[1].VoltsV = inputsData.VoltsVTx5;
            m_inputFrame.ControlCapacitorBanks[0].LockvV = inputsData.LocKvVCap1;
            m_inputFrame.ControlCapacitorBanks[1].LockvV = inputsData.LocKvVCap2;
            m_inputFrame.ControlTransformers[0].MwV = inputsData.MwVTx4;
            m_inputFrame.ControlTransformers[0].MvrV = inputsData.MvrVTx4;
            m_inputFrame.ControlTransformers[1].MwV = inputsData.MwVTx5;
            m_inputFrame.ControlTransformers[1].MvrV = inputsData.MvrVTx5;
            m_inputFrame.SubstationInformation.G1Mw = inputsData.G1Mw;
            m_inputFrame.SubstationInformation.G1Mvr = inputsData.G1Mvr;
            m_inputFrame.SubstationInformation.G2Mw = inputsData.G2Mw;
            m_inputFrame.SubstationInformation.G2Mvr = inputsData.G2Mvr;
            #endregion
            
            SubRoutine sub = new SubRoutine();
            ReadCurrentControl currCtrl = new ReadCurrentControl();
            
            VoltVarController Frame = new VoltVarController();

            #region [ Measurements Mapping ]

            m_inputFrame.OnNewMeasurements();

            #endregion

            #region [ Read The Previous Run ]

            m_inputFrame.ReadPreviousRun(PreviousFrame);

            #endregion

            #region[ Verify Program Controls ]

            currCtrl.VerifyProgramControl(m_inputFrame.SubstationAlarmDevice.LtcProgram);

            #endregion

            #region[ Adjust Control Delay Counters ]

            //#-----------------------------------------------------------------------#
            //# adjust the cap bank control delay counter, which is used to ensure:	  #
            //# a. we don't do two cap bank control within 30 minutes of each other.  #
            //# b. we don't do a tap control within a minute of a cap bank control.	  #
            //#-----------------------------------------------------------------------#

            if (m_inputFrame.SubstationInformation.Ncdel < m_inputFrame.SubstationInformation.Zcdel)
            {
                m_inputFrame.SubstationInformation.Ncdel = m_inputFrame.SubstationInformation.Ncdel + 1;
            }


            //#-----------------------------------------------------------------------#
            //# Adjust the tap control delay counter, which is used to ensure we	  #
            //# don't do a cap bank control within a minute of a tap control.	   	  #
            //#-----------------------------------------------------------------------#


            if (m_inputFrame.SubstationInformation.Ntdel < m_inputFrame.SubstationInformation.Zdel)
            {
                m_inputFrame.SubstationInformation.Ntdel = m_inputFrame.SubstationInformation.Ntdel + 1;
            }


            #endregion

            #region [ Read Curren Tx Values and Voltages ]

            m_inputFrame = currCtrl.ReadCurrentTransformerValuesAndVoltages(m_inputFrame);

            #endregion

            #region [ Check if the Previous Control Reults can Meet Our Expectation ]

            m_inputFrame = currCtrl.CheckPreviousControlResults(m_inputFrame);

            #endregion

            #region [ Call Sub Taps ]

            m_inputFrame = sub.Taps(m_inputFrame);

            #endregion

            #region [ CapBank ]

            m_inputFrame = sub.CapBank(m_inputFrame);

            #endregion

            #region [ Save before Exit ]

            m_logMessage = currCtrl.MessageInput;
            m_logMessage += sub.MessageInput;
            m_inputFrame.LtcStatus.Avv = 0;
            m_inputFrame.LtcStatus.Nins = 0;
            m_inputFrame.LtcStatus.MinVar = 99999;
            m_inputFrame.LtcStatus.MaxVar = -99999;

            #endregion

        }
        

        #endregion

        #region [ Xml Serialization/Deserialization methods ]
        public void SerializeToXml(string pathName)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(VoltVarControllerAdapter));
                TextWriter writer = new StreamWriter(pathName);
                serializer.Serialize(writer, this);
                writer.Close();
            }
            catch (Exception exception)
            {
                throw new Exception("Error: XML Serialization failed.");
            }
        }

        public static VoltVarControllerAdapter DeserializeFromXml(string pathName)
        {
            try
            {
                VoltVarControllerAdapter vca = null;
                XmlSerializer deserializer = new XmlSerializer(typeof(VoltVarControllerAdapter));
                StreamReader reader = new StreamReader(pathName);
                vca = (VoltVarControllerAdapter)deserializer.Deserialize(reader);
                reader.Close();
                return vca;
            }
            catch (Exception exception)
            {
                throw new Exception("Error: XML Deserialization failed.");
            }
        }

        #endregion

    }
}
