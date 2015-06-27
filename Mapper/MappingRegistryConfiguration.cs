using System;
using System.Collections.Generic;
using RobsonROX.Util.Extensions;
using RobsonROX.Util.Reflection;

namespace RobsonROX.Mapper
{
    /// <summary>
    /// Configura o mapeamento de um objeto
    /// </summary>
    /// <typeparam name="TSource">Tipo de origem a ser mapeado</typeparam>
    /// <typeparam name="TTarget">Tipo de destino para receber o mapeamento</typeparam>
    public sealed class MappingRegistryConfiguration<TSource, TTarget> : IMappingRegistryConfigurationContinuationFromConventions<TSource, TTarget>,
                                                                  IMappingRegistryConfigurationContinuationFromAttributes<TSource, TTarget>
        where TSource : class
    {
        private readonly Dictionary<string, object> _mappings;

        internal MappingRegistryConfiguration(Dictionary<string, object> mappings)
        {
            _mappings = mappings;
        }

        /// <summary>
        /// Define o mapeamento do tipo de dados de origem para o tipo de dados de destino especificado, desconsiderando os existentes
        /// </summary>
        /// <param name="mappingFunction">Método que receba um objeto do tipo de origem e um objeto do tipo de destino e popule este objeto de destino, o retornando</param>
        /// <remarks>Caso não seja definido um mapeamento, o mesmo será feito por convenções, ou seja, utilizando nomes e tipos coincidentes de propriedade/campo em ambos os objetos</remarks>
        public IMappingRegistryConfigurationContinuation<TSource, TTarget> Use(Func<TSource, TTarget, TTarget> mappingFunction)
        {
            _mappings.UpdateOrAdd(TypeCache<TTarget>.Type.Name, () => mappingFunction);
            return this;
        }


        /// <summary>
        /// Define o mapeamento do tipo de dados de origem para o tipo de dados de destino especificado, utilizando o mapeamento padrão, com base em nomenclatura de propriedades e campos e seus tipos
        /// </summary>
        public IMappingRegistryConfigurationContinuationFromConventions<TSource, TTarget> UseConventions()
        {
            Use(InternalMappers<TSource>.ConventionsMapper);
            return this;
        }

        /// <summary>
        /// Define o mapeamento do tipo de dados de origem para o tipo de dados de destino especificado, utilizando o mapeamento padrão, com base em nomenclatura de propriedades e campos e seus tipos
        /// </summary>
        /// <param name="useAttributesEnum">Tipo de mapeamento por atributos a ser utilizado. Por padrão utiliza ambos os tipos.</param>
        /// <returns></returns>
        public IMappingRegistryConfigurationContinuationFromAttributes<TSource, TTarget> UseAttributes(UseAttributesEnum useAttributesEnum = UseAttributesEnum.UseFromAndTo)
        {
            if(useAttributesEnum.In(UseAttributesEnum.UseFrom, UseAttributesEnum.UseFromAndTo))
                Use(InternalMappers<TSource>.AttributeFromMapper);
            if (useAttributesEnum.In(UseAttributesEnum.UseTo, UseAttributesEnum.UseFromAndTo))
                Use(InternalMappers<TSource>.AttributeToMapper);
            return this;
        }

        #region IMappingRegistryConfigurationContinuation
        /// <summary>
        /// Define um mapeamento adicional do tipo de dados de origem para o tipo de dados de destino especificado
        /// </summary>
        /// <param name="mappingFunction">Método que receba um objeto do tipo de origem e um objeto do tipo de destino e popule este objeto de destino, recebendo também a informação se os valores devem ser clonados ou não, o retornando</param>
        /// <returns>Instância de IMappingRegistryConfigurationContinuation{TSource, TTarget}, que permite a inclusão de mapeamentos adicionais</returns>
        IMappingRegistryConfigurationContinuation<TSource, TTarget> IMappingRegistryConfigurationContinuation<TSource, TTarget>.And(Func<TSource, TTarget, TTarget> mappingFunction)
        {
            var currentMapping = _mappings[TypeCache<TTarget>.Type.Name];
            var mappingList = currentMapping as List<object>;
            if (mappingList != null)
            {
                mappingList.Add(mappingFunction);
            }
            else
            {
                _mappings[TypeCache<TTarget>.Type.Name] = new List<object> { currentMapping, mappingFunction };
            }
            return this;
        }
        #endregion

        #region IMappingRegistryConfigurationContinuationFromConventions
        /// <summary>
        /// Define o mapeamento do tipo de dados de origem para o tipo de dados de destino especificado, utilizando o mapeamento padrão, com base em nomenclatura de propriedades e campos e seus tipos
        /// </summary>
        /// <param name="useAttributesEnum">Tipo de mapeamento por atributos a ser utilizado. Por padrão utiliza ambos os tipos.</param>
        /// <returns>Instância de IMappingRegistryConfigurationContinuation{TSource, TTarget}, que permite a inclusão de mapeamentos adicionais</returns>
        IMappingRegistryConfigurationContinuation<TSource, TTarget> IMappingRegistryConfigurationContinuationFromConventions<TSource, TTarget>.AndUseAttributes(UseAttributesEnum useAttributesEnum)
        {
            if (useAttributesEnum.In(UseAttributesEnum.UseFrom, UseAttributesEnum.UseFromAndTo))
                (this as IMappingRegistryConfigurationContinuation<TSource, TTarget>).And(InternalMappers<TSource>.AttributeFromMapper);
            if (useAttributesEnum.In(UseAttributesEnum.UseTo, UseAttributesEnum.UseFromAndTo))
                (this as IMappingRegistryConfigurationContinuation<TSource, TTarget>).And(InternalMappers<TSource>.AttributeToMapper);
            return this;
        }
        #endregion

        #region IMappingRegistryConfigurationContinuationFromAttributes
        /// <summary>
        /// Define o mapeamento do tipo de dados de origem para o tipo de dados de destino especificado, utilizando o mapeamento padrão, com base em nomenclatura de propriedades e campos e seus tipos
        /// </summary>
        /// <returns>Instância de IMappingRegistryConfigurationContinuation{TSource, TTarget}, que permite a inclusão de mapeamentos adicionais</returns>
        IMappingRegistryConfigurationContinuation<TSource, TTarget> IMappingRegistryConfigurationContinuationFromAttributes<TSource, TTarget>.AndUseConventions()
        {
            return (this as IMappingRegistryConfigurationContinuation<TSource, TTarget>).And(InternalMappers<TSource>.ConventionsMapper);
        }
        #endregion
    }
}