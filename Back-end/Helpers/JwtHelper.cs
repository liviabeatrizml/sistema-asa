using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Back_end.Helpers
{
    /// <summary>
    /// Classe auxiliar para configuração do JWT.
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Configura a autenticação JWT para a aplicação.
        /// </summary>
        /// <param name="services">A coleção de serviços onde a configuração será adicionada.</param>
        /// <param name="configuration">A configuração da aplicação que contém as chaves do JWT.</param>
        /// <exception cref="ArgumentNullException">Lançado se a chave JWT estiver nula ou vazia.</exception>
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Verifica se a chave JWT está configurada
            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentNullException("Jwt:Key", "A chave JWT não pode ser nula ou vazia.");
            }

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
        }
    }
}
