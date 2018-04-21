using System.Collections.Generic;
using System.Linq;
using Autofac;
using Vedaantees.Framework.Shell.Rules;

namespace Vedaantees.Framework.Providers.ServiceBus
{
    public class RuleManager : IRuleManager
    {
        private readonly IComponentContext _container;

        public RuleManager(IComponentContext container)
        {
            _container = container;
        }

        public RuleResults ExecuteBusinessRules<T>(T objectToValidate)
        {
            var results = new RuleResults { IsSuccessful = true };

            if (_container.IsRegistered<IRule<T>>())
            {
                dynamic applicableRules = _container.Resolve<IEnumerable<IRule<T>>>();

                foreach (var rule in applicableRules)
                    results.Add(rule.GetType().GetMethod("Validate", new[] { objectToValidate.GetType() }).Invoke(rule, new[] { objectToValidate }));

                results.IsSuccessful = results.Count(p => p.ErrorLevel == RuleMessageType.Error) == 0;
            }

            return results;
        }
    }
}