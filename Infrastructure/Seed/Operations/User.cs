﻿using Demo.Library.Extensions;
using NServiceBus;
using Seed.Attributes;
using Seed.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands = Demo.Domain.Authentication.Users.Commands;
using Type = Seed.Types.Authentication;

namespace Seed.Operations
{
    [Operation("User")]
    [Category("Authentication")]
    public class User : IOperation
    {
        public static IEnumerable<Type.User> Data = new[] {
            new Type.User { Id = "IMPORT", Name = "IMPORT" },
        };

        private readonly IBus _bus;

        public User(IBus bus)
        {
            _bus = bus;
        }
        public async Task<Boolean> Seed()
        {
            var commands = Data.Select(x => new Commands.Login { UserId = x.Id, Name = x.Name, Timestamp = DateTime.UtcNow });
            await commands.WhenAllAsync(x => _bus.Send(x).IsCommand<Command>());

            this.Done = true;
            return this.Done;
        }

        public Boolean Done { get; private set; }
    }
}