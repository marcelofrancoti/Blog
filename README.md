Documentação do Sistema de Blog

#Este documento detalha os conceitos, práticas e decisões de design utilizadas no desenvolvimento do sistema de blog, com foco especial nas controllers, arquitetura geral e uso de bibliotecas como o MediatR.

*1. Estrutura das Controllers
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

*4. Testes
![image](https://github.com/user-attachments/assets/8dcb748b-3a9a-4d6f-9d79-c874146069cb)

4.1 Cobertura de Testes
Foram desenvolvidos testes unitários para as principais partes do sistema:

Handlers:
Validação das regras de negócios implementadas.
Simulação de dependências com o uso do Moq.
Stores:
Testes diretos com um banco em memória (InMemoryDatabase).
Garantia de que operações de criação, leitura, atualização e exclusão (CRUD) estão funcionando corretamente.
*5. Notificações em Tempo Real
5.1 Uso do SignalR
A biblioteca SignalR foi integrada ao sistema para enviar notificações em tempo real para os clientes. Isso foi implementado para informar os usuários sempre que uma nova postagem for criada.

Hub SignalR:

Um hub chamado PostagemHub foi criado para gerenciar conexões de WebSocket.
Uso nos Handlers:

Exemplo: O handler InserirPostagemHandler envia uma mensagem ao hub quando uma postagem é criada.

await _hubContext.Clients.All.SendAsync("NovaPostagem", $"Nova postagem criada: {postagem.Titulo}");


*6. Padrões de Nomeação
Handlers:

Nomeados com base na ação executada. Ex.: InserirPostagemHandler.
Requests e Responses:

Nome das classes indica claramente o propósito. Ex.: CriarPostagemRequest, BuscarUsuariosRequest.
Stores:

Divididos em CommandStore (ações que alteram o estado) e QueryStore (ações de leitura).
Ex.: PostagemCommandStore, UsuarioQueryStore.

*7. Configuração do Servidor (API)
Certifique-se de que o servidor está configurado corretamente para suportar o SignalR:

Endpoint do SignalR:

O endpoint do hub deve estar registrado no arquivo Program.cs:

![image](https://github.com/user-attachments/assets/3d698b58-2784-488f-b05d-937e73136fa4)

Isso garante que o SignalR escutará no caminho /hubs/postagem.

Executar o Servidor:

Compile e execute a aplicação (Blog.API) para iniciar o servidor. Certifique-se de que a API está funcionando corretamente no navegador.

*8 testar o WebSocket
Abrir o HTML no Navegador:

Salve o arquivo HTML em seu computador, por exemplo, como index.html.
Abra o arquivo em um navegador.
Iniciar o Servidor:

Certifique-se de que a API está em execução e o SignalR configurado corretamente.
Criar uma Postagem:
Use o Swagger ou outra ferramenta para criar uma nova postagem na API. O handler responsável enviará uma mensagem ao hub SignalR.
Verificar Notificações:

A mensagem de nova postagem deve aparecer na lista da página HTML automaticamente.

![image](https://github.com/user-attachments/assets/2b5b3326-eb0d-4e45-9f8f-cbb16a6c34ed)

*9. Por que usar Entity Framework e Migrations?
Entity Framework (EF):

Abstração do Banco de Dados: O EF permite trabalhar com bancos de dados usando classes C# (Code-First), eliminando a necessidade de escrever consultas SQL complexas diretamente.
Manutenção Simplificada: Com o EF, as mudanças no modelo de dados são refletidas automaticamente no banco de dados, simplificando ajustes e evoluções no sistema.
Integração com LINQ: A facilidade de usar LINQ para manipular dados torna o desenvolvimento mais produtivo e legível.


*9.1. Migrations:

Gerenciamento de Alterações: As migrations documentam e aplicam alterações no banco de dados ao longo do tempo, garantindo a consistência entre o modelo de dados e o esquema do banco.
Automação: Gera scripts SQL automaticamente com base nas alterações feitas no modelo, reduzindo erros manuais.
Versões do Banco de Dados: Permite rastrear e reverter mudanças, útil para ambientes de desenvolvimento, teste e produção.
Resumo
Entity Framework e Migrations foram usados para:

Reduzir Complexidade: Abstrair as interações com o banco de dados.
Manter Consistência: Sincronizar o modelo de dados do sistema com o banco de dados.
Facilitar Evoluções: Gerenciar alterações no banco com segurança e rastreabilidade.

![image](https://github.com/user-attachments/assets/249fc473-befb-4eb4-8936-cb640af9fe58)

*10. Conclusão
Este sistema foi projetado com foco em desacoplamento, manutenção facilitada e escalabilidade. O uso do MediatR e da BaseController promove uma arquitetura limpa, enquanto as notificações em tempo real com SignalR oferecem uma experiência moderna aos usuários. O código é extensível, e as práticas adotadas garantem um sistema robusto e organizado.


