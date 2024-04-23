using System;
using System.IO;
using Macroscop.TextAnalyzer.ClientApp.Interfaces;

namespace Macroscop.TextAnalyzer.ClientApp
{
    public sealed class DirContext : IDirContext
    {
        public string GetProjectDirectory()
        {
            return Directory.GetParent(Environment.CurrentDirectory)?.Parent?.Parent?.FullName ?? "";
        }
    }
}