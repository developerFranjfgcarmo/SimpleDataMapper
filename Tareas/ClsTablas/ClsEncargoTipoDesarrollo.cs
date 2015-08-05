using System;
using System.Collections.Generic;
using System.Text;

namespace Tareas.ClsTablas
{
    class ClsEncargoTipoDesarrollo
    {
        #region"Declaración de datos miembros."

        private String sNombreTabla = "encargo_tipo_desarrollo";
        private int hr_total_estimada;
        private long id_encargo_tipo_desarrollo;
        private int hr_total_desarrollo;
        private int hr_imputada;
        private DateTime fc_ultima_imputacion;
        private String ds_encargo;
        private int id_estado_desarrollo;
        private DateTime fc_alta;
        private DateTime fc_modi;
        private DateTime fc_baja;
        private long id_encargo;
        private long id_tipo_desarrollo;
        private String observaciones;
        private DateTime fc_inicio_prevista;
        private DateTime fc_fin_prevista;
        private DateTime fc_inicio_real;
        private DateTime fc_fin_real;
        private char aviso;
        private DateTime hr_aviso;
        private DateTime fc_aviso;

        #endregion;

        public ClsEncargoTipoDesarrollo() { 
            //se debe obtener el valor de la secuencia para el campo clave
        }

        public long Id_encargo
        {
            get { return this.id_encargo; }
            set { this.id_encargo = value; }
        }
        
        public String Observaciones
        {
            get { return this.observaciones; }
            set { this.observaciones = value; }
        }
        
        public DateTime Fc_inicio_prevista
        {
            get { return this.fc_inicio_prevista; }
            set { this.fc_inicio_prevista = value; }
        }
        
        public DateTime Fc_fin_prevista
        {
            get { return this.fc_fin_prevista; }
            set { this.fc_fin_prevista = value; }
        }
        
        public DateTime Fc_inicio_real
        {
            get { return this.fc_inicio_real; }
            set { this.fc_inicio_real = value; }
        }

        public DateTime Fc_fin_real
        {
            get { return this.fc_fin_real; }
            set { this.fc_fin_real = value; }
        }

        public int Hr_total_estimada
        {
            get { return this.hr_total_estimada; }
            set { this.hr_total_estimada = value; }
        }

        public long Id_encargo_tipo_desarrollo
        {
            get { return this.id_encargo_tipo_desarrollo; }
            set { this.id_encargo_tipo_desarrollo = value; }
        }

        public int Hr_total_desarrollo
        {
            get { return this.hr_total_desarrollo; }
            set { this.hr_total_desarrollo = value; }
        }

        public int Hr_imputada
        {
            get { return this.hr_imputada; }
            set { this.hr_imputada = value; }
        }

        public DateTime Fc_ultima_imputacion
        {
            get { return this.fc_ultima_imputacion; }
            set { this.fc_ultima_imputacion = value; }
        }

        public String Ds_encargo
        {
            get { return this.ds_encargo; }
            set { this.ds_encargo = value; }
        }

        public int Id_estado_desarrollo
        {
            get { return this.id_estado_desarrollo; }
            set { this.id_estado_desarrollo = value; }
        }
        
        public DateTime Fc_alta
        {
            get { return this.fc_alta; }
            set { this.fc_alta = value; }
        }

        public DateTime Fc_modi
        {
            get { return this.fc_modi; }
            set { this.fc_modi = value; }
        }

        public DateTime Fc_baja
        {
            get { return this.fc_baja; }
            set { this.fc_baja = value; }
        }
        
        public DateTime Fc_aviso
        {
            get { return this.fc_aviso; }
            set { this.fc_aviso = value; }
        }

        public DateTime Hr_aviso
        {
            get { return this.hr_aviso; }
            set { this.hr_aviso = value; }
        }

        public char Aviso
        {
            get { return this.aviso; }
            set { this.aviso = value; }
        }

        public long Id_tipo_desarrollo
        {
            get { return this.id_tipo_desarrollo; }
        }

    }
}
