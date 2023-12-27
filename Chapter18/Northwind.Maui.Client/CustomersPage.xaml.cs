using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Maui.Client.ViewModels;

namespace Northwind.Maui.Client;

public partial class CustomersPage : ContentPage
{
    public CustomersPage()
    {
        InitializeComponent();
        
        CustomersListViewModel viewModel = new();
        viewModel.AddSampleData();
        BindingContext = viewModel;
    }

    private async void Add_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new CustomerDetailPage(
            BindingContext as CustomersListViewModel));
    }

    private async void Customer_Tapped(object? sender, ItemTappedEventArgs e)
    {
        if (e.Item is not CustomerDetailViewModel c) return;

        // navigate to the detail view and show the tapped customer
        await Navigation.PushAsync(new CustomerDetailPage(
            BindingContext as CustomersListViewModel, c));
    }

    private async void Customers_Refreshing(object? sender, EventArgs e)
    {
        if (sender is not ListView lv) return;
        
        lv.IsRefreshing = true;

        await Task.Delay(1500);
        
        lv.IsRefreshing = false;
    }

    private async void Customer_Phoned(object? sender, EventArgs e)
    {
        MenuItem menuItem = sender as MenuItem;
        if (menuItem.BindingContext is not CustomerDetailViewModel c) return;
        if (await DisplayAlert("Dial a Number",
                "Would you like to call " + c.Phone + "?",
                "Yes", "No"))
        {
            try
            {
                if (PhoneDialer.IsSupported)
                {
                    PhoneDialer.Open(c.Phone);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert(title: "Failed",
                    message: string.Format(
                        "Failed to dial {0} due to: {1}", c.Phone, ex.Message),
                    cancel: "OK");
            }
        }
    }

    private void Customer_Deleted(object? sender, EventArgs e)
    {
        MenuItem menuItem = sender as MenuItem;
        if (menuItem.BindingContext is not CustomerDetailViewModel c) return;
        (BindingContext as CustomersListViewModel).Remove(c);
    }
}