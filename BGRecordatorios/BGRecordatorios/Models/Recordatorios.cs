using System;
using System.Collections.Generic;
using System.Text;

namespace BGRecordatorios.Models
{
    public class Recordatorios
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public string Foto { get; set; }
        public string Audio { get; set; }
    }
}
