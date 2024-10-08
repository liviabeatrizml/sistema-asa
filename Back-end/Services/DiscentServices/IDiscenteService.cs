using System.Threading.Tasks;
using Back_end.Models;

public interface IDiscenteService
{
    Task<Discente> RegistrarDiscenteAsync(RegistrarDiscente registrarDiscente);
    Task<string> LoginDiscenteAsync(LoginDiscente loginDiscente);
    Task<bool> EmailJaCadastradoAsync(string email);
    Task<Profissional> RegistrarProfissionalAsync(RegistrarProfissional registrarProfissional);
    Task<string> LoginProfissionalAsync(LoginProfissional loginProfissional);
    Task<bool> AtualizarPerfilAsync(AtualizarPerfilDto atualizarPerfil);
    Task<bool> AlterarSenhaAsync(AlterarSenhaDto alterarSenha);

}
