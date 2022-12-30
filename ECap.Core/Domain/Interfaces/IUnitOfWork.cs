using System;
using System.Collections.Generic;
using System.Text;

namespace ECap.Core.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
    }
}
