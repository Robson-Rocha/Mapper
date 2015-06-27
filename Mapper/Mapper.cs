using System;
using RobsonROX.Util.Extensions;
using RobsonROX.Util.Reflection;

namespace RobsonROX.Mapper
{
    //TODO: [RobsonROX] Criar mecanismo de mapper para converter Model (MVC) <==> ViewModel (JS+KO/Angular/Backbone)
    /// <summary>
    /// Contém métodos para mapear (converter) classes de um tipo de origem para um tipo de destino, 
    /// permitindo definir opcionalmente código para realizar o mapeamento entre os tipos
    /// </summary>
    /// <typeparam name="TSource">Tipo de dados de origem para o mapeamento</typeparam>
    public sealed class Mapper<TSource> : IMapperContinuation<TSource>
        where TSource : class
    {
        #region Membros estáticos

        private static readonly MappingRegistry<TSource> ResolvedMappings;

        static Mapper()
        {
            ResolvedMappings = new MappingRegistry<TSource>();
            var types = Reflection.GetAssignableTypesFrom<IMapperRegister<TSource>>();
            types.ForEach(t => t.GetInstance<IMapperRegister<TSource>>().RegisterMappings(ResolvedMappings));
        }

        /// <summary>
        /// Inicia o mapeamento do objeto de origem fornecido, permitindo que o mesmo seja mapeado para um objeto de destino
        /// </summary>
        /// <param name="source">Objeto de origem</param>
        /// <returns>Instância de Mapper{TSource}, de onde o objeto do tipo de origem poderá ser mapeado para um objeto de destino</returns>
        public static Mapper<TSource> Map(TSource source)
        {
            return new Mapper<TSource>(source, new MappingRegistry<TSource>(ResolvedMappings), false);
        }

        #endregion

        
        private readonly TSource _source;

        private MappingRegistry<TSource> _mappings;

        private bool _withCloning;

        private Mapper(TSource source, MappingRegistry<TSource> mappings, bool withCloning)
        {
            _source = source;
            _mappings = mappings;
            _withCloning = withCloning;
        }

        /// <summary>
        /// Configura o mapper para clonar os tipos de referência ao invés de simplesmente copiar suas referências
        /// </summary>
        /// <param name="withCloning">Se true, ativa a clonagem. Se false, a desativa</param>
        /// <returns>Instância atual de Mapper{TSource}, configurada para clonagem como especificado</returns>
        public Mapper<TSource> WithCloning(bool withCloning = true)
        {
            _withCloning = withCloning;
            return this;
        }

        /// <summary>
        /// Desconsidera os mapeamentos carregados e utiliza apenas o mapeamento fornecido
        /// </summary>
        /// <typeparam name="TMapper">Tipo derivado de IMapperRegister{TSource}</typeparam>
        /// <returns>Nova instância de Mapper{TSource}, de onde o objeto do tipo de origem poderá ser mapeado para um objeto de destino, porém sem mapeamentos customizados</returns>
        public IMapperFinalization UsingOnly<TMapper>()
            where TMapper : IMapperRegister<TSource>, new()
        {
            var mapper = new TMapper();
            _mappings = new MappingRegistry<TSource>();
            mapper.RegisterMappings(_mappings);
            return this;
        }

        /// <summary>
        /// Desconsidera os mapeamentos carregados e utiliza apenas os mapeamentos padrões, baseados em convenções e atributos
        /// </summary>
        /// <returns>Nova instância de Mapper{TSource}, de onde o objeto do tipo de origem poderá ser mapeado para um objeto de destino, porém sem mapeamentos customizados</returns>
        public IMapperContinuation<TSource> UsingDefaultMappings()
        {
            return new Mapper<TSource>(_source, new MappingRegistry<TSource>(), _withCloning);
        }

        /// <summary>
        /// Adiciona uma regra de mapeamento as demais inseridas ou inferidas
        /// </summary>
        /// <param name="mappingAction">Ação que receberá a instância de MappingRegistry{TSource} contendo os mapeamentos já registrados patra TSource, permitindo que se adiiconem novos mapeamentos</param>
        /// <returns>A instância atual de Mapper{TSource}</returns>
        public IMapperContinuation<TSource> Using(Action<MappingRegistry<TSource>> mappingAction)
        {
            mappingAction(_mappings);
            return this;
        }

        /// <summary>
        /// Adiciona uma regra de mapeamento as demais inseridas ou inferidas
        /// </summary>
        /// <param name="mappingAction">Ação que receberá a instância de MappingRegistry{TSource} contendo os mapeamentos já registrados patra TSource, permitindo que se adiiconem novos mapeamentos</param>
        /// <returns>A instância atual de Mapper{TSource}</returns>
        IMapperContinuation<TSource> IMapperContinuation<TSource>.And(Action<MappingRegistry<TSource>> mappingAction)
        {
            return Using(mappingAction);
        }

        /// <summary>
        /// Mapeia o objeto do tipo de origem previamente fornecido para o objeto do tipo de destino fornecido
        /// </summary>
        /// <param name="target">Objeto do tipo de destino para onde os dados do objeto do tipo de origem devem ser mapeados</param>
        /// <typeparam name="TTarget">Tipo de destino</typeparam>
        /// <returns>Instância do tipo de destino fornecida, preenchida com os dados do objeto de origem</returns>
        public TTarget To<TTarget>(TTarget target)
            where TTarget : class
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            TTarget ret = target;
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var mapping in _mappings.GetMappersFor<TTarget>())
            {
                ret = mapping.Invoke(_withCloning ? _source.Clone() : _source, ret);
            }
            return ret;
        }
    }
}