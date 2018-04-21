using System;
using System.Collections.Generic;
using Vedaantees.Framework.Shell.UserContexts;
using Vedaantees.Framework.Types.Users;

namespace Vedaantees.Framework.Tests.Models
{
    public class TestContext2 : IUserContext
    {
        public TestContext2()
        {
            Claims = new List<UserClaim>();
        }

        public Guid ContextId { get; set; }
        public List<UserClaim> Claims { get; set;  }
        public string AssertContent { get; set; }
        string IUserContext.ContextId { get;  set; }
    }
}