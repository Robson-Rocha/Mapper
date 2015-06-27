using System;

namespace RobsonROX.Testing.Mapper.Tests
{
    [Serializable]
    internal class Funcionario
    {
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string SobreNome { get; set; }
        public int Idade { get; set; }
        public string NomeCompleto { get; set; }
    }
}