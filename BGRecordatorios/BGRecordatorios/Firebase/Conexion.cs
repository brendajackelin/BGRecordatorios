using System;
using System.Collections.Generic;
using Firebase.Database;
using System.Text;

namespace BGRecordatorios.Firebase
{
	public class Conexion
	{
                
		 public static FirebaseClient firebase = new FirebaseClient("https://recordatorios-be737-default-rtdb.firebaseio.com/");

    }
}
