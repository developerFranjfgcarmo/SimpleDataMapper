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
    internal class ObjectClass : Disposable
    {
        #region [Private Properties]

        /// <summary>
        ///     Variable obligatoria, Se debe definir el nombre de la tabla sobre la cual se va a realizar la operación DML.
        /// </summary>
        private const string Tablename = "TABLENAME";

        /// <summary>
        ///     Lista de propiedades del objeto.
        /// </summary>
        private List<PropertyClass> _listPropertiesClass;

        /// <summary>
        ///     Representa al tipo de objeto (myObject).
        /// </summary>
        private Type _type;

        #endregion

        #region Declaración de Constructor.

        /// <summary>
        ///     Constructor de la clase ClsObjectClass
        /// </summary>
        /// <param name="myObject"></param>
        internal ObjectClass(Object myObject)
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

        #region [Internal Properties]

        /// <summary>
        ///     Propiedad que obtiene o devuelve la instancia del objeto.
        /// </summary>
        internal object MyObject { get; set; }

        /// <summary>
        ///     Devuelve el nombre de la tabla.
        /// </summary>
        internal string TableName { get; private set; }

        /// <summary>
        ///     Devuelve la lista de campos o datos miembros de las clase.
        /// </summary>
        internal List<FieldOfClass> MembersOfClass { get; private set; }

        #endregion

        #region [Private Methods]

        /// <summary>
        ///     Carga los campos y las propiedades de la clase.
        /// </summary>
        private void LoadMenbersClass()
        {
            bool bExitTable = false;
            _listPropertiesClass = new List<PropertyClass>();
            var oListFielsClass = new List<String>();
            _type = MyObject.GetType();
            MembersOfClass = new List<FieldOfClass>();
            //Recorremos cada campo y propiedad de la clase.
            IEnumerable<MemberInfo> listMembers =
                _type.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .Where(oMember => oMember.DeclaringType != null && ((oMember.DeclaringType.Name == _type.Name) &&
                                                                        (oMember.MemberType == MemberTypes.Field ||
                                                                         oMember.MemberType == MemberTypes.Property)));
            foreach (MemberInfo member in listMembers)
            {
                if (member.MemberType == MemberTypes.Property)
                {
                    PropertyInfo myProperty = _type.GetProperty(member.Name);
                    _listPropertiesClass.Add(new PropertyClass(member.Name, myProperty.CanRead,
                        myProperty.CanWrite));
                }
                else //Cargamos los campos de la clase.
                {
                    //Cargamos el nombre de la tabla.
                    if (Tablename == member.Name.ToUpper())
                    {
                        FieldInfo myFieldInfo = _type.GetField(member.Name,
                            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                        if (myFieldInfo != null) TableName = (String) myFieldInfo.GetValue(MyObject);
                        if (String.IsNullOrEmpty(TableName))
                            bExitTable = true;
                    }
                    else
                    {
//Cargamos el nombre del campo.
                        oListFielsClass.Add(member.Name);
                    }
                }
            }
            //Relacionamos cada campo, con su propiedad.
            if ((oListFielsClass.Count > 0) && (_listPropertiesClass.Count > 0))
            {
                foreach (PropertyClass sProperty in _listPropertiesClass)
                {
                    bool exit = false;
                    int i = 0;
                    while (!exit)
                    {
                        if (string.Equals(oListFielsClass[i], sProperty.Property,
                            StringComparison.CurrentCultureIgnoreCase))
                        {
                            exit = true;
                            MembersOfClass.Add(new FieldOfClass(oListFielsClass[i], sProperty));
                            oListFielsClass.Remove(oListFielsClass[i]);
                        }
                        i++;
                        if (i > oListFielsClass.Count - 1)
                            exit = true;
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
        private void SetValue(Object value, String sName, int index = 0)
        {
            PropertyClass myElement = !String.IsNullOrEmpty(sName)
                ? FindElementPropertyClass(sName)
                : _listPropertiesClass[index];
            if (myElement != null)
            {
                if (myElement.CanWrite)
                {
                    PropertyInfo myProperty = _type.GetProperty(myElement.Property);
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
            PropertyClass myElement = !string.IsNullOrEmpty(sName)
                ? FindElementPropertyClass(sName)
                : _listPropertiesClass[index];

            if (myElement == null) throw new ArgumentException("No existe la propiedad " + sName, ToString());
            if (!myElement.CanRead)
                throw new ArgumentException("La propiedad " + sName + " no tiene definido la propiedad de lectura.",
                    ToString());
            PropertyInfo myProperty = _type.GetProperty(myElement.Property);
            return myProperty.GetValue(MyObject, null);
            //todo:MEJORA. Quitar las exception y sustituir por una clase de negocio.
        }

        //todo:Pendiente. Mirar si se puede buscar sucesivamente a través del método FIND.
        /// <summary>
        ///     Encuentra un elemento dentro del objeto list y se situa en este.
        /// </summary>
        /// <param name="sName">Nombre del objeto a buscar.</param>
        /// <returns></returns>
        private PropertyClass FindElementPropertyClass(String sName)
        {
            PropertyClass myElement =
                _listPropertiesClass.Find(
                    myFindElement =>
                        string.Equals(myFindElement.Property, sName, StringComparison.CurrentCultureIgnoreCase));

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
            set { SetValue(value, index); }
            get { return GetValue(index); }
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
            object obj = Activator.CreateInstance(_type);
            MyObject = obj;
        }

        /// <summary>
        ///     Comprueba si existe el campo en la clase.
        /// </summary>
        /// <param name="sName">Nombre del campo.</param>
        /// <returns>Devuelve true si existe el campo y false si no existe.</returns>
        internal Boolean ThereIsField(String sName)
        {
            return MembersOfClass.Any(myField => myField.Field == sName);
        }

        #endregion
    }
}