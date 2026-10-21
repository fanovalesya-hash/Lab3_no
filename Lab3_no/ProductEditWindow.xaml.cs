using Lab3_no.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab3_no
{
    /// <summary>
    /// Логика взаимодействия для ProductEditWindow.xaml
    /// </summary>
    public partial class ProductEditWindow : Window
    {
        private readonly ShopContext _context;
        private readonly Product _editingProduct; // null, если режим добавления
        private readonly bool _isEditMode;

        public ProductEditWindow(Product product)
        {
            InitializeComponent();
            _context = new ShopContext();
            _editingProduct = product;
            _isEditMode = product != null;

            LoadDictionaries();

            if (_isEditMode)
            {
                Title = "Редактирование товара";
                IdTextBox.Visibility = Visibility.Visible;
                LoadProductData();
            }
            else
            {
                Title = "Добавление товара";
                IdTextBox.Visibility = Visibility.Collapsed;
            }
        }

        // Загружаем справочники в ComboBox-ы
        private void LoadDictionaries()
        {
            NameComboBox.ItemsSource = _context.ProductNames.OrderBy(n => n.Name).ToList();
            CategoryComboBox.ItemsSource = _context.Categories.OrderBy(c => c.Name).ToList();
            ManufacturerComboBox.ItemsSource = _context.Manufacturers.OrderBy(m => m.Name).ToList();
            SupplierComboBox.ItemsSource = _context.Suppliers.OrderBy(s => s.Name).ToList();
        }

        // Заполняем поля данными редактируемого товара
        private void LoadProductData()
        {
            IdTextBox.Text = _editingProduct.Id.ToString();
            NameComboBox.SelectedValue = _editingProduct.NameId;
            CategoryComboBox.SelectedValue = _editingProduct.CategoryId;
            ManufacturerComboBox.SelectedValue = _editingProduct.ManufacturerId;
            SupplierComboBox.SelectedValue = _editingProduct.SupplierId;
            DescriptionTextBox.Text = _editingProduct.Description ?? "";
            PriceTextBox.Text = _editingProduct.Price?.ToString() ?? "";
            CountTextBox.Text = _editingProduct.Count?.ToString() ?? "";
            DiscountTextBox.Text = _editingProduct.Discount?.ToString() ?? "";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Валидация числовых полей
            if (!double.TryParse(PriceTextBox.Text, out double price) || price < 0)
            {
                MessageBox.Show("Цена должна быть неотрицательным числом.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(CountTextBox.Text, out double count) || count < 0)
            {
                MessageBox.Show("Количество должно быть неотрицательным числом.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(DiscountTextBox.Text, out double discount) || discount < 0)
            {
                MessageBox.Show("Скидка должна быть неотрицательным числом.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NameComboBox.SelectedValue == null || CategoryComboBox.SelectedValue == null ||
                ManufacturerComboBox.SelectedValue == null || SupplierComboBox.SelectedValue == null)
            {
                MessageBox.Show("Необходимо выбрать наименование, категорию, производителя и поставщика.", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_isEditMode)
            {
                // Обновляем существующий товар
                _editingProduct.NameId = (int)NameComboBox.SelectedValue;
                _editingProduct.CategoryId = (int)CategoryComboBox.SelectedValue;
                _editingProduct.ManufacturerId = (int)ManufacturerComboBox.SelectedValue;
                _editingProduct.SupplierId = (int)SupplierComboBox.SelectedValue;
                _editingProduct.Description = DescriptionTextBox.Text;
                _editingProduct.Price = price;
                _editingProduct.Count = count;
                _editingProduct.Discount = discount;

                _context.Products.Update(_editingProduct);
            }
            else
            {
                // Создаем новый товар с ID = Max + 1
                int newId = _context.Products.Any() ? _context.Products.Max(p => p.Id) + 1 : 1;

                var newProduct = new Product
                {
                    Id = newId,
                    NameId = (int)NameComboBox.SelectedValue,
                    CategoryId = (int)CategoryComboBox.SelectedValue,
                    ManufacturerId = (int)ManufacturerComboBox.SelectedValue,
                    SupplierId = (int)SupplierComboBox.SelectedValue,
                    Description = DescriptionTextBox.Text,
                    Price = price,
                    Count = count,
                    Discount = discount,
                    Article = "", // Артикул можно оставить пустым или генерировать
                    Unit = "1"    // Единица измерения по умолчанию
                };

                _context.Products.Add(newProduct);
            }

            _context.SaveChanges();
            MessageBox.Show("Данные успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
