using System.Threading.Tasks;
using Back_end.Models;

public interface IDiscenteService
{
    Task<Discente> RegistrarDiscenteAsync(RegistrarDiscente registrarDiscente);
    Task<LoginResponseDto> LoginDiscenteAsync(LoginDiscente loginDiscente);
    Task<bool> EmailJaCadastradoAsync(string email);
    Task<Profissional> RegistrarProfissionalAsync(RegistrarProfissional registrarProfissional);
    Task<LoginResponseDto> LoginProfissionalAsync(LoginProfissional loginProfissional);
    Task<bool> AtualizarPerfilAsync(AtualizarPerfilDto atualizarPerfil);
    Task<bool> AlterarSenhaAsync(AlterarSenhaDto alterarSenha);
    Task<DiscenteDto> ObterDiscentePorIdAsync(int id);
    Task<bool> AtualizarPerfilCompletoAsync(AtualizarPerfilDto atualizarPerfil);
    Task<bool> AtualizarPerfilParcialAsync(AtualizarPerfilDto atualizarPerfil);
    Task<bool> DeletarUsuarioAsync(int id, string senha);
}
