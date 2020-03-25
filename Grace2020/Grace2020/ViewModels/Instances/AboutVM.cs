using GalaSoft.MvvmLight.Command;
using Grace2020.Services;
using Grace2020.ViewModels.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.ViewModels.Instances
{
    public class AboutVM : VMBase
    {
        private bool _isDarkModeEnabled;
        public bool IsDarkModeEnabled
        {
            get { return _isDarkModeEnabled; }
            set { Set(() => IsDarkModeEnabled, ref _isDarkModeEnabled, value); }
        }

        public event EventHandler OpenWeblinkUnsuccessful;
        public event EventHandler OpenEmailUnsuccessful;
        public RelayCommand<string> OpenWeblink
        {
            get
            {
                return new RelayCommand<string>(async (link) =>
                {
                    var success = await WebService.OpenUri(link);
                    if (!success)
                    {
                        OpenWeblinkUnsuccessful?.Invoke(this, new EventArgs());
                    }
                });
            }
        }

        public RelayCommand<string> OpenEmail
        {
            get
            {
                return new RelayCommand<string>(async (email) =>
                {
                    var success = await WebService.OpenEmail(email);
                    if (!success)
                    {
                        OpenEmailUnsuccessful?.Invoke(this, new EventArgs());
                    }
                });
            }
        }
    }
}
