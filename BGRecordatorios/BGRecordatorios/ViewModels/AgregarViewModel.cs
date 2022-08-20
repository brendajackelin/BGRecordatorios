using System;
using System.Collections.Generic;
using System.Text;
using BGRecordatorios.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Plugin.Media;
using Plugin.Media.Abstractions;
using BGRecordatorios.Services;
using BGRecordatorios.Views;
using Xamarin.Essentials;
using Plugin.AudioRecorder;

namespace BGRecordatorios.ViewModels
{
    public class AgregarViewModel : BaseViewModels
    {
        #region VARIABLES

        private string _Descripcion;
        private DateTime _Fecha;
        private TimeSpan _Hora;
        private string _Foto;
        private string _Audio;
        Image imagen;
        RecordatorioServices services;
        private string opcion;
        private string key;
        private bool _IsImageDefault;
        private bool _IsImageEdit;
        public readonly AudioRecorderService audioRecorderService = new AudioRecorderService();
        public readonly AudioPlayer audioPlayer = new AudioPlayer();
        public string AudioPath, fileName;
        #endregion

        #region OBJETOS
        public string Key
        {
            get { return key; }
            set
            {
                key = value;
                OnPropertyChanged();
            }
        }

        public string Descripcion
        {
            get { return _Descripcion; }
            set
            {
                _Descripcion = value;
                OnPropertyChanged();
            }
        }

        public DateTime Fecha
        {
            get { return _Fecha; }
            set
            {
                _Fecha = value;
                OnPropertyChanged();
            }
        }

        public TimeSpan Hora
        {
            get { return _Hora; }
            set
            {
                _Hora = value;
                OnPropertyChanged();
            }
        }

        public string Foto
        {
            get { return _Foto; }
            set
            {
                _Foto = value;
                OnPropertyChanged();
            }
        }

        public string Audio
        {
            get { return _Audio; }
            set
            {
                _Audio = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageDefault
        {
            get { return _IsImageDefault; }
            set
            {
                _IsImageDefault = value;
                OnPropertyChanged();
            }
        }

        public bool IsImageEdit
        {
            get { return _IsImageEdit; }
            set
            {
                _IsImageEdit = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region MÉTODOS
        public AgregarViewModel(Image imageParam, string opcionReceived, Recordatorios personaReceived)
        {
            imagen = imageParam;
            services = new RecordatorioServices();
            opcion = opcionReceived;

            if (opcion.Equals("Editar"))
            {
                CargarParaEditar(personaReceived);
                IsImageDefault = false;
                IsImageEdit = true;
            }
            else
            {
                IsImageDefault = true;
                IsImageEdit = false;
            }

            TomarFotoCommand = new Command(async () => await TomarFoto());
            GuardarCommand = new Command(() => GuardarRecordatorio());
            ListarCommand = new Command(() => ListarRecordatorio());
            GrabarCommand = new Command(() => GrabarAudio());
            DetenerCommand = new Command(() => DetenerAudio());
        }

        private void CargarParaEditar(Recordatorios ToDoReceived)
        {
            Descripcion = ToDoReceived.Descripcion;
            Fecha = ToDoReceived.Fecha;
            Foto = ToDoReceived.Foto;
            Audio = ToDoReceived.Audio;
            key = ToDoReceived.Id;
        }

        
        private async void GuardarRecordatorio()
        {
            string response = ValidarCampos();
            if (!response.Equals("OK"))
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", response, "Ok");
                return;
            }

            saveAudio();

            DateTime aaa = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, Hora.Hours, Hora.Minutes, Hora.Seconds);

            Recordatorios rec = new Recordatorios()
            {
                Descripcion = Descripcion,
                Fecha = aaa,
                Foto = Foto,
                Audio = AudioPath
            };

            if (opcion.Equals("Editar"))
            {
                // EDITAR
                bool confirm = await services.UpdateRecordatorio(rec, key);
                if (confirm)
                {
                    await Application.Current.MainPage.DisplayAlert("Confirmacion", "Recordatorio actualizado correctamente.", "Ok");
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Confirmacion", "Se produjo un error al actualizar el Recordatorio.", "Ok");
                }
            }
            else
            {
                // GUARDAR
                bool confirm = await services.InsertarRecordatorio(rec);
                if (confirm)
                {
                    await Application.Current.MainPage.DisplayAlert("Confirmacion", "Recordatorio registrado correctamente.", "Ok");
                    Limpiar();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Confirmacion", "Se produjo un error al registrar el Recordatorio.", "Ok");
                }
            }

        }

        private void saveAudio()
        {
            if (audioRecorderService.FilePath != null)
            {

                var stream = audioRecorderService.GetAudioFileStream();

                fileName = Path.Combine(FileSystem.CacheDirectory, DateTime.Now.ToString("ddMMyyyymmss") + "_VoiceNote.wav");

                using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    stream.CopyTo(fileStream);
                }

                AudioPath = fileName;

            }
        }
        
        private void Limpiar()
        {
            Descripcion = "";
            Fecha = DateTime.Now;
            imagen.Source = "photos";
            Foto = "";
            Audio = "";
        }

        private string ValidarCampos()
        { 
            DateTime ahora = DateTime.Now;
            //DateTime fecha = _Fecha;
            TimeSpan hora = _Hora;

            //DateTime _fechahora = fecha + hora;

            if (string.IsNullOrEmpty(Descripcion))
            {
                return "Debe ingresar la descripción";
            }/*else if (_fechahora <= ahora)
            {
                return "No puede seleccionar una fecha y hora anterior";
            }*/
            
            return "OK";
        }

        private async void ListarRecordatorio()
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new Lista());
        }

        private async Task TomarFoto()
        {
            GetImageFromCamera();
        }

        private async void GetImageFromCamera()
        {
            try
            {
                var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Medium,
                });

                if (file == null)
                    return;

                imagen.Source = ImageSource.FromStream(() => { return file.GetStream(); });
                byte[] byteArray = File.ReadAllBytes(file.Path);
                Foto = System.Convert.ToBase64String(byteArray);
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Advertencia", "Se produjo un error al tomar la fotografia.", "Ok");
            }
        }

        private async void GrabarAudio()
        {
            var status = await Permissions.RequestAsync<Permissions.Microphone>();
            var status2 = await Permissions.RequestAsync<Permissions.StorageRead>();
            var status3 = await Permissions.RequestAsync<Permissions.StorageWrite>();
            
            if (status != PermissionStatus.Granted & status2 != PermissionStatus.Granted & status3 != PermissionStatus.Granted)
            {
                return; // si no tiene los permisos no avanza
            }

            if (audioRecorderService.IsRecording)
            {
                await audioRecorderService.StopRecording();
                audioPlayer.Play(audioRecorderService.GetAudioFilePath());
            }
            else
            {
                await audioRecorderService.StartRecording();
            }
        }

        private async void DetenerAudio()
        {
            if (audioRecorderService.IsRecording)
            {
                await audioRecorderService.StopRecording();
                saveAudio();

                if (await Application.Current.MainPage.DisplayAlert("Audio", "¿Desea reproducir el audio?", "SI", "NO"))
                {
                    audioPlayer.Play(audioRecorderService.GetAudioFilePath());
                }
            }
            else
            {
                await audioRecorderService.StartRecording();
            }
        }
        
        #endregion

        #region FUNCIONES
        public ICommand TomarFotoCommand { get; set; }
        public ICommand GuardarCommand { get; set; }
        public ICommand ListarCommand { get; set; }
        public ICommand GrabarCommand { get; set; }
        public ICommand DetenerCommand { get; set; }       
        #endregion
    }
}
