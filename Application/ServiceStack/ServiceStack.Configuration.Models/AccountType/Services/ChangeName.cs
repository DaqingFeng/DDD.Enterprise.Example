﻿using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.AccountType.Services
{
    [Api("Configuration")]
    [Route("/configuration/account-type/{AccountTypeId}/name", "PUT POST")]
    public class ChangeName : IReturn<Base<Command>>
    {
        public Guid AccountTypeId { get; set; }

        public String Name { get; set; }
    }
}