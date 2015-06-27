using System;

namespace RobsonROX.Mapper
{
    /// <summary>
    /// Métodos para continuação de mapeamento fluente
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public interface IMapperContinuation<TSource> : IMapperFinalization
        where TSource : class
    {
        /// <summary>
        /// Adiciona uma regra de mapeamento as demais inseridas ou inferidas
        /// </summary>
        /// <param name="mappingAction">Ação que receberá a instância de MappingRegistry{TSource} contendo os mapeamentos já registrados patra TSource, permitindo que se adiiconem novos mapeamentos</param>
        /// <returns>A instância atual de Mapper{TSource}</returns>
        IMapperContinuation<TSource> And(Action<MappingRegistry<TSource>> mappingAction);
    }
}