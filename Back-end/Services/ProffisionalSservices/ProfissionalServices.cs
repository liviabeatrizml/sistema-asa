using Back_end.Data;
using Microsoft.EntityFrameworkCore;

public class ProfissionalService : IProfissionalService
{
    private readonly ApiDbContext _context;

    public ProfissionalService(ApiDbContext context)
    {
        _context = context;
    }

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
