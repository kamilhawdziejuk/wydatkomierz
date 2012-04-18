using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using Wydatki.Model;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Wydatki.ViewModel
{
    public class CostViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local database.
        private CostDataContext costDoDB;

        // Class constructor, create the data context object.
        public CostViewModel(string toDoDBConnectionString)
        {
            costDoDB = new CostDataContext(toDoDBConnectionString);
        }

        private bool addingMoney = false;
        public bool AddingMoney
        {
            get
            {
                return this.addingMoney;
            }
            set
            {
                this.addingMoney = value;
            }
        }

        // All to-do items.
        private ObservableCollection<CostItem> _allCostItems;
        public ObservableCollection<CostItem> AllCostItems
        {
            get 
            { 
                return new ObservableCollection<CostItem>( from cost in _allCostItems orderby cost.DateTime ascending select cost);
            }
            set
            {
                _allCostItems = value;
                NotifyPropertyChanged("AllCostItems");
            }
        }

        public string AllCosts
        {
            get
            {
                double sum = 0;
                foreach (CostItem ci in this.AllCostItems)
                {
                    double d = Double.Parse(ci.ItemName, CultureInfo.InvariantCulture);
                    sum += d;
                }
                return sum + " zł";
            }
        }

        public string Title
        {
            get
            {
                if (this.AddingMoney)
                {
                    return "Nowy przychód";
                }
                else
                {
                    return "Nowy wydatek";
                }
            }
        }

        public string DateNowStr
        {
            get
            {
                DateTime dt = this.DateTimeSetted;
                return dt.Month + "/" +dt.Day + "/" + dt.Year;
            }
        }


        private DateTime dateTimeSetted = DateTime.Now;
        public DateTime DateTimeSetted
        {
            get
            {
                return this.dateTimeSetted;
            }
            set
            {
                this.dateTimeSetted = value;
            }
        }

        // A list of all categories, used by the add task page.
        private List<CostCategory> _categoriesList;
        public List<CostCategory> CategoriesList
        {
            get { return _categoriesList; }
            set
            {
                _categoriesList = value;
                NotifyPropertyChanged("CategoriesList");
            }
        }

        public List<CostCategory> NadCategoriesList
        {
            get
            {
                //return _categoriesList;
                List<CostCategory> results = new List<CostCategory>();
                foreach (CostCategory cc in this.CategoriesList)
                {
                    if (cc.ParentId == 0)
                    {
                        results.Add(cc);
                    }
                }
                return results;
            }
        }

        public List<CostCategory> NadCategoriesListAll
        {
            get
            {
                //return _categoriesList;
                List<CostCategory> results = new List<CostCategory>();
                CostCategory costAllCategory = new CostCategory { Name = "Wszystkie" };
                results.Add(costAllCategory);
                foreach (CostCategory cc in this.CategoriesList)
                {
                    if (cc.ParentId == 0)
                    {
                        results.Add(cc);
                    }
                }

                return results;
            }
        }

        private CostCategory selectedNadCategory = null;
        public CostCategory SelectedNadCategory
        {
            get
            {
                return selectedNadCategory;
            }
            set
            {
                this.selectedNadCategory = value;
            }
        }

        private CostCategory selectedSubCategory = null;
        public CostCategory SelectedSubCategory
        {
            get
            {
                return selectedSubCategory;
            }
            set
            {
                this.selectedSubCategory = value;
            }
        }

        /// <summary>
        /// Suma wszystkich kategorii aktywnie wybranej nadkategorii
        /// </summary>
        public double SumChosenCategory
        {
            get
            {
                double sum = 0;

                if (App.ViewModel.SelectedNadCategory != null && App.ViewModel.SelectedNadCategory.Name.Equals("Wszystkie"))
                {
                    foreach (CostItem ci in this.AllCostItems)
                    {
                        double d = Double.Parse(ci.ItemName, CultureInfo.InvariantCulture);
                        sum += d;
                    }
                }
                //wybrano tylko kategorie nadrzedna:
                else if (App.ViewModel.SelectedSubCategory == null || App.ViewModel.SelectedSubCategory.Name.Equals("Wszystkie"))
                {
                    if (App.ViewModel.SelectedNadCategory != null)
                    {
                        foreach (CostItem costItem in App.ViewModel.AllCostItems)
                        {
                            if (costItem.Category.ParentId == App.ViewModel.SelectedNadCategory.Id)
                            {
                                sum += Double.Parse(costItem.ItemName, CultureInfo.InvariantCulture);
                            }
                        }
                    }
                }
                else
                {
                    foreach (CostItem costItem in App.ViewModel.AllCostItems)
                    {
                        if (costItem.Category.Id == App.ViewModel.SelectedSubCategory.Id)
                        {
                            sum += Double.Parse(costItem.ItemName, CultureInfo.InvariantCulture);
                        }
                    }
                }

                return sum;
            }
        }

        public List<CostCategory> SubCategoriesList
        {
            get 
            { 
                //return _categoriesList;
                List<CostCategory> results = new List<CostCategory>();
                if (this.selectedNadCategory != null)
                {
                    foreach (CostCategory cc in this.CategoriesList)
                    {
                        if (cc.ParentId == this.selectedNadCategory.Id)
                        {
                            results.Add(cc);
                        }
                    }
                }
                return results;
            }
        }

        public List<CostCategory> SubCategoriesListWithAll
        {
            get
            {
                //return _categoriesList;
                List<CostCategory> results = new List<CostCategory>();
                CostCategory costAllCategory = new CostCategory { Name = "Wszystkie" };
                results.Add(costAllCategory);
                if (this.selectedNadCategory != null && !this.selectedNadCategory.Name.Equals("Wszystkie"))
                {
                    foreach (CostCategory cc in this.CategoriesList)
                    {
                        if (cc.ParentId == this.selectedNadCategory.Id)
                        {
                            results.Add(cc);
                        }
                    }
                }

                return results;
            }
        }


        // Write changes in the data context to the database.
        public void SaveChangesToDB()
        {
            costDoDB.SubmitChanges();
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all to-do items in the database.
            var costItemsInDB = from CostItem todo in costDoDB.Items
                                select todo;

            // Query the database and load all to-do items.
            AllCostItems = new ObservableCollection<CostItem>(costItemsInDB);

            // Specify the query for all categories in the database.
            var toDoCategoriesInDB = from CostCategory category in costDoDB.Categories
                                     select category;


            // Query the database and load all associated items to their respective collections.
            /*foreach (ToDoCategory category in toDoCategoriesInDB)
            {
                switch (category.Name)
                {
                    case "Home":
                        HomeToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "Work":
                        WorkToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    case "Hobbies":
                        HobbiesToDoItems = new ObservableCollection<ToDoItem>(category.ToDos);
                        break;
                    default:
                        break;
                }
            }*/

            // Load a list of all categories.
            CategoriesList = costDoDB.Categories.ToList();

        }

        // Remove a to-do task item from the database and collections.
        public void DeleteCostItem(CostItem costForDelete)
        {

            // Remove the to-do item from the "all" observable collection.
            AllCostItems.Remove(costForDelete);

            // Remove the to-do item from the data context.
            costDoDB.Items.DeleteOnSubmit(costForDelete);

            // Remove the to-do item from the appropriate category.   
           /* switch (costForDelete.Category.Name)
            {
                case "Home":
                    HomeToDoItems.Remove(toDoForDelete);
                    break;
                case "Work":
                    WorkToDoItems.Remove(toDoForDelete);
                    break;
                case "Hobbies":
                    HobbiesToDoItems.Remove(toDoForDelete);
                    break;
                default:
                    break;
            }*/

            // Save changes to the database.
            costDoDB.SubmitChanges();
        }

        // Add a to-do item to the database and collections.
        public void AddCostItem(CostItem newCostItem)
        {
            // Add a to-do item to the data context.
            costDoDB.Items.InsertOnSubmit(newCostItem);

            // Save changes to the database.
            costDoDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            AllCostItems.Add(newCostItem);

            // Add a to-do item to the appropriate filtered collection.
            /*switch (newCostItem.Category.Name)
            {
                case "Home":
                    HomeToDoItems.Add(newToDoItem);
                    break;
                case "Work":
                    WorkToDoItems.Add(newToDoItem);
                    break;
                case "Hobbies":
                    HobbiesToDoItems.Add(newToDoItem);
                    break;
                default:
                    break;
            }*/
        }


        #endregion
    }
}
