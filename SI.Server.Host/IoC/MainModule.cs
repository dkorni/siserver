using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contracts;
using Microsoft.Extensions.DependencyInjection;
using SI.Server.Application;
using SI.Server.Domain;
using SI.Server.Domain.Converters;
using SI.Server.Domain.Enums;
using SI.Server.Domain.Interfaces;
using SI.Server.Domain.Utils.Serializers;

namespace ConsoleApp1.IoC
{
    public static class MainModule
    {
        public static IServiceCollection RegitserMainModule(this IServiceCollection container)
        {
            container.AddSingleton<GameState>();
            
            var converters = new Dictionary<PacketType, BasicPacketConverter>()
            {
                [PacketType.ObjectChangedTransform] = new ObjectChangedTransformPacketConverter(),
                [PacketType.ConnectionRequest] = new ConnectionRequestPacketConverter(),
                [PacketType.PlayerJoined] = new PlayerJoinedPacketConverter(),
                [PacketType.Disconnect] = new DisconnectPacketConverter()
            };

            container.AddSingleton<IDictionary<PacketType, BasicPacketConverter>>(converters);
            container.AddSingleton<INetworkSerializer, BinaryNetworkSerializer>();
            
            var handlerTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.Name.Equals("IPacketHandler")));
            var handlerInstances =
                handlerTypes.Select(x => (IPacketHandler) Activator.CreateInstance(x));
            var handlers = handlerInstances.ToDictionary(x => x.PacketType, x => x);

            container.AddSingleton<IDictionary<PacketType, IPacketHandler>>(handlers);

            container.AddSingleton<ISocketSender, SocketSender>();
            container.AddSingleton<IAsynchronousSocketListener, UdpAsynchronousSocketListener>();
            container.AddSingleton<Server>();
            
            return container;
        }
    }
}