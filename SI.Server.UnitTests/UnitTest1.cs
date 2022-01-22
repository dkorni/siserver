using System.Linq;
using AutoFixture;
using NUnit.Framework;
using NUnit.Framework.Internal;
using SI.Server.Domain.Packets;
using SI.Server.Domain.Utils.Serializers;

namespace SI.Server.UnitTests
{
    public class BinaryNetworkSerializerTests
    {
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
        }
        
        [Test]
        public void ShouldSerializeAndDeserialize()
        {
           // var objTransformPackets = _fixture.
             //   CreateMany<ObjectChangedTransformPacket>(100).ToArray();
            //var serializer = new BinaryNetworkSerializer();
            //var bytes = serializer.Serialize(objTransformPackets);
        }
    }
}