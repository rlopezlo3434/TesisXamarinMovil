using AppComedor.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppComedor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public async Task NavegarWelcome()
        {
            await Navigation.PushAsync(new Welcome());
        }

        public ICommand NavegarCommand => new Command(async () => await NavegarWelcome());

        
    }


}