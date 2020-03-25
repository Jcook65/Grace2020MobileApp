using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.ViewModels.Abstractions
{
    public abstract class AbstractCollectionVM : VMBase, ILoad
    {
        public event EventHandler<SuccessEventArgs> Loaded;

        public async Task Load()
        {
            bool success = false;
            try
            {
                IsBusy = true;
                await LoadItems();
                success = true;
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw ex;
            }
            finally
            {
                IsBusy = false;
                Loaded?.Invoke(this, new SuccessEventArgs(success));
            }
        }

        protected abstract Task LoadItems();
    }
}
