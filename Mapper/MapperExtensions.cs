using System.Collections.Generic;
using RobsonROX.Util.Extensions;

namespace RobsonROX.Mapper
{
    /// <summary>
    /// Métodos de extensão para mapeamento de tipos
    /// </summary>
    public static class MapperExtensions
    {
        /// <summary>
        /// Inicia o mapeamento do objeto de origem fornecido, permitindo que o mesmo seja mapeado para um objeto de destino
        /// </summary>
        /// <param name="source">Objeto de origem a ser mapeado para o objeto de destino</param>
        /// <typeparam name="TSource">Tipo do objeto de origem</typeparam>
        /// <returns>Instância de Mapper{TSource}, de onde o objeto do tipo de origem poderá ser mapeado para um objeto de destino</returns>
        public static Mapper<TSource> Map<TSource>(this TSource source) 
            where TSource : class
        {
            return Mapper<TSource>.Map(source);
        }

        /// <summary>
        /// Mapeia o objeto de origem extendido para o objeto de destino fornecido
        /// </summary>
        /// <typeparam name="TSource">Tipo de origem</typeparam>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <param name="source">Objeto de origem</param>
        /// <param name="target">Objeto de destino</param>
        /// <returns>Objeto de destino, já preenchido com as informações do objeto de origem</returns>
        public static TTarget MapTo<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            return source.Map().To(target);
        }

        /// <summary>
        /// Mapeia os objetos contidos na coleção extendida para uma nova coleção de instâncias do tipo de destino
        /// </summary>
        /// <typeparam name="TSource">Tipo de origem</typeparam>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <param name="source">Coleção do tipo de origem</param>
        /// <param name="target">Coleção do tipo de destino</param>
        /// <returns>Nova coleção de instâncias do tipo de destino, já preenchidas com as informações dos objetos de origem</returns>
        public static IList<TTarget> MapToMany<TSource, TTarget>(this IEnumerable<TSource> source, IList<TTarget> target)
            where TSource : class
            where TTarget : class, new()
        {
            source.ForEach(s => target.Add(s.MapTo(new TTarget())));
            return target;
        }
    }
}
