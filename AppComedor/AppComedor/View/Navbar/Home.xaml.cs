using AppComedor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using WebSocketSharp;
using WebSocketSharp.Net;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;

namespace AppComedor.View.Navbar
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public string name { get; set; }
        public string email { get; set; }
        public ImageSource image { get; set; }

        private WebSocket webSocket;
        public Home()
        {
            InitializeComponent();

            LoadCategories();

            ConnectWebSocket();

            LoadCarta();    

        }

        public Home(string NameValue, string EmailValue, ImageSource ImageValue)
        {
            InitializeComponent();
            name = NameValue;
            email = EmailValue;
            image = ImageValue;

            lblName.Text = name;
            lblEmail.Text = email;
            imgProfile.Source = image;

            LoadCategories();
            // Resto del código...
            ConnectWebSocket();
            LoadCarta();
        }

        public async Task LoadCategories()
        {
            HttpClient client = new HttpClient();

            var response = await client.GetStringAsync("https://apiapptesis.up.railway.app/api/categoria");
            var categorias = JsonConvert.DeserializeObject<List<CategoriasAPI>>(response);

            Device.BeginInvokeOnMainThread(() =>
            {
                stackLayout.Children.Clear(); // Limpiar los botones existentes antes de agregar los nuevos

                foreach (var categoria in categorias)
                {
                    Button btnCategoria = new Button
                    {
                        Text = categoria.nombre,
                        CornerRadius = 5,
                        WidthRequest = 120,
                        TextColor = Color.White,
                        TextTransform = TextTransform.None,
                        BackgroundColor = Color.FromHex("#1F86FF"),
                        CommandParameter = categoria.id_categoria
                    };

                    btnCategoria.Clicked += (sender, e) =>
                    {
                        LoadComidasPorCategoria(categoria.nombre);
                    };

                    // Agregar el botón a un contenedor, por ejemplo, un StackLayout
                    stackLayout.Children.Add(btnCategoria);
                }
            });
        }


        public async void LoadCarta()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync("https://apiapptesis.up.railway.app/api/comidas/movil");
            var menus = JsonConvert.DeserializeObject<List<MenuAPI>>(response);

            foreach (var menu in menus)
            {
                Frame unFrame = new Frame
                {
                    Opacity = 1,
                    WidthRequest = 120,
                    HeightRequest = 190,
                    CornerRadius = 8,
                    Margin = new Thickness(0, 0, 5, 0),
                    Padding = new Thickness(5),
                    HasShadow = true

                };

                var interComida = new StackLayout();


                Label labelTitle = new Label
                {
                    Text = menu.nombre,
                    FontSize = 14,
                    TextColor = Color.Black,
                    FontFamily = Device.RuntimePlatform == Device.iOS ? "HelveticaNeue-Bold" : Device.RuntimePlatform == Device.Android ? "sans-serif-light" : null,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                Label labelPrecio = new Label
                {
                    Text = "S/ " + menu.precio,
                    FontSize = 15,
                    TextColor = Color.Black,
                    FontFamily = Device.RuntimePlatform == Device.iOS ? "HelveticaNeue-Bold" : Device.RuntimePlatform == Device.Android ? "sans-serif-light" : null,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                Image image = new Image
                {
                    Source = menu.fileUrl,
                    WidthRequest = 108,
                    HeightRequest = 120,
 
                };

                unFrame.Content = interComida;
                interComida.Children.Add(image);
                interComida.Children.Add(labelTitle);
                interComida.Children.Add(labelPrecio);
                stackComida.Children.Add(unFrame);
            }
        }
        private void ConnectWebSocket()
        {
            webSocket = new WebSocket("wss://apiapptesis.up.railway.app/api/categoria"); // Actualiza la URL del WebSocket según corresponda

            webSocket.OnMessage += async (sender, e) =>
            {
                // Aquí puedes procesar los datos recibidos en tiempo real
                var message = e.Data;

                // Realizar la llamada a la carga de categorías nuevamente
                await LoadCategories();
            };

            webSocket.OnOpen += (sender, e) =>
            {
                // La conexión se ha abierto correctamente
            };

            webSocket.OnClose += (sender, e) =>
            {
                // La conexión se ha cerrado
            };

            webSocket.Connect();
        }

        public void UpdateData(string NameValue, string EmailValue, ImageSource ImageValue)
        {
            lblName.Text = NameValue;
            lblEmail.Text = EmailValue;
            imgProfile.Source = ImageValue;
        }

        public async void LoadComidasPorCategoria(string nombreCategoria)
        {
            HttpClient client = new HttpClient();
            var apiUrl = $"https://apiapptesis.up.railway.app/api/comidas/categoria/{nombreCategoria}";
            var response = await client.GetStringAsync(apiUrl);
            var comidas = JsonConvert.DeserializeObject<List<MenuAPI>>(response);

            stackComida.Children.Clear(); // Limpiar las comidas actuales en el contenedor

            foreach (var comida in comidas)
            {
                Frame unFrame = new Frame
                {
                    Opacity = 1,
                    WidthRequest = 120,
                    HeightRequest = 190,
                    CornerRadius = 8,
                    Margin = new Thickness(0, 0, 5, 0),
                    Padding = new Thickness(5),
                    HasShadow = true

                };
                var interComida = new StackLayout();

                Label labelTitle = new Label
                {
                    Text = comida.nombre,
                    FontSize = 14,
                    TextColor = Color.Black,
                    FontFamily = Device.RuntimePlatform == Device.iOS ? "HelveticaNeue-Bold" : Device.RuntimePlatform == Device.Android ? "sans-serif-light" : null,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                Label labelPrecio = new Label
                {
                    Text = "S/ " + comida.precio,
                    FontSize = 15,
                    TextColor = Color.Black,
                    FontFamily = Device.RuntimePlatform == Device.iOS ? "HelveticaNeue-Bold" : Device.RuntimePlatform == Device.Android ? "sans-serif-light" : null,
                    HorizontalTextAlignment = TextAlignment.Center,
                };

                Image image = new Image
                {
                    Source = comida.fileUrl,
                    WidthRequest = 108,
                    HeightRequest = 120,

                };

                unFrame.Content = interComida;
                interComida.Children.Add(image);
                interComida.Children.Add(labelTitle);
                interComida.Children.Add(labelPrecio);
                stackComida.Children.Add(unFrame);
            }
        }

        private void todopress(object sender, EventArgs e)
        {
            stackComida.Children.Clear();
            LoadCarta();
        }


        public class CategoriasAPI
        {
            public int id_categoria { get; set; }
            public string nombre { get; set; }
            //public DateTime createdAt { get; set; }
            //public DateTime updatedAt { get; set; }
        }


        public class MenuAPI
        {
            public string nombre { get; set; }
            public string precio { get; set; }
            public string fileUrl { get; set; }
        }
    }
}
