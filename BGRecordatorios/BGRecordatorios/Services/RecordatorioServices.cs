using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;
using BGRecordatorios.Firebase;
using BGRecordatorios.Models;
using Firebase.Database.Query;

namespace BGRecordatorios.Services
{
    public class RecordatorioServices
    {
        public async Task<bool> InsertarRecordatorio(Recordatorios ToDo)
        {
            bool response = false;
            try
            {
                await Conexion.firebase
                .Child("Recordatorios")
                .PostAsync(new Recordatorios()
                {
                    Descripcion = ToDo.Descripcion,
                    Fecha = ToDo.Fecha,
                    Foto = ToDo.Foto,
                    Audio = ToDo.Audio
                });
                response = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }

        public async Task<List<Recordatorios>> ListarRecordatorio()
        {
            try
            {
                var data = (await Conexion.firebase
                            .Child("Recordatorios")
                            .OnceAsync<Recordatorios>()).Select(item => new Recordatorios
                            {
                                Id = item.Key, // ID
                                Descripcion = item.Object.Descripcion,
                                //Fecha = item.Object.Fecha,
                                Foto = item.Object.Foto,
                                Audio = item.Object.Audio
                            }).ToList();

                return data;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public async Task<bool> DeleteRecordatorio(string key)
        {
            bool response = false;
            try
            {
                await Conexion.firebase.Child("Recordatorios").Child(key).DeleteAsync();
                response = true;
            }
            catch (Exception ex)
            {
                response = false;
                Debug.WriteLine(ex.Message);
            }
            return response;
        }

        public async Task<bool> UpdateRecordatorio(Recordatorios ToDo, string key)
        {
            bool response = false;
            try
            {
                await Conexion.firebase
                              .Child("Recordatorios")
                              .Child(key)
                              .PutAsync(ToDo);
                response = true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                response = false;
            }
            return response;
        }
    }
}
