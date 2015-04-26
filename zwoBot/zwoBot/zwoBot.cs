//*********************************************************************
// Program: zwoBot
// Files: Program.cs
// zwoBot.cs
// zwoBot.Designer.cs
// zwoBot.resx
// Description: Connect to an IRC server and perform various functions
//              based on the IRC chat.
// Version: 1.9.2
// Date Created: 02.28.15
// Updated Date: 04.25.15
// Author: Kevin Nguyen 
//*********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Threading;
using TechLifeForum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using zwoBot.Classes;

namespace zwoBot
{
    public partial class zwoBot : Form
    {
        private delegate void _delString(string s);

        private bool isTest = false;
        private string _server;
        private string _channel;
        private List<string> _following = new List<string>();
        private List<string> _service = new List<string>();
        private IrcClient _client;
        private Thread _checkFollowers;

        public zwoBot()
        {
            InitializeComponent();
        }

        #region GUI Functions
        // Connect to an IRC Server and initialize event handlers for chat
        // Side Effects: Make the TextBox read only
        private void zwoBot_Load(object sender, EventArgs e)
        {
            UI_RTB_Chat.ReadOnly = true;

            LoadFollowers();

            if (isTest)
            {
                _server = "underworld1.no.quakenet.org";
                _channel = "#zwoBot";
            }
            else
            {
                _server = "blacklotus.ca.us.quakenet.org";
                _channel = "#teamfs";
            }

            _client = new IrcClient(_server);
            _client.Nick = "beta000";
            _client.AltNick = "zwoBawt";

            ChannelEvents();

            // Connect to server at their port
            _client.Connect();
        }

        // Close the connection between the application and the IRC server
        // Side Effects: Save the followers list to the provided text file
        private void zwoBot_FormClosing(object sender, FormClosingEventArgs e)
        {
            StreamWriter writer = new StreamWriter("following.txt");

            for (int i = 0; i < _following.Count ; ++i)
                writer.WriteLine(_following[i] + " " + _service[i]);

            writer.Flush();
            writer.Close();
        }

        // Add text to chat
        // Side Effects: Scroll all the way down
        private void AddToChat(string msg)
        {
            UI_RTB_Chat.AppendText("[" + DateTime.Now.ToString("hh:mm tt") + "] " + msg + "\n");
            UI_RTB_Chat.ScrollToCaret();
        }
        #endregion

        //*********************************************************************
        // Method:  private void LoadFollowers()
        // Purpose: Load followers from the text file contents to the internal List<string>
        //*********************************************************************
        private void LoadFollowers()
        {
            StreamReader reader = new StreamReader("following.txt");

            string following = reader.ReadLine();

            while (following != null)
            {
                var n = following.Split(' ');
                _following.Add(n[0]);
                _service.Add(n[1]);
                following = reader.ReadLine();
            }

            reader.Close();
        }

        //*********************************************************************
        // Method: private void ChannelEvents()
        // Purpose: Intiailize chat events
        //*********************************************************************
        private void ChannelEvents()
        {
            _client.ChannelMessage += client_ChannelMessage;
            _client.ExceptionThrown += client_ExceptionThrown;
            _client.NoticeMessage += client_NoticeMessage;
            _client.OnConnect += client_OnConnect;
            _client.PrivateMessage += client_PrivateMessage;
            _client.ServerMessage += client_ServerMessage;
            _client.UpdateUsers += client_UpdateUsers;
            _client.UserJoined += client_UserJoined;
            _client.UserLeft += client_UserLeft;
            _client.UserNickChange += client_UserNickChange;
        }

        #region Listeners
        // Connect to the IRC Server
        // Side Effects: Check the streams in the follow list for availability
        private void client_OnConnect(object sender, EventArgs e)
        {
            _client.JoinChannel(_channel);
            _client.SendMessage(_channel, "!mute");
            OnLoadCheck();
        }

        // Add the user to the listbox if someone joins the channel
        // Side Effects: Nothing
        private void client_UserJoined(object sender, UserJoinedEventArgs e)
        {
            UI_LB_Users.Items.Add(e.User.Substring(1, e.User.IndexOf("!") - 1));
        }

        // Remove the user from the list box if someone leave the channel
        // Side Effects: Nothing
        private void client_UserLeft(object sender, UserLeftEventArgs e)
        {
            UI_LB_Users.Items.Remove(e.User);
        }

