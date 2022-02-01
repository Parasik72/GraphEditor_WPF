using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphApp {
    public interface ICommandUR {
        string Name { get; }
        void Execute();
        void UnExecute();
    }
}
