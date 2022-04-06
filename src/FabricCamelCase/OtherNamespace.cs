using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabricCamelCase;

internal class OtherNamespace
{
    int count = 0;
    int _total = 0;
    public int Add()
    {
        count++;
        _total++;
        return count + _total;
    }
}