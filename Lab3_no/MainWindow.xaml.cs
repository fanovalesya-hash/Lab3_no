using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Lab3_no.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3_no
{
    public partial class MainWindow : Window
    {
        private readonly ShopContext _context;

        public MainWindow()
        {
            InitializeComponent();
            _context = new ShopContext();
            LoadProducts();
        }


        private void LoadProducts(string searchText = "")
        {
            var allProducts = _context.Products
                .Include(p => p.Category)
                .Include(p => p.Manufacturer)
                .Include(p => p.Supplier)
                .Include(p => p.Name)
                .ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchWords = searchText.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                allProducts = allProducts.Where(p =>
                {
                    string fullText = string.Join(" ",
                        p.Article ?? "",
                        p.Description ?? "",
                        p.Name?.Name ?? "",
                        p.Category?.Name ?? "",
                        p.Manufacturer?.Name ?? "",
                        p.Supplier?.Name ?? ""
                    ).ToLower();

                    return searchWords.All(word => fullText.Contains(word));
                }).ToList();
            }

            ProductsListBox.ItemsSource = allProducts;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts(SearchTextBox.Text);
        }
    }
}