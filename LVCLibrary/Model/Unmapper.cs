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

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

            {
                // We don't need to do anything, but we burn a key index to keep our
                // array index in sync with where we are in the data structure
                BurnKeyIndex();
            }

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
                // Initialize meta value structure to "OperationMode" field
                FieldMapping fieldMapping = fieldLookup["OperationMode"];
                obj.OperationMode = CreateMetaValues(fieldMapping);
            }

            {
                // Initialize meta value structure to "LoadIncrementPercentage" field
                FieldMapping fieldMapping = fieldLookup["LoadIncrementPercentage"];
                obj.LoadIncrementPercentage = CreateMetaValues(fieldMapping);
            }

            {
                // Initialize meta value structure to "TapVTx4" field
                FieldMapping fieldMapping = fieldLookup["TapVTx4"];
                obj.TapVTx4 = CreateMetaValues(fieldMapping);
            }

            {
                // Initialize meta value structure to "TapVTx5" field
                FieldMapping fieldMapping = fieldLookup["TapVTx5"];
                obj.TapVTx5 = CreateMetaValues(fieldMapping);
            }

            {
                // Initialize meta value structure to "CapBkrVCap1" field
                FieldMapping fieldMapping = fieldLookup["CapBkrVCap1"];
                obj.CapBkrVCap1 = CreateMetaValues(fieldMapping);
            }

            {
                // Initialize meta value structure to "CapBkrVCap2" field
                FieldMapping fieldMapping = fieldLookup["CapBkrVCap2"];
                obj.CapBkrVCap2 = CreateMetaValues(fieldMapping);
            }

            {
                // Initialize meta value structure to "BusBkrVCap1" field
                FieldMapping fieldMapping = fieldLookup["BusBkrVCap1"];
                obj.BusBkrVCap1 = CreateMetaValues(fieldMapping);
            }

            {
                // Initialize meta value structure to "BusBkrVCap2" field
                FieldMapping fieldMapping = fieldLookup["BusBkrVCap2"];
                obj.BusBkrVCap2 = CreateMetaValues(fieldMapping);
            }

            return obj;
        }

        private void CollectFromLVCDataOutputs(List<IMeasurement> measurements, TypeMapping typeMapping, LVC.Model.LVCData.Outputs data, LVC.Model.LVCData._OutputsMeta meta)
        {
            Dictionary<string, FieldMapping> fieldLookup = typeMapping.FieldMappings.ToDictionary(mapping => mapping.Field.Identifier);

            {
                // Convert value from "OperationMode" field to measurement
                FieldMapping fieldMapping = fieldLookup["OperationMode"];
                IMeasurement measurement = MakeMeasurement(meta.OperationMode, (double)data.OperationMode);
                measurements.Add(measurement);
            }

            {
                // Convert value from "LoadIncrementPercentage" field to measurement
                FieldMapping fieldMapping = fieldLookup["LoadIncrementPercentage"];
                IMeasurement measurement = MakeMeasurement(meta.LoadIncrementPercentage, (double)data.LoadIncrementPercentage);
                measurements.Add(measurement);
            }

            {
                // Convert value from "TapVTx4" field to measurement
                FieldMapping fieldMapping = fieldLookup["TapVTx4"];
                IMeasurement measurement = MakeMeasurement(meta.TapVTx4, (double)data.TapVTx4);
                measurements.Add(measurement);
            }

            {
                // Convert value from "TapVTx5" field to measurement
                FieldMapping fieldMapping = fieldLookup["TapVTx5"];
                IMeasurement measurement = MakeMeasurement(meta.TapVTx5, (double)data.TapVTx5);
                measurements.Add(measurement);
            }

            {
                // Convert value from "CapBkrVCap1" field to measurement
                FieldMapping fieldMapping = fieldLookup["CapBkrVCap1"];
                IMeasurement measurement = MakeMeasurement(meta.CapBkrVCap1, (double)data.CapBkrVCap1);
                measurements.Add(measurement);
            }

            {
                // Convert value from "CapBkrVCap2" field to measurement
                FieldMapping fieldMapping = fieldLookup["CapBkrVCap2"];
                IMeasurement measurement = MakeMeasurement(meta.CapBkrVCap2, (double)data.CapBkrVCap2);
                measurements.Add(measurement);
            }

            {
                // Convert value from "BusBkrVCap1" field to measurement
                FieldMapping fieldMapping = fieldLookup["BusBkrVCap1"];
                IMeasurement measurement = MakeMeasurement(meta.BusBkrVCap1, (double)data.BusBkrVCap1);
                measurements.Add(measurement);
            }

            {
                // Convert value from "BusBkrVCap2" field to measurement
                FieldMapping fieldMapping = fieldLookup["BusBkrVCap2"];
                IMeasurement measurement = MakeMeasurement(meta.BusBkrVCap2, (double)data.BusBkrVCap2);
                measurements.Add(measurement);
            }
        }

        #endregion
    }
}
