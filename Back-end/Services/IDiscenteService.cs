using System.Threading.Tasks;
using Back_end.Models;

public interface IDiscenteService
{
    Task<Discente> RegistrarDiscenteAsync(RegistrarDiscente registrarDiscente);
    Task<string> LoginDiscenteAsync(LoginDiscente loginDiscente);
}
