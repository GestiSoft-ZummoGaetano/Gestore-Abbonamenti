using AutoMapper;
using GestoreAbbonamenti.Model;
using System.Collections;

namespace GestoreAbbonamenti.Logic.Mapping
{
    public class DomainToDtoMappingProfile : Profile
    {
        public DomainToDtoMappingProfile()
        {
            /* #region Mapping OspitiRow 

             CreateMap<ObservabelOspitiRow, OspitiRow>()
                 .ForMember(oo => oo.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(oo => oo.IdOspite, opt => opt.MapFrom(src => src.IdOspite))
                 .ForMember(oo => oo.Permanenza, opt => opt.MapFrom(src => src.Permanenza))
                 .ForMember(oo => oo.DataNascita, opt => opt.MapFrom(src => src.DataNascita))
                 .ForMember(oo => oo.Sesso, opt => opt.MapFrom(src => src.Sesso))
                 .ForMember(oo => oo.Cognome, opt => opt.MapFrom(src => src.Cognome))
                 .ForMember(oo => oo.Nome, opt => opt.MapFrom(src => src.Nome))
                 .ForMember(oo => oo.Cittadinanza, opt => opt.MapFrom(src => src.Cittadinanza))
                 .ForMember(oo => oo.LuoogoNascita, opt => opt.MapFrom(src => src.LuoogoNascita))
                 .ForMember(oo => oo.LuogoResidenza, opt => opt.MapFrom(src => src.LuogoResidenza))
                 .ForMember(oo => oo.PostoLetto, opt => opt.MapFrom(src => src.PostoLetto))
                 .ForMember(oo => oo.Ospite, opt => opt.MapFrom(src => src.Ospite))
                 .ForMember(oo => oo.StatoNascita, opt => opt.MapFrom(src => src.StatoNascita))
                 .PreserveReferences();

             CreateMap<OspitiRow, ObservabelOspitiRow>()
                .ForMember(oo => oo.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(oo => oo.IdOspite, opt => opt.MapFrom(src => src.IdOspite))
                .ForMember(oo => oo.Permanenza, opt => opt.MapFrom(src => src.Permanenza))
                .ForMember(oo => oo.DataNascita, opt => opt.MapFrom(src => src.DataNascita))
                .ForMember(oo => oo.Sesso, opt => opt.MapFrom(src => src.Sesso))
                .ForMember(oo => oo.Cognome, opt => opt.MapFrom(src => src.Cognome))
                .ForMember(oo => oo.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(oo => oo.Cittadinanza, opt => opt.MapFrom(src => src.Cittadinanza))
                .ForMember(oo => oo.LuoogoNascita, opt => opt.MapFrom(src => src.LuoogoNascita))
                .ForMember(oo => oo.LuogoResidenza, opt => opt.MapFrom(src => src.LuogoResidenza))
                .ForMember(oo => oo.PostoLetto, opt => opt.MapFrom(src => src.PostoLetto))
                .ForMember(oo => oo.Ospite, opt => opt.MapFrom(src => src.Ospite))
                .ForMember(oo => oo.StatoNascita, opt => opt.MapFrom(src => src.StatoNascita))
                .PreserveReferences();

             #endregion*/
        }



        public class DtoToDomainMappingProfile : Profile
        {
            public DtoToDomainMappingProfile()
            {
               /* CreateMap<OspitiRow, ObservabelOspitiRow>()
                .PreserveReferences();

                CreateMap<ObservabelOspitiRow, OspitiRow>()
                .PreserveReferences();*/              

            }
        }
    }
}
