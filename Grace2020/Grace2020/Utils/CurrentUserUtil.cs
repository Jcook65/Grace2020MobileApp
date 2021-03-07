using Grace2020.Models.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Grace2020.Utils
{
    public class CurrentUserUtil
    {
        public static async Task<Config> GetCurrentUserConfigurationAsync()
        {
            using(var db = new DbUtil())
            {
                var configList = await db.AsyncConnection.Table<Config>().ToListAsync();
                if(configList == null || configList.Count == 0)
                {
                    var config = new Config { ConfigId = 1 };
                    await db.AsyncConnection.InsertAsync(config);
                    return config;
                }
                else
                {
                    return configList[0];
                }
            }
        }

        public static async Task UpdateCurrentUserConfigAsync(string currentModuleId, string currentTopicId)
        {
            using(var db = new DbUtil())
            {
                await db.AsyncConnection.InsertOrReplaceAsync(new Config { ConfigId = 1, CurrentModuleId = currentModuleId, CurrentTopicId = currentTopicId, AppTheme = (int)App.AppTheme });
            }
        }

        public static async Task UpdateCurrentUserConfigAsync(string currentModuleId)
        {
            using(var db = new DbUtil())
            {
                await db.AsyncConnection.InsertOrReplaceAsync(new Config { ConfigId = 1, CurrentModuleId = currentModuleId, AppTheme = (int)App.AppTheme });
            }
        }

        public static async Task UpdateCurrentUserConfigAsync(int appTheme)
        {
            using (var db = new DbUtil())
            {
                var config = await db.AsyncConnection.Table<Config>().FirstOrDefaultAsync();
                if (config != null)
                {
                    config.AppTheme = appTheme;
                    await db.AsyncConnection.UpdateAsync(config);
                }
            }
        }

        public static async Task UpdateCurrentUserConfigAsync(Config config)
        {
            if(config != null)
            {
                using(var db = new DbUtil())
                {
                    await db.AsyncConnection.InsertOrReplaceAsync(config);
                }
            }
        }
    }
}
