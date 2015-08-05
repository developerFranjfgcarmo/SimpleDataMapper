using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;
using ClsAccessData.Connection;

namespace Tareas.ControlData
{
    class ClsDataQuery
    {
        #region"Declaración de métodos miembros."

        private ClsConnection oConn;

        /// <summary>
        /// Indica si se ha llegado al final de los registros
        /// </summary>
        private bool  iAuxEof ; 

        /// <summary>
        /// Obtiene o establece el origen de datos con el que está enlazado el origen de datos.
        /// </summary>
        private BindingSource oBindingSource;  

        /// <summary>
        /// Representa una vista personalizada que puede enlazar datos de un DataTable 
        /// para ordenación, filtrado, búsqueda, edición y exploración. Es mucho mejor trabajar
        /// con este tipo de objetos que con el propio DataTable
        /// </summary>
        private DataView oDataView; 

        /// <summary>
        /// Representa una vista de datos, se utiliza para la insertar nuevos registros.
        /// </summary>
        private DataRowView oDataRowView; 

        /// <summary>
        /// Contiene la Query actual
        /// </summary>
        private String sSqlQuery;

        /// <summary>
        /// Almacena los datos datos obtenidos de la Query.
        /// </summary>
        private DataSet oMiDataSet;

       /* /// <summary>
        /// Representa un conjunto de comandos de datos y una conexión de base de 
        /// datos que se utilizan para rellenar un DataSet 
        /// </summary>
        private NpgsqlDataAdapter oMiDataAdapter;*/

        /// <summary>
        /// Condición Where
        /// </summary>
        private String sSqlWhere;

        /// <summary>
        /// Contiene el indice Actual del Registro
        /// </summary>
        private int IndiceRowActual;

        /// <summary>
        /// Nombre de la tabla de la consulta.<br/>
        /// Se utiliza para guardar los registro de una tabla que son obtenidos por 
        /// medio de una consulta de INNER JOIN
        /// </summary>
        private String sNombreTabla;
        #endregion;

        #region"Desplazamiento y Edición de Registros."
        /// <summary>
        /// Devuelve el valor de un campo de la base dato.
        /// </summary>
        /// <param name="Campo">Nombre de la columna.</param>
        /// <value></value>
        /// <returns></returns>
        public String Fields(Object oCampo) 
        {           
            String sResult = "";

            if (this.Count > 0 )
                if (typeof(String) == oCampo.GetType())
                    //sResult = NoNull(this.MiDataView.Item(this.AbsolutePosition).Item(Campo.ToString)).ToString
                    sResult = ClsGeneral.NoNull(this.MiDataView[this.AbsolutePosition].Row[oCampo.ToString()]).ToString();
                else if (typeof(int)== oCampo.GetType()) 
                    //sResult = FunGeneral.NoNull(Me.MiDataView.Item(Me.AbsolutePosition).Item(CInt(Campo))).ToString
                    sResult = ClsGeneral.NoNull(this.MiDataView[this.AbsolutePosition].Row[(int)oCampo]).ToString();    

            return sResult.Trim();
        }
    
