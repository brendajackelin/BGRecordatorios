using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using BGRecordatorios.Models;
using BGRecordatorios.ViewModels;

namespace BGRecordatorios
{
    public partial class Agregar : ContentPage
    {
        public Agregar(string opcion = "Guardar", Recordatorios r = null)
        {
            InitializeComponent();
            if (opcion.Equals("Guardar"))
            {
                BindingContext = new AgregarViewModel(image, opcion, r);
            }
            else
            {
                BindingContext = new AgregarViewModel(image2, opcion, r);
            }
        }
        //Command="{Binding GuardarCommand}"


        /*
         
                <Image  x:Name="image"
                    BackgroundColor="#fce6ea"
                    Source="photos"
                    Aspect="AspectFit"
                    HorizontalOptions="FillAndExpand" 
                    WidthRequest="300"
                    HeightRequest="220"
                    IsVisible="{Binding IsImageDefault}">

                    <Image.GestureRecognizers>
                        <TapGestureRecognizer Command="{Binding TomarFotoCommand}"/>
                    </Image.GestureRecognizers>

                </Image>
        */




        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            DateTime ahora = DateTime.Now;
            DateTime fecha = Fecha.Date;
            TimeSpan hora = Hora.Time;

            DateTime _fechahora = fecha + hora;

            if (_fechahora < ahora)
            {
                await DisplayAlert("Notificación", "No puede seleccionar una fecha y ahora anterior", "OK");
            }
            else
            {
                await DisplayAlert("Notificación", "fecha y ahora válidas", "OK");
            }
        }

        private async void btnGrabar_Clicked(object sender, EventArgs e)
        {
            btnGrabar.IsEnabled = false;
            btnDetener.IsEnabled = true;
        }

        private async void btnDetener_Clicked(object sender, EventArgs e)
        {
            btnGrabar.IsEnabled = true;
            btnDetener.IsEnabled = false;
        }
    }
}
