# ExemploEstruturaMensageria
Projetos como exemplo produtor e consumidor de mensagens usando RabbitMQ

Executei o projeto instalando a imagem e criando um container utilizando o tutorial de instalação do Rabbit MQ disponível no site oficial do Rabbit MQ:

  https://www.rabbitmq.com/download.html

Em resumo, se tiver o docker instalado na máquina pode instalar a imagem usando o comando abaixo:

  docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management
  
  
 Para abrir o manager do Rabbit MQ, basta digitar no browser o endereço localhost:15672

