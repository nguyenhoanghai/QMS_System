using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Enum
{
    public enum eCounterSoftRequireType
    {
        ReadSound=1,
        PrintTicket=2 ,
        SendSMS = 3,
        CounterEvent = 4,
        SendNextToMainDisplay = 5,
        SendRecallToMainDisplay = 6,
        CheckUserFree = 7,
    }
}
