﻿using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.AccountType.Validators
{
    public class Get : AbstractValidator<Services.Get>
    {
        public Get()
        {
            RuleFor(x => x.AccountTypeId).NotEmpty();
        }
    }
}