using static GestoreAbbonamenti.Logic.Mapping.DomainToDtoMappingProfile;

namespace GestoreAbbonamenti.Logic.Mapping;
public static class AutoMapperConfiguration
{
    #region Public methods

    public static void Configure()
    {
        MapperWrapper.Initialize(cfg =>
        {
            cfg.AddProfile<DomainToDtoMappingProfile>();
            cfg.AddProfile<DtoToDomainMappingProfile>();
            //cfg.CreateMissingTypeMaps = false;
            cfg.AllowNullCollections = true;
            cfg.AllowNullDestinationValues = true;
        });

        try
        {
        }
        catch (Exception e)
        {
            //Logger.Error(e);
        }
    }

    #endregion
}
