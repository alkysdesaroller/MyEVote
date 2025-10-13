
using AutoMapper;
using myEVote.Application.DTOs;
using myEVote.Domain.Entities;
using myEVote.Domain.Enums;
using myEVote.Application.DTOs.Account;
using myEVote.Application.DTOs.Ciudadano;
using myEVote.Application.DTOs.Usuario;
using myEVote.Application.DTOs.PuestoElectivo;
using myEVote.Application.DTOs.PartidoPolitico;
using myEVote.Application.DTOs.Candidato;
using myEVote.Application.DTOs.CandidatoPuesto;
using myEVote.Application.DTOs.DirigentePolitico;
using myEVote.Application.DTOs.AlianzaPolitica;
using myEVote.Application.DTOs.SolicitudAlianza;
using myEVote.Application.DTOs.Eleccion;
using myEVote.Application.DTOs.Voto;
namespace myEVote.Application.Mappings;

public class GeneralPerfil : Profile 
{
     public GeneralPerfil()
        {
            #region Ciudadano
            CreateMap<Ciudadano, CiudadanoDto>()
                .ReverseMap();

            CreateMap<SaveCiudadanoDto, Ciudadano>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => EstadoEntidad.Activo))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.Votos, opt => opt.Ignore());
            #endregion

            #region Usuario
            CreateMap<Usuario, UsuarioDto>()
                .ReverseMap()
                .ForMember(dest => dest.DirigentePartido, opt => opt.Ignore());

            CreateMap<SaveUsuarioDto, Usuario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => EstadoEntidad.Activo))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.DirigentePartido, opt => opt.Ignore());

            CreateMap<Usuario, LoginDto>()
                .ForMember(dest => dest.Rol, opt => opt.MapFrom(src => src.RolUsuario.ToString()))
                .ForMember(dest => dest.PartidoPoliticoId, opt => opt.MapFrom(src => src.DirigentePartido != null ? src.DirigentePartido.PartidoPoliticoId : (int?)null))
                .ForMember(dest => dest.PartidoPoliticoNombre, opt => opt.MapFrom(src => src.DirigentePartido != null ? src.DirigentePartido!.PartidoPolitico!.Nombre : null))
                .ForMember(dest => dest.PartidoPoliticoSiglas, opt => opt.MapFrom(src => src.DirigentePartido != null ? src.DirigentePartido!.PartidoPolitico!.Siglas : null))
                .ForMember(dest => dest.PartidoPoliticoLogoUrl, opt => opt.MapFrom(src => src.DirigentePartido != null ? src.DirigentePartido!.PartidoPolitico!.LogoUrl : null));
            #endregion

            #region PuestoElectivo
            CreateMap<PuestoElectivo, PuestoElectivoDto>()
                .ReverseMap()
                .ForMember(dest => dest.CandidatoPuestos, opt => opt.Ignore());

            CreateMap<SavePuestoElectivoDto, PuestoElectivo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => EstadoEntidad.Activo))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.CandidatoPuestos, opt => opt.Ignore());
            #endregion

            #region PartidoPolitico
            CreateMap<PartidoPolitico, PartidoPoliticoDto>()
                .ReverseMap()
                .ForMember(dest => dest.Candidatos, opt => opt.Ignore())
                .ForMember(dest => dest.DirigentePartidos, opt => opt.Ignore())
                .ForMember(dest => dest.AlianzasComoPartido1, opt => opt.Ignore())
                .ForMember(dest => dest.AlianzasComoPartido2, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudesEnviadas, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudesRecibidas, opt => opt.Ignore());

            CreateMap<SavePartidoPoliticoDto, PartidoPolitico>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => EstadoEntidad.Activo))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.Candidatos, opt => opt.Ignore())
                .ForMember(dest => dest.DirigentePartidos, opt => opt.Ignore())
                .ForMember(dest => dest.AlianzasComoPartido1, opt => opt.Ignore())
                .ForMember(dest => dest.AlianzasComoPartido2, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudesEnviadas, opt => opt.Ignore())
                .ForMember(dest => dest.SolicitudesRecibidas, opt => opt.Ignore());
            #endregion

            #region Candidato
            CreateMap<Candidato, CandidatoDto>()
                .ForMember(dest => dest.PartidoPoliticoNombre, opt => opt.MapFrom(src => src.PartidoPolitico!.Nombre))
                .ForMember(dest => dest.PartidoPoliticoSiglas, opt => opt.MapFrom(src => src.PartidoPolitico!.Siglas))
                .ForMember(dest => dest.PartidoPoliticoLogoUrl, opt => opt.MapFrom(src => src.PartidoPolitico!.LogoUrl))
                .ForMember(dest => dest.PuestoElectivoNombre, opt => opt.Ignore());

            CreateMap<CandidatoDto, Candidato>()
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore())
                .ForMember(dest => dest.CandidatoPuestos, opt => opt.Ignore())
                .ForMember(dest => dest.Votos, opt => opt.Ignore());

            CreateMap<SaveCandidatoDto, Candidato>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => EstadoEntidad.Activo))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore())
                .ForMember(dest => dest.CandidatoPuestos, opt => opt.Ignore())
                .ForMember(dest => dest.Votos, opt => opt.Ignore());
            #endregion

            #region CandidatoPuesto
            CreateMap<CandidatoPuesto, CandidatoPuestoDto>()
                .ForMember(dest => dest.CandidatoNombre, opt => opt.MapFrom(src => src.Candidato!.Nombre))
                .ForMember(dest => dest.CandidatoApellido, opt => opt.MapFrom(src => src.Candidato!.Apellido))
                .ForMember(dest => dest.CandidatoFotoUrl, opt => opt.MapFrom(src => src.Candidato!.FotoUrl))
                .ForMember(dest => dest.PuestoElectivoNombre, opt => opt.MapFrom(src => src.PuestoElectivo!.Nombre))
                .ForMember(dest => dest.PartidoPoliticoNombre, opt => opt.MapFrom(src => src.PartidoPolitico!.Nombre))
                .ForMember(dest => dest.PartidoPoliticoSiglas, opt => opt.MapFrom(src => src.PartidoPolitico!.Siglas));

            CreateMap<SaveCandidatoPuestoDto, CandidatoPuesto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.Candidato, opt => opt.Ignore())
                .ForMember(dest => dest.PuestoElectivo, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore());
            #endregion

            #region DirigentePartido
            CreateMap<DirigentePartido, DirigentePartidoDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuario!.Nombre))
                .ForMember(dest => dest.UsuarioApellido, opt => opt.MapFrom(src => src.Usuario!.Apellido))
                .ForMember(dest => dest.PartidoPoliticoNombre, opt => opt.MapFrom(src => src.PartidoPolitico!.Nombre))
                .ForMember(dest => dest.PartidoPoliticoSiglas, opt => opt.MapFrom(src => src.PartidoPolitico!.Siglas));

            CreateMap<SaveDirigentePartidoDto, DirigentePartido>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore());
            #endregion

            #region AlianzaPolitica
            CreateMap<AlianzaPolitica, AlianzaPoliticaDto>()
                .ForMember(dest => dest.PartidoPolitico1Nombre, opt => opt.MapFrom(src => src.PartidoPolitico1!.Nombre))
                .ForMember(dest => dest.PartidoPolitico1Siglas, opt => opt.MapFrom(src => src.PartidoPolitico1!.Siglas))
                .ForMember(dest => dest.PartidoPolitico2Nombre, opt => opt.MapFrom(src => src.PartidoPolitico2!.Nombre))
                .ForMember(dest => dest.PartidoPolitico2Siglas, opt => opt.MapFrom(src => src.PartidoPolitico2!.Siglas));
            #endregion

            #region SolicitudAlianza
            CreateMap<SolicitudAlianza, SolicitudAlianzaDto>()
                .ForMember(dest => dest.PartidoSolicitanteNombre, opt => opt.MapFrom(src => src.PartidoSolicitante!.Nombre))
                .ForMember(dest => dest.PartidoSolicitanteSiglas, opt => opt.MapFrom(src => src.PartidoSolicitante!.Siglas))
                .ForMember(dest => dest.PartidoReceptorNombre, opt => opt.MapFrom(src => src.PartidoReceptor!.Nombre))
                .ForMember(dest => dest.PartidoReceptorSiglas, opt => opt.MapFrom(src => src.PartidoReceptor!.Siglas));

            CreateMap<SaveSolicitudAlianzaDto, SolicitudAlianza>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaSolicitud, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => EstadoSolicitud.EnProceso))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoSolicitante, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoReceptor, opt => opt.Ignore());
            #endregion

            #region Eleccion
            CreateMap<Eleccion, EleccionDto>()
                .ForMember(dest => dest.CantidadPartidos, opt => opt.Ignore())
                .ForMember(dest => dest.CantidadCandidatos, opt => opt.Ignore())
                .ForMember(dest => dest.CantidadPuestos, opt => opt.Ignore())
                .ForMember(dest => dest.TotalVotos, opt => opt.Ignore());

            CreateMap<SaveEleccionDto, Eleccion>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => EstadoEleccion.EnProceso))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.Votos, opt => opt.Ignore());
            #endregion

            #region Voto
            CreateMap<SaveVotoDto, Voto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FechaVoto, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.FechaModificacion, opt => opt.Ignore())
                .ForMember(dest => dest.Creador, opt => opt.Ignore())
                .ForMember(dest => dest.Modificador, opt => opt.Ignore())
                .ForMember(dest => dest.Ciudadano, opt => opt.Ignore())
                .ForMember(dest => dest.Candidato, opt => opt.Ignore())
                .ForMember(dest => dest.PuestoElectivo, opt => opt.Ignore())
                .ForMember(dest => dest.Eleccion, opt => opt.Ignore());
            #endregion
        }
}