using System;
using System.Collections.Generic;
using System.Linq;
using RobsonROX.Util.Reflection;

namespace RobsonROX.Mapper
{
    /// <summary>
    /// Registra os mapeamentos de um tipo
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class MappingRegistry<TSource>
        where TSource : class
    {
        private Dictionary<string, object> MappingFunctions { get; }

        internal MappingRegistry()
        {
            MappingFunctions = new Dictionary<string, object>();
        }

        internal MappingRegistry(MappingRegistry<TSource> sourceMappingRegistry)
        {
            MappingFunctions = new Dictionary<string, object>();
            foreach (var mappingFunction in sourceMappingRegistry.MappingFunctions)
            {
                MappingFunctions.Add(mappingFunction.Key, mappingFunction.Value);
            }
        }  

        internal IEnumerable<Func<TSource, TTarget, TTarget>> GetMappersFor<TTarget>()
        {
            Type targetType = TypeCache<TTarget>.Type;
            Type sourceType = TypeCache<TSource>.Type;
            string key = targetType.Name;
            if (!MappingFunctions.ContainsKey(key))
            {
                IMappingRegistryConfigurationContinuation<TSource, TTarget> mappings = null;

                if (TypeCache<TSource>.FieldsAttributes.Values.SelectMany(attributes => attributes).OfType<MapsToAttribute>().Any(a => a.MapToType == targetType) |
                    TypeCache<TSource>.PropertiesAttributes.Values.SelectMany(attributes => attributes).OfType<MapsToAttribute>().Any(a => a.MapToType == targetType))
                {
                    mappings = For<TTarget>().Use(InternalMappers<TSource>.AttributeToMapper);
                }

                if (TypeCache<TTarget>.FieldsAttributes.Values.SelectMany(attributes => attributes).OfType<MapsFromAttribute>().Any(a => a.MapFromType == sourceType) |
                    TypeCache<TTarget>.PropertiesAttributes.Values.SelectMany(attributes => attributes).OfType<MapsFromAttribute>().Any(a => a.MapFromType == sourceType))
                {
                    if (mappings == null)
                        mappings = For<TTarget>().Use(InternalMappers<TSource>.AttributeFromMapper);
                    else
                        mappings.And(InternalMappers<TSource>.AttributeFromMapper);
                }

                if (mappings == null)
                    For<TTarget>().Use(InternalMappers<TSource>.ConventionsMapper);
                else
                    mappings.And(InternalMappers<TSource>.ConventionsMapper);
            }

            var mappingsList = MappingFunctions[key] as List<object>;
            if (mappingsList == null)
                return new List<Func<TSource, TTarget, TTarget>>
                {
                    MappingFunctions[key] as Func<TSource, TTarget, TTarget>
                };
            return mappingsList.Cast<Func<TSource, TTarget, TTarget>>();
        }

        /// <summary>
        /// Define o mapeamento do tipo de dados de origem para o tipo de dados de destino especificado
        /// </summary>
        /// <typeparam name="TTarget">Tipo de dados de destino</typeparam>
        /// <remarks>Caso não seja definido um mapeamento, o mesmo será feito por convenções, ou seja, utilizando nomes e tipos coincidentes de propriedade/campo em ambos os objetos</remarks>
        public MappingRegistryConfiguration<TSource, TTarget> For<TTarget>()
        {
            return new MappingRegistryConfiguration<TSource, TTarget>(MappingFunctions);
        }
    }
}