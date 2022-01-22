using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using AutoFixture;
using BenchmarkDotNet.Attributes;
using SI.Server.Domain.Packets;

namespace SI.Server.LoadTests
{
    public class BinarryFormatterBenchmark
    {
        private BinaryFormatter _binaryFormatter;
        private Fixture _testFixture;
        private ObjectChangedTransformPacket[] _packets;
        private byte[] _batch;
        
        [GlobalSetup]
        public void SetUp()
        {
            _testFixture = new Fixture();
            _binaryFormatter = new BinaryFormatter();
            _packets = _testFixture.CreateMany<ObjectChangedTransformPacket>(100).ToArray();
            _batch = SerializeInternal();
        }
        
        [Benchmark]
        public void SerializeBatchOfPositionTest()
        {
            SerializeInternal();
        }
        
        [Benchmark]
        public void Deserialize()
        {
            using (var ms = new MemoryStream(_batch))
            {
                 var packets = _binaryFormatter.Deserialize(ms) as ObjectChangedTransformPacket[];
            }
        }

        private byte[] SerializeInternal()
        {
            using (var ms = new MemoryStream())
            {
                _binaryFormatter.Serialize(ms, _packets);
                return ms.ToArray();
            }
        }
    }
}