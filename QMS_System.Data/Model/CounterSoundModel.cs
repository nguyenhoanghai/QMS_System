 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QMS_System.Data.Model
{
  public  class CounterSoundModel
    {
        public int Id { get; set; }
        public int CounterId { get; set; }
        public int LanguageId { get; set; }
        public string SoundName { get; set; }
        public string Note { get; set; } 
    }
} 
