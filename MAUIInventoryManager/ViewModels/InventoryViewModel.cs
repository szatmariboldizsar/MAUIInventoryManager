using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Models;
using DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MAUIInventoryManager.ViewModels
{
    public partial class InventoryViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        public InventoryViewModel(IProductService productService)
        {
            _productService = productService;
        }

        private List<Product> _allProducts = new();

        [ObservableProperty]
        public partial ObservableCollection<Product> Products { get; set; }

        [ObservableProperty]
        public partial string SearchText { get; set; }

        [ObservableProperty]
        public partial bool IsRefreshing { get; set; }

        [ObservableProperty]
        public partial bool IsFlyoutOpen { get; set; }

        [ObservableProperty]
        public partial Product? SelectedProduct { get; set; }

        [ObservableProperty]
        public partial string FlyoutTitle { get; set; }


        [RelayCommand]
        public async Task LoadProductsAsync()
        {
            IsRefreshing = true;
            try
            {
                _allProducts = await _productService.GetProductsAsync();
                FilterProducts();
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            FilterProducts();
        }

        private void FilterProducts()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Products = new ObservableCollection<Product>(_allProducts);
            }
            else
            {
                var query = SearchText.Trim().ToLower();
                var filtered = _allProducts.Where(p =>
                    (!string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(query)) ||
                    (!string.IsNullOrEmpty(p.Barcode) && p.Barcode.ToLower().Contains(query))
                ).ToList();

                Products = new ObservableCollection<Product>(filtered);
            }
        }

        [RelayCommand]
        public async Task AddNewProduct()
        {
            SelectedProduct = new Product();
            FlyoutTitle = "Add New Product";
            IsFlyoutOpen = true;
        }

        [RelayCommand]
        public async Task SelectProduct(Product? product)
        {
            if (product == null) return;

            SelectedProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Barcode = product.Barcode,
                Price = product.Price,
                Quantity = product.Quantity
            };

            FlyoutTitle = "Edit Product";
            IsFlyoutOpen = true;
        }

        [RelayCommand]
        public async Task CloseFlyout()
        {
            IsFlyoutOpen = false;
        }

        [RelayCommand]
        public async Task SaveProductAsync()
        {
            if (SelectedProduct == null || string.IsNullOrWhiteSpace(SelectedProduct.Name))
            {
                await Shell.Current.DisplayAlertAsync("Error", "Product name cannot be empty!", "OK");
                return;
            }

            if (SelectedProduct.Id == 0)
            {
                await _productService.CreateProductAsync(SelectedProduct);
            }
            else
            {
                await _productService.UpdateProductAsync(SelectedProduct);
            }

            IsFlyoutOpen = false;
            await LoadProductsAsync();
        }

        [RelayCommand]
        public async Task DeleteProductAsync()
        {
            if (SelectedProduct == null || SelectedProduct.Id == 0)
            {
                IsFlyoutOpen = false;
                return;
            }

            bool confirm = await Shell.Current.DisplayAlertAsync("Confirm", "Are you sure you want to delete this product?", "Yes", "No");
            if (confirm)
            {
                await _productService.DeleteProductAsync(SelectedProduct);
                IsFlyoutOpen = false;
                await LoadProductsAsync();
            }
        }
    }
}
