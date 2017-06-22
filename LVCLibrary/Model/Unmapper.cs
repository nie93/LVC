// COMPILER GENERATED CODE
// THIS WILL BE OVERWRITTEN AT EACH GENERATION
// EDIT AT YOUR OWN RISK

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ECAClientFramework;
using ECAClientUtilities;
using ECACommonUtilities;
using ECACommonUtilities.Model;
using GSF.TimeSeries;

namespace LVC.Model
{
    [CompilerGenerated]
    public class Unmapper : UnmapperBase
    {
        #region [ Constructors ]

        public Unmapper(Framework framework, MappingCompiler mappingCompiler)
            : base(framework, mappingCompiler, SystemSettings.OutputMapping)
        {
            Algorithm.Output.CreateNew = () => new Algorithm.Output()
            {
                OutputData = FillOutputData(),
                OutputMeta = FillOutputMeta()
            };
        }

        #endregion

        #region [ Methods ]

        public LVC.Model.LVCData.Outputs FillOutputData()
        {
            TypeMapping outputMapping = MappingCompiler.GetTypeMapping(OutputMapping);
            Reset();
            return FillLVCDataOutputs(outputMapping);
        }

        public LVC.Model.LVCData._OutputsMeta FillOutputMeta()
        {
            TypeMapping outputMeta = MappingCompiler.GetTypeMapping(OutputMapping);
            Reset();
            return FillLVCData_OutputsMeta(outputMeta);
        }

        public IEnumerable<IMeasurement> Unmap(LVC.Model.LVCData.Outputs outputData, LVC.Model.LVCData._OutputsMeta outputMeta)
        {
            List<IMeasurement> measurements = new List<IMeasurement>();
            TypeMapping outputMapping = MappingCompiler.GetTypeMapping(OutputMapping);

            CollectFromLVCDataOutputs(measurements, outputMapping, outputData, outputMeta);

            return measurements;
        }

        private LVC.Model.LVCData.Outputs FillLVCDataOutputs(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            LVC.Model.LVCData.Outputs obj = new LVC.Model.LVCData.Outputs();

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

            return obj;
        }

        private LVC.Model.LVCData._OutputsMeta FillLVCData_OutputsMeta(TypeMapping typeMapping)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);
            LVC.Model.LVCData._OutputsMeta obj = new LVC.Model.LVCData._OutputsMeta();

            {
                // Initialize meta value structure to "IntOutput" field
                FieldMapping fieldMapping = fieldLookup["IntOutput"];
                obj.IntOutput = CreateMetaValues(fieldMapping);
            }

            return obj;
        }

        private void CollectFromLVCDataOutputs(List<IMeasurement> measurements, TypeMapping typeMapping, LVC.Model.LVCData.Outputs data, LVC.Model.LVCData._OutputsMeta meta)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);

            {
                // Convert value from "IntOutput" field to measurement
                FieldMapping fieldMapping = fieldLookup["IntOutput"];
                IMeasurement measurement = MakeMeasurement(meta.IntOutput, (double)data.IntOutput);
                measurements.Add(measurement);
            }
        }

        #endregion
    }
}
