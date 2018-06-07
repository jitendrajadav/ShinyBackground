﻿using System.Threading.Tasks;

namespace KegID.DependencyServices
{
    public interface IFileStore
    {
        string GetFilePath();
        string WriteFile(string fileName, byte[] bytes);
        string SafeHTMLToPDF(string html, string filename);
    }
}