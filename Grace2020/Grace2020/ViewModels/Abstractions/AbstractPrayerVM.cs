using Grace2020.Models.Tables;
using System;
using System.Collections.Generic;
using System.Text;

namespace Grace2020.ViewModels.Abstractions
{
    public abstract class AbstractPrayerVM : VMBase
    {
        private Prayer _prayer;
        public Prayer Prayer
        {
            get { return _prayer; }
            set { Set(() => Prayer, ref _prayer, value); }
        }

        public AbstractPrayerVM(Prayer prayer)
        {
            _prayer = prayer;
        }
    }
}
