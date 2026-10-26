using MAUIInventoryManager.ViewModels;

namespace MAUIInventoryManager.Pages;

public partial class ProductDetailPage : ContentPage
{
	public ProductDetailPage(ProductDetailViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}