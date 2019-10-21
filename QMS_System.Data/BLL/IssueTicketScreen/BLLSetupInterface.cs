using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QMS_System.Data.Model.IssueTicketScreen;

namespace QMS_System.Data.BLL.IssueTicketScreen
{
    public class BLLSetupInterface
    {
        #region constructor
        QMSSystemEntities db;
        static object key = new object();
        private static volatile BLLSetupInterface _Instance;  //volatile =>  tranh dung thread
        public static BLLSetupInterface Instance
        {
            get
            {
                if (_Instance == null)
                    lock (key)
                        _Instance = new BLLSetupInterface();

                return _Instance;
            }
        }
        private BLLSetupInterface() { }
        #endregion
        public List<ButtonServiceModel> GetButtonService(string connectString)
        {
            using (db = new QMSSystemEntities(connectString))
            {
                var list = db.Q_Service.Where(x => !x.IsDeleted).Select(x => new ButtonServiceModel() { Id = x.Id, Name = x.Name }).ToList();
                int i = 1;
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        item.Index = i++;
                    }
                }
                return list;
            }
        }
    }
}
