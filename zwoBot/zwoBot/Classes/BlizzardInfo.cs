using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net;
using TechLifeForum;

namespace zwoBot.Classes
{
    class BlizzardInfo
    {
        protected readonly string BLIZZARD_API_KEY = "f56qqfrxy83eza8n348eqxzec7k9gpc4";
        protected string name;
        protected string bclass;
        protected string level;

        public BlizzardInfo(string name)
        {
            this.name = name;
        }

        public BlizzardInfo(string name, string bClass, string level)
        {
            this.name = name;
            bclass = bClass;
            this.level = level;
        }
    }

    class Warcraft : BlizzardInfo
    {
        protected string realm;
       
        public Warcraft(string name, string realm)
            : base(name)
        {
            this.realm = realm;
        }

        public Warcraft(string name, string bClass, string level, string realm)
            : base(name, bClass, level)
        {
            this.realm = realm;
        }


        public string ProgressionCheck()
        {
            string output = null;
            string field = "progression";
            string locale = "en_US";

            string api = "https://us.api.battle.net/wow/character/" + realm + "/" + name + "?fields=" + field + "&locale=" + locale + "&apikey=" + BLIZZARD_API_KEY;
            WebClient web = new WebClient();

            try
            {
                string dataInfo = web.DownloadString(api);
                var data = JObject.Parse(dataInfo);

                var raids = data["progression"]["raids"];

                int latestRaidNumber = raids.Count();
                var latestRaid = raids[latestRaidNumber - 1];

                name = data["name"].ToString();
                bclass = "druid";
                level = data["level"].ToString();
                realm = data["realm"].ToString();
                string raidName = latestRaid["name"].ToString();
                var bosses = latestRaid["bosses"];

                int totalBosses = bosses.Count();
                int bossesKilled_N = 0, bossesKilled_H = 0, bossesKilled_M = 0;

                for (int i = 0; i < totalBosses; ++i)
                {
                    if (bosses[i]["normalTimestamp"].ToString() != "0")
                        bossesKilled_N++;
                    if (bosses[i]["heroicTimestamp"].ToString() != "0")
                        bossesKilled_H++;
                    if (bosses[i]["mythicTimestamp"].ToString() != "0")
                        bossesKilled_M++;
                }

                output = name + " in " + realm + " is : "
                    + bossesKilled_N.ToString() + "/" + totalBosses.ToString() + "N "
                    + bossesKilled_H.ToString() + "/" + totalBosses.ToString() + "H "
                    + bossesKilled_M.ToString() + "/" + totalBosses.ToString() + "M on " + raidName;
            }
            catch (Exception e)
            {
                output = "Character or Realm does not exist.";
            }

            return output;
        }
    }
}
