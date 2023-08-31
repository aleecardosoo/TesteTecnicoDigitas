# TesteTecnicoDigitas

O Objetivo do teste é disponibilizar uma API que consome dados a partir de um websocket e realizar o cálculo do melhor preço para compra e venda do ativo naquele momento.

## Execução

Para rodar o projeto localmente, é necesário que o Docker esteja em execução. Acesse a pasta /app/src e execute o comando
>docker-compose up
após executar esse comando o ambiente com a aplicação e as dependências necessárias serão provisionados.

## Collection

Está disponibilizada uma collection do Insomnia que contém a requisição para a API.

>http://localhost:80/api/bestprice

### Requisição

|            | required | type    | examples             | description                                                                        |
|------------|----------|---------|----------------------|------------------------------------------------------------------------------------|
| operation  | yes      | string  | "buy", "sell"        | Tipo de operação a ser feita ("buy", "sell")                                       |
| instrument | yes      | string  | "BTC/USD", "ETH/USD" | Instrumento a ser comprado/vendido ("BTC/USD", "ETH/USD")                          |
| quantity   | yes      | decimal | 10, 5, 1.5, 0.457    | Quantidade a ser comprada/vendida, aceita unidades fracionadas (10, 5, 1.5, 0.457) |

### Resposta

|            | type    | examples                                                                     | description                                                                        |
|------------|---------|------------------------------------------------------------------------------|------------------------------------------------------------------------------------|
| id         | guid    | "5e28ee27-c330-4c9d-9f92-590dcffb8550"                                       | Identificador único do cálculo                                                     |
| collection | list    | [["1854.0","12.80000000"],["1854.0","12.80000000"],["1854.0","12.80000000"]] | Coleção contendo os valores e quantidades utilizados para o cálculo                |
| quantity   | decimal | 10, 5, 1.5, 0.457                                                            | Quantidade a ser comprada/vendida, aceita unidades fracionadas (10, 5, 1.5, 0.457) |
| operation  | string  | "buy", "sell"                                                                | Tipo de operação a ser feita ("buy", "sell")                                       |
| result     | decimal | 9270.0                                                                       | O melhor valor para a compra ou venda da quantidade requisitada                    |

