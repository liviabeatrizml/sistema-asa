public interface IProfissionalService
{
    Task<ProfissionalDto> ObterProfissionalPorIdAsync(int id);
    Task<IEnumerable<ProfissionalDto>> ListarProfissionaisAsync();
}
