using CandyApi.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using static CandyApi.Entity.Elemento;

namespace CandyApi.Controllers
{
    public class PartidaController : ApiController
    {
        static Partida oPartida;
        static List<Elemento> listElementos;
        static int mov1=0, mov2=0;
        static bool cambio,cambio2;
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // POST api/Partida
        public async Task<IHttpActionResult> PostPartida([FromBody] Usuario usuario)
        {
            Array values = Enum.GetValues(typeof(Color));
            Random random = new Random();
            var color = Color.red;

            listElementos = new List<Elemento>();
        
            for (int i = 0; i < 81; i++)
            {
                color = (Color)values.GetValue(random.Next(values.Length));
                
                listElementos.Add(new Elemento { id = i, color = color});
            }
           oPartida = new Partida();
            oPartida.id = 1;
            oPartida.elementos = listElementos;
            oPartida.movimientosrestantes = 2;
            return Json(oPartida);
        }
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // Put api/Partida
        public async Task<IHttpActionResult> PutPartida(int id, 
            [FromBody] UsuarioMovimientos usuarioMovimientos)
        {
           oPartida.movimientosrestantes = oPartida.movimientosrestantes-1;
            mov1 = usuarioMovimientos.movimiento1;
            mov2 = usuarioMovimientos.movimiento2;
            //Logica de negocios
            if (NullMove(usuarioMovimientos.movimiento1, usuarioMovimientos.movimiento2))
            {
                oPartida.mover = "no";
            }
            else
            {
                var move = listElementos.First(x => x.id == usuarioMovimientos.movimiento1);
                var col = move.color;
                var move2 = listElementos.First(x => x.id == usuarioMovimientos.movimiento2);
                var col2 = move2.color;
                listElementos[usuarioMovimientos.movimiento2].color = col;
                listElementos[usuarioMovimientos.movimiento1].color = col2;
                oPartida.mover = "si";
            }
            if (oPartida.movimientosrestantes == 0)
            {
                return Json(oPartida);
            }
            oPartida.elementos = listElementos.OrderBy(x => x.id).ToList();
            return Json(oPartida);
        }


        [EnableCors(origins: "*", headers: "*", methods: "*")]
        // GET api/Partida
        public async Task<IHttpActionResult> GetPartida(int mover, 
            [FromBody] Usuario usuario)
        {
            //Logica de negocios
            Thread.Sleep(100);
               cambio=RealizarMovHorizontal(mover);
               cambio2=RealizarMovVertical(mover);
            oPartida.elementos = listElementos.OrderBy(x => x.id).ToList();
            if (cambio&&cambio2)
            {
                oPartida.mover = "no";
                RevertirMov();
            }else
            {
                oPartida.mover = "si";
                mov1 = 0;
                mov2 = 0;
            }
             return Json(oPartida);
        }

