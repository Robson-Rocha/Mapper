using System;
using System.Linq;
using System.Reflection;
using RobsonROX.Util.Reflection;

namespace RobsonROX.Mapper
{
    internal class InternalMappers<TSource>
        where TSource : class
    {
        //Duplicar método para testes mais amplos
        //A versão original preferencialmente deve considerar heranças
        private static bool CompareTypes(Type leftType, Type rightType)
        {
            Type actualLeftType = Nullable.GetUnderlyingType(leftType) ?? leftType;
            Type actualRightType = Nullable.GetUnderlyingType(rightType) ?? rightType;
            
            //Testar se os tipos são idênticos
            return actualLeftType == actualRightType;
            //Testar se existem conversões implícitas e/ou explícitas e/ou primitiva
            //Testar se a classe convert possui alguma conversão implementada
            //Retornar qual o tipo de teste que passou
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static bool MapPropertyToField<TTarget>(TSource source, TTarget target, string propertyName, PropertyInfo sourceProperty)
        {
            // Comparar apenas os nomes. 
            var targetField = TypeCache<TTarget>.Fields.Values.FirstOrDefault(fi => fi.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase) &&
                                                                                    CompareTypes(fi.FieldType, sourceProperty.PropertyType));
            if (targetField == null) return false;

            // Usar a versão original do CompareTypes
            if (TypeCache<TSource>.PropertiesAttributes[sourceProperty.Name]
                                  .OfType<DontMapToAttribute>()
                                  .Any(a => CompareTypes(a.DontMapToType, TypeCache<TTarget>.Type) &&
                                            a.MemberName.Equals(targetField.Name, StringComparison.OrdinalIgnoreCase)))
                return false;

            // Usar a versão original do CompareTypes
            if (TypeCache<TTarget>.FieldsAttributes[targetField.Name]
                                  .OfType<DontMapFromAttribute>()
                                  .Any(a => CompareTypes(a.DontMapFromType, TypeCache<TSource>.Type) &&
                                            a.MemberName.Equals(sourceProperty.Name, StringComparison.OrdinalIgnoreCase)))
                return false;

            // se os tipos forem identicos ou implicitamente conversiveis
            targetField.SetValue(target, sourceProperty.GetValue(source));

            // se os tipos permitirem conversao explicita

            // se houver metodo convert

            // se tudo mais falhar, usar outro mapper
            return true;
        }

        private static bool MapPropertyToProperty<TTarget>(TSource source, TTarget target, string propertyName, PropertyInfo sourceProperty)
        {
            var targetProperty = TypeCache<TTarget>.Properties.Values.FirstOrDefault(pi => pi.CanWrite &&
                                                                                           pi.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase) &&
                                                                                           CompareTypes(pi.PropertyType, sourceProperty.PropertyType));

            if (targetProperty == null) return false;

            if (TypeCache<TSource>.PropertiesAttributes[sourceProperty.Name]
                                  .OfType<DontMapToAttribute>()
                                  .Any(a => CompareTypes(a.DontMapToType, TypeCache<TTarget>.Type) &&
                                            a.MemberName.Equals(targetProperty.Name, StringComparison.OrdinalIgnoreCase)))
                return false;

            if (TypeCache<TTarget>.PropertiesAttributes[targetProperty.Name]
                                  .OfType<DontMapFromAttribute>()
                                  .Any(a => CompareTypes(a.DontMapFromType, TypeCache<TSource>.Type) &&
                                            a.MemberName.Equals(sourceProperty.Name, StringComparison.OrdinalIgnoreCase)))
                return false;
            
            targetProperty.SetValue(target, sourceProperty.GetValue(source));
            return true;
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private static bool MapFieldToProperty<TTarget>(TSource source, TTarget target, string fieldName, FieldInfo sourceField)
        {
            var targetProperty = TypeCache<TTarget>.Properties.Values.FirstOrDefault(pi => pi.CanWrite &&
                                                                                           pi.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase) &&
                                                                                           CompareTypes(pi.PropertyType, sourceField.FieldType));
            if (targetProperty == null) return false;

            if (TypeCache<TSource>.FieldsAttributes[sourceField.Name]
                                  .OfType<DontMapToAttribute>()
                                  .Any(a => CompareTypes(a.DontMapToType, TypeCache<TTarget>.Type) &&
                                            a.MemberName.Equals(targetProperty.Name, StringComparison.OrdinalIgnoreCase)))
                return false;

            if (TypeCache<TTarget>.PropertiesAttributes[targetProperty.Name]
                                  .OfType<DontMapFromAttribute>()
                                  .Any(a => CompareTypes(a.DontMapFromType, TypeCache<TSource>.Type) &&
                                            a.MemberName.Equals(sourceField.Name, StringComparison.OrdinalIgnoreCase)))
                return false;
            
            targetProperty.SetValue(target, sourceField.GetValue(source));
            return true;
        }

