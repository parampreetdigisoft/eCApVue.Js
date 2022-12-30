using ECap.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECap.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        public UnitOfWork(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }
        public IUserRepository UserRepository { get; }
    }
}
