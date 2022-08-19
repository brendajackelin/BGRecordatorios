using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using BGRecordatorios.ViewModels;
using BGRecordatorios.Models;

namespace BGRecordatorios.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Lista : ContentPage
    {
        ListaViewModel listaViewModel;
        public Lista()
        {
            try
            {
                InitializeComponent();
                listaViewModel = new ListaViewModel();
                BindingContext = listaViewModel;
            }
            catch (Exception error)
            {

            }
            
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                listaViewModel.CargarDatos();
            }
            catch (Exception error)
            {

                
            }
            
        }


       
    }
}