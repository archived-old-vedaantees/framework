namespace Vedaantees.Framework.Providers.Mailing
{
    public interface ITemplateBuilder
    {
        string Build<T>(string template, T model) where T : class, ITemplateModel;
    }
}