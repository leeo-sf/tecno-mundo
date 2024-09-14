# TecnoMundo

Um projeto que decidi desenvolver para impulsionar meus conhecimentos e habilidades.

O Projeto utiliza tecnologias do mercado (Está detalhado mais abaixo). Todo o backend foi desenvolvido em .NET 8 com autenticação e autorização JWT, banco de dados MySQL, AutoMapper, CORS Etc. Já o frontend desenvolido em Angular 17, junto com, angular material, bootstrap e Etc.
Resumidamente o projeto se baseia em um eCommerce de produtos tecnologicos onde é possível adicionar produtos no carrinho, realizar a compra e acompanhar os pedidos realizados, etc. (Mais detalhes abaixo).

Projeto responsivo para aplicativos móveis.

## Funcionalidades

- __Criar Conta__: Alguns dados são solicitados como Nome, Sobrenome, CPF, Telefone, Email, Senha e Role. Tem Algumas validação de dados como CPF, Email e Telefone.
- __Login__: Informa email e senha, caso estejam válidos o serviço gera um Token JWT e você tem acesso as funcionalidades que seu Role permite.
- __Busca por Produtos__: Você pode visualizar todos os produtos em estoque ou filtrar por nome (todos os produtos que contenha o nome digitado serão listados) ou filtrar todos os produtos daquela categoria. Por Exemplo (Vídeo Game, Headset, Notebook, Etc.).
  - CRUD: CREATE, UPDATE e DELETE, opções disponíveis para usuários Admin.
- __Carrinho de compras__:
  - __Adicionar items__: Ao visualizar os produtos na página você pode adicionar quantos produtos desejar em seu carrinho.
  - __Alterar carrinho__: Em seu carrinho de compras você pode alterar a quantidade do produto presente.
  - __Excluir items__: Em seu carrinho de compras você pode excluir items.
- __Seus pedidos__: Você pode visualizar os produtos que você comprou.
- __Cupom__: Prestes a finalizar a compra você pode inserir 1 cupom cadastrado que terá desconto na compra.
- __Pagamento__: O serviço de pagamento é uma simulação. Mas ao finalizar uma compra ele vai até o RabbitMQ e processa seu pedido concluindo o pagamento.


## Tecnologias Utilizadas

- CLOUD
- AWS | EC2 | RDS
- GitHub Actions | CI-CD
- Docker | Docker Compose
- RabbitMQ
- .NET 8
- MySQL
- Angular 9, Angular Materiale e Bootstrap