        private static bool MapFieldToField<TTarget>(TSource source, TTarget target, string fieldName, FieldInfo sourceField)
        {
            var targetField = TypeCache<TTarget>.Fields.Values.FirstOrDefault(fi => fi.Name.Equals(fieldName, StringComparison.OrdinalIgnoreCase) &&
                                                                                    CompareTypes(fi.FieldType, sourceField.FieldType));
            
            if (targetField == null) return false;

            if (TypeCache<TSource>.FieldsAttributes[sourceField.Name]
                                  .OfType<DontMapToAttribute>()
                                  .Any(a => CompareTypes(a.DontMapToType, TypeCache<TTarget>.Type) &&
                                            a.MemberName.Equals(targetField.Name)))
                return false;

            if (TypeCache<TTarget>.FieldsAttributes[targetField.Name]
                                  .OfType<DontMapFromAttribute>()
                                  .Any(a => CompareTypes(a.DontMapFromType, TypeCache<TSource>.Type) &&
                                            a.MemberName.Equals(sourceField.Name)))
                return false;

            targetField.SetValue(target, sourceField.GetValue(source));
            return true;
        }

        internal static TTarget AttributeToMapper<TTarget>(TSource source, TTarget target)
        {
            var targetType = TypeCache<TTarget>.Type;

            foreach (FieldInfo field in TypeCache<TSource>.Fields.Values)
            {
                foreach (MapsToAttribute attribute in TypeCache<TSource>.FieldsAttributes[field.Name]
                                                                        .OfType<MapsToAttribute>()
                                                                        .Where(a => a.MapToType == targetType))
                {
                    if (TypeCache<TTarget>.Fields.ContainsKey(attribute.MemberName))
                    {
                        MapFieldToField(source, target, attribute.MemberName, field);
                        continue;
                    }
                    if (TypeCache<TTarget>.Properties.ContainsKey(attribute.MemberName))
                    {
                        MapFieldToProperty(source, target, attribute.MemberName, field);
                    }
                }
            }

            foreach (PropertyInfo property in TypeCache<TSource>.Properties.Values)
            {
                foreach (MapsToAttribute attribute in TypeCache<TSource>.PropertiesAttributes[property.Name]
                                                                        .OfType<MapsToAttribute>()
                                                                        .Where(a => a.MapToType == targetType))
                {
                    if (TypeCache<TTarget>.Fields.ContainsKey(attribute.MemberName))
                    {
                        MapPropertyToField(source, target, attribute.MemberName, property);
                        continue;
                    }
                    if (TypeCache<TTarget>.Properties.ContainsKey(attribute.MemberName))
                    {
                        MapPropertyToProperty(source, target, attribute.MemberName, property);
                    }
                }
            }

            return target;
        }


        internal static TTarget AttributeFromMapper<TTarget>(TSource source, TTarget target)
        {
            var sourceType = TypeCache<TSource>.Type;

            foreach (FieldInfo field in TypeCache<TTarget>.Fields.Values)
            {
                foreach (MapsFromAttribute attribute in TypeCache<TTarget>.FieldsAttributes[field.Name]
                                                                          .OfType<MapsFromAttribute>()
                                                                          .Where(a => a.MapFromType == sourceType))
                {
                    if (TypeCache<TSource>.Fields.ContainsKey(attribute.MemberName))
                    {
                        MapFieldToField(source, target, field.Name, TypeCache<TSource>.Fields[attribute.MemberName]);
                        continue;
                    }
                    if (TypeCache<TSource>.Properties.ContainsKey(attribute.MemberName))
                    {
                        MapPropertyToField(source, target, field.Name, TypeCache<TSource>.Properties[attribute.MemberName]);
                    }
                }
            }

            foreach (PropertyInfo property in TypeCache<TTarget>.Properties.Values)
            {
                foreach (MapsFromAttribute attribute in TypeCache<TTarget>.PropertiesAttributes[property.Name]
                                                                          .OfType<MapsFromAttribute>()
                                                                          .Where(a => a.MapFromType == sourceType))
                {
                    if (TypeCache<TSource>.Fields.ContainsKey(attribute.MemberName))
                    {
                        MapFieldToProperty(source, target, property.Name, TypeCache<TSource>.Fields[attribute.MemberName]);
                        continue;
                    }
                    if (TypeCache<TSource>.Properties.ContainsKey(attribute.MemberName))
                    {
                        MapPropertyToProperty(source, target, property.Name, TypeCache<TSource>.Properties[attribute.MemberName]);
                    }
                }
            }

            return target;
        }


        internal static TTarget ConventionsMapper<TTarget>(TSource source, TTarget target)
        {
            foreach (FieldInfo field in TypeCache<TSource>.Fields.Values)
            {
                if (MapFieldToField(source, target, field.Name, field)) continue;
                MapFieldToProperty(source, target, field.Name, field);
            }

            foreach (PropertyInfo property in TypeCache<TSource>.Properties.Values.Where(pi => pi.CanRead))
            {
                if (MapPropertyToProperty(source, target, property.Name, property)) continue;
                MapPropertyToField(source, target, property.Name, property);
            }

            return target;
        }
    }
}
