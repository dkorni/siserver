using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Contracts;
using Microsoft.Extensions.DependencyInjection;
using SI.Server.Application;
using SI.Server.Application.Handlers;
using SI.Server.Application.Providers;
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
                [PacketType.WorldDataRequest] = new WorldDataRequestConverter(),
                [PacketType.Disconnect] = new DisconnectPacketConverter()
            };

            container.AddSingleton<IDictionary<PacketType, BasicPacketConverter>>(converters);
            container.AddSingleton<INetworkSerializer, BinaryNetworkSerializer>();
            container.AddSingleton<PlayerProvider>();

            container.AddSingleton<IDictionary<PacketType, IPacketHandler>>(sp =>
                new Dictionary<PacketType, IPacketHandler>()
                {
                    [PacketType.ObjectChangedTransform] = sp.GetRequiredService<ObjectChangedTransformPacketHandler>(),
                    [PacketType.ConnectionRequest] = sp.GetRequiredService<ConnectionRequestHandler>(),
                    [PacketType.WorldDataRequest] = sp.GetRequiredService<WorldDataRequestHandler>(),
                    [PacketType.Disconnect] = sp.GetRequiredService<DisconnectHandler>()
                });

            container.AddSingleton<ObjectChangedTransformPacketHandler>();
            container.AddSingleton<ConnectionRequestHandler>();
            container.AddSingleton<WorldDataRequestHandler>();
            container.AddSingleton<DisconnectHandler>();

            container.AddSingleton<ISocketService, UdpSocketService>();
            container.AddSingleton<Server>();
            container.AddSingleton<IServiceProvider>(sp => sp);

            return container;
        }
    }
}