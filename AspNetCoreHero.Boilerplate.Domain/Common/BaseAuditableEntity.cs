using AspNetCoreHero.Abstractions.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreHero.Boilerplate.Domain.Common
{
    public abstract class BaseAuditableEntity : AuditableEntity
    {
        public bool IsDeleted { get; set; }
    }
}
