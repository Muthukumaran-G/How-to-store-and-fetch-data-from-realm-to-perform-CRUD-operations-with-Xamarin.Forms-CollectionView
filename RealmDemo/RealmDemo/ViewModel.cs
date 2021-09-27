using Realms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace RealmDemo
{
    public class ViewModel: INotifyPropertyChanged
    {
        Realm realm;
        private ObservableCollection<Model> books;
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Model> Books
        {
            get { return books; }
            set { this.books = value;
                OnPropertyChanged("Books");
            }
        }

        private object selectedItem;

        public object SelectedItem
        {
            get { return selectedItem; }
            set { this.selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public Command AddCommand { get; set; }
        public Command RemoveCommand { get; set; }
        public Command RemoveAllCommand { get; set; }
        public Command FetchDataCommand { get; set; }

        public ViewModel()
        {
            realm = Realm.GetInstance();

            AddCommand = new Command(ExecuteAdd);
            RemoveCommand = new Command(ExecuteRemove);
            RemoveAllCommand = new Command(ExecuteRemoveAll);
            FetchDataCommand = new Command(ExecuteFetchItems);

            _ = realm.All<Model>().Count() > 0 ? false : PopulateData();
        }

        private void ExecuteFetchItems(object obj)
        {
            if (Books == null)
            {
                Books = new ObservableCollection<Model>(realm.All<Model>().AsRealmCollection());
            }
            else
            {
                if (Books.Count <= 0)
                {
                    App.Current.MainPage.DisplayAlert("No Items in Realm", "Add new items to Realm", "OK");
                }
            }
        }

        private bool PopulateData()
        {
            realm.Write(() =>
            {
                for (int i = 0; i < 4; i++)
                {
                    var book = new Model()
                    {
                        BookName = BookNames[i],
                        BookDescription = BookDescriptions[i],
                        BookAuthor = BookAuthers[i],
                    };
                    realm.Add(book);
                }
            });

            
            return true;
        }

        private void ExecuteRemove(object obj)
        {
            if(SelectedItem == null)
            {
                App.Current.MainPage.DisplayAlert("No Item Selected!", "Select an item to delete", "OK");
                return;
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Book Removed!", (selectedItem as Model).BookName + " book has been removed!", "OK");
            }

            realm.Write(() =>
            {
                var book = Books.FirstOrDefault(x => x.Equals(SelectedItem));
                realm.Remove(book);
                Books.Remove(book);
            });
        }

        private void ExecuteRemoveAll(object obj)
        {
            realm.Write(() =>
            {
                realm.RemoveAll<Model>();
            });

            Books.Clear();
        }

        private void ExecuteAdd(object obj)
        {
            if(Books == null)
                Books = new ObservableCollection<Model>(realm.All<Model>().AsRealmCollection());

            realm.Write(() =>
            {
                var book = new Model()
                {
                    BookName = BookNames[new Random().Next(0, 10)],
                    BookDescription = BookDescriptions[new Random().Next(0, 10)],
                    BookAuthor = BookAuthers[new Random().Next(0, 10)],
                };

                realm.Add(book);
                Books.Add(book);
                App.Current.MainPage.DisplayAlert("Book Added!", book.BookName + " book has been added!", "OK");

            });
        }


        public void OnPropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        string[] BookNames = new string[] {
            "Neural Networks Using C#",
            "C# Code Contracts",
            "Machine Learning Using C#",
            "Object-Oriented Programming in C#",
            "Visual Studio Code",
            "Android Programming",
            "iOS Succinctly",
            "Visual Studio 2015",
            "Xamarin.Forms",
            "Windows Store Apps",
            ".Net Core Succinctly",
            "Assembly Language",
            "SQL Server for C# Developers",
            "Entity Framework Code First",
            "Localization for .NET",
            "Developing Windows Services",
            "Data Structures",
            "Objective-C",
            "SciPy Programming",
            "Visual Studio 2017"
        };

        string[] BookDescriptions = new string[] {
            "Neural networks are an exciting field of software development used to calculate outputs from input data.",
            "Code Contracts provide a way to convey code assumptions in your .NET applications. In C# Code Contracts Succinctly, author Dirk Strauss demonstrates.",
            "In Machine Learning Using C# Succinctly, you’ll learn several different approaches to applying machine learning to data analysis and prediction problems.",
            "Object-oriented programming is the de facto programming paradigm for many programming languages. Object-Oriented Programming in C# Succinctly provides an introduction to OOP for C# developers.",
            "Visual Studio Code is a powerful tool for editing code and serves as a complete environment for end-to-end programming. Alessandro Del Sole Visual Studio Code Succinctly will guide readers to mastery of this valuable tool so that they can make full use of its features.",
            "Ryan Hodson provides a useful overview of the Android application lifecycle.",
            "iOS Succinctly is for developers looking to step into the sometimes frightening world of iPhone and iPad app development. Written as the companion to Objective-C Succinctly, this e-book guides you from creating a simple, single page application to managing assets in a complex, multi-scene application.",
            "Microsoft Visual Studio 2015 is the new version of the widely-used integrated development environment for building modern, high-quality applications for a number of platforms such as Windows, the web, the cloud, and mobile devices.",
            "With the fragmented landscape of mobile device platforms, tools for creating cross-platform apps have sprung up as varied and numerous as apps themselves.",
            "Windows Store apps present a radical shift in Windows development.",
            "With .NET Core, cross-platform develop is easier and backward compatibility is no problem. Author Giancarlo Lelli guides you through the fundamentals of .NET Core in his latest book, .NET Core Succinctly. Within its pages you will learn to harness this open-source, cloud-optimized port of the .NET Framework for modern apps. ",
            "Assembly language is as close to writing machine code as you can get without writing in pure hexadecimal.",
            "Developers of C# applications with a SQL Server database can learn to connect to a database using classic ADO.NET and look at different methods of developing databases using the Entity Framework.",
            "Follow author Ricardo Peres as he introduces the newest development mode for Entity Framework, Code First.",
            "Learn to write applications that support different languages and cultures, with an emphasis on .NET development. With the help of author Jonas Gauffin, Localization for .NET Succinctly will help you become an effective developer in the global community. ",
            "Learn the basics of Windows Services and how to develop and deploy basic apps.",
            "Data Structures is your concise guide to skip lists, hash tables, heaps, priority queues, AVL trees, and B-trees.",
            "Objective-C Succinctly is the only book you need for getting started with Objective-C—the primary language beneath all Mac, iPad, and iPhone apps.",
            "James McCaffrey’s SciPy Programming Succinctly offers readers a quick, thorough grounding in knowledge of the Python open source extension SciPy. The SciPy library, accompanied by its interdependent NumPy, offers Python programmers advanced functions that work with arrays and matrices. Each section presents a complete demo program for programmers to experiment with, illustrate each function.",
            "The release of Visual Studio 2017 is another critical element in Microsoft’s pivot to the “any developer, any platform, any device”."
        };

        string[] BookAuthers = new string[] {
            "James McCaffrey",
            "Dirk Strauss",
            "James McCaffrey",
            "Sander Rossel",
            "Alessandro Del Sole",
            "Ryan Hodson",
            "Ryan Hodson",
            "Alessandro Del Sole",
            "Derek Jensen",
            "John Garland",
            "Ray Bradbury",
            "Nathaniel Hawthorne",
            "Joseph Conrad",
            "Mark Twain",
            "Pete Waite",
            "Mark Twain",
            "Joseph Heller",
            "Ernest Hemingway",
            "Mark Twain",
            "Neil Gaiman"
        };

    }
}
