using System;
using System.Text;
using System.Data;
using System.Collections.Generic;
using System.Collections;
using NpgsqlTypes;
using Npgsql;



namespace Tareas.AccessData
{
    class ClsStructure
    {
        public enum TipoPropiedad{TipoDatos,TamañoCampo,PermiteNull,DefaultData};
        private Hashtable oSchemaStruct;
        public ClsConnection oCon;

        public void CargaStruct()
        {
            Hashtable oColTable ;
            DataTable dtShema;
            DataTable dtTable;
            String sNombreTabla="";
            Hashtable Fila ;//= new Hashtable();
            
            //Abrimos la conexión.
            this.oCon.DBOpen();
            oSchemaStruct = new Hashtable();
            //Para ver información de las diferentes colecciones de datos devueltas por el método GetSchema:
            // http://msdn2.microsoft.com/es-es/library/ms254969(VS.80).aspx
            // http://msdn2.microsoft.com/es-es/library/ms254501(VS.80).aspx
            
            //Obtenemos todas las tablas de base datos.
            dtShema = this.oCon.DBConnection.GetSchema("Tables", new String[] { null, "public", null, "BASE TABLE" });
            foreach (DataRow rowSchema in dtShema.Rows)
            {
                oColTable = new Hashtable();
                foreach (DataColumn colSchema in dtShema.Columns)
                {
                   // Console.WriteLine(colSchema.ColumnName + ": " + rowSchema[colSchema].ToString());
                    
                    //Comprobamos si ColumnName es igual table_name, es el nombre de tabla.         
                    switch (colSchema.ColumnName)
                    { 
                        case "table_name":
                            //Almacenamos el nombre de la tabla en una variable para poder almacenarlo en el hashtable.
                            sNombreTabla = rowSchema[colSchema].ToString();
                            //Obtenemos la estructura de tabla.
                            dtTable = this.oCon.DBConnection.GetSchema("Columns", new String[] { null, null, rowSchema[colSchema].ToString(), null });
                            
                            //Recorremos todos los registros, para obtener las propiedades de cada una de las columnas de la tabla.
                            foreach ( DataRow  row in dtTable.Rows)
                            {
                                Fila = new Hashtable();
                                //recorre cada propiedad del campo 
                                foreach( DataColumn col  in dtTable.Columns)
                                {
                                    //Console.WriteLine(col.ColumnName + ": " + row[col].ToString());
                                    switch  (col.ColumnName.ToUpper())
                                    {
                                        case "DATA_TYPE": //Tipo de datos
                                            Fila.Add(TipoPropiedad.TipoDatos.ToString(),row[col].ToString());
                                            break;

                                        case "CHARACTER_MAXIMUM_LENGTH" ://Longitud del campo
                                            Fila.Add(TipoPropiedad.TamañoCampo.ToString(),(row[col].ToString() == "") ? "0" : row[col].ToString());
                                            break;

                                        case "IS_NULLABLE": //Permite valores nulos
                                            Fila.Add( TipoPropiedad.PermiteNull.ToString(),row[col].ToString());
                                            break;

                                        case "COLUMN_DEFAULT": //Valor por defecto
                                            Fila.Add( TipoPropiedad.DefaultData.ToString(),(row[col].ToString() == "")? "Null": row[col].ToString());
                                            break;
                                        case "COLUMN_NAME" ://Nombre del campo
                                            Fila.Add( "NombreCampo",row[col].ToString());
                                            break;
                                    }

                                }
                                //Añadimos la propidades de las columnas de la tabla.
                                oColTable.Add(Fila["NombreCampo"].ToString(), Fila);
                                Fila = null;
                            }
                            break;
                    }                 
                }
                //Añadimos la tabla, con las propiedades de sus columnas.                                         
                oSchemaStruct.Add(sNombreTabla, oColTable);
                oColTable = null;
            }
            
        }

          
   /* private String GetPrimaryKey(String sNombreTabla)
    {
        DataQuery oQuery = new DataQuery();
        String sCampo;
        String sSql = "SELECT I.indexdef " +
                      "FROM pg_indexes I,pg_CONSTRAINT C " +
                      "WHERE I.indexname    = C.conname " +
                        "AND I.tablename    = '" + sNombreTabla + "' " +
                        "AND C.CONTYPE      = 'p'";

        //oQuery.SQLQuery(this.oCon, sSql);
        //if (oQuery.CountDv > 0)
           // sCampo = oQuery.


    }*/


    }
}
