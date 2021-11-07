using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
    [AttributeUsage(AttributeTargets.Field)]
    public class FileStringAttribute : Attribute
    {
        public string Text { get; private set; }
        public FileStringAttribute(string Path)
        {

        }
    }
}