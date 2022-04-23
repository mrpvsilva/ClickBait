# ClickBait
Aplicação de exemplo utilizando .Net 6 kafka streams para contabilizar quantidades de cliques em determinados links.

Foi utilizado
- Kafka
- Kafka connect (para persistir os dados agregados na base de dados)
- Ksqldb server (para agregar os dados provenientes de um tópico do Kafka)
- Schema registry (para definir um esquema das mensagens do Kafka)
- Web Api .Net 6
- Entity framework
- MediatR
- Mysql
- Docker
- Docker-compose
