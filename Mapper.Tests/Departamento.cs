using System;
using System.Collections.Generic;

namespace RobsonROX.Testing.Mapper.Tests
{
    [Serializable]
    internal class Departamento
    {
        public string Nome { get; set; }
        public List<Funcionario> Funcionarios { get; set; }
    }
}