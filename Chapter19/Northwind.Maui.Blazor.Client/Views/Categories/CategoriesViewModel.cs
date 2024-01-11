using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Northwind.Maui.Blazor.Client.Views.Categories;

internal partial class CategoriesViewModel : ObservableCollection<Category>
{
    public string InfoMessage { get; set; } = string.Empty;
    public string ErrorMessage { get; set; } = string.Empty;
    public bool ErrorMessageVisible { get; set; }

    public CategoriesViewModel()
    {
        try
        {

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}