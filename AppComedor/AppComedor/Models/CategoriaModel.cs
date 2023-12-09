using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace AppComedor.Models
{
    public class CategoriaModel:INotifyPropertyChanged
    {
        
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange([CallerMemberName] string nombre="")
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
        public int id_categoria { get; set; }
        public string nombre { get; set; }
    }
}
