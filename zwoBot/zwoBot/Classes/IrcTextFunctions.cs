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
        public List<string> _service { get; private set; }

        public IrcTextFunctions(IrcClient client, string channel, List<string> following, List<string> service)
        {
            _client = client;
            _following = following;
            _channel = channel;
            _service = service;
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
                        MessageThree(message, userSent);
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
            if (message[0].ToLower() == "!zwobot")
                BotCommands();
            if (message[0].ToLower() == "!top5")
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
            if (message[0].ToLower() == "!stats")
            {
                HitBox hitbox = new HitBox(message[1]);
                bool isOffHB = false;
                bool isOffTW = false;
                bool isExist = hitbox.CheckStream(_client, _channel, false, ref isOffHB);

                Twitch twitch = new Twitch(message[1]);
                isExist = twitch.CheckStream(_client, _channel, false, ref isOffTW);

                if (isExist && isOffTW && isOffHB)
                    _client.SendMessage(_channel, message[1] + " is 13offline1. ");
            }

            if (message[0].ToLower() == "!paragon")
            {
                Diablo diablo = new Diablo(message[1]);
                _client.SendMessage(_channel, diablo.ParagonChecks());
            }
        }

        private void MessageThree(string[] message, string userSent)
        {
            // Follow a channel
            if (message[0] == "!follow" && userSent == "zwo")
                Channel(message[1], message[2], true);
            // Unfollow a channel
            else if (message[0] == "!unfollow" && userSent == "zwo")
                Channel(message[1], message[2], false);
        }

        private void MessageX(string[] message, string userSent)
        {
            if (message[0].ToLower() == "!wowprogress")
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
            else if (message[0].ToLower() == "!game")
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
        private void Channel(string channel, string stream, bool follow)
        {
            string text = null;

            if (follow)
            {
                // Stream isn't followed yet
                if (!_following.Contains(channel.ToLower()))
                {
                    bool isExist = false;
                    bool placeholdr = false;

                    if (stream == "twitch")
                    {
                        Twitch twitch = new Twitch(channel);
                        isExist = twitch.CheckStream(_client, _channel, follow, ref placeholdr);
                    }
                    else if (stream == "hitbox")
                    {
                        HitBox hitbox = new HitBox(channel);
                        isExist = hitbox.CheckStream(_client, _channel, follow, ref placeholdr);
                    }

                    // Stream exists, add them
                    if (isExist)
                    {
                        if (stream == "twitch")
                            _service.Add("tw");
                        else if (stream == "hitbox")
                            _service.Add("hb");

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
                string str = null;

                if (stream == "twitch")
                    str = "tw";
                else if (stream == "hitbox")
                    str = "hb";

                // Remove stream
                if (_following.Contains(channel.ToLower()))
                {
                    int index = _following.IndexOf(channel);

                    if (_service[index] == str)
                    {
                        _following.Remove(channel);
                        _service.RemoveAt(index);

                        text = "Removed " + channel + " from the followers list.";
                    }
                    else
                        text = channel + " is not in the followers list.";
                }
                else
                    text = channel + " is not in the followers list.";
            }

            _client.SendMessage(_channel, text);
        }
    }
}
