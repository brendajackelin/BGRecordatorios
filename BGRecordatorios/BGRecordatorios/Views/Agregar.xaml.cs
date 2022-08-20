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
        TimeSpan max;
        String Opcion;
        Recordatorios recordatorio;

        public Agregar(string opcion = "Guardar", Recordatorios r = null)
        {
            InitializeComponent();
            Opcion = opcion;
            recordatorio = r;


            if (opcion.Equals("Guardar"))
            {
                BindingContext = new AgregarViewModel(image, opcion, r);
            }
            else
            {
                BindingContext = new AgregarViewModel(image2, opcion, r);
            }
        }

        protected override void OnAppearing()
        {
            Fecha.MinimumDate = DateTime.Now;
            max = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            
            if (Opcion != "Editar")
            {
                
                Fecha.Date = DateTime.Now;
                Hora.Time = max;
            }
            else
            {
                var _fecha = recordatorio.Fecha;
                TimeSpan hora = new TimeSpan(_fecha.Hour, _fecha.Minute, _fecha.Second);

                Hora.Time = hora;
            }

        }
        /*
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
                if (opcion.Equals("Guardar"))
                {
                    BindingContext = new AgregarViewModel(image, opcion, r);
                }
                else
                {
                    BindingContext = new AgregarViewModel(image2, opcion, r);
                }
            }
        }*/

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

        private void Hora_Unfocused(object sender, FocusEventArgs e)
        {
            validarHora();
        }

        private void Fecha_Unfocused(object sender, FocusEventArgs e)
        {
            validarHora();
        }

        void validarHora()
        {
            if (Fecha.Date <= DateTime.Now)
            {
                max = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if (Hora.Time.Ticks <= max.Ticks)
                {
                    DisplayAlert("Aviso", "Debe seleccionar una hora diferente", "OK");
                    Hora.Time = max;
                    return;
                }
            }
        }

        
    }
}
