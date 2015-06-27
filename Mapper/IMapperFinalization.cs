namespace RobsonROX.Mapper
{
    /// <summary>
    /// Métodos para finalização de mapeamento fluente
    /// </summary>
    public interface IMapperFinalization
    {
        /// <summary>
        /// Mapeia o objeto do tipo de origem previamente fornecido para o objeto do tipo de destino fornecido
        /// </summary>
        /// <param name="target">Objeto do tipo de destino para onde os dados do objeto do tipo de origem devem ser mapeados</param>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <returns>Instância do tipo de destino fornecida, preenchida com os dados do objeto de origem</returns>
        TTarget To<TTarget>(TTarget target) where TTarget : class;
    }
}