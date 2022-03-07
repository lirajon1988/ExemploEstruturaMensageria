# ExemploEstruturaMensageria
Projetos de exemplo para simular o processo de produtor e consumidor de mensagens usando RabbitMQ.

Os projetos e soluções foram executados instalando a imagem e criando um container utilizando o tutorial de instalação do Rabbit MQ disponível no site oficial do Rabbit MQ:

      https://www.rabbitmq.com/download.html

Em resumo, se tiver o Docker Desktop instalado na máquina pode instalar a imagem usando o comando abaixo no Prompt de comando ou no Powershell:

      docker run -d -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management
 
Para abrir o manager do Rabbit MQ, basta digitar no browser o endereço localhost:15672, utilizando a senha de convidado padrão (user: guest, senha: guest)

Os projetos em formato de API não estão com Swagger habilitado, sendo necessário testar os métodos com Postman.


#Estrutura Mensageria

O projeto que consolida o modelo é o que está na pasta EstruturaMensageria

Os projetos estão estruturados da seguinte forma:


- Produtor: projeto para produzir mensagens.
- Consumidor: projeto em background services para consumir as mensagens
- Rabbit: projeto com métodos para inserir as mensagens para fila
- Domain Classes: projeto librarires com Classes POCO utilizadas por todos os outros projetos.
- ConsumidorConsole: projeto consumidor em formato console para facilitar a execução.


# Tutoriais RabbitMQ
As outras pastas estão os projetos com os tutoriais do RabbitMQ.

- PublishSubscribe: Padrão para enviar as mensagens para um exchange, que vai se encarregar de enviar as mensagens para os consumidores
- Routing: Padrão para direcionar a mensagem para uma mais filas, conforme a configuração de rota determinada no Exchange