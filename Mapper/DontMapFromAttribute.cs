using System;

namespace RobsonROX.Mapper
{
    /// <summary>
    /// Atributo utilizado para indicar que a propriedade ou campo onde ele for aplicado não pode ser mapeada do membro indicado do tipo indicado
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
    public sealed class DontMapFromAttribute : Attribute
    {
        /// <summary>
        /// Indica que a propriedade ou campo onde for aplicada não pode ser mapeada do membro indicado do tipo indicado
        /// </summary>
        /// <param name="dontMapFromType">Tipo que não pode receber o mapeamento deste membro</param>
        /// <param name="memberName">Membro do tipo indicado que não pode receber o valor deste membro</param>
        public DontMapFromAttribute(Type dontMapFromType, string memberName)
        {
            DontMapFromType = dontMapFromType;
            MemberName = memberName;
        }

        /// <summary>
        /// Tipo que não pode receber o mapeamento do membro onde este atributo foi aplicado
        /// </summary>
        public Type DontMapFromType { get; }

        /// <summary>
        /// Membro do tipo indicado que não pode receber o valor do membro onde este atributo foi aplicado
        /// </summary>
        public string MemberName { get; }
    }
}