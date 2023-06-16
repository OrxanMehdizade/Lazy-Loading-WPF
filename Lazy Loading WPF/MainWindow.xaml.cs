using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lazy_Loading_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Book> GetBooks { get; } = new ObservableCollection<Book>();

        public MainWindow()
        {
            InitializeComponent();
            FillTableComboBox();
            DataContext = this;
        }

        private void FillTableComboBox()
        {
            using (var db = new LibraryContext())
            {
                var tableNames = db.Model.GetEntityTypes()
                    .Select(c => c.GetTableName())
                    .ToList();

                ComboClass.Items.Clear();

                foreach (var tableName in tableNames)
                {
                    ComboClass.Items.Add(new ComboBoxItem { Content = tableName });
                }
            }
        }

        private void ComboClass_SelectionChanged(object sender, RoutedEventArgs e)
        {
            using (var database = new LibraryContext())
            {
                if (ComboClass.SelectedItem is ComboBoxItem selectedItem)
                {
                    string selectedTable = selectedItem.Content.ToString();

                    ComboColumns.Items.Clear();

                    if (selectedTable == "Authors")
                    {
                        var authors = database.Authors.ToList();
                        authors.ForEach(a => ComboColumns.Items.Add($"{a.FirstName} {a.LastName}"));
                    }
                    else if (selectedTable == "Themes")
                    {
                        var themes = database.Themes.ToList();
                        themes.ForEach(t => ComboColumns.Items.Add($"{t.Name}"));
                    }
                    else if (selectedTable == "Categories")
                    {
                        var categories = database.Categories.ToList();
                        categories.ForEach(c => ComboColumns.Items.Add($"{c.Name}"));
                    }
                }
            }
        }

        private void ComboColumns_SelectionChanged(object sender, RoutedEventArgs e)
        {
            using (var database = new LibraryContext())
            {
                if (ComboClass.SelectedItem is ComboBoxItem selectedItem)
                {
                    string selectedTable = selectedItem.Content.ToString();
                    GetBooks.Clear();

                    if (selectedTable == "Authors" && ComboColumns.SelectedItem is string author)
                    {
                        var authorBooks = database.Books
                            .Include(b => b.Authors)
                            .Where(b => $"{b.Authors.FirstName} {b.Author.LastName}" == author)
                            .ToList();
                        authorBooks.ForEach(s => GetBooks.Add(s));
                    }
                    else if (selectedTable == "Themes" && ComboColumns.SelectedItem is string theme)
                    {
                        var themeBooks = database.Books
                            .Include(b => b.Theme)
                            .Where(b => $"{b.Theme.Name}" == theme)
                            .ToList();
                        themeBooks.ForEach(s => GetBooks.Add(s));
                    }
                    else if (selectedTable == "Categories" && ComboColumns.SelectedItem is string category)
                    {
                        var categoryBooks = database.Books
                            .Include(b => b.Category)
                            .Where(b => $"{b.Category.Name}" == category)
                            .ToList();
                        categoryBooks.ForEach(s => GetBooks.Add(s));
                    }
                }
            }
        }
    }

}

