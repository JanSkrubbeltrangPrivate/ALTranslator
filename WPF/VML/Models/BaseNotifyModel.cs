using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace VML.Models
{
    public abstract class BaseNotifyModel : INotifyPropertyChanged
    {
        #region INotifyMember
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string PropertyName)
        {
            PropertyChangedEventHandler? handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }
        #endregion
    }
}