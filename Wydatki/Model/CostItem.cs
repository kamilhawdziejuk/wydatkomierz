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
using System.Data.Linq.Mapping;
using System.ComponentModel;
using System.Data.Linq;

namespace Wydatki.Model
{
    [Table]
    public class CostItem : INotifyPropertyChanged, INotifyPropertyChanging
    {

        #region Fields and Properies

        // Define ID: private field, public property, and database column.
        private int _costItemId;

        [Column(IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity", CanBeNull = false, AutoSync = AutoSync.OnInsert)]
        public int CostItemId
        {
            get { return _costItemId; }
            set
            {
                if (_costItemId != value)
                {
                    NotifyPropertyChanging("CostItemId");
                    _costItemId = value;
                    NotifyPropertyChanged("CostItemId");
                }
            }
        }

        // Define item name: private field, public property, and database column.
        private string _itemName;

        [Column]
        public string ItemName
        {
            get { return _itemName; }
            set
            {
                if (_itemName != value)
                {
                    NotifyPropertyChanging("ItemName");
                    _itemName = value;
                    NotifyPropertyChanged("ItemName");
                }
            }
        }

        public string ItemNameZL
        {
            get
            {
                return ItemName + " zł";
            }
        }

        public string CategoryName
        {
            get
            {
                return _category.Entity.Name;
            }
        }

        public string CategoryDesc
        {
            get
            {
                CostCategory costCatNad = null;
                foreach (CostCategory ccat in App.ViewModel.CategoriesList)
                {
                    if (ccat.Id == _category.Entity.ParentId)
                    {
                        costCatNad = ccat;
                        break;
                    }
                }

                string nameNad = costCatNad != null ? costCatNad.Name : "";
                return nameNad + "->" + _category.Entity.Name;
            }
        }

        // Define completion value: private field, public property, and database column.
        private bool _isComplete;

        [Column]
        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                if (_isComplete != value)
                {
                    NotifyPropertyChanging("IsComplete");
                    _isComplete = value;
                    NotifyPropertyChanged("IsComplete");
                }
            }
        }

        // Define completion value: private field, public property, and database column.
        private DateTime _dateTime;

        [Column]
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                if (_dateTime != value)
                {
                    NotifyPropertyChanging("DateTime");
                    _dateTime = value;
                    NotifyPropertyChanged("DateTime");
                }
            }
        }

        public string DateTimeStr
        {
            get
            {
                return  this.DateTime.Day + "." + this.DateTime.Date.Month + "." + this.DateTime.Date.Year;
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;


        // Internal column for the associated ToDoCategory ID value
        [Column]
        internal int _categoryId;

        // Entity reference, to identify the ToDoCategory "storage" table
        private EntityRef<CostCategory> _category;

        // Association, to describe the relationship between this key and that "storage" table
        [Association(Storage = "_category", ThisKey = "_categoryId", OtherKey = "Id", IsForeignKey = true)]
        public CostCategory Category
        {
            get { return _category.Entity; }
            set
            {
                NotifyPropertyChanging("Category");
                _category.Entity = value;

                if (value != null)
                {
                    _categoryId = value.Id;
                }

                NotifyPropertyChanging("Category");
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion
    }
}
