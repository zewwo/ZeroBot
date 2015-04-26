using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLifeForum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

namespace zwoBot.Classes
{
    abstract class StreamInfo
    {
        public string channelName { get; private set; }
        protected string displayName;
        protected string url;
        protected string viewers;
        protected string status;
        protected string game;
        protected string streamCreated;
        protected List<string> msg = new List<string>();
        protected bool isOnlineMsg;
        protected bool bootUp;

        public StreamInfo(string channelName)
        {
            this.channelName = channelName;
        }

        public abstract bool CheckStream(IrcClient _client, string ircChannel, bool follow, ref bool isOffline);

        protected abstract void StreamData(JObject data);
    }

    class Twitch : StreamInfo
    {
        public Twitch(string channelName)
            : base(channelName) { }

        protected override void StreamData(JObject data)
        {
            displayName = data["stream"]["channel"]["display_name"].ToString();
            url = data["stream"]["channel"]["url"].ToString();
            viewers = data["stream"]["viewers"].ToString();
            status = data["stream"]["channel"]["status"].ToString();
            game = data["stream"]["game"].ToString();
            streamCreated = data["stream"]["created_at"].ToString();
        }

        public override bool CheckStream(IrcClient _client, string ircChannel, bool follow, ref bool isOffline)
        {           
            bool isExists = false;
            string api = "https://api.twitch.tv/kraken/streams/" + channelName;
            WebClient web = new WebClient();

            try
            {
                var dataInfo = web.DownloadString(api);
                var data = JObject.Parse(dataInfo);

                if (!follow)
                {
                    // Stream is offline
                    if (data["stream"].ToString() == String.Empty)
                        isOffline = true;
                    else
                    {
                        isOffline = false;

                        StreamData(data);

                        // No Zyori allowed
                        if (status.Contains("@ZyoriTV"))
                            msg.Add("zyori's butt");
                        else
                        {
                            msg.Add(displayName + " is 3online and is streaming : ");

                            msg.Add(game + " [ " + status + " ]  at 6"
                                   + url + " 1( " + viewers
                                   + " viewer(s), streaming for : ");

                            var result = DateTime.UtcNow - DateTime.Parse(streamCreated);

                            // The elapse time the stream streamed for
                            if (result.Hours > 0)
                                msg[1] += result.Hours + " hours " + result.Minutes + " minutes " + result.Seconds + " seconds" + " ) ";
                            else if (result.Minutes > 0)
                                msg[1] += result.Minutes + " minutes " + result.Seconds + " seconds" + " ) ";
                            else if (result.Seconds > 0)
                                msg[1] += result.Minutes + " minutes " + result.Seconds + " seconds" + " ) ";
                        }

                        foreach (string n in msg)
                            _client.SendMessage(ircChannel, n);
                    }
                }

                isExists = true;
            }
            catch
            {
                if (!follow)
                    // If someone placed an unknown channel for both Twitch + Hitbox
                    msg.Add(channelName + " 4 does not exist1.");
            }

            return isExists;
        }

        public List<string> CheckFollowers(ref string offline)
        {
            string api = "https://api.twitch.tv/kraken/streams/" + channelName;
            WebClient web = new WebClient();
            msg.Clear();

            try
            {
                // Perform a GET request to the API
                string dataInfo = web.DownloadString(api);
                var data = JObject.Parse(dataInfo);
                // Stream is Online
                if (data["stream"].ToString() != String.Empty)
                {
                    if (!isOnlineMsg || !bootUp)
                    {
                        StreamData(data);
                        
                        var detailed = "1" + displayName + " is 3online 1and streaming : " + game + " at 6"
                                + url + " 1( " + viewers + " viewer(s), streaming for : ";

                        var result = DateTime.UtcNow - DateTime.Parse(streamCreated);

                        // How long Channel was Streaming for
                        if (result.Hours > 0)
                            detailed += result.Hours + " hours " + result.Minutes + " minutes " + result.Seconds + " seconds" + " ) ";
                        else if (result.Minutes > 0)
                            detailed += result.Minutes + " minutes " + result.Seconds + " seconds" + " ) ";
                        else if (result.Seconds > 0)
                            detailed += result.Minutes + " minutes " + result.Seconds + " seconds" + " ) ";

                        msg.Add(detailed);

                        // In order to avoid duplicate messages for Online streams
                        isOnlineMsg = true;

                        // When the Application first boots up
                        if (!bootUp)
                            bootUp = true;
                    }
                }
                else if (data["stream"].ToString() == String.Empty)
                {
                    if (isOnlineMsg || !bootUp)
                    {
                        // In order to avoid duplicate messages for Offline streams
                        isOnlineMsg = false;

                        if (!bootUp)
                        {
                            // Consolidate all offline streams in a single string
                            offline += " " + channelName;
                            bootUp = true;
                        }
                        else
                            msg.Add(channelName + " is now 4offline1.");
                    }
                }
            }
            catch
            {

            }

            return msg;
        }

        public List<string> TopFiveGames()
        {
            List<string> topGames = new List<string>();

            string api = "https://api.twitch.tv/kraken/games/top";

            WebClient web = new WebClient();

            try
            {
                var dataInfo = web.DownloadString(api);
                var data = JObject.Parse(dataInfo);

                for (int i = 0; i < 5; ++i)
                {
                    var n = "#" + (i + 1).ToString() + " : " 
                        + data["top"][i]["game"]["name"] 
                        + " - " + data["top"][i]["viewers"] + " viewer(s)";
                    topGames.Add(n);
                }
            }
            catch (Exception e)
            {
                topGames.Add("Games do not exist.");
            }

            return topGames;
        }