        // Change the user's nickname if they do
        // Side Effects: Nothing
        private void client_UserNickChange(object sender, UserNickChangedEventArgs e)
        {
            UI_LB_Users.Items[UI_LB_Users.Items.IndexOf(e.Old)] = e.New;
        }

        // Update the user list
        //Side Effects: Nothing
        private void client_UpdateUsers(object sender, UpdateUsersEventArgs e)
        {
            UI_LB_Users.Items.Clear();
            UI_LB_Users.Items.AddRange(e.UserList);
        }

        // Add all channel messages to the chat
        // Side Effects: Nothing
        private void client_ChannelMessage(object sender, ChannelMessageEventArgs e)
        {
            AddToChat("<" + e.From + "> : " + e.Message);

            IrcTextFunctions textFunctions = new IrcTextFunctions(_client, _channel, _following, _service);
            textFunctions.ChatFunctions(e.Message, e.From);
        }

        // Add all server messages to the chat
        // Side Effects: Nothing
        private void client_ServerMessage(object sender, StringEventArgs e)
        {
            AddToChat(e.Result);
        }

        // Add all private messages to chat
        // Side Effects: Nothing
        private void client_PrivateMessage(object sender, PrivateMessageEventArgs e)
        {
            AddToChat("PM FROM " + e.From + ": " + e.Message);
        }

        // Add all notice messages to the chat
        // Side Effects: Nothing
        private void client_NoticeMessage(object sender, NoticeMessageEventArgs e)
        {
            AddToChat("NOTICE FROM " + e.From + ": " + e.Message);
        }

        // Show any exceptions that occured
        //Side Effects: Message Box dialog will pop up
        private void client_ExceptionThrown(object sender, ExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message);
        }
        #endregion

        #region On Connected Follower Check
        //*********************************************************************
        // Method: private void OnLoadCheck()
        // Purpose: Check the streams in the follow list for availiability in a background Thread
        //*********************************************************************
        private void OnLoadCheck()
        {
            _checkFollowers = new Thread( () => CheckFollowers(_following, _service));
            _checkFollowers.IsBackground = true;
            _checkFollowers.Start();

            string text = "";
            AddToChat("<" + _client.Nick + "> : " + text);
            _client.SendMessage(_channel, text);
        }

        //*********************************************************************
        // Method: private void CheckFollowers(object obj)
        // Purpose: Check the follow list for availability
        //*********************************************************************
        private void CheckFollowers(List<string> following, List<string> service)
        {
            bool isBoot = false;

            List<StreamInfo> ch = new List<StreamInfo>();

            string pastTimeStamp = DateTime.Now.ToString("hh:mm");

            for (int i = 0; i < following.Count; ++i)
            {
                if (service[i] == "hb")
                {
                    HitBox channel = new HitBox(following[i]);
                    ch.Add(channel);
                }
                else if (service[i] == "tw")
                {
                    Twitch channel = new Twitch(following[i]);
                    ch.Add(channel);
                }
            }

            while (true)
            {
                string timestamp = DateTime.Now.ToString("hh:mm");
                string offline = "";

                if (timestamp != pastTimeStamp || !isBoot)
                {
                    isBoot = true;

                    pastTimeStamp = timestamp;

                    for (int i = 0; i < ch.Count; ++i)
                    {
                        List<string> msg = new List<string>();

                        if (ch[i] is HitBox)
                        {
                            var channel = ch[i] as HitBox;
                            msg = channel.CheckFollowers(ref offline);
                        }
                        else if (ch[i] is Twitch)
                        {
                            var channel = ch[i] as Twitch;
                            msg = channel.CheckFollowers(ref offline);
                        }

                        foreach (string n in msg)
                            Invoke(new _delString(InvokeSendChat), n);
                    }

                    // Output Offline Streams from Twitch from Boot
                    if (offline != String.Empty)
                        Invoke(new _delString(InvokeSendChat), "4OFFLINE 1:" + offline);
                }
            }
        }

        //*********************************************************************
        // Method: private void InvokeSendChat(string n)
        // Purpose: Send the availability status of the channel to chat
        //*********************************************************************
        private void InvokeSendChat(string n)
        {
            AddToChat("<" + _client.Nick + "> : " + n);
            _client.SendMessage(_channel, n);
        }
        #endregion

        // ********************************************************************
        //                          WEB API LIST
        // Twitch.TV - https://github.com/justintv/Twitch-API
        // Hitbox.TV - http://developers.hitbox.tv/
        // Blizzard - https://dev.battle.net/
        // Dota 2 - 
        // ********************************************************************
    }
}
