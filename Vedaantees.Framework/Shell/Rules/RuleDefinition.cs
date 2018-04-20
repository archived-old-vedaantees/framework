#region  usings 

using System;

#endregion

namespace Vedaantees.Framework.Shell.Rules
{
    [Serializable]
    public class RuleDefinition : IEquatable<RuleDefinition>
    {
        private RuleDefinitionData _currentDefinitionData;

        public Type RuleType
        {
            get => _currentDefinitionData.RuleType;
            set => _currentDefinitionData.RuleType = value;
        }

        public string RuleId
        {
            get => _currentDefinitionData.RuleId;
            set => _currentDefinitionData.RuleId = value;
        }

        public string Name
        {
            get => _currentDefinitionData.Name;
            set => _currentDefinitionData.Name = value;
        }


        public string Description
        {
            get => _currentDefinitionData.Description;
            set => _currentDefinitionData.Description = value;
        }


        public string ClassName
        {
            get => _currentDefinitionData.ClassName;
            set => _currentDefinitionData.ClassName = value;
        }


        public string SerializedRuleScript
        {
            get => _currentDefinitionData.SerializedRuleScript;
            set => _currentDefinitionData.SerializedRuleScript = value;
        }


        public string ErrorMessage
        {
            get => _currentDefinitionData.ErrorMessage;
            set => _currentDefinitionData.ErrorMessage = value;
        }


        public string SerializedSettings
        {
            get => _currentDefinitionData.SerializedSettings;
            set => _currentDefinitionData.SerializedSettings = value;
        }


        public RuleMessageType SeverityLevel
        {
            get => _currentDefinitionData.SeverityLevel;
            set => _currentDefinitionData.SeverityLevel = value;
        }


        public string TargetType
        {
            get => _currentDefinitionData.TargetType;
            set => _currentDefinitionData.TargetType = value;
        }


        public int Priority
        {
            get => _currentDefinitionData.Priority;
            set => _currentDefinitionData.Priority = value;
        }

        public bool IsChanged { get; private set; }

        public bool Equals(RuleDefinition other)
        {
            return RuleId == other.RuleId &&
                   Name == other.Name &&
                   Description == other.Description &&
                   ClassName == other.ClassName &&
                   SerializedRuleScript == other.SerializedRuleScript &&
                   ErrorMessage == other.ErrorMessage &&
                   SerializedSettings == other.SerializedSettings &&
                   TargetType == other.TargetType &&
                   Priority == other.Priority;
        }


        public object Clone()
        {
            return MemberwiseClone();
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 31 + RuleId.GetHashCode();
            hash = hash * 31 + Name.GetHashCode();
            hash = hash * 31 + Description.GetHashCode();
            hash = hash * 31 + ClassName.GetHashCode();
            hash = hash * 31 + SerializedRuleScript.GetHashCode();
            hash = hash * 31 + ErrorMessage.GetHashCode();
            hash = hash * 31 + SerializedSettings.GetHashCode();
            hash = hash * 31 + TargetType.GetHashCode();
            hash = hash * 31 + Priority.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            return Equals((RuleDefinition) obj);
        }

        public static bool operator ==(RuleDefinition first, RuleDefinition second)
        {
            return first != null && first.Equals(second);
        }

        public static bool operator !=(RuleDefinition first, RuleDefinition second)
        {
            return first != null && !first.Equals(second);
        }

        private struct RuleDefinitionData
        {
            internal Type RuleType;
            internal string RuleId;
            internal string Name;
            internal string Description;
            internal string ClassName;
            internal string SerializedRuleScript;
            internal string ErrorMessage;
            internal string SerializedSettings;
            internal string TargetType;
            internal int Priority;
            internal RuleMessageType SeverityLevel;
        }
    }
}