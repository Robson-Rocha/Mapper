namespace RobsonROX.Mapper
{
    /// <summary>
    /// Define método para registro de mapeamentos da classe Mapper{T}
    /// </summary>
    public interface IMapperRegister<TSource>
        where TSource : class
    {
        /// <summary>
        /// Registra os mapeamentos
        /// </summary>
        void RegisterMappings(MappingRegistry<TSource> mappings);
    }
}