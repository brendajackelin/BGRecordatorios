using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using BGRecordatorios.Models;
using BGRecordatorios.Services;
using BGRecordatorios.Views;
using Plugin.AudioRecorder;

namespace BGRecordatorios.ViewModels
{
    public class ListaViewModel : BaseViewModels
    {
        #region VARIABLES
        private List<Recordatorios> _ListaRecordatorios;
        RecordatorioServices recordatoriosServices;
        private readonly AudioPlayer audioPlayer = new AudioPlayer();
        #endregion

        #region OBJETOS
        public List<Recordatorios> ListaRecordatorios
        {
            get { return _ListaRecordatorios; }
            set
            {
                _ListaRecordatorios = value;
                OnPropertyChanged();
            }
        }

        public ListaViewModel()
        {
            recordatoriosServices = new RecordatorioServices();

            EditarRecordatorioCommand = new Command<Recordatorios>(async (Recor) => await EditarRecordatorio(Recor));
            EliminarRecordatorioCommand = new Command<Recordatorios>(async (Recor) => await EliminarRecordatorio(Recor));
            PlayAudioCommand = new Command<Recordatorios>(async (Recor) => await PlayAudio(Recor));
        }

        #endregion

        #region MÉTODOS
        private async Task EliminarRecordatorio(Recordatorios r)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Advertencia", "¿Esta seguro de eliminar a " + r.Descripcion + "?", "Si", "No");

            if (confirm)
            {
                bool response = await recordatoriosServices.DeleteRecordatorio(r.Id);
                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Recordatorio Eliminado Correctamente", "Ok");
                    CargarDatos();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "Se produjo un error al eliminar el Recordatorio", "Ok");
                }
            }
        }

        private async Task EditarRecordatorio(Recordatorios r)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Agregar("Editar", r));
        }

        private async Task PlayAudio(Recordatorios r)
        {
            audioPlayer.Play(r.Audio);
        }

        public async void CargarDatos()
        {
            try
            {
                ListaRecordatorios = await recordatoriosServices.ListarRecordatorio();
                if (ListaRecordatorios.Count == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Advertencia", "No hay recordatorios registrados", "Ok");
                }
            }
            catch (Exception error)
            {

                
            }
        }

        #endregion

        #region FUNCIONES
        public ICommand EditarRecordatorioCommand { get; set; }
        public ICommand EliminarRecordatorioCommand { get; set; }
        public ICommand PlayAudioCommand { get; set; }
        #endregion
    }
}
