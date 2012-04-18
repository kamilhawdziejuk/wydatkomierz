/* 
    Copyright (c) 2011 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://go.microsoft.com/fwlink/?LinkID=219604 
  
*/
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

// Directive for the data model.
using Wydatki.Model;
using Wydatki;

namespace sdkLocalDatabaseCS
{
    public partial class NewTaskPage : PhoneApplicationPage
    {
        public NewTaskPage()
        {
            InitializeComponent();

            // Set the page DataContext property to the ViewModel.
            this.DataContext = App.ViewModel;
        }

        /// <summary>
        /// Dodawanie nowego wydatku
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void appBarOkButton_Click(object sender, EventArgs e)
        {
            // Confirm there is some text in the text box.
            if (newTaskNameTextBox.Text.Length > 0)
            {
                // Create a new to-do item.
                CostItem newToDoItem = new CostItem
                {
                    ItemName = newTaskNameTextBox.Text,
                    Category = (CostCategory)this.categoriesListPicker2.SelectedItem,
                    DateTime = (DateTime)this.dataPicker.Value
                };

                // Add the item to the ViewModel.
                App.ViewModel.AddCostItem(newToDoItem);

                // Return to the main page.
                if (NavigationService.CanGoBack)
                {
                    NavigationService.GoBack();
                }
            }
        }

        private void appBarCancelButton_Click(object sender, EventArgs e)
        {
            // Return to the main page.
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        private void DatePickerValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            try
            {

                App.ViewModel.DateTimeSetted = (DateTime)e.NewDateTime;
            }
            catch (Exception)
            {
            }
        }

        private void NadCategoriesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CostCategory selectedNadCategory = ((sender as Microsoft.Phone.Controls.ListPicker).SelectedItem) as CostCategory;
                if (selectedNadCategory != null)
                {
                    App.ViewModel.SelectedNadCategory = selectedNadCategory;
                    this.categoriesListPicker2.ItemsSource = App.ViewModel.SubCategoriesList;

                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
