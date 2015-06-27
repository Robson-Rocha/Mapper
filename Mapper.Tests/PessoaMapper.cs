using RobsonROX.Mapper;

namespace RobsonROX.Testing.Mapper.Tests
{
    internal class PessoaMapper : IMapperRegister<Pessoa>
    {
        public void RegisterMappings(MappingRegistry<Pessoa> mappings)
        {
            mappings.For<Funcionario>().UseConventions().AndUseAttributes().And((pessoa, funcionario) => {
                funcionario.NomeCompleto = pessoa.Nome + " " + pessoa.Sobrenome;
                return funcionario;
            });
        }
    }
}