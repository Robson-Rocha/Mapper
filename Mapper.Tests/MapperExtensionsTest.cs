using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobsonROX.Mapper;

namespace RobsonROX.Testing.Mapper.Tests
{
    [TestClass]
    public class MapperExtensionsTest
    {
        private static Pessoa ObterPessoa()
        {
            var pessoa = new Pessoa { Nome = "Robson", Sobrenome = "Rocha", Idade = 32 };
            return pessoa;
        }

        [TestMethod]
        public void MapToTest()
        {
            var funcionario = new Funcionario();
            Pessoa pessoa = ObterPessoa();
            funcionario = pessoa.MapTo(funcionario);

            Assert.AreEqual(pessoa.Nome, funcionario.Nome);
        }

        [TestMethod]
        public void MapToManyTest()
        {
            var pessoas = new List<Pessoa> {ObterPessoa(), ObterPessoa(), ObterPessoa()};
            var funcionarios = pessoas.MapToMany(new List<Funcionario>());

            foreach (var funcionario in funcionarios)
            {
                Assert.IsNotNull(funcionario);
            }
        }

    }
}
