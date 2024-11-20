Documentação do Sistema de Blog

#Este documento detalha os conceitos, práticas e decisões de design utilizadas no desenvolvimento do sistema de blog, com foco especial nas controllers, arquitetura geral e uso de bibliotecas como o MediatR.

1. Estrutura das Controllers
1.1 Uso da BaseController
A criação de uma classe base chamada BaseController foi adotada com o objetivo de:

Evitar repetição de código:

Muitas controllers possuem lógica repetitiva, como manipulação de respostas de APIs (Ok, BadRequest, etc.).
A BaseController encapsula essa lógica para ser reutilizada.
Uniformidade:

As respostas das controllers seguem um padrão único, garantindo consistência no retorno das APIs.
Fácil manutenção:

Alterações na manipulação de respostas podem ser feitas apenas na BaseController, propagando automaticamente para todas as controllers que a herdam.
Estrutura da BaseController
A BaseController usa o MediatR para simplificar a comunicação entre a camada de apresentação (controllers) e as camadas de aplicação e infraestrutura.

![image](https://github.com/user-attachments/assets/5912ce00-c541-42b9-8154-ca7db1f7e82a)


Documentação do Sistema de Blog
Este documento detalha os conceitos, práticas e decisões de design utilizadas no desenvolvimento do sistema de blog, com foco especial nas controllers, arquitetura geral e uso de bibliotecas como o MediatR.

1. Estrutura das Controllers
1.1 Uso da BaseController
A criação de uma classe base chamada BaseController foi adotada com o objetivo de:

Evitar repetição de código:

Muitas controllers possuem lógica repetitiva, como manipulação de respostas de APIs (Ok, BadRequest, etc.).
A BaseController encapsula essa lógica para ser reutilizada.
Uniformidade:

As respostas das controllers seguem um padrão único, garantindo consistência no retorno das APIs.
Fácil manutenção:

Alterações na manipulação de respostas podem ser feitas apenas na BaseController, propagando automaticamente para todas as controllers que a herdam.
Estrutura da BaseController
A BaseController usa o MediatR para simplificar a comunicação entre a camada de apresentação (controllers) e as camadas de aplicação e infraestrutura.

csharp
Copiar código
public abstract class BaseController : ControllerBase
{
    private readonly IMediator _mediator;

    protected BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }

    protected async Task<IActionResult> RequestService<TRequest, TResponse>(
        TRequest request,
        Func<Response<TResponse>, IActionResult> successResult,
        Func<string, IActionResult> failureResult) where TRequest : IRequest<Response<TResponse>>
    {
        var result = await _mediator.Send(request);

        if (result == null || !result.Success)
        {
            return failureResult(result?.Message ?? "Erro ao processar a requisição");
        }

        return successResult(result);
    }
}
1.2 Benefícios do IMediator
O MediatR é um padrão que ajuda a desacoplar as camadas do sistema, promovendo:

Comunicação centralizada:

As controllers não possuem dependências diretas com as camadas de aplicação ou infraestrutura, apenas interagem com o MediatR.
Manutenção mais fácil:

Alterações na lógica de negócios podem ser feitas nos handlers, sem impactar diretamente as controllers.
Organização:

As responsabilidades são bem divididas, com cada handler focado em uma operação específica.
2. Fluxo de Requisições
2.1 Exemplo de Fluxo
Ao fazer uma solicitação de criação de postagem na controller, o seguinte fluxo ocorre:

Controller recebe o request:

Um objeto da classe CriarPostagemRequest é enviado para a API.
Controller chama o RequestService:

A lógica é passada para o MediatR, que encontra o handler correspondente.
Handler executa a lógica de negócios:

O handler InserirPostagemHandler valida os dados, executa comandos na infraestrutura e retorna um resultado.
Controller retorna a resposta:

A resposta (ex.: 201 Created) é enviada ao cliente.
3. Organização do Sistema
3.1 Camadas
O sistema foi projetado usando princípios de Clean Architecture, dividindo responsabilidades em camadas bem definidas:

Camada de Apresentação (Controllers):

Responsável por receber e processar as requisições HTTP.
Utiliza a BaseController e o MediatR para delegar responsabilidades.
Camada de Aplicação:

Contém os handlers que implementam a lógica de negócios.
Interage com os stores (query e command) da camada de infraestrutura.
Camada de Infraestrutura:

Contém os stores responsáveis por interagir com o banco de dados.
Ex.: PostagemCommandStore, UsuarioQueryStore.
Banco de Dados:

Usado para persistir e recuperar dados. Foi utilizado o Entity Framework com suporte a PostgreSQL.
4. Testes
4.1 Cobertura de Testes
Foram desenvolvidos testes unitários para as principais partes do sistema:

Handlers:
Validação das regras de negócios implementadas.
Simulação de dependências com o uso do Moq.
Stores:
Testes diretos com um banco em memória (InMemoryDatabase).
Garantia de que operações de criação, leitura, atualização e exclusão (CRUD) estão funcionando corretamente.
5. Notificações em Tempo Real
5.1 Uso do SignalR
A biblioteca SignalR foi integrada ao sistema para enviar notificações em tempo real para os clientes. Isso foi implementado para informar os usuários sempre que uma nova postagem for criada.

Hub SignalR:

Um hub chamado PostagemHub foi criado para gerenciar conexões de WebSocket.
Uso nos Handlers:

Exemplo: O handler InserirPostagemHandler envia uma mensagem ao hub quando uma postagem é criada.
