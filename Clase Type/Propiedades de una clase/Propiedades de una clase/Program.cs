using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Propiedades_de_una_clase
{
    class PropertyClass2
    {

        private String v4;
        private String v1;
        private String v2;
        private String v3;
        private List<String> v5;
        private String[] v6;

        
        public String V1
        {
            get { return v1; }
            set { v1 = value; }
        }

        

        public String V3
        {
            get { return v3; }
            set { v3 = value; }
        }

        public String V2
        {
            get { return v2; }
            set { v2 = value; }
        }

        private void prubadeproperty (String v)
        {  

        }
    }


    class PropertyClass {
        private String sProperty;
        private Boolean bCanRead;
        private Boolean bCanWrite;

        public PropertyClass(String sProperty, Boolean bCanRead, Boolean bCanWrite)
        {
            this.sProperty = sProperty;
            this.bCanRead = bCanRead;
            this.bCanWrite = bCanWrite;
        }

        public String SProperty
        {
            get { return sProperty; }
            set { sProperty = value; }
        }
        public Boolean BCanWrite
        {
            get { return bCanWrite; }
            set { bCanWrite = value; }
        }
        
        public Boolean BCanRead
        {
            get { return bCanRead; }
            set { bCanRead = value; }
        }
    }
    class MemberClass
    {
        private String sField;
        private PropertyClass sProperty;

        public MemberClass(String sField, PropertyClass sProperty)
        {
            this.sField = sField;
            this.sProperty = sProperty;
        }
        public MemberClass() { }

        public String SField
        {
            get { return sField; }
            set { sField = value; }
        }
        public PropertyClass SProperty
        {
            get { return sProperty; }
            set { sProperty = value; }
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            PropertyClass2 c1 = new PropertyClass2();
            List<PropertyClass> oListPropertiesClass = new List<PropertyClass>();
            List<String> oListFielsClass = new List<String>();
            Type oTipe = c1.GetType();
            List<MemberClass> oMyMembersClass = new List<MemberClass>();
            Console.WriteLine(oTipe.Name);
            Console.WriteLine(oTipe.IsGenericType);
            foreach (MemberInfo oMember in oTipe.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                if ((oMember.DeclaringType.Name == oTipe.Name) && (oMember.MemberType == MemberTypes.Field || oMember.MemberType == MemberTypes.Property))
                {
                    Console.WriteLine(oMember.Name.ToString() + " Tipo: " + oMember.MemberType);
                    
                    if (oMember.MemberType == MemberTypes.Property)
                    {
                        PropertyInfo myProperty = oTipe.GetProperty(oMember.Name.ToString());
                        oListPropertiesClass.Add(new PropertyClass(oMember.Name.ToString(), myProperty.CanRead,myProperty.CanWrite));
                    }
                    else
                    {
                        FieldInfo myMember = oTipe.GetField(oMember.Name.ToString(), BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic |
                                            BindingFlags.InvokeMethod | BindingFlags.PutDispProperty | BindingFlags.GetProperty);
                        Console.WriteLine(myMember.GetType());
                        Console.WriteLine("Es array: " + myMember.FieldType.IsArray);
                        Console.WriteLine("Es Collection: " + myMember.FieldType.IsGenericTypeDefinition);
                        Console.WriteLine("Es Collection: " + myMember.FieldType.IsGenericType);
                        Console.WriteLine("Es Collection: " + myMember.FieldType.IsGenericParameter);
                        oListFielsClass.Add(oMember.Name.ToString());
                    }
               }
                }
            
                if ((oListFielsClass.Count > 0) && (oListPropertiesClass.Count > 0))
                {
                    foreach (PropertyClass sProperty in oListPropertiesClass)
                    {
                        Boolean bSalir = false;
                        int i=0;
                        while (!bSalir)
                        { 
                            if(oListFielsClass[i].ToUpper() == sProperty.SProperty.ToUpper())
                            {
                                bSalir = true;
                                oMyMembersClass.Add(new MemberClass(oListFielsClass[i], sProperty));
                                oListFielsClass.Remove(oListFielsClass[i]);
                            }
                            i++;
                            if (i > oListFielsClass.Count - 1)
                                bSalir = true;
                        }                            
                    }
                    if (oMyMembersClass.Count > 0)
                    { 
                        foreach(MemberClass myMemberClass in oMyMembersClass)
                        {
                            Console.WriteLine("Campo: " + myMemberClass.SField.ToString() + " Propiedad:" + myMemberClass.SProperty.SProperty.ToString());
                        }
                    }
                  //todo: Los campos y las propiedades. Se debe relacionar por el nombre, se debe comprobar através de su propiedad el nombre del campo, es decir a través de la propiedad buscar el campo.
                //Todos los campos de la clase deben estar en la base de datos, si no están, debe dar un error. La clase sólo devolvera aquellos campos con sus propiedades.
                 //Almacenar la propiedad CanRead y CanWrite de las propiedades para saber que pueden hacer. En el caso de que se quiera acceder a una propiedad no declarada deberá mostrar un error.

            }
            Console.Read();
        }
    }
}
