# Criptografia de Imagem com ASP.NET Core

Este é um exemplo de um controlador ASP.NET Core para realizar a criptografia e descriptografia de imagens usando RSA.

## Funcionalidades

### Geração de Par de Chaves

- A rota GET `/Encrypt/GenerateKeyPair` exibe um formulário para gerar um novo par de chaves RSA.
- A rota POST `/Encrypt/GenerateKeyPair` manipula o envio do formulário, gerando um novo par de chaves e exibindo-as na tela.

### Criptografia de Imagem

- A rota GET `/Encrypt/EncryptImage` exibe um formulário para enviar uma imagem a ser criptografada.
- A rota POST `/Encrypt/EncryptImage` manipula o envio do formulário, criptografando a imagem usando a chave pública fornecida e disponibilizando o download da imagem criptografada.

### Descriptografia de Imagem

- A rota GET `/Encrypt/DecryptImage` exibe um formulário para enviar uma imagem criptografada a ser descriptografada.
- A rota POST `/Encrypt/DecryptImage` manipula o envio do formulário, descriptografando a imagem usando a chave privada fornecida e disponibilizando o download da imagem descriptografada.

## Modelo de Dados

O modelo `KeyPairModel` é usado para transportar os dados entre as visualizações e as ações do controlador. Ele contém campos para armazenar a chave privada, chave pública, imagem normal, dados criptografados e um indicador de submissão.

## Tecnologias Utilizadas

- ASP.NET Core: Framework para desenvolvimento de aplicativos web.
- RSA Encryption: Algoritmo de criptografia assimétrica usado para gerar e manipular chaves RSA.
- C#: Linguagem de programação usada para desenvolver o aplicativo.

## Bibliotecas Utilizadas

- `System.Security.Cryptography`: Biblioteca para operações criptográficas, incluindo a implementação de RSA.
- Outras bibliotecas padrão do .NET Framework.

## Como Executar

1. Certifique-se de ter o ambiente ASP.NET Core configurado.
2. Clone este repositório.
3. Execute o projeto e acesse-o no navegador.

## Observações

- Certifique-se de proteger adequadamente as chaves privadas, pois elas são essenciais para a segurança do sistema.
