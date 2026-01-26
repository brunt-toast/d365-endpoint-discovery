using System.Collections.ObjectModel;

namespace Dev.JoshBrunton.DynamicsEndpointDiscovery.BlazorHybrid.Extensions.System.Collections.ObjectModel;

internal static class ObservableCollectionExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> newRange)
    {
        foreach (T newItem in newRange)
        {
            source.Add(newItem);
        }
    }

    public static void ReplaceRange<T>(this ObservableCollection<T> source, IEnumerable<T> newRange)
    {
        source.Clear();

        foreach (T newItem in newRange)
        {
            source.Add(newItem);
        }
    }
}
