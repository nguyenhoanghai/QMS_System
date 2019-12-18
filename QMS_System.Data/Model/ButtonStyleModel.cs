using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QMS_System.Data.Model
{
    public class ButtonStyleModel
    {
        public int Height { get; set; }
        public int Width { get; set; }
        public int Margin { get; set; } 
        public String fontStyle { get; set; }
        public String BackColor { get; set; }
        public String ForeColor { get; set; }
        public int ButtonInRow { get; set; }
    }
}
