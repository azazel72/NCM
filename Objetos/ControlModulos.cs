using System.Collections.Generic;
using System.Text.Json.Serialization;
using System;

namespace NoCocinoMas
{
    public class ControlModulos
    {
        private List<Liberacion> pendientes;

        public ControlModulos()
        {
            this.pendientes = new List<Liberacion>();
        }

        public void PosponerLiberacion(Modulo m, Tarea t, Modulo mProximo)
        {
            this.pendientes.Add(new Liberacion(3000, m, t, mProximo));
            ConectorPLC.Informar(m, mProximo);
        }

        public void Liberar()
        {
            lock (Tareas.bloqueoBusquedaPosicion)
            {
                this.pendientes.RemoveAll(Comprobar);
            }
        }

        private bool Comprobar(Liberacion l)
        {
            if (l.Comprobar())
            {
                ConectorPLC.Liberar(l.modulo);
                l.modulo.DesvincularTarea(l.tarea);
                if (l.moduloProximo == null)
                {
                    ConectorPLC.FinalizarPosicion(l.tarea);
                }
                return true;
            }
            return false;
        }

        private class Liberacion
        {
            private DateTime finalizacion;
            public Modulo modulo { get; }
            public Tarea tarea { get; }
            public Modulo moduloProximo { get; }

            public Liberacion(long duracion, Modulo m, Tarea t, Modulo mProximo)
            {
                this.finalizacion = DateTime.Now.AddMilliseconds(duracion);
                this.modulo = m;
                this.tarea = t;
                this.moduloProximo = mProximo;
            }

            public bool Comprobar()
            {
                return this.finalizacion <= DateTime.Now;
            }
        }


    }
}
