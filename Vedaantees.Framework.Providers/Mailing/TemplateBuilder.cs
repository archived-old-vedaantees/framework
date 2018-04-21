using DotLiquid;

namespace Vedaantees.Framework.Providers.Mailing
{
    public class TemplateBuilder : ITemplateBuilder
    {
        public string Build<T>(string script, T model) where T : class, ITemplateModel
        {
            var template = Template.Parse(script);
            return template.Render(Hash.FromAnonymousObject(model));
        }
    }
}