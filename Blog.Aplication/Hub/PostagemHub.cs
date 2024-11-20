using Microsoft.AspNetCore.SignalR;

namespace Blog.API.Hubs
{
    public class PostagemHub : Hub
    {
        public async Task NotificarNovaPostagem(string mensagem)
        {
            await Clients.All.SendAsync("NovaPostagem", mensagem);
        }
    }
}
