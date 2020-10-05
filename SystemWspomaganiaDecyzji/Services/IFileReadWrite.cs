using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using SystemWspomaganiaDecyzji.Models;

namespace SystemWspomaganiaDecyzji.Services
{
    public interface IFileReadWrite
    {
        void ReadFileFromPath(string path);
    }
}
