//*********************************************************************
// Program: zwoBot
// Files: Program.cs
// zwoBot.cs
// zwoBot.Designer.cs
// zwoBot.resx
// IrcClient.cs
// IrcTextFunctions.cs
// StreamInfo.cs
// BlizzardInfo.cs
// Description: Connect to an IRC server and perform various functions
//              based on the IRC chat.
// Version: 1.9.3
// Date Created: 02.28.15
// Updated Date: 04.25.15
// Author: Kevin Nguyen 
//*********************************************************************

// ********************************************************************
//                          WEB API LIST
// Twitch.TV - https://github.com/justintv/Twitch-API
// Hitbox.TV - http://developers.hitbox.tv/
// Blizzard - https://dev.battle.net/
// Dota 2 - 
// ********************************************************************

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
        private volatile bool _closeThread = false;
        private bool isTest = true;
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

        private void UI_B_Connect_Click(object sender, EventArgs e)
        {
            if (UI_B_Connect.Text == "Connect")
            {
                if (isTest)
                {
                    _server = "underworld1.no.quakenet.org";
                    _channel = "#zwoBot";
                }
                else
                {
                    _server = "blacklotus.ca.us.quakenet.org";
                    _channel = UI_TB_Channel.Text;
                }

                _client = new IrcClient(_server);
                _client.Nick = UI_TB_Nick.Text;
                _client.AltNick = UI_TB_AltNick.Text;

                _closeThread = false;
                UI_B_Connect.Enabled = false;
                UI_TB_Channel.Enabled = false;
                UI_TB_AltNick.Enabled = false;
                UI_TB_Nick.Enabled = false;

                LoadFollowers();
                ChannelEvents();
                // Connect to server at their port
                _client.Connect();
            }
            else
            {
                StreamWriter writer = new StreamWriter("following.txt");

                for (int i = 0; i < _following.Count; ++i)
                    writer.WriteLine(_following[i] + " " + _service[i]);

                writer.Flush();
                writer.Close();

                _client.SendMessage(_channel, "!mute");

                _service.Clear();
                _following.Clear();

                UI_B_Connect.Text = "Connect";
                UI_L_Connected.Text = "Not Connected";
                UI_L_Connected.ForeColor = System.Drawing.Color.Red;
                UI_TB_Channel.Enabled = true;
                UI_TB_AltNick.Enabled = true;
                UI_TB_Nick.Enabled = true;

                CloseThread();
                _client.Disconnect();
            }
        }

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
            _client.OnConnect += client_OnConnect;
            _client.ChannelMessage += client_ChannelMessage;
            _client.ExceptionThrown += client_ExceptionThrown;
        }

        #region Listeners
        // Connect to the IRC Server
        // Side Effects: Check the streams in the follow list for availability
        private void client_OnConnect(object sender, EventArgs e)
        {
            _client.JoinChannel(_channel);
            _client.SendMessage(_channel, "!mute");

            UI_B_Connect.Enabled = true;
            UI_B_Connect.Text = "Disconnect";
            UI_L_Connected.Text = "Connected";
            UI_L_Connected.ForeColor = System.Drawing.Color.Green;

            OnLoadCheck();
        }

        // Add all channel messages to the chat
        // Side Effects: Nothing
        private void client_ChannelMessage(object sender, ChannelMessageEventArgs e)
        {
            IrcTextFunctions textFunctions = new IrcTextFunctions(_client, _channel, _following, _service);
            textFunctions.ChatFunctions(e.Message, e.From);
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

            while (!_closeThread)
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
            _client.SendMessage(_channel, n);
        }

        private void CloseThread()
        {
            _closeThread = true;
        }
        #endregion
    }
}
