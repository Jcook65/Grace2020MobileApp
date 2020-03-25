using Grace2020.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.ViewModels.Instances
{
    public class SelectableItemVM : VMBase
    {
        private object _item;
        public object Item
        {
            get { return _item; }
            set 
            { 
                Set(() => Item, ref _item, value);
                if(Item != null)
                {
                    ItemSelected?.Invoke(this, Item);
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { Set(() => IsSelected, ref _isSelected, value); }
        }

        public event EventHandler<object> ItemSelected;

        public SelectableItemVM(object item)
        {
            _item = item;
        }
    }
}
