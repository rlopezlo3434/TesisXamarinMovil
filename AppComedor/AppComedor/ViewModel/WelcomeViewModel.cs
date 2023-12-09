using AppComedor.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppComedor.ViewModel
{
	public class WelcomeViewModel : ViewModelBase
    {
        public WelcomeViewModel(INavigation navigation)
        {
            Navigation = navigation;
        }

        public async Task NavegarInicio()
        {
            await Navigation.PushAsync(new View.Navbar.ContainerTabbedPage());
        }

        public ICommand InicioCommand => new Command(async () => await NavegarInicio());
    }
}