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
}
