using System;
using System.Collections.Generic;
using System.Text;
using VS.Core.DomainObjects;

namespace VS.Core.Data
{
    public interface IRepository<T> where T: IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