        public List<string> TopFiveChannels(string game)
        {
            List<string> topChannels = new List<string>();
            string urlGame = game.Remove(game.Length - 1, 1);
            urlGame = WebUtility.UrlEncode(urlGame);
            string api = "https://api.twitch.tv/kraken/streams?game=" + urlGame;
            WebClient web = new WebClient();

            try
            {
                var dataInfo = web.DownloadString(api);
                var data = JObject.Parse(dataInfo);
                
                for (int i = 0; i < 5; ++i)
                {
                    var n = "#" + (i + 1).ToString() + " : " +
                        data["streams"][i]["channel"]["display_name"] +
                        " : 3" + data["streams"][i]["channel"]["url"]
                        + "1 - " + data["streams"][i]["viewers"] + " viewer(s)";
                    topChannels.Add(n);
                }
            }
            catch (Exception e)
            {
                topChannels.Add(game + " does not exist.");
            }

            return topChannels;
        }
    }

    class HitBox : StreamInfo
    {
        public HitBox(string channelName)
            : base(channelName) { }

        protected override void StreamData(JObject data)
        {
            displayName = data["livestream"][0]["media_user_name"].ToString();
            url = data["livestream"][0]["channel"]["channel_link"].ToString();
            viewers = data["livestream"][0]["category_viewers"].ToString();
            game = data["livestream"][0]["category_name"].ToString();
            streamCreated = data["livestream"][0]["media_live_since"].ToString();
        }

        public override bool CheckStream(IrcClient _client, string ircChannel, bool follow, ref bool isOffline)
        {
            bool isExists = false;
            string api = "https://api.hitbox.tv/media/live/" + channelName;
            WebClient web = new WebClient();

            try
            {
                string dataInfo = web.DownloadString(api);
                var data = JObject.Parse(dataInfo);

                if (!follow)
                {
                    // Stream is offline
                    if (data["livestream"][0]["media_is_live"].ToString() == "1")
                    {
                        StreamData(data);

                        msg.Add(displayName + " is 3online and is streaming : ");

                        var detailed = game + " at 6" + url
                            + " 1( " + viewers + " viewer(s), streaming for : ";

                        // The elapse time the stream streamed for
                        var result = DateTime.UtcNow - DateTime.Parse(streamCreated);

                        if (result.Hours > 0)
                            detailed += result.Hours + " hours " + result.Minutes + " minutes " + result.Seconds + " seconds." + " )";
                        else if (result.Minutes > 0)
                            detailed += result.Minutes + " minutes " + result.Seconds + " seconds." + " )";
                        else if (result.Seconds > 0)
                            detailed += result.Seconds + " seconds." + " )";

                        msg.Add(detailed);

                        foreach (string n in msg)
                            _client.SendMessage(ircChannel, n);
                    }
                    else
                        isOffline = true;
                }

                isExists = true;
            }
            catch
            {

            }

            return isExists;
        }

        public List<string> CheckFollowers(ref string offline)
        {
            string api = "https://api.hitbox.tv/media/live/" + channelName;
            WebClient web = new WebClient();
            msg.Clear();

            try
            {
                string dataInfo = web.DownloadString(api);
                var data = JObject.Parse(dataInfo);

                StreamData(data);

                if (streamCreated == String.Empty)
                    return msg;

                // Stream is Online
                if (data["livestream"][0]["media_is_live"].ToString() == "1")
                {
                    if (!isOnlineMsg || !bootUp)
                    {
                        msg.Add("1" + displayName + " is 3online 1and is streaming at 6"
                            + url + " 1( " + viewers + " viewer(s), streaming for : ");

                        var result = DateTime.UtcNow - DateTime.Parse(streamCreated);
                        if (result.Hours > 0)
                            msg[0] += result.Hours + " hours " + result.Minutes + " minutes " + result.Seconds + " seconds.";
                        else if (result.Minutes > 0)
                            msg[0] += result.Minutes + " minutes " + result.Seconds + " seconds.";
                        else if (result.Seconds > 0)
                            msg[0] += result.Seconds + " seconds.";

                        // In order to avoid duplicate messages for Online streams
                        isOnlineMsg = true;

                        // When Application first boots up
                        if (!bootUp)
                            bootUp = true;
                    }
                }
                else if (data["livestream"][0]["media_is_live"].ToString() == "0")
                {
                    if (isOnlineMsg || !bootUp)
                    {
                        // In order to avoid duplicate messages for Offline streams
                        isOnlineMsg = false;

                        if (!bootUp)
                        {
                            bootUp = true;
                            offline += " " + channelName;
                        }
                        else
                        {
                            msg.Add(channelName + " is 4offline. It was live for ");

                            var result = DateTime.UtcNow - DateTime.Parse(streamCreated);
                            if (result.Hours > 0)
                                msg[0] += result.Hours + " hours " + result.Minutes + " minutes " + result.Seconds + " seconds.";
                            else if (result.Minutes > 0)
                                msg[0] += result.Minutes + " minutes " + result.Seconds + " seconds.";
                            else if (result.Seconds > 0)
                                msg[0] += result.Seconds + " seconds.";
                        }
                    }
                }
            }
            catch
            {

            }

            return msg;
        }
    }
}
