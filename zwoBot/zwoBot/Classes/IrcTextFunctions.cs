using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechLifeForum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;

namespace zwoBot.Classes
{
    class IrcTextFunctions
    {
        public IrcClient _client { get; private set; }
        public string _channel { get; private set; }
        public List<string> _following { get; private set; }

        public IrcTextFunctions(IrcClient client, string channel, List<string> following)
        {
            _client = client;
            _following = following;
            _channel = channel;
        }

        //*********************************************************************
        // Method:  private void ChatFunctions(string msg)
        // Purpose: Handles all chat functions based on what users type in chat
        //*********************************************************************
        public void ChatFunctions(string msg, string userSent)
        {
            string[] message = msg.Split(' ');

            switch (message.Length)
            {
                case 1:
                    {
                        MessageOne(message);
                        break;
                    }
                case 2:
                    {
                        MessageTwo(message, userSent);
                        break;
                    }
                case 3:
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }

            MessageX(message, userSent);
        }

        private void MessageOne(string[] message)
        {
            if (message[0] == "!zwobot")
                BotCommands();
            if (message[0] == "!top5")
            {
                Twitch twitch = new Twitch(null);
                var games = twitch.TopFiveGames();

                foreach (string n in games)
                    _client.SendMessage(_channel, n);
            }
        }

        private void MessageTwo(string[] message, string userSent)
        {
            // Check the status of a Twitch.TV Channel
            if (message[0] == "!stats")
            {
                Twitch twitch = new Twitch(message[1]);
                if (!twitch.CheckStream(_client, _channel, false))
                {
                    HitBox hitbox = new HitBox(message[1]);
                    hitbox.CheckStream(_client, _channel, false);
                }
            }
            // Follow a channel
            if (message[0] == "!follow" && userSent == "zwo")
                Channel(message[1], true);
            // Unfollow a channel
            else if (message[0] == "!unfollow" && userSent == "zwo")
                Channel(message[1], false);

            if (message[0] == "!paragon")
            {
                Diablo diablo = new Diablo(message[1]);
                _client.SendMessage(_channel, diablo.ParagonChecks());
            }
        }

        private void MessageX(string[] message, string userSent)
        {
            //if (message[0] == "!mute" && userSent != "whereIsBib")
            //    _client.SendMessage(_channel, "!mute");

            if (message[0] == "!wowprogress")
            {
                string realm = "";
                for (int i = 2; i < message.Length; ++i)
                    realm += message[i] + " ";

                if (realm.Trim() != String.Empty)
                {
                    Warcraft warcraft = new Warcraft(message[1], realm);
                    _client.SendMessage(_channel, warcraft.ProgressionCheck());
                }
            }
            else if (message[0] == "!game")
            {
                string game = "";
                for (int i = 1; i < message.Length; ++i)
                    game += message[i] + " ";

                if (game.Trim() != String.Empty)
                {
                    Twitch twitch = new Twitch(null);
                    var channel = twitch.TopFiveChannels(game);

                    foreach (string n in channel)
                        _client.SendMessage(_channel, n);
                }
            }
        }
        

        //*********************************************************************
        // Method: private void BotCommands()
        // Purpose: Sends the commands that users can use in chat
        //*********************************************************************
        private void BotCommands()
        {
            string text = "List of Available Commands :";
            _client.SendMessage(_channel, text);

            text = "!stats 3<channel>1, ";
            text += "!top5, ";
            text += "!game 3<game>1, ";
            text += "!wowprogress 3<character> <realm> 1";
            _client.SendMessage(_channel, text);
        }

        //*********************************************************************
        // Method:  private void Channel(string channel, bool follow)
        // Purpose: Add or Remove followers from the list
        //*********************************************************************
        private void Channel(string channel, bool follow)
        {
            string text = null;

            if (follow)
            {
                // Stream isn't followed yet
                if (!_following.Contains(channel.ToLower()))
                {
                    Twitch twitch = new Twitch(channel);
                    bool checkTW = twitch.CheckStream(_client, _channel, follow);
                    bool checkHB = false;

                    // Check if the stream actually exists
                    if (!checkTW)
                    {
                        HitBox hitbox = new HitBox(channel);
                        checkHB = hitbox.CheckStream(_client, _channel, follow);
                    }

                    // Stream exists, add them
                    if (checkTW || checkHB)
                    {
                        _following.Add(channel.ToLower());
                        text = channel + " has been added to the followers list.";
                    }
                    else
                        text = channel + " does not exist on Twitch.TV or Hitbox.TV. It is not added to the followers list.";
                }
                else
                    text = "Already following : " + channel + ".";
            }
            else
            {
                // Remove stream
                if (_following.Contains(channel.ToLower()))
                {
                    _following.Remove(channel);

                    text = "Removed " + channel + " from the followers list.";
                }
                else
                    text = channel + " is not in the followers list.";

            }

            _client.SendMessage(_channel, text);
        }
    }
}
