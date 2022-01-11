using System.Net;

namespace SI.Server.Domain.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IPEndPoint Address { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public Player(int id, string name, IPEndPoint address, Vector3 position, Quaternion rotation)
        {
            Id = id;
            Name = name;
            Address = address;
            Position = position;
            Rotation = rotation;
        }
    }
}