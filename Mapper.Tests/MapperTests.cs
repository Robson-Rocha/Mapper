using System;
using System.Collections.Generic;
using RobsonROX.Mapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RobsonROX.Testing.Mapper.Tests
{
    [TestClass]
    public class MapperTests
    {
        private static Pessoa ObterPessoa()
        {
            var pessoa = new Pessoa {Nome = "Robson", Sobrenome = "Rocha", Idade = 32, Nascimento = DateTime.Parse("18/01/1982")};
            return pessoa;
        }

        [TestMethod]
        public void DefaultMappingTest()
        {
            var pessoa = ObterPessoa();
            var funcionario = Mapper<Pessoa>.Map(pessoa).UsingDefaultMappings().To(new Funcionario());

            Assert.IsNotNull(funcionario);
            Assert.IsNotNull(funcionario.Nome);
            Assert.IsNotNull(funcionario.SobreNome);
            Assert.AreNotEqual(0, funcionario.Idade);
            Assert.AreEqual(0, funcionario.Matricula);
        }

        [TestMethod]
        public void MappingToNullTest()
        {
            var pessoa = ObterPessoa();
            try
            {
                Mapper<Pessoa>.Map(pessoa).UsingDefaultMappings().To<Funcionario>(null);
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentNullException);
                return;
            }
            Assert.Fail();
        }


        [TestMethod]
        public void DefaultMappingToExistingTest()
        {
            var pessoa = ObterPessoa();
            var funcionario = new Funcionario {Nome = "Lorem", SobreNome = "Ipsum", Idade = 55, Matricula = 123456};

            Mapper<Pessoa>.Map(pessoa).UsingDefaultMappings().To(funcionario);

            Assert.IsNotNull(funcionario);
            Assert.AreEqual(pessoa.Nome, funcionario.Nome);
            Assert.AreEqual(pessoa.Sobrenome, funcionario.SobreNome);
            Assert.AreEqual(pessoa.Idade, funcionario.Idade);
            Assert.AreEqual(123456, funcionario.Matricula);
        }

        [TestMethod]
        public void CustomMappingTest()
        {
            var pessoa = ObterPessoa();
            var funcionario = Mapper<Pessoa>.Map(pessoa).To(new Funcionario());

            Assert.IsNotNull(funcionario);
            Assert.IsNotNull(funcionario.Nome);
            Assert.IsNotNull(funcionario.SobreNome);
            Assert.AreNotEqual(0, funcionario.Idade);
            Assert.AreEqual(0, funcionario.Matricula);
            Assert.AreEqual(pessoa.Nome + " " + pessoa.Sobrenome, funcionario.NomeCompleto);
        }

        [TestMethod]
        public void SpecificMappingTest()
        {
            var pessoa = ObterPessoa();
            var funcionario = Mapper<Pessoa>.Map(pessoa).UsingOnly<PessoaMapper>().To(new Funcionario());

            Assert.IsNotNull(funcionario);
            Assert.IsNotNull(funcionario.Nome);
            Assert.IsNotNull(funcionario.SobreNome);
            Assert.AreNotEqual(0, funcionario.Idade);
            Assert.AreEqual(0, funcionario.Matricula);
            Assert.AreEqual(pessoa.Nome + " " + pessoa.Sobrenome, funcionario.NomeCompleto);
        }

        [TestMethod]
        public void FieldCustomMappingTest()
        {
            var ponto = new Ponto {X = 10, Y = 20};
            var dimensao = Mapper<Ponto>.Map(ponto).To(new Dimensao());

            Assert.AreEqual(ponto.X, dimensao.Left);
            Assert.AreEqual(ponto.Y, dimensao.Top);
        }

        [TestMethod]
        public void FieldToFieldDefaultMappingTest()
        {
            var ponto = new Ponto {X = 10, Y = 20};
            var dimensao = Mapper<Ponto>.Map(ponto).UsingDefaultMappings().To(new Dimensao());

            Assert.AreEqual(ponto.X, dimensao.X);
            Assert.AreEqual(ponto.Y, dimensao.Y);
        }

        [TestMethod]
        public void FieldToPropertyDefaultMappingTest()
        {
            var ponto = new Ponto {A = 10, B = 20};
            var dimensao = Mapper<Ponto>.Map(ponto).UsingDefaultMappings().To(new Dimensao());

            Assert.AreEqual(ponto.A, dimensao.A);
            Assert.AreEqual(ponto.B, dimensao.B);
        }

        [TestMethod]
        public void PropertyToFieldDefaultMappingTest()
        {
            var ponto = new Ponto {C = 10, D = 20};
            var dimensao = Mapper<Ponto>.Map(ponto).UsingDefaultMappings().To(new Dimensao());

            Assert.AreEqual(ponto.C, dimensao.C);
            Assert.AreEqual(ponto.D, dimensao.D);
        }


        [TestMethod]
        public void MapsFromAttributeTests()
        {
            var pessoa = ObterPessoa();
            var pessoaModel = Mapper<Pessoa>.Map(pessoa).To(new PessoaModel());

            Assert.AreEqual(pessoa.Nome, pessoaModel.MeuNome);
            Assert.AreEqual(pessoa.Sobrenome, pessoaModel.MeuSobrenome);
            Assert.AreEqual(pessoa.Idade, pessoaModel.MinhaIdade);
            Assert.AreEqual(pessoa.Nascimento, pessoaModel.MeuNascimento);
            Assert.AreEqual(pessoa.Nascimento, pessoaModel.Nascimento2);
        }

        [TestMethod]
        public void MapsToAttributeTests()
        {
            var pessoaModel = Mapper<Pessoa>.Map(ObterPessoa()).To(new PessoaModel());
            var pessoa = Mapper<PessoaModel>.Map(pessoaModel).To(new Pessoa());

            Assert.AreEqual(pessoa.Nome, pessoaModel.MeuNome);
            Assert.AreEqual(pessoa.Sobrenome, pessoaModel.MeuSobrenome);
            Assert.AreEqual(pessoa.Idade, pessoaModel.MinhaIdade);
            Assert.AreEqual(pessoa.Nascimento, pessoaModel.MeuNascimento);
            Assert.AreEqual(pessoa.Nascimento, pessoaModel.Nascimento2);
        }

        [TestMethod]
        public void MixedTests()
        {
            DefaultMappingTest();
            FieldCustomMappingTest();

        }

        [TestMethod]
        public void NullableTests()
        {
            var origem = new NullableTestsFrom {Altura = null, Idade = 32, Peso = 140M};
            var destino = Mapper<NullableTestsFrom>.Map(origem).To(new NullableTestsTo());
            Assert.AreEqual(0, destino.Altura);
        }


        [TestMethod]
        public void ListMappingSameTypeTest()
        {
            var origem = new Departamento
            {
                Nome = "TI",
                Funcionarios = new List<Funcionario>
                {
                    new Funcionario {Nome = "Lorem", SobreNome = "Ipsum", Idade = 55, Matricula = 123456},
                    new Funcionario {Nome = "Dolor", SobreNome = "Sit", Idade = 44, Matricula = 789012},
                    new Funcionario {Nome = "Consectetuer", SobreNome = "Adipiscin", Idade = 33, Matricula = 345678},
                }
            };
            var destino = Mapper<Departamento>.Map(origem).WithCloning().To(new Departamento());
            Assert.AreEqual(origem.Funcionarios.Count, destino.Funcionarios.Count);
            Assert.AreEqual(origem.Funcionarios[0].Nome, destino.Funcionarios[0].Nome);
            Assert.AreNotEqual(origem.Funcionarios.GetHashCode(), destino.Funcionarios.GetHashCode());
        }
    }
}
