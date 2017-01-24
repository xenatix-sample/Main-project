using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Helpers.Infrastructure.Automapper
{
    public interface IAutomapperRegistrar
    {
        void Register(ITypeFinder typeFinder);

        int Order { get; }
    }
}