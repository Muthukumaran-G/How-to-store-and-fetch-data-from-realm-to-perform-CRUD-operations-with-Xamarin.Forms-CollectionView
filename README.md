# How-to-store-and-fetch-data-from-realm-to-perform-CRUD-operations-with-Xamarin.Forms-CollectionView

Xamarin Realm is a best alternative for SQLite due to its developer friendly usage as well as for providing optimized performance. First and foremost step is adding **Realm** nuget package in Xamarin project. In latest version (as of now v10.5.1), building the projects will automatically create a FodeWeaver.xml file. If not, please add the below code snippets manually in all the projects(PCL as well as native).

**Note:** The RealmWeaver was renamed to just Realm after v4.

```
<Weavers xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="FodyWeavers.xsd">
  <Realm />
</Weavers>
```

Now we can move on to creating realm objects which is just a piece of a cake. Inherit the RealmObject class for the Model class.

```
public class Model : RealmObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
}
```

So simple, right! Here comes the best part. Storing model values to realm which is done in ViewModel. First create an instance for realm.

```
public class ViewModel: INotifyPropertyChanged
{
    Realm realm;
    
    public ViewModel()
    {
        realm = Realm.GetInstance();
        _ = realm.All<Model>().Count() > 0 ? false : PopulateData();
    }
    
    private bool PopulateData()
    {
        realm.Write(() =>
        {
            for (int i = 0; i < 4; i++)
            {
                var book = new Model()
                {
                    Name = Names[i],
                    Description = Descriptions[i],
                    Author = Authors[i],
                };
                realm.Add(book);
            }
        });

        return true;
    }
```

Make sure that the object is added within the write method to avoid exception. Here I have used discard variable since, there is no need to allocate memory for the boolean value returned from the PopulateData method. Moreover the use of discard is just for showcase purpose ;)

The collection added into the realm DB will be retreived in the form of IEnumerable collection. However when performing realtime CRUD actions to IEnumerable collection, it won't reflected in the UI. To overcome this make sure to cast the RealmCollection to ObservableCollection as like below.

```
private ObservableCollection<Model> books;
public ObservableCollection<Model> Books
{
    get { return books; }
    set { this.books = value;
        OnPropertyChanged("Books");
    }
}

Books = new ObservableCollection<Model>(realm.All<Model>().AsRealmCollection());
```

Now its time to bind this collection to the CollectionView.

```
<CollectionView ItemsSource="{Binding Books}">
.
.
.
</CollectionView>
```

Similarly you can modify, remove or remove all the objects from realm like below.

Add:
```
realm.Write(() =>
{
    var book = new Model()
    {
        Name = Names[new Random().Next(0, 10)],
        Description = Descriptions[new Random().Next(0, 10)],
        Author = Authors[new Random().Next(0, 10)],
    };

    realm.Add(book);
    Books.Add(book);
});
```

Remove:
```
realm.Write(() =>
{
    var book = Books.FirstOrDefault(x => x.Equals(SelectedItem));
    realm.Remove(book);
    Books.Remove(book);
});
```

Here I have removed the selected item from the CollectionView by maintaining the SelectedItem through property binding.

Clear:
```
realm.Write(() =>
{
    realm.RemoveAll<Model>();
});

Books.Clear();
```

Make sure to update the bound collection along with the realm to update the source changes of Collection. 
![image](https://user-images.githubusercontent.com/26808947/134950604-34063f7f-7163-4449-87ad-acf50ea7afec.png)


That's it! Easy peasy code it easy ;)
