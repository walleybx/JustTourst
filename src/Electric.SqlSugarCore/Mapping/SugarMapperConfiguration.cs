using AutoMapper;

namespace Electric.SqlSugarCore.Mapping
{
    public class SugarMapperConfiguration : MapperConfiguration
    {
        public SugarMapperConfiguration(MapperConfigurationExpression configurationExpression)
            : base(configurationExpression)
        {
        }

        public SugarMapperConfiguration(Action<IMapperConfigurationExpression> configure)
            : base(configure)
        {

        }
    }
}