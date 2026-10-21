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

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new ProductEditWindow(null);
            editWindow.ShowDialog();
            LoadProducts(SearchTextBox.Text);
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsListBox.SelectedItem is Product selectedProduct)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить товар \"{selectedProduct.Name?.Name}\"?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    bool isInOrder = _context.OrderArticles.Any(oa => oa.ProductId == selectedProduct.Id);

                    if (isInOrder)
                    {
                        MessageBox.Show("Невозможно удалить товар, так как он присутствует в одном из заказов.",
                                        "Ошибка удаления",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Error);
                    }
                    else
                    {
                        _context.Products.Remove(selectedProduct);
                        _context.SaveChanges();
                        MessageBox.Show("Товар успешно удален.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadProducts(SearchTextBox.Text);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите товар для удаления.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ProductsListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ProductsListBox.SelectedItem is Product selectedProduct)
            {
                var editWindow = new ProductEditWindow(selectedProduct);
                editWindow.ShowDialog();
                LoadProducts(SearchTextBox.Text);
            }
        }
    }
}