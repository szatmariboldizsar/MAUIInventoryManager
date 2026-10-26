using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Models;
using DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MAUIInventoryManager.ViewModels
{
    public partial class ProductDetailViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IProductService _productService;

        [ObservableProperty]
        public partial Product Product { get; set; }

        [ObservableProperty]
        public partial string Title { get; set; } = "Add New Product";

        public ProductDetailViewModel(IProductService productService)
        {
            _productService = productService;
            // Initialize with a blank product for "Add New" scenario
            Product = new Product();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue("Product", out var obj) && obj is Product incomingProduct)
            {
                Title = "Edit Product";

                // Safe, single-time cloning to protect the list item from live-editing
                Product = new Product
                {
                    Id = incomingProduct.Id,
                    Name = incomingProduct.Name,
                    Barcode = incomingProduct.Barcode,
                    Price = incomingProduct.Price,
                    Quantity = incomingProduct.Quantity
                };
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            if (Product == null || string.IsNullOrWhiteSpace(Product.Name))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Product name cannot be empty!", "OK");
                return;
            }

            if (Product.Id == 0)
            {
                await _productService.CreateProductAsync(Product);
            }
            else
            {
                await _productService.UpdateProductAsync(Product);
            }
            await Shell.Current.GoToAsync(".."); // Navigate back to list
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (Product != null && Product.Id != 0)
            {
                bool confirm = await Shell.Current.DisplayAlertAsync("Confirm", "Are you sure you want to delete this product?", "Yes", "No");
                if (confirm)
                {
                    await _productService.DeleteProductAsync(Product);
                    await Shell.Current.GoToAsync("..");
                }
            }
            else
            {
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
