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
    public class CostCategory : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Fields and Properties

                // Define ID: private field, public property, and database column.
        private int _id;

        [Column(DbType = "INT NOT NULL IDENTITY", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id
        {
            get { return _id; }
            set
            {
                NotifyPropertyChanging("Id");
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }

        private int _parentId;

        [Column]
        public int ParentId
        {
            get { return _parentId; }
            set
            {
                NotifyPropertyChanging("ParentId");
                _parentId = value;
                NotifyPropertyChanged("ParentId");
            }
        }

        // Define category name: private field, public property, and database column.
        private string _name;

        [Column]
        public string Name
        {
            get { return _name; }
            set
            {
                NotifyPropertyChanging("Name");
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        // Define the entity set for the collection side of the relationship.
        private EntitySet<CostItem> _costs;

        [Association(Storage = "_costs", OtherKey = "_categoryId", ThisKey = "Id")]
        public EntitySet<CostItem> Costs
        {
            get { return this._costs; }
            set { this._costs.Assign(value); }
        }

        // Assign handlers for the add and remove operations, respectively.
        public CostCategory()
        {
            _costs = new EntitySet<CostItem>(
                new Action<CostItem>(this.attach_Cost),
                new Action<CostItem>(this.detach_Cost)
                );
        }

        // Called during an add operation
        private void attach_Cost(CostItem cost)
        {
            NotifyPropertyChanging("CostItem");
            cost.Category = this;
        }

        // Called during a remove operation
        private void detach_Cost(CostItem cost)
        {
            NotifyPropertyChanging("CostItem");
            cost.Category = null;
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