        private bool RealizarMovHorizontal(int mover)
        {
            int posicion = 0,cont=0,suma=0;
            bool salir=false,cambio=false;
           
          
            for (int i = 0; i < 81; i++)
                {
                    posicion = i + 1;
                    cont = 0;
                    salir = false;
                    while (!salir)
                    {
                        if (i == 7)
                        {
                            i = 9;
                            posicion = i + 1;
                        }
                        if (i == 16)
                        {
                            i = 18;
                            posicion = i + 1;
                        }
                        if (i == 25)
                        {
                            i = 27;
                            posicion = i + 1;
                        }
                        if (i == 34)
                        {
                            i = 36;
                            posicion = i + 1;
                        }
                        if (i == 43)
                        {
                            i = 45;
                            posicion = i + 1;
                        }
                        if (i == 52)
                        {
                            i = 54;
                            posicion = i + 1;
                        }
                        if (i == 61)
                        {
                            i = 63;
                            posicion = i + 1;
                        }
                        if (i == 70)
                        {
                            i = 72;
                            posicion = i + 1;
                        }
                        if (i == 79)
                        {
                            i = 81;
                            break;
                        }
                        else
                        {
                            if (listElementos[i].color == listElementos[posicion].color)
                            {
                            
                                cont++;
                                 posicion++;
                                if (cont == 2)
                                {
                                suma = cont;
                                if (posicion==8||posicion==17 || posicion == 26 || posicion == 35 || posicion == 44
                                    || posicion == 53 || posicion == 62 || posicion == 71 || posicion == 80)
                                {
                                    BajarCandies(i, posicion-1);
                                    oPartida.puntos = oPartida.puntos + 60;
                                }
                                else
                                {
                                    if (listElementos[i].color == listElementos[posicion].color)
                                    {
                                        posicion++;
                                        BajarCandies(i, posicion - 1);
                                        oPartida.puntos = oPartida.puntos + 120;
                                    }
                                    else
                                    {
                                        BajarCandies(i, posicion - 1);
                                        oPartida.puntos = oPartida.puntos + 60;
                                    }
                                    i = -1;
                                    salir = true;
                                    cambio = false;
                                }
                            }
                                
                            }
                            else
                            {
                                cambio = false;
                                salir = true;
                            }
                        }
                    }
                }
            if (mover==1)
            {

                if (suma>0)
                {
                    cambio = false;
                }else
                {
                   
                    cambio = true;
                }
            }
            return cambio;
        }

