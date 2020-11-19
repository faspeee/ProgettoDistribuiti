using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Zyzzyva.src.Main.Database.Tables
{
    public class Ordini
    {
        public long id { get; set; }
        public string customer { get; set; }
        public int time { get; set; }
        public int quant { get; set; }
    }
}
