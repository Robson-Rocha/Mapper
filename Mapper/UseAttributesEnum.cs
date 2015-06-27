namespace RobsonROX.Mapper
{
    /// <summary>
    /// Determina quais mapeamentos por atributos devem ser utilizados
    /// </summary>
    public enum UseAttributesEnum
    {

        /// <summary>
        /// Utiliza os mapeamentos de atributos {MapsFromAttribute} a partir do tipo de destino
        /// </summary>
        UseFrom,

        /// <summary>
        /// Utiliza os mapeamentos de atributos {MapsToAttribute} a partir do tipo de origem
        /// </summary>
        UseTo,

        /// <summary>
        /// Utiliza tanto os mapeamentos de atributos {MapsFromAttribute} quanto {MapsToAttribute}
        /// </summary>
        UseFromAndTo
    }
}