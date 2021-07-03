using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sh.Domain.Interfaces
{
    public interface IEmailService
    {
        Task SendToAllTGAsync(string msg);
    }
}
