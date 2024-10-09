using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Back_end.Helpers
{
    /// <summary>
    /// Faz a configuração
    /// </summary>
    public static class JwtHelper
    {
        /// <summary>
        /// Metodo para configurar jwt
        /// </summary>
        /// <param name="services">Coleções de serviços</param>
        /// <param name="configuration">Coleções de serviços</param>
        /// <exception cref="ArgumentNullException">Lançando exeção da configuração do jwt</exception>
        public static void AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Verifica se a chave JWT está configurada
            var jwtKey = configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
            {
                throw new ArgumentNullException("Jwt:Key", "A chave JWT não pode ser nula ou vazia.");
            }

            //Lançando as autentificação de acordo com a coleção de serviços do jwt
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
