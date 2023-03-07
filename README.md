# siserver

Console server application for own MMO game. Server and client communicate by UDP sockets. 
Server architecture consists of 2 layers
<br/>![alt text](https://i.imgur.com/nlfkm5r.png)

Serializers/Deserializers level. That level serialize/deserialize each message of client/server. 
Each message is serialized to byte array packet.  
