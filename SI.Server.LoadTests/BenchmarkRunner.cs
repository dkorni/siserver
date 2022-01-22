using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using NUnit.Framework;

namespace SI.Server.LoadTests
{
    public class Tests
    {
        [Test]
        public void Run()
        {
            var config = new ManualConfig()
                .WithOptions(ConfigOptions.DisableOptimizationsValidator)
                .AddValidator(JitOptimizationsValidator.DontFailOnError)
                .AddLogger(ConsoleLogger.Default);
            BenchmarkRunner.Run<BinarryFormatterBenchmark>(config);
        }
    }
}