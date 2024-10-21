using Back_end.Models;

/// <summary>
/// Interface para serviços relacionados a profissionais.
/// </summary>
public interface IProfissionalService
{
    Task<ProfissionalDto> ObterProfissionalPorIdAsync(int id);
    Task<IEnumerable<ProfissionalDto>> ListarProfissionaisAsync();    

}
