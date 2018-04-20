namespace Vedaantees.Framework.Providers.Storages.Graphs
{
    public interface IGraphRelation<TSource, TDestination> where TSource : IEntity<string>
        where TDestination : IEntity<string>
    {
        TSource Source { get; set; }

        TDestination Destination { get; set; }

        string Id { get; set; }

        string GetLabel();
    }
}