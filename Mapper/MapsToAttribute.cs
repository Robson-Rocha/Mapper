using System;

namespace RobsonROX.Mapper
{
    /// <summary>
    /// Atributo utilizado para indicar que a propriedade ou campo onde ele for aplicado pode ser mapeada para o membro indicado do tipo indicado
    /// </summary>
    [AttributeUsage(AttributeTargets.Property|AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class MapsToAttribute : Attribute
    {
        /// <summary>
        /// Indica que a propriedade ou campo onde for aplicada pode ser mapeada para o membro indicado do tipo indicado
        /// </summary>
        /// <param name="mapToType">Tipo que pode receber o mapeamento deste membro</param>
        /// <param name="memberName">Membro do tipo indicado que pode receber o valor deste membro</param>
        public MapsToAttribute(Type mapToType, string memberName)
        {
            MapToType = mapToType;
            MemberName = memberName;
        }

        /// <summary>
        /// Tipo que pode receber o mapeamento do membro onde este atributo foi aplicado
        /// </summary>
        public Type MapToType { get; }

        /// <summary>
        /// Membro do tipo indicado que pode receber o valor do membro onde este atributo foi aplicado
        /// </summary>
        public string MemberName { get; }
    }
}
