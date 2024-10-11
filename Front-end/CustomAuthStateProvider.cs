using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

public class CustomAuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorage;

    public CustomAuthStateProvider(ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Tente obter o token do localStorage
        var token = await localStorage.GetItemAsync<string>("authToken");

        var identity = string.IsNullOrEmpty(token)
            ? new ClaimsIdentity()
            : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        NotifyAuthenticationStateChanged(Task.FromResult(state));
        
        return state;
    }

    public async Task<int> GetUserIdFromLocalStorage()
    {
        // Obter o valor armazenado no localStorage
        var authDataJson = await localStorage.GetItemAsync<string>("authToken");

        if (!string.IsNullOrEmpty(authDataJson))
        {
            // Desserializar o JSON para um objeto
            var authData = JsonSerializer.Deserialize<Dictionary<string, object>>(authDataJson);
            
            // Acessar o userId a partir do objeto
            if (authData.ContainsKey("userId"))
            {
                var userId = int.Parse(authData["userId"].ToString());
                return userId;
            }
        }
        
        return -1; // Retorna -1 se o userId não for encontrado
    }

    public void NotifyUserAuthentication(string token)
    {
        var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt"));
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        NotifyAuthenticationStateChanged(authState);
    }

    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        // Lógica para analisar as reivindicações do token JWT
        var payload = jwt.Split('.')[1]; // Parte do payload do JWT
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    private byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        return Convert.FromBase64String(base64);
    }

}
