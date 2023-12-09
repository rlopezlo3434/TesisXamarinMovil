using AppComedor.Models;
using AppComedor.View.Navbar;
using AppComedor.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppComedor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly IGoogleManager _googleManager;
        GoogleUser GoogleUser = new GoogleUser();
        public bool IsLogedIn { get; set; }

        public string NameValue { get; set; }
        public string EmailValue { get; set; }

        public ImageSource ImageValue { get; set; }
        public MainPage()
        {
            _googleManager = DependencyService.Get<IGoogleManager>();
            CheckUserLoggedIn();
            InitializeComponent();

            var viewModel = new MainViewModel(Navigation);
            BindingContext = viewModel;
        }

        private void CheckUserLoggedIn()
        {
            _googleManager.Login(OnLoginComplete);
        }

        private void btnLogin_Clicked(object sender, EventArgs e)
        {
            _googleManager.Logout();
            _googleManager.Login(OnLoginComplete);
        }

        private async void OnLoginComplete(GoogleUser googleUser, string message)
        {
            if (googleUser != null && googleUser.Email.EndsWith("@tecsup.edu.pe"))
            {
                GoogleUser = googleUser;
                NameValue = GoogleUser.Name;
                EmailValue = GoogleUser.Email;
                ImageValue = ImageSource.FromUri(new Uri(GoogleUser.Picture.ToString()));


                var containerTabbedPage = new ContainerTabbedPage();

                var homePage = containerTabbedPage.Children.FirstOrDefault() as NavigationPage;
                if (homePage != null && homePage.RootPage is Home home)
                {
                    home.name = NameValue;
                    home.email = EmailValue;
                    home.image = ImageValue;

                    home.UpdateData(NameValue, EmailValue, ImageValue);
                }

                await Navigation.PushAsync(containerTabbedPage);
                // Resto del código
            }
            else
            {
                await DisplayAlert("Message", "Only @tecsup.edu.pe emails are allowed.", "Ok");
            }
        }

        private void GoogleLogout()
        {
            _googleManager.Logout();
            IsLogedIn = false;
        }

        private void btnLogout_Clicked(object sender, EventArgs e)
        {
            _googleManager.Logout();

            //txtName.Text = "Name :";
            //txtEmail.Text = "Email: ";
            //imgProfile.Source = "";
        }
    }
}