        private void RevertirMov()
        {
            if (!NullMove(mov1, mov2))
            {
                var color1 = Color.red;
                var color2 = Color.red;
                color1 = listElementos[mov1].color;
                color2 = listElementos[mov2].color;
                listElementos[mov2].color = color1;
                listElementos[mov1].color = color2;
                mov1 = 0;
                mov2 = 0;
                oPartida.elementos = listElementos.OrderBy(x => x.id).ToList();
            }
        }
        private bool RealizarMovVertical(int mover)
        {
            int posicion = 0, cont = 0,suma=0;
            bool salir = false, cambio = false;
          
            for (int i = 0; i < 81; i++)
                {
                    posicion = i + 9;
                    cont = 0;
                    salir = false;
                    if (i == 63)
                    {
                        break;
                    }
                    while (!salir)
                    {
                        if (listElementos[i].color == listElementos[posicion].color)
                        {
                       
                        cont++;
                            if (cont == 2)
                        {
                           suma = cont;
                            if (posicion == 72 || posicion == 73 || posicion == 74 || posicion == 75 || posicion == 76
                                    || posicion == 77 || posicion == 78 || posicion == 79 || posicion == 80)
                            {
                                BajarCandiesv(i, posicion);
                                oPartida.puntos = oPartida.puntos + 60;
                            }
                            else
                            {
                                if (listElementos[i].color == listElementos[posicion + 9].color)
                                {
                                    posicion++;
                                    BajarCandiesv(i, posicion - 1);
                                    oPartida.puntos = oPartida.puntos + 120;
                                }
                                else
                                {
                                    BajarCandiesv(i, posicion);
                                    oPartida.puntos = oPartida.puntos + 60;
                                }
                            }
                          
                            i = -1;
                                salir = true;
                                cambio = true;
                            }
                            posicion = posicion + 9;
                        }
                        else
                    {
                        cambio = false;
                            salir = true;
                        }
                    }
            }
            if (mover == 1)
            {
                if (suma>0)
                {
                    cambio = false;
                }
                else
                {
                    cambio = true;
                }
            }
                return cambio;
        }
        /// <summary>
        /// metodo para bajar las candies 
        /// </summary>
        /// <param name="i">indica la posicion de inicio donde se van a bajar los candies</param>
        /// <param name="v">indica la posicion final donde se van a bajar los candies</param>
        private void BajarCandies(int i, int v)
        {
            if (i == 0 || i == 1 || i == 2 || i == 3 ||
                             i == 4 || i == 5 || i == 6 || i == 7 || i == 8)
            {
               
                listElementos[i].tipodecolor = "cambiar";
                listElementos[i + 1].tipodecolor = "cambiar";
                listElementos[i + 2].tipodecolor = "cambiar";
                if (i + 3 == v)
                {
                    listElementos[i + 3].tipodecolor = "cambiar";
                }
            }
            if (i < 7)
            {
                CambiarCandy(i,v);
            }else
            {
                int x = i,y=i,cont=0,c=i;
                for (int h = i; h < v+1; h++)
                {
                    c = h;
                    cont = 0;
                    x = h;
                    y = h;
                    while (c>0)
                    {
                        if (c == 0 || c == 1 || c == 2 || c == 3|| 
                            c == 4 || c == 5 || c == 6 || c == 7 || c == 8)
                        {
                            break;
                        }else
                        {
                            c = c - 9;
                            cont++;
                        }
                    }
                    while (cont > 0)
                    {
                        x = x - 9;
                        listElementos[y].color = listElementos[x].color;
                        cont--;
                        y = y - 9;
                    }
                    if (cont == 0)
                    {
                        while (y > -1)
                        {
                            listElementos[y].tipodecolor="cambiar";
                            y = y - 9;
                        }
                    }
                }
                CambiarCandy(i, v);
            }
        }
        private void BajarCandiesv(int i, int v)
        {
            if (i == 0 || i == 1 || i == 2 || i == 3 ||
                             i == 4 || i == 5 || i == 6 || i == 7 || i == 8)
            {
                
                listElementos[i].tipodecolor = "cambiar";
                listElementos[i +9].tipodecolor = "cambiar";
                listElementos[i + 18].tipodecolor = "cambiar";
                if (i + 27 == v)
                {
                    listElementos[i + 27].tipodecolor = "cambiar";
                }
            }
            if (i < 9)
            {
                CambiarCandy(i, v);
            }
            else
            {
                int x = i, y = v, cont = 0, c = i;
                    while (c > 0)
                    {
                        if (c == 0 || c == 1 || c == 2 || c == 3 ||
                            c == 4 || c == 5 || c == 6 || c == 7 || c == 8)
                        {
                            break;
                        }
                        else
                        {
                            c = c - 9;
                         cont++;
                        }
                    }
                    while (cont > 0)
                    {
                        x = x - 9;
                        listElementos[y].color = listElementos[x].color;
                        cont--;
                        y = y - 9;
                    }
                    if (cont == 0)
                    {
                        while (y > -1)
                        {
                           listElementos[y].tipodecolor = "cambiar";
                            y = y - 9;
                        }
                    }
                 CambiarCandy(i, v);
            }
        }
        private void CambiarCandy(int i, int v)
        {
            Array values = Enum.GetValues(typeof(Color));
            Random random = new Random();
            var color = Color.red;
            for (int y = 0; y < 81; y++)
            {
                if (listElementos[y].tipodecolor == "cambiar")
                {
                    color = (Color)values.GetValue(random.Next(values.Length));
                    listElementos[y].color = color;
                    listElementos[y].tipodecolor = "";
                }
            }
        }

        //private void CambiarCandy(int i, int v)
        //{
        //    Array values = Enum.GetValues(typeof(Color));
        //    Random random = new Random();
        //    var color = Color.red;
        //    for (int y = i; y < v; y++)
        //    {
        //        color = (Color)values.GetValue(random.Next(values.Length));
        //        listElementos[y].color = color;
        //    }

        //}

        /// <summary>
        /// metodo para verificar que el movimiento del usuario es correcto
        /// </summary>
        /// <param name="movimiento1">trae el mov1 del usuario</param>
        /// <param name="movimiento2">trae el mov2 del usuario</param>
        /// <returns>true si es un mal movimiento</returns>
        private bool NullMove(int movimiento1, int movimiento2)
        {
            if (movimiento1 + 1 == movimiento2 || movimiento1 - 1 == movimiento2||movimiento1+9==movimiento2||movimiento1-9==movimiento2)
            {
                return false;
            }
            return true;
        }
    }
}
