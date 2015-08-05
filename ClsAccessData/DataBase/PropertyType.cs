namespace SimpleDataMapper.DataBase
{
    internal enum PropertyType
    {
        /// <summary>
        ///     Tipo de dato.
        /// </summary>
        DataType,

        /// <summary>
        ///     Longuitud del campo.
        /// </summary>
        FieldLength,

        /// <summary>
        ///     Indica si permite nulos el campo.
        /// </summary>
        IsNull,

        /// <summary>
        ///     Valor por defecto del campo.
        /// </summary>
        DefaultData,

        /// <summary>
        ///     Indica si el campo es primary key.
        /// </summary>
        PrimaryKey,

        /// <summary>
        ///     Contiene el numero de campos clave de una tabla.
        /// </summary>
        CountPrimarykeys
    };
}
