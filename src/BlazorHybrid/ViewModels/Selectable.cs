namespace BlazorHybrid.ViewModels;

public class Selectable<T>
{
    public T Item { get; init; }
    public bool IsSelected { get; set; }

    public Selectable(T item)
    {
        Item = item;
    }
}