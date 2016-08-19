using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CandyApi.Entity
{
    public class Partida
    {
       

        public int id { set; get; }
        public string mover { set; get; }
        public int puntos { set; get; }
        public int movimientosrestantes { set; get; }
        public string yamovi { set; get; }
            public List<Elemento> elementos { set; get; }
    }
}