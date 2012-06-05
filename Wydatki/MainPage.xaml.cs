using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Wydatki.Model;

namespace Wydatki
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;
        }

        public void deleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            // Cast the parameter as a button.
            var button = sender as Button;

            if (button != null)
            {
                // Get a handle for the to-do item bound to the button.
                CostItem costForDelete = button.DataContext as CostItem;

                App.ViewModel.DeleteCostItem(costForDelete);

                this.suma1.Text = "Razem: " + App.ViewModel.SumChosenCategory.ToString() + "zł";

                this.allCostItemsListBox.ItemsSource = App.ViewModel.ChosenCostsItems;
            }

            // Put the focus back to the main page.
            this.Focus();
        }

        protected override void OnNavigatedFrom(System.Windows.Navigation.NavigationEventArgs e)
        {
            // Save changes to the database.
            App.ViewModel.SaveChangesToDB();
        }

        private void newTaskAppBarButton_Click(object sender, EventArgs e)
        {
            Microsoft.Phone.Shell.ApplicationBarIconButton button = sender as Microsoft.Phone.Shell.ApplicationBarIconButton;
            //dodajemy wydatek
            if (button.Text.Equals("wydatek"))
            {
                App.ViewModel.AddingMoney = false;
            }
            else  //dodajemy przychód
            {
                App.ViewModel.AddingMoney = true;
            }
            NavigationService.Navigate(new Uri("/NewTaskPage.xaml", UriKind.Relative));
        }

        private void delBarCancelButton_Click(object sender, EventArgs e)
        {
            var costItem = this.allCostItemsListBox.SelectedItem as CostItem;
            if (costItem != null)
            {
                App.ViewModel.DeleteCostItem(costItem);
                this.suma1.Text = "Razem: " + App.ViewModel.SumChosenCategory.ToString() + "zł";

                this.allCostItemsListBox.ItemsSource = App.ViewModel.ChosenCostsItems;
            }

            // Put the focus back to the main page.
            this.Focus();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.suma1.Text = "Razem: " + App.ViewModel.SumChosenCategory.ToString() + "zł";
            this.allCostItemsListBox.ItemsSource = App.ViewModel.ChosenCostsItems;
        }

        private void NadCategoriesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CostCategory selectedNadCategory = ((sender as Microsoft.Phone.Controls.ListPicker).SelectedItem) as CostCategory;
                if (selectedNadCategory != null)
                {
                    App.ViewModel.SelectedNadCategory = selectedNadCategory;
                    this.categoriesListPicker2_.ItemsSource = App.ViewModel.SubCategoriesListWithAll;
                    this.categoriesListPicker2_.SelectedIndex = 0;
                    this.suma1.Text = "Razem: " + App.ViewModel.SumChosenCategory.ToString() + "zł";
                    this.allCostItemsListBox.ItemsSource = App.ViewModel.ChosenCostsItems;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SubCategoriesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CostCategory selectedsubCategory = ((sender as Microsoft.Phone.Controls.ListPicker).SelectedItem) as CostCategory;
                if (selectedsubCategory != null)
                {
                    App.ViewModel.SelectedSubCategory = selectedsubCategory;
                    this.suma1.Text = "Razem: " + App.ViewModel.SumChosenCategory.ToString() + "zł";

                    this.allCostItemsListBox.ItemsSource = App.ViewModel.ChosenCostsItems;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void DatePickerValueChangedTo(object sender, DateTimeValueChangedEventArgs e)
        {
            try
            {

                App.ViewModel.DateTimeTo = (DateTime)e.NewDateTime;

                this.suma1.Text = "Razem: " + App.ViewModel.SumChosenCategory.ToString() + "zł";

                this.allCostItemsListBox.ItemsSource = App.ViewModel.ChosenCostsItems;
            }
            catch (Exception)
            {
            }
        }

        private void DatePickerValueChangedFrom(object sender, DateTimeValueChangedEventArgs e)
        {
            try
            {

                App.ViewModel.DateTimeFrom = (DateTime)e.NewDateTime;

                this.suma1.Text = "Razem: " + App.ViewModel.SumChosenCategory.ToString() + "zł";

                this.allCostItemsListBox.ItemsSource = App.ViewModel.ChosenCostsItems;
            }
            catch (Exception)
            {
            }
        }

    }
}