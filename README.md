# siserver

Console server application for own MMO game. Server and client communicate by UDP sockets. 
Server architecture consists of 2 layers
<br/>![alt text](https://i.imgur.com/nlfkm5r.png)

Serializers/Deserializers level. That level serialize/deserialize each message of client/server. 
Each message is serialized to specific byte array packet. 
For example, _ObjectChangedTransformPacket_:

```
using System;
using SI.Server.Domain.Constants;
using SI.Server.Domain.Entities;
using SI.Server.Domain.Enums;

namespace SI.Server.Domain.Packets
{
    [Serializable]
    public class ObjectChangedTransformPacket : Packet
    {
        public override PacketType Type => PacketType.ObjectChangedTransform;

        public override byte DataSize => (byte)PacketDataSize.ObjectChangedTransform;

        public Vector3 Position { get; }
    
        public Quaternion Rotation { get; }

        public ObjectChangedTransformPacket(int objectId, Vector3 position, 
            Quaternion rotation) :
            base(objectId)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
```
This packet has **32** byte array size and next binary structure:
<br/>![alt text](https://i.imgur.com/KnkTFWB.png)<br/>
0-1 bytes - ObjectId, INT (player or other object on the scene)<br/>
2 - PacketType, BYTE<br/>
3 - PacketSize, BYTE<br/>
4-7 bytes - X value of object position, FLOAT<br/>
8-11 bytes - Y value of object position, FLOAT<br/>
12-15 bytes - Z value of object position, FLOAT<br/>
16-19 bytes - X value of object rotation, FLOAT<br/>
20-23 bytes - Y value of object rotation, FLOAT<br/>
24-27 bytes - z value of object rotation, FLOAT<br/>
28-31 bytes - W value of object rotation, FLOAT<br/>

