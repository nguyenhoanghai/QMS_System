using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using QMS_System.Properties;

namespace QMS_System.WebApi
{
  public  class CallServiceApi
    {
      #region constructor
       static HttpClient client;
        static object key = new object();
        private static volatile CallServiceApi _Instance; 
        public static CallServiceApi Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                    {
                        _Instance = new CallServiceApi();
                        client = new HttpClient();
                        client.BaseAddress = new Uri(Settings.Default.ServerUrl);
                    }

                return _Instance;
            }
        }
        private CallServiceApi() { }
        #endregion

        public void ResetDayInfo()
        {
            HttpResponseMessage res = client.GetAsync("api/ServiceApi/ResetDayInfo").Result;
            res.Content.ReadAsStringAsync();
        }
    }
}
