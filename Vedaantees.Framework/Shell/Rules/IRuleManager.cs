namespace Vedaantees.Framework.Shell.Rules
{
    public interface IRuleManager
    {
        RuleResults ExecuteBusinessRules<T>(T objectToValidate);
    }
}