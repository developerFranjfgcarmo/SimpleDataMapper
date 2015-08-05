using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SimpleDataMapper.utilities;

namespace SimpleDataMapper.Controller.ClsMemberClass
{
    //En el caso que se haga una iteración sobre un Objeto, evitar volver a cargarlo, en este caso sólo debería realizar el insert.
    //todo:limpiar la collección de oListPropertiesClass.
    //todo: Cambiar para que el objeto sea obligatorio, sólo debe ser obligatorio en uno de los dos sitios.
    //todo: En caso de cargar más de un objeto de la misma clase, que no carge los campos y las propiedades, puesto que ya estarían cargado,Debo mirar este punto.
    /// <summary>
    ///     Esta clase almacena todas las propiedades y campos de una clase.
    /// </summary>
    internal class ClsObjectClass : Disposable
    {
        #region Declaración de campos.

        /// <summary>
        ///     Variable obligatoria, Se debe definir el nombre de la tabla sobre la cual se va a realizar la operación DML.
        /// </summary>
        private const String Tablename = "TABLENAME";

        /// <summary>
        ///     Lista de propiedades del objeto.
        /// </summary>
        private List<ClsPropertyClass> _oListPropertiesClass;

        /// <summary>
        ///     Representa al tipo de objeto (myObject).
        /// </summary>
        private Type _oType;

        #endregion

        #region Declaración de Constructor.

        /// <summary>
        ///     Constructor de la clase ClsObjectClass
        /// </summary>
        /// <param name="myObject"></param>
        internal ClsObjectClass(Object myObject)
        {
            if (myObject != null)
            {
                MyObject = myObject;
                LoadMenbersClass();
            }
            else
                ClsTraccer.RunException("El elemento myObject no puede ser nulo.", "LoadMenbersClass");
        }

        #endregion

        #region Declaración de propiedades.

        /// <summary>
        ///     Propiedad que obtiene o devuelve la instancia del objeto.
        /// </summary>
        internal object MyObject { get; set; }

        /// <summary>
        ///     Devuelve el nombre de la tabla.
        /// </summary>
        internal string STableName { get; private set; }

        /// <summary>
        ///     Devuelve la lista de campos o datos miembros de las clase.
        /// </summary>
        internal List<ClsFieldClass> OMyMembersClass { get; private set; }

        #endregion

        #region Declaración de Métodos privados.

