namespace Vedaantees.Framework.Shell.Rules
{
    public interface IRule<in T>
    {
        RuleResult Validate(T command);
        RuleDefinition GetRuleDefinition();
    }
}