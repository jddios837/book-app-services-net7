using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Northwind.Maui.Client.ViewModels;

namespace Northwind.Maui.Client;

public partial class CustomerDetailPage : ContentPage
{
    private CustomersListViewModel customers;
    
    public CustomerDetailPage(CustomersListViewModel customers)
    {
        InitializeComponent();
        
        this.customers = customers;
        BindingContext = new CustomerDetailViewModel();
        Title = "New Customer";
    }

    public CustomerDetailPage(CustomersListViewModel customers, 
        CustomerDetailViewModel customer)
    {
        InitializeComponent();
        
        this.customers = customers;
        BindingContext = customer;
        InsertButton.IsVisible = false;
    }

    private async void InsertButton_Clicked(object? sender, EventArgs e)
    {
        customers.Add((CustomerDetailViewModel)BindingContext);
        await Navigation.PopAsync(animated: true);
    }
}