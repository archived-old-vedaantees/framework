#region  usings 

using System;

#endregion

namespace Vedaantees.Framework.Shell.Rules
{
    [Serializable]
    public class RuleResult : IEquatable<RuleResult>
    {
        private RuleResultData _currentData;

        public bool IsSuccessful
        {
            get => _currentData.IsSuccessful;
            set => _currentData.IsSuccessful = value;
        }

        public RuleMessageType ErrorLevel
        {
            get => _currentData.ErrorLevel;
            set => _currentData.ErrorLevel = value;
        }

        public string Message
        {
            get => _currentData.Message;
            set => _currentData.Message = value;
        }

        public bool Equals(RuleResult other)
        {
            return IsSuccessful == other.IsSuccessful &&
                   ErrorLevel == other.ErrorLevel &&
                   Message == other.Message;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public override int GetHashCode()
        {
            var hash = 17;

            hash = hash * 31 + IsSuccessful.GetHashCode();
            hash = hash * 31 + ErrorLevel.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            var ruleResult = obj as RuleResult;
            return ruleResult != null && Equals(ruleResult);
        }

        public static bool operator ==(RuleResult first, RuleResult second)
        {
            return first != null && first.Equals(second);
        }

        public static bool operator !=(RuleResult first, RuleResult second)
        {
            return first != null && !first.Equals(second);
        }

        private struct RuleResultData
        {
            internal bool IsSuccessful;
            internal RuleMessageType ErrorLevel;
            internal string Message;
        }
    }
}