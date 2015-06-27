namespace RobsonROX.Testing.Mapper.Tests
{
    internal class NullableTestsFrom
    {
        public int? Idade { get; set; }
        public decimal? Altura { get; set; }
        public decimal Peso { get; set; }
    }

    internal class NullableTestsTo
    {
        public int? Idade { get; set; }
        public decimal Altura { get; set; }
        public decimal? Peso { get; set; }
    }

}
