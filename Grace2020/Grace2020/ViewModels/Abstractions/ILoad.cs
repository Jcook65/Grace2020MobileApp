using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.ViewModels.Abstractions
{
    interface ILoad
    {
        event EventHandler<SuccessEventArgs> Loaded;
        Task Load();
    }

    public class SuccessEventArgs : EventArgs
    {
        public bool Success { get; set; }
        public SuccessEventArgs(bool success)
        {
            Success = success;
        }
    }
}
