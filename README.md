# TecnoMundo

Projeto backend em API's ASP.NET 8, frontend em Angular 9. Projeto voltado a vendas de produtos tecnológicos.

## Funcionalidades

- __Criar Conta__: Alguns dados são solicitados como Nome, Sobrenome, CPF, Telefone, Email, Senha e Role. Tem Algumas validação de dados como CPF, Email e Telefone.
- __Login__: Informa email e senha, caso estejam válidos o serviço gera um Token JWT e você tem acesso as funcionalidades que seu Role permite.
- __Busca por Produtos__: Você pode visualizar todos os produtos em estoque ou filtrar por nome (todos os produtos que contenha o nome digitado serão listados) ou filtrar todos os produtos daquela categoria. Por Exemplo (Vídeo Game, Headset, Notebook, Etc.).
- __Carrinho de compras__:
  - __Adicionar items__: Ao visualizar os produtos na página você pode adicionar quantos produtos desejar em seu carrinho.
  - __Alterar carrinho__: Em seu carrinho de compras você pode alterar a quantidade do produto presente.
  - __Excluir items__: Em seu carrinho de compras você pode excluir items.
- __Seus pedidos__: Você pode visualizar os produtos que você comprou.
- __Cupom__: Prestes a finalizar a compra você pode inserir 1 cupom cadastrado que terá desconto na compra.
- __Pagamento__: O serviço de pagamento é uma simulação. Mas ao finalizar uma compra ele vai até o RabbitMQ e processa seu pedido concluindo o pagamento.


## Tecnologias Utilizadas

- Docker compose.
- RabbitMQ.
- .NET 8.
- Banco de dados MySQL.
- Angular 9.
