using System;
using System.Collections.Generic;
using System.Text;

namespace KegID.DependencyServices
{
    public interface IFileUtil
    {
        string[] GetFiles(string extension);
        string ReadAllText(string path);
    }
}
