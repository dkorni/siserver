using System;

namespace SI.Server.Domain.Enums
{
    public enum PacketType
    {
        ConnectionRequest = 1,
        SpawnPlayer = 2,
        ObjectChangedTransform = 3,
        PlayerJoined = 4
    }
}