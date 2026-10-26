using MAUIInventoryManager.ViewModels;

namespace MAUIInventoryManager.Pages;

public partial class InventoryPage : ContentPage
{
	public InventoryPage(InventoryViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is InventoryViewModel vm)
        {
            await vm.LoadProductsAsync();
        }
    }
}