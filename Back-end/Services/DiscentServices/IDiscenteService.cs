using System.Threading.Tasks;
using Back_end.Models;

/// <summary>
/// Interface que define os contratos que devem ser implementados pela
/// classe DiscenteService
/// </summary>
public interface IDiscenteService
{
    Task<Discente> RegistrarDiscenteAsync(RegistrarDiscente registrarDiscente);
    Task<LoginResponseDto> LoginDiscenteAsync(LoginDiscente loginDiscente);
    Task<bool> EmailJaCadastradoAsync(string email);
    Task<Profissional> RegistrarProfissionalAsync(RegistrarProfissional registrarProfissional);
    Task<LoginResponseDto> LoginProfissionalAsync(LoginProfissional loginProfissional);
    Task<bool> AtualizarPerfilAsync(AtualizarPerfilDto atualizarPerfil);
    Task<bool> AlterarSenhaAsync(AlterarSenhaDto alterarSenha);

}
