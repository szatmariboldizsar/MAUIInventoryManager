using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DAL.Models;
using DAL.Services.Interfaces;
using MAUIInventoryManager.Pages;
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
        public partial Product? SelectedProduct { get; set; }


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
            var navigationParameter = new Dictionary<string, object>
            {
                { "Product", new Product() }
            };

            await Shell.Current.GoToAsync(nameof(ProductDetailPage), navigationParameter);
        }

        [RelayCommand]
        public async Task SelectProduct(Product? product)
        {
            if (product == null) return;

            var navigationParameter = new Dictionary<string, object>
            {
                { "Product", product }
            };

            await Shell.Current.GoToAsync(nameof(ProductDetailPage), navigationParameter);
        }
    }
}
