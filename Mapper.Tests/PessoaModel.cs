using System;
using RobsonROX.Mapper;

namespace RobsonROX.Testing.Mapper.Tests
{
    internal class PessoaModel
    {
        [MapsFrom(typeof(Pessoa), "Nome")]
        [MapsTo(typeof(Pessoa), "Nome")]
        public string MeuNome { get; set; }

        [MapsFrom(typeof(Pessoa), "Sobrenome")]
        [MapsTo(typeof(Pessoa), "Sobrenome")]
        public string MeuSobrenome { get; set; }

        [MapsFrom(typeof(Pessoa), "Idade")]
        [MapsTo(typeof(Pessoa), "Idade")]
        public int MinhaIdade;

        [MapsFrom(typeof(Pessoa), "Nascimento")]
        [MapsTo(typeof(Pessoa), "Nascimento")]
        public DateTime MeuNascimento { get; set; }

        [MapsFrom(typeof (Pessoa), "Nascimento")]
        [MapsTo(typeof(Pessoa), "Nascimento")]
        public DateTime Nascimento2;
    }
}
