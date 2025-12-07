using System.Collections.Specialized;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace GrpcDemo.Dashboard.Behaviors;

public class ScrollOnNewItemBehavior : Behavior<ListBox>
{
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null)
            return;
        
        if (AssociatedObject.Items is INotifyCollectionChanged inch)
            inch.CollectionChanged += ItemsOnCollectionChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (AssociatedObject is null)
            return;

        if (AssociatedObject.Items is INotifyCollectionChanged inch)
            inch.CollectionChanged -= ItemsOnCollectionChanged;
    }
    
    private void ItemsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Add or NotifyCollectionChangedAction.Reset)
            ScrollToEnd();
    }

    private void ScrollToEnd()
    {
        if (AssociatedObject is null)
            return;
        
        if (AssociatedObject.ItemCount <= 0)
            return;
        
        AssociatedObject.ScrollIntoView(AssociatedObject.ItemCount - 1);
    }
}