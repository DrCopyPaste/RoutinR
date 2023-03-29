using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace RoutinR.MAUI.Controls;

public partial class DictionaryEditor : ContentView
{
    private string editing = string.Empty;

    public static readonly BindableProperty CollectionSourceProperty = BindableProperty.Create(nameof(CollectionSource), typeof(ObservableCollection<KeyValuePair<string, string>>), typeof(DictionaryEditor), default(ObservableCollection<KeyValuePair<string, string>>),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (DictionaryEditor)bindable;
        });

    public ObservableCollection<KeyValuePair<string, string>> CollectionSource
    {
        get => (ObservableCollection<KeyValuePair<string, string>>)GetValue(DictionaryEditor.CollectionSourceProperty);
        set => SetValue(DictionaryEditor.CollectionSourceProperty, value);
    }

    public static readonly BindableProperty EditingKeyCollectionSourceProperty = BindableProperty.Create(nameof(EditingKeyCollectionSourceProperty), typeof(ObservableCollection<string>), typeof(DictionaryEditor), default(ObservableCollection<string>),
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            var control = (DictionaryEditor)bindable;

            (control as DictionaryEditor).PendingKey.IsVisible = newValue == null;
            (control as DictionaryEditor).PendingCollectionKey.IsVisible = newValue != null;
        });

    public ObservableCollection<string> EditingKeyCollectionSource
    {
        get => (ObservableCollection<string>)GetValue(DictionaryEditor.EditingKeyCollectionSourceProperty);
        set => SetValue(DictionaryEditor.EditingKeyCollectionSourceProperty, value);
    }

    public DictionaryEditor()
	{
		InitializeComponent();
    }

    [RelayCommand]
    async Task AddOrUpdate()
    {
        if (string.IsNullOrEmpty(getPendingKey())) return;
        if (string.IsNullOrEmpty(PendingValue.Text)) return;
        if (string.IsNullOrEmpty(editing) && CollectionSource.Any(x => x.Key == getPendingKey())) return;

        // keyname was changed but already exists in dictionary
        if (!string.IsNullOrEmpty(editing) && editing != getPendingKey() && CollectionSource.Any(x => x.Key == getPendingKey())) return;
        if (!string.IsNullOrEmpty(editing) && !CollectionSource.Any(x => x.Key == editing)) return;
        if (!string.IsNullOrEmpty(editing)) CollectionSource.RemoveAt(CollectionSource.IndexOf(CollectionSource.First(x => x.Key == editing)));

        CollectionSource.Add(new KeyValuePair<string, string>(getPendingKey(), PendingValue.Text));

        PendingKey.Text = string.Empty;
        PendingValue.Text = string.Empty;
        editing = string.Empty;
    }

    [RelayCommand]
    async Task Edit(string key)
    {
        if (!CollectionSource.Any(x => x.Key == key)) return;
        var item = CollectionSource.First(x => x.Key == key);

        PendingKey.Text = item.Key;
        if (EditingKeyCollectionSource != null) PendingCollectionKey.SelectedItem = item.Key;
        PendingValue.Text = item.Value;

        editing = key;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(getPendingKey())) return;
        if (string.IsNullOrEmpty(PendingValue.Text)) return;
        if (string.IsNullOrEmpty(editing) && CollectionSource.Any(x => x.Key == getPendingKey())) return;

        // keyname was changed but already exists in dictionary
        if (!string.IsNullOrEmpty(editing) && editing != getPendingKey() && CollectionSource.Any(x => x.Key == getPendingKey())) return;
        if (!string.IsNullOrEmpty(editing) && !CollectionSource.Any(x => x.Key == editing)) return;
        if (!string.IsNullOrEmpty(editing)) CollectionSource.RemoveAt(CollectionSource.IndexOf(CollectionSource.First(x => x.Key == editing)));

        CollectionSource.Add(new KeyValuePair<string, string>(getPendingKey(), PendingValue.Text));

        PendingKey.Text = string.Empty;
        PendingValue.Text = string.Empty;
        editing = string.Empty;
    }

    private string getPendingKey()
    {
        return EditingKeyCollectionSource == null ? PendingKey.Text : PendingCollectionKey.SelectedItem as string;
    }
}