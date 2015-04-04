# ZeroBot
*Created by Kevin Nguyen*

It is an IRC Bot that periodically receive messages from users and outputs a response depending if the users used a chat command or not.

The users cannot type in a channel name, server name, or nickname for the bot yet. Everything is hardcoded in. **This will change later.**

---
Current custom functions created :

    OnLoadCheck - Initializes a background Thread for periodic checking
    CheckFollowers - Periodically check followers in Twitch.TV and Hitbox.TV if they go online/offline
    ChatFunctions - Handle all chat functions in an IRC Channel
    CheckTwitch - Check stream availability on Twitch.TV
    CheckHitbox - Check stream availablity on Hitbox.TV
    LoadFollowers - Load all followers from a text file
--

APIs Used in this project:
* [Twitch.TV](https://github.com/justintv/Twitch-API)
* [Hitbox.TV](http://developers.hitbox.tv/)

---
Sources Used:
* [JSON.NET](http://www.newtonsoft.com/json)
* [ircClient](https://github.com/cshivers/IrcClient-csharp)
