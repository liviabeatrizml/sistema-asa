using System.Threading.Tasks;
using Back_end.Models;

/// <summary>
/// Interface que define os contratos que devem ser implementados pela
/// classe DiscenteService
/// </summary>
public interface IDiscenteService
{
    Task<Discente> RegistrarDiscenteAsync(RegistrarDiscente registrarDiscente);
    Task<string> LoginDiscenteAsync(LoginDiscente loginDiscente);
    Task<bool> EmailJaCadastradoAsync(string email); // Verificar se o email já foi cadastrado
    Task<Profissional> RegistrarProfissionalAsync(RegistrarProfissional registrarProfissional);
    Task<string> LoginProfissionalAsync(LoginProfissional loginProfissional);

}
