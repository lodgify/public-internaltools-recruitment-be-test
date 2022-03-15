using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperPanel.Abstractions.Actions
{
    public interface IJsonSerialization
    {
        Task<T> Deserialize<T>(Stream stream);
        T Deserialize<T>(string str);
    }
}