        /// <summary>
        ///     Carga los campos y las propiedades de la clase.
        /// </summary>
        private void LoadMenbersClass()
        {
            bool bExitTable = false;
            _oListPropertiesClass = new List<ClsPropertyClass>();
            var oListFielsClass = new List<String>();
            _oType = MyObject.GetType();
            OMyMembersClass = new List<ClsFieldClass>();
            //Recorremos cada campo y propiedad de la clase.
            foreach (
                MemberInfo oMember in
                    _oType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                //Cargamos solamente los campos y sus propiedades, el resto de elementos de la clase los ignoramos.
                if (oMember.DeclaringType != null && ((oMember.DeclaringType.Name == _oType.Name) &&
                                                      (oMember.MemberType == MemberTypes.Field ||
                                                       oMember.MemberType == MemberTypes.Property)))
                {
                    if (oMember.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo myProperty = _oType.GetProperty(oMember.Name);
                        _oListPropertiesClass.Add(new ClsPropertyClass(oMember.Name, myProperty.CanRead,
                            myProperty.CanWrite));
                    }
                    else //Cargamos los campos de la clase.
                    {
                        //Cargamos el nombre de la tabla.
                        if (Tablename == oMember.Name.ToUpper())
                        {
                            FieldInfo myFieldInfo = _oType.GetField(oMember.Name,
                                BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                            if (myFieldInfo != null) STableName = (String) myFieldInfo.GetValue(MyObject);
                            if (String.IsNullOrEmpty(STableName))
                                bExitTable = true;
                        }
                        else //Cargamos el nombre del campo.
                            oListFielsClass.Add(oMember.Name);
                    }
                }
            }
            //Relacionamos cada campo, con su propiedad.
            if ((oListFielsClass.Count > 0) && (_oListPropertiesClass.Count > 0))
            {
                foreach (ClsPropertyClass sProperty in _oListPropertiesClass)
                {
                    bool bSalir = false;
                    int i = 0;
                    while (!bSalir)
                    {
                        if (String.Equals(oListFielsClass[i], sProperty.SProperty,
                            StringComparison.CurrentCultureIgnoreCase))
                        {
                            bSalir = true;
                            OMyMembersClass.Add(new ClsFieldClass(oListFielsClass[i], sProperty));
                            oListFielsClass.Remove(oListFielsClass[i]);
                        }
                        i++;
                        if (i > oListFielsClass.Count - 1)
                            bSalir = true;
                    }
                }
            }
            //Si el nombre de la tabla no existe lanzamos una excepción.
            if (bExitTable)
                //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
                throw new ArgumentException(
                    "Debe existir el campo TABLENAME y debe estar relleno con el nombre de la tabla.", ToString());
        }

        /// <summary>
        ///     Modifica el Valor de la propiedad de la clase
        /// </summary>
        /// <param name="value">Valor del campo de la propiedad</param>
        /// <param name="sName">Nombre de la propiedad.</param>
        private void SetValue(Object value, String sName)
        {
            SetValue(value, sName, 0);
        }

        /// <summary>
        ///     Modifica el Valor de la propiedad de la clase.
        /// </summary>
        /// <param name="value">Valor del campo de la propiedad.</param>
        /// <param name="index">Índice de la propiedad a modificar.</param>
        private void SetValue(Object value, int index)
        {
            SetValue(value, null, index);
        }

        /// <summary>
        ///     Modifica el Valor de la propiedad de la clase.
        /// </summary>
        /// <param name="value">Valor del campo de la propiedad.</param>
        /// <param name="sName">Nombre de la propiedad.</param>
        /// <param name="index">Índice de la propiedad a modificar.</param>
        private void SetValue(Object value, String sName, int index)
        {
            ClsPropertyClass myElement = !String.IsNullOrEmpty(sName)
                ? FindElementPropertyClass(sName)
                : _oListPropertiesClass[index];
            if (myElement != null)
            {
                if (myElement.BCanWrite)
                {
                    PropertyInfo myProperty = _oType.GetProperty(myElement.SProperty);
                    object result;
                    switch (value.GetType().UnderlyingSystemType.Name)
                    {
                        case "String":
                            result = value;
                            break;

                        case "DateTime":
                            result = (DateTime) value;
                            break;

                        case "Decimal":
                            result = Convert.ChangeType(value, typeof (int));
                            break;

                        case "Int16":
                            result = (Int16) value;
                            break;

                        case "Int32":
                            result = (Int32) value;
                            break;

                        case "Int64":
                            result = (Int64) value;
                            break;

                        case "Double":
                            result = (Double) value;
                            break;

                        case "Boolean":
                            result = (Boolean) value;
                            break;

                        default:
                            result = value;
                            break;
                    }
                    myProperty.SetValue(MyObject, result, null);
                }
                else
                {
                    ClsTraccer.RunException("La propiedad " + sName + " no tiene definido la propiedad de escritura.",
                        ToString());
                }
            }
            else
                ClsTraccer.RunException("No existe la propiedad " + sName, ToString());
        }

        /// <summary>
        ///     Obtiene el valor de la propiedad de la clase.
        /// </summary>
        /// <param name="index">Índice de la propiedad.</param>
        /// <returns>Devuelve el valor de la propiedad</returns>
        private Object GetValue(int index)
        {
            return GetValue(null, index);
        }

        /// <summary>
        ///     Obtiene el valor de la propiedad de la clase.
        /// </summary>
        /// <param name="sName">Nombre de la propiedad.</param>
        /// <param name="index">Índice de la propiedad.</param>
        /// <returns>Devuelve el valor de la propiedad</returns>
        private Object GetValue(String sName, int index = 0)
        {
            //Dependiendo si se 
            ClsPropertyClass myElement = !String.IsNullOrEmpty(sName)
                ? FindElementPropertyClass(sName)
                : _oListPropertiesClass[index];

            if (myElement == null) throw new ArgumentException("No existe la propiedad " + sName, ToString());
            if (!myElement.BCanRead)
                throw new ArgumentException("La propiedad " + sName + " no tiene definido la propiedad de lectura.",
                    ToString());
            PropertyInfo myProperty = _oType.GetProperty(myElement.SProperty);
            return myProperty.GetValue(MyObject, null);
            //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
        }

        //todo:Pendiente. Mirar si se puede buscar sucesivamente a través del método FIND.
        /// <summary>
        ///     Encuentra un elemento dentro del objeto list y se situa en este.
        /// </summary>
        /// <param name="sName">Nombre del objeto a buscar.</param>
        /// <returns></returns>
        private ClsPropertyClass FindElementPropertyClass(String sName)
        {
            ClsPropertyClass myElement =
                _oListPropertiesClass.Find(
                    myFindElement =>
                        String.Equals(myFindElement.SProperty, sName, StringComparison.CurrentCultureIgnoreCase));

            return myElement;
        }

        #endregion

        #region Declaración de Métodos públicos.

        /// <summary>
        ///     Propiedad que lee o modifica los valores de las propiedades de la clase.
        /// </summary>
        /// <param name="index">Número del elemento a devolver.</param>
        /// <returns>Obtiene o almacena un valor en elemento indicado</returns>
        public Object this[int index]
        {
            set
            {
                SetValue(value, index);
                /*if (oListPropertiesClass[index].BCanWrite)
                {
                    PropertyInfo myProperty = oType.GetProperty(oListPropertiesClass[index].SProperty.ToString());

                    myProperty.SetValue(myObject, value, null);
                }
                else
                {
                    //todo:Lanzar excepción de negocio.
                    
                }*/
            }
            get
            {
                return GetValue(index);
                /*if (oListPropertiesClass[index].BCanRead) 
                {
                    PropertyInfo myProperty = oType.GetProperty(oListPropertiesClass[index].SProperty.ToString());
                    return myProperty.GetValue(MyObject,null);
                }
                else
                {
                    //todo:Lanzar excepción de negocio.
                    return null;
                }*/
            }
        }

        /// <summary>
        ///     Propiedad que lee o modifica la propiedad de la clase.
        /// </summary>
        /// <param name="sName">Nombre del elemento.</param>
        /// <returns>Obtiene o almacena un valor en elemento indicado</returns>
        public Object this[String sName]
        {
            set { SetValue(value, sName); }
            get { return GetValue(sName); }
        }

        /// <summary>
        ///     Crea una nueva instancia del objeto.
        /// </summary>
        internal void CreateInstance()
        {
            Type newType = GetType();
            object obj = Activator.CreateInstance(_oType);
            MyObject = obj;
        }

        /// <summary>
        ///     Comprueba si existe el campo en la clase.
        /// </summary>
        /// <param name="sName">Nombre del campo.</param>
        /// <returns>Devuelve true si existe el campo y false si no existe.</returns>
        internal Boolean ThereIsField(String sName)
        {
            return OMyMembersClass.Any(myField => myField.SField == sName);
        }

        #endregion
    }
}