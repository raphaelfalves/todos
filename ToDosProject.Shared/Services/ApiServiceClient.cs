using System.Net.Http.Json;
using System.Text;
using ToDosProject.Domain.DTOs;
using ToDosProject.Domain.Entities;

namespace ToDosProject.Shared.Services;

public class ApiServiceClient(HttpClient httpClient)
{
    public async Task<ToDo[]> GetToDosAsync(CancellationToken cancellationToken = default)
    {
        IEnumerable<ToDo>? toDos = [];

        var response = await httpClient.GetAsync("/todoitems", cancellationToken);

        if (response.IsSuccessStatusCode)
            toDos = await response.Content.ReadFromJsonAsync<IEnumerable<ToDo>>(cancellationToken);

        return toDos!.ToArray();
    }

    public async Task<ToDo?> CreateToDosAsync(ToDo? toDo, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync("/todoitems", toDo, cancellationToken);

        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<ToDo>(cancellationToken);

        return null;
    }

    public async Task<bool> DeleteToDoAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.DeleteAsync($"/todoitems/{id}", cancellationToken);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateToDoAsync(ToDo toDo, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PutAsJsonAsync($"/todoitems/{toDo.Id}", toDo, cancellationToken);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UnconcludeToDoAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsJsonAsync($"/todoitems/Unconclude/{id}", new StringContent(""), cancellationToken);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ConcludeToDoAsync(int id, CancellationToken cancellationToken = default)
    {
        var response = await httpClient.PostAsync($"/todoitems/Conclude/{id}", new StringContent(""), cancellationToken);

        return response.IsSuccessStatusCode;
    }
    public async Task<Response<string>> Login(LoginDTO loginRequest)
    {
        try
        {
            var result = await httpClient.PostAsJsonAsync("/login?useCookies=true", loginRequest);
            return result.IsSuccessStatusCode
            ? new(200, "Login realizado com sucesso!")
            : new(400, "Erro ao fazer login");
        }
        catch (Exception ex)
        {

            Console.WriteLine(ex);
            throw;
        }

        
    }

    public async Task<Response<string>> Register(RegisterDTO register)
    {
        var result = await httpClient.PostAsJsonAsync("/register", register);

        return result.IsSuccessStatusCode
            ? new(200, "Cadastro realizado com sucesso!")
            : new(400, "Erro ao fazer cadastro");
    }

    public async Task<User?> Info()
    {
        try
        {
            return await httpClient.GetFromJsonAsync<User?>("/manage/info");
        }
        catch
        {
            return null;
        }
    }

    public async Task Logout()
    {
        var emptyContent = new StringContent("{}", Encoding.UTF8, mediaType: "application/json");
        await httpClient.PostAsJsonAsync(requestUri: "/logout", emptyContent);
    }
}
