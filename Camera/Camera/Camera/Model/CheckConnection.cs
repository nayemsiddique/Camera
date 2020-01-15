using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Camera.Model
{
   public class  CheckConnection
    {
        public static bool isConnected = CrossConnectivity.Current.IsConnected;
        private CheckConnection()
        {
            //isConnected = CrossConnectivity.Current.IsConnected;
        }
        
    }
}
