#region  usings 

using System.Collections.Generic;
using System.Text;

#endregion

namespace Vedaantees.Framework.Shell.Rules
{
    public class RuleResults : List<RuleResult>
    {
        public bool IsSuccessful { get; set; }

        public new void Add(RuleResult result)
        {
            if (!result.IsSuccessful)
                IsSuccessful = false;

            base.Add(result);
        }

        public void Add(IEnumerable<RuleResult> results)
        {
            foreach (var result in results) Add(result);
        }

        public void Add(bool isSuccessful, RuleMessageType errorLevel, string errorMessage)
        {
            var result = new RuleResult {IsSuccessful = isSuccessful, ErrorLevel = errorLevel, Message = errorMessage};
            Add(result);
        }

        public void AddFailure(RuleMessageType errorLevel, string message)
        {
            Add(false, errorLevel, message);
        }

        public void AddFailure(string message)
        {
            Add(false, RuleMessageType.Error, message);
        }

        public void AddSuccess()
        {
            Add(true, RuleMessageType.Information, string.Empty);
        }

        public void AddSuccess(string information)
        {
            Add(true, RuleMessageType.Information, information);
        }

        public override string ToString()
        {
            var results = new StringBuilder();
            var i = 1;

            results.AppendLine($"You need to correct {Count} issues to continue");

            foreach (var ruleResult in this)
                results.AppendLine($"{i++}: {ruleResult.Message}");

            return results.ToString();
        }
    }
}