using CommunityToolkit.Mvvm.ComponentModel;
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
    }
}
