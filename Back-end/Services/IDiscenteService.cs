using System.Threading.Tasks;
using Back_end.Models;

public interface IDiscenteService
{
    Task<Discente> RegistrarDiscenteAsync(RegistrarDiscente registrarDiscente);
    Task<string> LoginDiscenteAsync(LoginDiscente loginDiscente);
    Task<bool> EmailJaCadastradoAsync(string email); // Verificar se o email já foi cadastrado
    Task<Profissional> RegistrarProfissionalAsync(RegistrarProfissional registrarProfissional);
    Task<string> LoginProfissionalAsync(LoginProfissional loginProfissional);

}
