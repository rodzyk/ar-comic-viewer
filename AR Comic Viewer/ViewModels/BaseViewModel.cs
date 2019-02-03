using System.Collections.Generic;
using System.ComponentModel;

namespace AR_Comic_Viewer.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        event PropertyChangedEventHandler PropertyChanged;

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add
            {
                PropertyChanged += value;
            }

            remove
            {
                PropertyChanged -= value;
            }
        }

        protected bool SetField<T>(ref T field, T value, string propName)
        {
            // if (EqualityComparer<T>.Default.Equals(field, value)) return false;

            field = value;
            OnPropertyChanged(propName);
            return true;
        }

        public void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }

}
