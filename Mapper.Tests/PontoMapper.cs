using RobsonROX.Mapper;

namespace RobsonROX.Testing.Mapper.Tests
{
    internal class PontoMapper : IMapperRegister<Ponto>
    {
        public void RegisterMappings(MappingRegistry<Ponto> mappings)
        {
            mappings.For<Dimensao>().Use((ponto, dimensao) =>
            {
                dimensao.Left = ponto.X;
                return dimensao;
            }).And((ponto, dimensao) =>
            {
                dimensao.Top = ponto.Y;
                return dimensao;
            }).And((ponto, dimensao) =>
            {
                dimensao.Depth = ponto.Z;
                return dimensao;
            });
        }
    }
}