        /// <summary>
        /// Asigna el valor de un campo de base datos.
        /// </summary>
        /// <param name="oCampo">Campo de la base datos.</param>
        /// <param name="value">Valor asignado al campo.</param>
        public void Fields(Object oCampo, String value) 
        {
            try{
                //Damos formato al valor, para que no de problemas porque no sea un tipo valido.
                value = GetFormatoCampo(this.MiDataView.Table.Columns[oCampo.ToString()], value);
                if (this.Count >= 0)
                    if (typeof(String) == oCampo.GetType())
                        //Comprobamos que el registro sea nuevo
                        if ((oDataRowView != null) & (oDataRowView.IsNew))
                            oDataRowView[oCampo.ToString()] = ClsGeneral.NoNull(value);

                        else
                            this.MiDataView[this.AbsolutePosition].Row[oCampo.ToString()] = ClsGeneral.NoNull(value);

                    else
                        //Comprobamos que el registro sea nuevo
                        if ((oDataRowView !=null) & (oDataRowView.IsNew))
                            oDataRowView[oCampo.ToString()] = ClsGeneral.NoNull(value); // FunGeneral.NoNull(value)
                        else
                            this.MiDataView[this.AbsolutePosition].Row[(int)oCampo] = ClsGeneral.NoNull(value);
            
            }catch (Exception ex){
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
        }
  
        /// <summary>
        /// Devuelve el valor pasado por el parámetro sValorCampo formateado dependiendo del tipo 
        /// de datos para asignarlo como valor a un campo en una SQL.
        /// </summary>
        /// <param name="sTipoCampo">
        /// Tipo de datos del campo
        /// </param>
        /// <param name="sValorCampo">
        /// Valor que se devolverá formateado
        /// </param>
        /// <returns></returns>
        private String GetFormatoCampo(DataColumn sTipoCampo, String sValorCampo) 
        {
            switch (sTipoCampo.GetType().ToString())
            {
                case "System.String":
                    if (String.IsNullOrEmpty(sValorCampo))
                        sValorCampo = "";
                
                    break;

                case "System.DateTime":
                    if (ClsGeneral.IsDate(sValorCampo) )
                        sValorCampo = DateTime.Parse(sValorCampo).ToString();
                    else
                        sValorCampo = "";
                    
                    break;
                                   

                case "System.Decimal":
                    if (ClsGeneral.IsNumeric(sValorCampo) )
                        //convierte la coma en punto
                        sValorCampo = sValorCampo.Replace(",", ".");
                    else
                        sValorCampo = "0";
                    
                    break;

                case "System.Int16":case "System.Int32":case "System.Int64":
                    if (ClsGeneral.IsNumeric(sValorCampo))
                        //si viene con decimales se los quita
                        sValorCampo = int.Parse(sValorCampo).ToString();
                    else
                        sValorCampo = "0";
                    
                   break;

                case  "System.Double":
                    if (ClsGeneral.IsNumeric(sValorCampo))
                        sValorCampo = sValorCampo.Replace(",", ".");
                    else
                        sValorCampo = "0";

                    break;
                default:
                    //----------- NO COMENTAR ESTAS LINEAS -----------
                    //hay que definir el tipo de datos del campo para mayor seguridad
                    System.Windows.Forms.MessageBox.Show("El tipo de datos no esta definido en ClsTableStructure.");
                    break;

            //devuelve el valor entre comillas (si es varchar o datetime) y reemplazando todas las comillas simples por dobles
                   
            }
            return sValorCampo;
        }

        /// <summary>
        /// Obtiene si el registro actual es el último
        /// </summary>
        /// <returns></returns>
        public bool EOF() 
        {           
            bool isEof = (iAuxEof) |(oBindingSource.Position == -1);
            return isEof;
          //TODO: Cambio de leo no me fio un pelo porque esta parte la probe bastante.
                /*If isEof AndAlso this.CountDv > 0 Then
                   return False
                Else
                    return isEof
                End If*/      
        }

        /// <summary>
        /// Se desplaza al siguiente elemento de la lista 
        /// </summary>
        public void MoveNext()
        {
            if (oBindingSource.Position == oBindingSource.Count - 1)
                iAuxEof = true;
               
            oBindingSource.MoveNext();

        }

        /// <summary>
        /// Se desplaza al último elemento de la lista 
        /// </summary>
        public void MoveLast()
        {
            oBindingSource.MoveLast();   
        }

        /// <summary>
        /// Se desplaza al anterior elemento de la lista 
        /// </summary>
        public void MovePrevious()
        {
            oBindingSource.MovePrevious();
        }

        /// <summary>
        /// Se desplaza al primer elemento de la lista 
        /// </summary>
        public void MoveFirst()
        {
            iAuxEof = false;
            oBindingSource.MoveFirst();            
        }

         /// <summary>
        /// Se situa en el registro que coincida con los criterios de busqueda.<br/>
        /// Tiene que haber el mismo número de campos de busqueda que creterios 
        /// </summary>
        /// <param name="sCampoBusqueda">Campos de busqueda, si hay mas de un campo pasarlos separados por comas.</param>
        /// <param name="sCriterioBusqueda">Criterio de busqueda, si hay mas de un campo pasarlos separados por comas.</param>
        public int FindRows(String sCampoBusqueda , Object sCriterioBusqueda )  
        {
            int iRow = -1;

            try{
                iRow = this.oBindingSource.Find(sCampoBusqueda, sCriterioBusqueda);
                if (iRow >= 0 )
                    oBindingSource.Position = iRow;

            }catch (Exception ex )
            {       //If Depurando 
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                /*Else
                    Throw ex
                End If*/
            }
            return iRow;

        }

        /// <summary>
        /// Estable un criterio de busqueda, para quitar el filtro pasar cadena vacia
        /// </summary>
        public String FilterRows 
        {
            set{this.oBindingSource.Filter = value;}
        }

        /// <summary>
        /// Obtiene la cantidad total de registro del DataView.<br/>
        /// Este CountDv no es el mismo que la propiedad Count, porque es DataView puede estar filtrado.
        /// </summary>
        public int Count {
            get{return this.MiDataTable.DefaultView.Count;}
        }

        /// <summary>
        /// Agrega una nueva fila al obtejo DataView
        /// </summary>
        public void NewRow()
        {oDataRowView = this.MiDataView.AddNew();}

        /// <summary>
        /// Elimina la fila actual
        /// </summary>
        public void DeleteRow()
        { this.RowActual.Delete(); }

        /// <summary>
        /// Obtiene o establece la posición  la posición actual de registro 
        /// </summary>
        public int AbsolutePosition {
            get{return oBindingSource.Position;}
            set{oBindingSource.Position = value;}
        }

        /// <summary>
        /// Devuelve el registro Actual. <br/>
        /// Devuelve el registro actual, de esta forma podemos hacer operaciones de Delete, Update, Insert si 
        /// sin la necesidad de indicar el indice del registro.
        /// </summary>
        public DataRowView  RowActual
        { get { return MiDataView[this.AbsolutePosition]; } }
        #endregion

        #region "Métodos miembros para enlazar los controles del formulario con el origen de datos"

        /// <summary>
        /// Elimina o añade el DataBinding de los controles.<br/>
        /// Permite enlazar los controles del formulario con el origen de datos, de esta forma se 
        /// automatiza la navagación por los registros.
        /// </summary>
        private void ManejaDataBinding( Control oControl, String sCampo, bool sCargaTag )
        {
             Binding oBinding =null;
             String sControlProperty  = "";
            //Se Identifica el tipo de control y se le indica la propiedad del control que esta 
            //enlazada con el origen de datos.
            //(typeof(String) == oCampo.GetType())
            if (typeof(ComboBox) ==oControl.GetType()) 
                sControlProperty = "SelectedValue";
               // oBinding = (ComboBox)oControl..DataBindings(sControlProperty)

            else if (typeof(TextBox) == oControl.GetType())
                sControlProperty = "Text";

            else if(typeof(CheckBox)== oControl.GetType())  
                sControlProperty = "Checked";
            
            if (sCargaTag)
                oControl.Tag = sCampo;

            if (! String.IsNullOrEmpty(sControlProperty))
                oBinding = oControl.DataBindings[sControlProperty];

                if (oBinding!=null)
                    oControl.DataBindings.Remove(oBinding);

                if (!String.IsNullOrEmpty(sCampo))
                {
                    //Inicializamos Binding.
                    oBinding = new Binding(sControlProperty, this.oBindingSource, sCampo);
                    //Permite que el objeto Binding se actualice automaticamente, es decir si cambia el contenido del control
                    // que automaticamente cambie en el origen de datos.
                    //Con esta propiedad no es necesario la propiedad AceptarCambios del ctlDataGridView
                    oBinding.DataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
                    oControl.DataBindings.Add(oBinding);
                }          
        }

        /// <summary>
        /// Establece un control con el origen de datos actual.
        /// </summary>
        /// <param name="oControl">Control a enlazar con la base de datos.</param>
        /// <param name="sCampo">Nombre de la base de datos.</param>
        public void AddDataBind(Control oControl , String sCampo )
        {
            ManejaDataBinding(oControl, sCampo, false);
        }

        /// <summary>
        /// Suspende el enlace a datos para impedir que se actualice el origen de datos
        /// </summary>
        public void RemoveDataBinding(Control oControl )
        {
            ManejaDataBinding(oControl, "", false);
        }

        /// <summary>
        /// Suspende el enlace a datos para impedir que se actualice el origen de datos
        /// </summary>
        public void RemoveDataBinding()
        {
            this.oBindingSource.SuspendBinding();
        }

        /// <summary>
        /// Reanuda el enlace a datos
        /// </summary>
        public void ResumeBinding()
        {
            this.oBindingSource.ResumeBinding();
        }

        #endregion

        #region '"Métodos miembros de accesos, manejo y visualización de datos"
        /// <summary>
        /// Devuelve la coleccion actual de registros
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public DataRowCollection Rows 
            {get{return MiDataTable.Rows;}}    

        /// <summary>
        /// Obtiene el objeto BindingSource. <br/>
        /// Devuelve el registro actual, de esta forma podemos hacer operaciones de Delete, Update, Insert si 
        /// sin la necesidad de indicar el indice del registro.
        /// </summary>
        public BindingSource MiBindingSource
        { get { return this.oBindingSource; } }

        /*/// <summary>
        /// Devuelve el DataAdapter.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        public NpgsqlDataAdapter MiDataAdapter
        {get{ return oMiDataAdapter;}}*/

        /// <summary>
        /// Devuelve la coleccion actual de registros
        /// </summary>
        public DataView MiDataView
        {get{return this.MiDataTable.DefaultView;}}

        /// <summary>
        /// Devuelve el DataSet
        /// </summary>
        public DataSet MiDataSet
        {get{return oMiDataSet; }}

        /// <summary>
        /// Devuelve la coleccion actual de tablas del DataSet
        /// </summary>
        public DataTable MiDataTable
        {
            get
            {
                DataTable oRes = null;
                if ((this.MiDataSet != null) & (this.MiDataSet.Tables.Count > 0))
                    {oRes = this.MiDataSet.Tables[0];}

                return oRes;
            }
        }

        /// <summary>
        /// Ejecuta una sentencia SQL de selección (SELECT)
        /// </summary>
        /// <param name="oConnBD"> Instrucción SQL a ejecutar</param>
        /// <param name="sSQL">
        /// Indica si se produce un error, si debe motrar un mensaje o debe elevar el error al método o función desde el que 
        ///  fué llamada esta función.</param>
        /// <param name="ElevaError"></param>
        /// <returns>Devuelve un Datatable con el resultado de la SQL</returns>
        public DataTable SQLQuery(ClsConnection oConnBD, String sSQL, bool ElevaError) 
        {
            DataTable oRes   = null;

            try{
                this.sSqlQuery = sSQL;

                oMiDataSet =oConnBD.InitDataAdapter(sSQL);
                oDataView = new DataView(MiDataTable);
                //CargaEstructura()

                this.oBindingSource = new BindingSource();
                this.oBindingSource.DataSource = MiDataView;

                oRes = MiDataTable;

            }catch(Exception ex)
            {
                if (ElevaError )
                    //eleva el error al método que llamó a esta función y el objeto exception original va en el InnerException
                    throw new Exception("Error al ejecutar la sentencia: " + sSQL, ex);
                else
                    System.Windows.Forms.MessageBox.Show("Error al ejecutar la sentencia: " + sSQL + (char)13,  ex.ToString());
            }

            return oRes;
        }

  
        #endregion //"Métodos miembros de accesos, manejo y visualización de datos"
    }
}
