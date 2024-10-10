using ToDosProject.Domain.Entities;

namespace ToDosProject.Web;

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
}
