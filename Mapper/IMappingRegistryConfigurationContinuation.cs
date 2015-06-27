using System;

namespace RobsonROX.Mapper
{
    /// <summary>
    /// Configura mapeamentos adicionais de um objeto
    /// </summary>
    /// <typeparam name="TSource">Tipo de origem a ser mapeado</typeparam>
    /// <typeparam name="TTarget">Tipo de destino para receber o mapeamento</typeparam>
    public interface IMappingRegistryConfigurationContinuation<out TSource, TTarget>
        where TSource : class
    {
        /// <summary>
        /// Define um mapeamento adicional do tipo de dados de origem para o tipo de dados de destino especificado
        /// </summary>
        /// <param name="mappingFunction">Método que receba um objeto do tipo de origem e um objeto do tipo de destino e popule este objeto de destino, o retornando</param>
        /// <returns>Instância de IMappingRegistryConfigurationContinuation{TSource, TTarget}, que permite a inclusão de mapeamentos adicionais</returns>
        IMappingRegistryConfigurationContinuation<TSource, TTarget> And(Func<TSource, TTarget, TTarget> mappingFunction);
    }

    /// <summary>
    /// Configura mapeamentos adicionais de um objeto
    /// </summary>
    /// <typeparam name="TSource">Tipo de origem a ser mapeado</typeparam>
    /// <typeparam name="TTarget">Tipo de destino para receber o mapeamento</typeparam>
    public interface IMappingRegistryConfigurationContinuationFromConventions<out TSource, TTarget> : IMappingRegistryConfigurationContinuation<TSource, TTarget>
        where TSource : class
    {
        /// <summary>
        /// Define o mapeamento do tipo de dados de origem para o tipo de dados de destino especificado, utilizando o mapeamento padrão, com base em nomenclatura de propriedades e campos e seus tipos
        /// </summary>
        /// <param name="useAttributesEnum">Tipo de mapeamento por atributos a ser utilizado. Por padrão utiliza ambos os tipos.</param>
        /// <returns>Instância de IMappingRegistryConfigurationContinuation{TSource, TTarget}, que permite a inclusão de mapeamentos adicionais</returns>
        IMappingRegistryConfigurationContinuation<TSource, TTarget> AndUseAttributes(UseAttributesEnum useAttributesEnum = UseAttributesEnum.UseFromAndTo);
    }

    /// <summary>
    /// Configura mapeamentos adicionais de um objeto
    /// </summary>
    /// <typeparam name="TSource">Tipo de origem a ser mapeado</typeparam>
    /// <typeparam name="TTarget">Tipo de destino para receber o mapeamento</typeparam>
    public interface IMappingRegistryConfigurationContinuationFromAttributes<out TSource, TTarget> : IMappingRegistryConfigurationContinuation<TSource, TTarget>
        where TSource : class
    {
        /// <summary>
        /// Define o mapeamento do tipo de dados de origem para o tipo de dados de destino especificado, utilizando o mapeamento padrão, com base em nomenclatura de propriedades e campos e seus tipos
        /// </summary>
        /// <returns>Instância de IMappingRegistryConfigurationContinuation{TSource, TTarget}, que permite a inclusão de mapeamentos adicionais</returns>
        IMappingRegistryConfigurationContinuation<TSource, TTarget> AndUseConventions();
    }
}