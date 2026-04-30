using AutoMapper;
using CaronaAlvinegra.Application.DTOs;
using CaronaAlvinegra.Domain.Entities;
using CaronaAlvinegra.Domain.Enums;

namespace CaronaAlvinegra.Application.Mapping;

public class DomainToDtoProfile : Profile
{
    public DomainToDtoProfile()
    {
        // Usuario
        CreateMap<Usuario, UsuarioResponse>()
            .ForMember(d => d.RotaNome, o => o.MapFrom(s => s.RotaPreferencial != null ? s.RotaPreferencial.Nome : null))
            .ForMember(d => d.GrupoNome, o => o.MapFrom(s => s.Grupo != null ? s.Grupo.Nome : null));

        // Grupo
        CreateMap<Grupo, GrupoResponse>()
            .ForMember(d => d.TotalMembros, o => o.Ignore()); // Calculado no service

        // Rota
        CreateMap<Rota, RotaResponse>();

        // Jogo
        CreateMap<Jogo, JogoResponse>()
            .ForMember(d => d.TotalPresentes, o => o.Ignore())
            .ForMember(d => d.AlocacaoRealizada, o => o.Ignore());

        // Presenca
        CreateMap<Presenca, PresencaResponse>()
            .ForMember(d => d.UsuarioNome, o => o.MapFrom(s => s.Usuario != null ? s.Usuario.Nome : null))
            .ForMember(d => d.RotaNome, o => o.MapFrom(s => s.RotaEfetiva != null ? s.RotaEfetiva.Nome : null));

        // Passageiro -> PassageiroDto
        CreateMap<Passageiro, PassageiroDto>()
            .ForMember(d => d.Numero, o => o.Ignore())
            .ForMember(d => d.IsLider, o => o.Ignore())
            .ForMember(d => d.Telefone, o => o.Ignore());

        // Veiculo -> VeiculoDto
        CreateMap<Veiculo, VeiculoDto>()
            .ForMember(d => d.TipoDescricao, o => o.MapFrom(s => ObterDescricaoTipo(s.Classificacao)))
            .ForMember(d => d.Passageiros, o => o.Ignore());
    }

    private static string ObterDescricaoTipo(ETipoVeiculo tipo) => tipo switch
    {
        ETipoVeiculo.Van => "Van",
        ETipoVeiculo.DobloSpin => "Doblo/Spin",
        ETipoVeiculo.ListaEspera => "Lista de Espera",
        _ => "Desconhecido"
    };
}
