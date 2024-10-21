using Back_end.Data;
using Microsoft.EntityFrameworkCore;

public class ProfissionalService : IProfissionalService
{
    /// <summary>
    /// Contexto do banco de dados para operações com profissionais.
    /// </summary>
    /// <param name="context">O contexto do banco de dados.</param>
    private readonly ApiDbContext _context;

    public ProfissionalService(ApiDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtém um profissional específico pelo seu ID.
    /// </summary>
    /// <param name="id">O ID do profissional a ser obtido.</param>
    /// <returns>Um objeto <see cref="ProfissionalDto"/> representando o profissional ou null se não encontrado.</returns>
    public async Task<ProfissionalDto> ObterProfissionalPorIdAsync(int id)
    {
        var profissional = await _context.Profissionais
            .Include(p => p.Servico)
            .SingleOrDefaultAsync(p => p.IdProfissional == id);

        if (profissional == null)
        {
            return null;
        }

        return new ProfissionalDto
        {
            Nome = profissional.Nome,
            Email = profissional.Email,
            ServicoId = profissional.ServicoId,
            ServicoNome = profissional.Servico?.Tipo
        };
    }
    
    /// <summary>
    /// Lista todos os profissionais disponíveis.
    /// </summary>
    /// <returns>Uma coleção de objetos <see cref="ProfissionalDto"/> representando os profissionais.</returns>
    public async Task<IEnumerable<ProfissionalDto>> ListarProfissionaisAsync()
    {
        var profissionais = await _context.Profissionais
            .Include(p => p.Servico) // Inclui a relação com o serviço
            .Select(p => new ProfissionalDto
            {
                Nome = p.Nome,
                Email = p.Email,
                ServicoId = p.ServicoId,
                ServicoNome = p.Servico.Tipo // Pegando o nome do serviço
            })
            .ToListAsync();

        return profissionais;
    }
}
