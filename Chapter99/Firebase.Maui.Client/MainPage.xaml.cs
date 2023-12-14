using System.Collections.ObjectModel;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Maui.Client.Models;
using Plugin.Firebase.CloudMessaging;

namespace Firebase.Maui.Client;

public partial class MainPage : ContentPage
{
    FirebaseClient _firebaseClient = new("https://dotnet-maui-android-firebase-default-rtdb.firebaseio.com/");
    public ObservableCollection<TodoItem> TodoItems { get; set; } = new ObservableCollection<TodoItem>();

    
    //int count = 0;

    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
        
        var collection = _firebaseClient
            .Child("Todo")
            .AsObservable<TodoItem>()
            .Subscribe((item) =>
            {
                if (item.Object != null)
                {
                    TodoItems.Add(item.Object);
                }
            });
    }

    private async void OnCounterClicked(object sender, EventArgs e)
    {
        // _firebaseClient.Child("Todo")
        //     .PostAsync(new TodoItem
        //     {
        //         Title = TitleEntry.Text
        //     });
        
        await CrossFirebaseCloudMessaging.Current.CheckIfValidAsync();
        var token = await CrossFirebaseCloudMessaging.Current.GetTokenAsync();
        Console.WriteLine($"FCM token: {token}");
        // count++;
        //
        // if (count == 1)
        //     CounterBtn.Text = $"Clicked {count} time";
        // else
        //     CounterBtn.Text = $"Clicked {count} times";
        //
        // SemanticScreenReader.Announce(CounterBtn.Text);
    }
}