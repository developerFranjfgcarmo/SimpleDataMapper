using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tareas.ClsTablas
{
    class ClsPrueba
    {
        private String TABLENAME = "prueba";
        private int? p1;

        public int? P1
        {
            get { return p1; }
            set { p1 = value; }
        }
        private int? p2;

        public int? P2
        {
            get { return p2; }
            set { p2 = value; }
        }
        private Boolean? pruebabool;

        public Boolean? Pruebabool
        {
            get { return pruebabool; }
            set { pruebabool = value; }
        }
    }
}
