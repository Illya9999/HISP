﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Horse_Isle_Server
{
    class Server
    {

        public static Socket ServerSocket;


        public static Client[] ConnectedClients // Done to prevent Enumerator Changed errors.
        {
            get {
                return connectedClients.ToArray();
            }
        }

        public static int IdleTimeout;
        public static int IdleWarning;



        // used for world time,
        private static int gameTickSpeed = 4320; // Changing this to ANYTHING else will cause desync with the client.
        private static Timer serverTimer;

        private static List<Client> connectedClients = new List<Client>();
        public static void OnCrossdomainPolicyRequest(Client sender) // When a cross-domain-policy request is received.
        {
            Logger.DebugPrint("Cross-Domain-Policy request received from: " + sender.RemoteIp);

            byte[] crossDomainPolicyResponse = CrossDomainPolicy.GetPolicy(); // Generate response packet

            sender.SendPacket(crossDomainPolicyResponse); // Send to client.
        }

        public static void OnUserInfoRequest(Client sender, byte[] packet)
        {
            if (!sender.LoggedIn)
            {
                Logger.ErrorPrint(sender.RemoteIp + " Requested user information when not logged in.");
                return;
            }
            Logger.DebugPrint(sender.LoggedinUser.Username + " Requested user information.");

            User user = sender.LoggedinUser;

            byte[] MovementPacket = PacketBuilder.CreateMovementPacket(user.X, user.Y, user.CharacterId, user.Facing, PacketBuilder.DIRECTION_TELEPORT, true);
            sender.SendPacket(MovementPacket);

            byte[] WelcomeMessage = PacketBuilder.CreateWelcomeMessage(user.Username);
            sender.SendPacket(WelcomeMessage);

            byte[] WorldData = PacketBuilder.CreateWorldData(World.ServerTime.Minutes, World.ServerTime.Days, World.ServerTime.Years, World.GetWeather());
            sender.SendPacket(WorldData);

            byte[] SecCodePacket = PacketBuilder.CreateSecCode(user.SecCodeSeeds, user.SecCodeInc, user.Administrator, user.Moderator);
            sender.SendPacket(SecCodePacket);

            byte[] BaseStatsPacketData = PacketBuilder.CreatePlayerData(user.Money, Server.GetNumberOfPlayers(), user.MailBox.MailCount);
            sender.SendPacket(BaseStatsPacketData);

            byte[] AreaMessage = PacketBuilder.CreateAreaMessage(user.X, user.Y);
            sender.SendPacket(AreaMessage);

            foreach (Client client in ConnectedClients)
            {
                if (client.LoggedIn)
                {
                    if (client.LoggedinUser.Id != user.Id)
                    {
                        byte[] PlayerInfo = PacketBuilder.CreatePlayerInfoUpdateOrCreate(client.LoggedinUser.X, client.LoggedinUser.Y, client.LoggedinUser.Facing, client.LoggedinUser.CharacterId, client.LoggedinUser.Username);
                        sender.SendPacket(PlayerInfo);
                    }
                }
            }

            foreach (User nearbyUser in Server.GetNearbyUsers(sender.LoggedinUser.X, sender.LoggedinUser.Y, false, false))
                if (nearbyUser.Id != sender.LoggedinUser.Id)
                    UpdateArea(nearbyUser.LoggedinClient);

            byte[] IsleData = PacketBuilder.CreatePlaceData(World.Isles.ToArray(), World.Towns.ToArray(), World.Areas.ToArray());
            sender.SendPacket(IsleData);

            byte[] TileFlags = PacketBuilder.CreateTileOverlayFlags(Map.OverlayTileDepth);
            sender.SendPacket(TileFlags);

            byte[] MotdData = PacketBuilder.CreateMotd();
            sender.SendPacket(MotdData);


        }

        public static void OnKeepAlive(Client sender, byte[] packet)
        {
            if (!sender.LoggedIn)
            {
                Logger.ErrorPrint(sender.RemoteIp + " Requested update when not logged in.");
                return;
            }
            if (packet.Length < 2)
            {
                Logger.ErrorPrint(sender.LoggedinUser.Username + " Sent an invalid update Packet");
                return;
            }

            if (packet[1] == PacketBuilder.PACKET_CLIENT_TERMINATOR)
            {
                Logger.DebugPrint("Sending " + sender.LoggedinUser.Username + " updated info...");
                UpdatePlayer(sender);
            }
        }
        public static void OnProfilePacket(Client sender, byte[] packet)
        {
            if (!sender.LoggedIn)
            {
                Logger.ErrorPrint(sender.RemoteIp + " Requested to change profile page when not logged in.");
                return;
            }
            if (packet.Length < 2)
            {
                Logger.ErrorPrint(sender.LoggedinUser.Username + " Sent an invalid Profile Packet");
                return;
            }

            byte method = packet[1];
            if (method == PacketBuilder.VIEW_PROFILE)
            {
                byte[] profilePacket = PacketBuilder.CreateProfilePacket(sender.LoggedinUser.ProfilePage);
                sender.SendPacket(profilePacket);
            }
            else if (method == PacketBuilder.SAVE_PROFILE)
            {

                string packetStr = Encoding.UTF8.GetString(packet);
                if (packet.Length < 3 || !packetStr.Contains('|'))
                {
                    Logger.ErrorPrint(sender.LoggedinUser.Username + " Sent an invalid Profile SAVE Packet");
                    return;
                }

                int characterId = (packet[2] - 20) * 64 + (packet[3] - 20);

                string profilePage = packetStr.Split('|')[1];
                profilePage = profilePage.Substring(0, profilePage.Length - 2);

                sender.LoggedinUser.CharacterId = characterId;
                sender.LoggedinUser.ProfilePage = profilePage;

                Logger.DebugPrint(sender.LoggedinUser.Username + " Changed to character id: " + characterId + " and set there Profile Description to '" + profilePage + "'");

                byte[] chatPacket = PacketBuilder.CreateChat(Messages.ProfileSavedMessage, PacketBuilder.CHAT_BOTTOM_RIGHT);
                sender.SendPacket(chatPacket);

                UpdateArea(sender);
                UpdateUserInfo(sender.LoggedinUser);
            }

        }

        public static void OnMovementPacket(Client sender, byte[] packet)
        {
            if (!sender.LoggedIn)
            {
                Logger.ErrorPrint(sender.RemoteIp + " Sent movement packet when not logged in.");
                return;
            }

            User loggedInUser = sender.LoggedinUser;
            byte movementDirection = packet[1];

            if (movementDirection == PacketBuilder.MOVE_UP)
            {
                loggedInUser.Facing = PacketBuilder.DIRECTION_UP;
                if (Map.CheckPassable(loggedInUser.X, loggedInUser.Y - 1))
                {
                    loggedInUser.Y -= 1;

                    byte[] moveUpResponse = PacketBuilder.CreateMovementPacket(loggedInUser.X, loggedInUser.Y, loggedInUser.CharacterId, sender.LoggedinUser.Facing, PacketBuilder.DIRECTION_UP, true);
                    sender.SendPacket(moveUpResponse);
                }
                else
                {
                    byte[] moveUpResponse = PacketBuilder.CreateMovementPacket(loggedInUser.X, loggedInUser.Y, loggedInUser.CharacterId, sender.LoggedinUser.Facing, PacketBuilder.DIRECTION_NONE, false);
                    sender.SendPacket(moveUpResponse);
                }
            }
            else if (movementDirection == PacketBuilder.MOVE_LEFT)
            {
                loggedInUser.Facing = PacketBuilder.DIRECTION_LEFT;
                if (Map.CheckPassable(loggedInUser.X - 1, loggedInUser.Y))
                {
                    loggedInUser.X -= 1;
                    byte[] moveLeftResponse = PacketBuilder.CreateMovementPacket(loggedInUser.X, loggedInUser.Y, loggedInUser.CharacterId, loggedInUser.Facing, PacketBuilder.DIRECTION_LEFT, true);
                    sender.SendPacket(moveLeftResponse);
                }
                else
                {
                    byte[] moveLeftResponse = PacketBuilder.CreateMovementPacket(loggedInUser.X, loggedInUser.Y, loggedInUser.CharacterId, loggedInUser.Facing, PacketBuilder.DIRECTION_NONE, false);
                    sender.SendPacket(moveLeftResponse);
                }
            }
            else if (movementDirection == PacketBuilder.MOVE_RIGHT)
            {
                loggedInUser.Facing = PacketBuilder.DIRECTION_RIGHT;
                if (Map.CheckPassable(loggedInUser.X + 1, loggedInUser.Y))
                {
                    loggedInUser.X += 1;
                    byte[] moveLeftResponse = PacketBuilder.CreateMovementPacket(loggedInUser.X, loggedInUser.Y, loggedInUser.CharacterId, loggedInUser.Facing, PacketBuilder.DIRECTION_RIGHT, true);
                    sender.SendPacket(moveLeftResponse);
                }
                else
                {
                    byte[] moveLeftResponse = PacketBuilder.CreateMovementPacket(loggedInUser.X, loggedInUser.Y, loggedInUser.CharacterId, loggedInUser.Facing, PacketBuilder.DIRECTION_NONE, false);
                    sender.SendPacket(moveLeftResponse);
                }
            }
            else if (movementDirection == PacketBuilder.MOVE_DOWN)
            {
                loggedInUser.Facing = PacketBuilder.DIRECTION_DOWN;
                if (Map.CheckPassable(loggedInUser.X, loggedInUser.Y + 1))
                {
                    loggedInUser.Y += 1;
                    byte[] moveDownResponse = PacketBuilder.CreateMovementPacket(loggedInUser.X, loggedInUser.Y, loggedInUser.CharacterId, loggedInUser.Facing, PacketBuilder.DIRECTION_DOWN, true);
                    sender.SendPacket(moveDownResponse);
                }
                else
                {
                    byte[] moveDownResponse = PacketBuilder.CreateMovementPacket(loggedInUser.X, loggedInUser.Y, loggedInUser.CharacterId, loggedInUser.Facing, PacketBuilder.DIRECTION_NONE, false);
                    sender.SendPacket(moveDownResponse);
                }

            }


            UpdateUserInfo(sender.LoggedinUser);
            UpdateArea(sender);
            foreach (User nearbyUser in Server.GetNearbyUsers(sender.LoggedinUser.X, sender.LoggedinUser.Y, false, false))
                if(nearbyUser.Id != sender.LoggedinUser.Id)
                    UpdateArea(nearbyUser.LoggedinClient);
        }

        public static void OnChatPacket(Client sender, byte[] packet)
        {
            if (!sender.LoggedIn)
            {
                Logger.ErrorPrint(sender.RemoteIp + " Sent chat packet when not logged in.");
                return;
            }

            if (packet.Length < 4)
            {
                Logger.ErrorPrint(sender.RemoteIp + " Sent an invalid chat packet.");
                return;
            }


            string packetStr = Encoding.UTF8.GetString(packet);

            Chat.ChatChannel channel = (Chat.ChatChannel)packet[1];
            string message = packetStr.Substring(2, packetStr.Length - 4);
           

            Logger.DebugPrint(sender.LoggedinUser.Username + " Attempting to say '" + message + "' in channel: " + channel.ToString());

            string nameTo = null;
            if (channel == Chat.ChatChannel.Dm)
            {
                nameTo = Chat.GetDmRecipiant(message);
                message = Chat.GetDmMessage(message);
            }

            if (message == "")
                return;

            Object violationReason = Chat.FilterMessage(message);
            if (violationReason != null)
            {
                sender.LoggedinUser.ChatViolations += 1;
                string chatViolationMessage = Messages.FormatGlobalChatViolationMessage((Chat.Reason)violationReason);
                byte[] chatViolationPacket = PacketBuilder.CreateChat(chatViolationMessage, PacketBuilder.CHAT_BOTTOM_RIGHT);
                sender.SendPacket(chatViolationPacket);
                return;
            }

            byte chatSide = Chat.GetSide(channel);
            message = Chat.DoCorrections(message);
            message = Chat.EscapeMessage(message);


            string failedReason = Chat.NonViolationChecks(sender.LoggedinUser, message);
            if (failedReason != null)
            {
                byte[] failedMessage = PacketBuilder.CreateChat(failedReason, PacketBuilder.CHAT_BOTTOM_RIGHT);
                sender.SendPacket(failedMessage);
                return;
            }

            Client[] recipiants = Chat.GetRecipiants(sender.LoggedinUser, channel, nameTo);

            // Finally send chat message.
            string formattedMessage = Chat.FormatChatForOthers(sender.LoggedinUser, channel, message);
            string formattedMessageSender = Chat.FormatChatForSender(sender.LoggedinUser, channel, message, nameTo);
            byte[] chatPacketOthers = PacketBuilder.CreateChat(formattedMessage, chatSide);
            byte[] chatPacketSender = PacketBuilder.CreateChat(formattedMessageSender, chatSide);
            byte[] playDmSound = PacketBuilder.CreatePlaysoundPacket(Chat.PrivateMessageSound);
            // Send to clients ...
            foreach (Client recipiant in recipiants)
            {
                recipiant.SendPacket(chatPacketOthers);
                if (channel == Chat.ChatChannel.Dm)
                    recipiant.SendPacket(playDmSound);
            }

            // Send to sender
            sender.SendPacket(chatPacketSender);
        }

        public static void OnLoginRequest(Client sender, byte[] packet)
        {
            Logger.DebugPrint("Login request received from: " + sender.RemoteIp);

            string loginRequestString = Encoding.UTF8.GetString(packet).Substring(1);

            if (!loginRequestString.Contains('|') || packet.Length < 3)
            {
                Logger.ErrorPrint(sender.RemoteIp + " Sent an invalid login request");
                return;
            }

            if (packet[1] != PacketBuilder.PACKET_CLIENT_TERMINATOR)
            {
                string[] loginParts = loginRequestString.Split('|');
                if (loginParts.Length < 3)
                {
                    Logger.ErrorPrint(sender.RemoteIp + " Sent a login request of invalid length. " + loginRequestString);
                    return;
                }

                int version = int.Parse(loginParts[0]);
                string encryptedUsername = loginParts[1];
                string encryptedPassword = loginParts[2];
                string username = Authentication.DecryptLogin(encryptedUsername);
                string password = Authentication.DecryptLogin(encryptedPassword);

                if (Authentication.CheckPassword(username, password))
                {
                    // Obtain user information
                    int userId = Database.GetUserid(username);
                    sender.Login(userId);
                    sender.LoggedinUser.Password = password;

                    byte[] ResponsePacket = PacketBuilder.CreateLoginPacket(true);
                    sender.SendPacket(ResponsePacket);

                    Logger.DebugPrint(sender.RemoteIp + " Logged into : " + sender.LoggedinUser.Username + " (ADMIN: " + sender.LoggedinUser.Administrator + " MOD: " + sender.LoggedinUser.Moderator + ")");

                    // Send login message
                    byte[] loginMessageBytes = PacketBuilder.CreateChat(Messages.FormatLoginMessage(sender.LoggedinUser.Username), PacketBuilder.CHAT_BOTTOM_LEFT);
                    foreach (Client client in ConnectedClients)
                        if (client.LoggedIn)
                            if (!client.LoggedinUser.MuteLogins)
                                if (client.LoggedinUser.Id != userId)
                                    client.SendPacket(loginMessageBytes);

                    UpdateUserInfo(sender.LoggedinUser);

                }
                else
                {
                    Logger.WarnPrint(sender.RemoteIp + " Attempted to login to: " + username + " with incorrect password " + password);
                    byte[] ResponsePacket = PacketBuilder.CreateLoginPacket(false);
                    sender.SendPacket(ResponsePacket);
                }
            }

        }

        public static void OnDisconnect(Client sender)
        {
            if (sender.LoggedIn)
            {
                // Send disconnect message
                byte[] logoutMessageBytes = PacketBuilder.CreateChat(Messages.FormatLogoutMessage(sender.LoggedinUser.Username), PacketBuilder.CHAT_BOTTOM_LEFT);
                foreach (Client client in ConnectedClients)
                    if (client.LoggedIn)
                        if (!client.LoggedinUser.MuteLogins)
                            if (client.LoggedinUser.Id != sender.LoggedinUser.Id)
                                client.SendPacket(logoutMessageBytes);
                // Tell clients of diconnect (remove from chat)
                byte[] playerRemovePacket = PacketBuilder.CreatePlayerLeavePacket(sender.LoggedinUser.Username);
                foreach (Client client in ConnectedClients)
                    if (client.LoggedIn)
                        if (client.LoggedinUser.Id != sender.LoggedinUser.Id)
                            client.SendPacket(playerRemovePacket);
            }

            connectedClients.Remove(sender);
        }

        public static void UpdateArea(Client forClient)
        {
            if (!forClient.LoggedIn)
            {
                Logger.ErrorPrint(forClient.RemoteIp + "tried to update tile information when not logged in.");
                return;
            }

            byte[] areaData = PacketBuilder.CreateAreaMessage(forClient.LoggedinUser.X, forClient.LoggedinUser.Y);
            forClient.SendPacket(areaData);

        }

        public static void UpdateWorld(Client forClient)
        {
            if (!forClient.LoggedIn)
            {
                Logger.ErrorPrint(forClient.RemoteIp + "tried to update world information when not logged in.");
                return;
            }

            byte[] WorldData = PacketBuilder.CreateWorldData(World.ServerTime.Minutes, World.ServerTime.Days, World.ServerTime.Years, World.GetWeather());
            forClient.SendPacket(WorldData);
        }

        public static void UpdatePlayer(Client forClient)
        {
            if (!forClient.LoggedIn)
            {
                Logger.ErrorPrint(forClient.RemoteIp + "tried to update player information when not logged in.");
                return;
            }
            byte[] PlayerData = PacketBuilder.CreatePlayerData(forClient.LoggedinUser.Money, Server.GetNumberOfPlayers(), forClient.LoggedinUser.MailBox.MailCount);
            forClient.SendPacket(PlayerData);
        }

        public static void UpdateUserInfo(User user)
        {
            byte[] playerInfoBytes = PacketBuilder.CreatePlayerInfoUpdateOrCreate(user.X, user.Y, user.Facing, user.CharacterId, user.Username);



            List<User> users = new List<User>();
            foreach (Client client in ConnectedClients)
                if (client.LoggedIn)
                {
                    if (client.LoggedinUser.Id != user.Id)
                        client.SendPacket(playerInfoBytes);
                }
               

        }


        public static User[] GetUsersUsersInIsle(World.Isle isle, bool includeStealth = false, bool includeMuted = false)
        {
            List<User> usersInIsle = new List<User>();
            foreach (Client client in ConnectedClients)
                if (client.LoggedIn)
                {
                    if (!includeStealth && client.LoggedinUser.Stealth)
                        continue;
                    if (!includeMuted && client.LoggedinUser.MuteIsland)
                        continue;
                    if (World.InIsle(client.LoggedinUser.X, client.LoggedinUser.Y))
                        if (World.GetIsle(client.LoggedinUser.X, client.LoggedinUser.Y).Name == isle.Name)
                            usersInIsle.Add(client.LoggedinUser);
                }

            return usersInIsle.ToArray();
        }

        public static User[] GetUsersAt(int x, int y, bool includeStealth = false, bool includeMuted = false)
        {
            List<User> usersHere = new List<User>();
            foreach(Client client in ConnectedClients)
            {
                if(client.LoggedIn)
                {
                    if (!includeStealth && client.LoggedinUser.Stealth)
                        continue;
                    if (!includeMuted && client.LoggedinUser.MuteNear)
                        continue;
                    if (client.LoggedinUser.X == x && client.LoggedinUser.Y == y)
                        usersHere.Add(client.LoggedinUser);
                }
            }
            return usersHere.ToArray();
        }

        public static User[] GetNearbyUsers(int x, int y, bool includeStealth=false, bool includeMuted=false)
        {
            int startX = x - 15;
            int endX = x + 15;
            int startY = y - 19;
            int endY = y + 19;
            List<User> usersNearby = new List<User>();

            foreach (Client client in ConnectedClients)
                if (client.LoggedIn)
                {
                    if (!includeStealth && client.LoggedinUser.Stealth)
                        continue;
                    if (!includeMuted && client.LoggedinUser.MuteNear)
                        continue;
                    if (startX <= client.LoggedinUser.X && endX >= client.LoggedinUser.X && startY <= client.LoggedinUser.Y && endY >= client.LoggedinUser.Y)
                        usersNearby.Add(client.LoggedinUser);
                }

            return usersNearby.ToArray();
        }

        public static int GetNumberOfPlayers(bool includeStealth=false)
        {
            int count = 0;
            foreach(Client client in ConnectedClients)
                if (client.LoggedIn)
                {
                    if (!includeStealth && client.LoggedinUser.Stealth)
                        continue;
                    if (!client.LoggedinUser.Stealth)
                        count++;
                }
            
            return count;
        }

        public static int GetNumberOfModsOnline()
        {
            int count = 0;
            foreach (Client client in ConnectedClients)
            {
                if (client.LoggedIn)
                    if(client.LoggedinUser.Moderator)
                        count++;
            }
            return count;
        }

        public static int GetNumberOfAdminsOnline()
        {
            int count = 0;
            foreach (Client client in ConnectedClients)
            {
                if (client.LoggedIn)
                    if (client.LoggedinUser.Administrator)
                        count++;
            }
            return count;
        }

        private static void onTick(object state)
        {
            World.TickWorldClock();
        }
        public static void StartServer()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress hostIP = IPAddress.Parse(ConfigReader.BindIP);
            IPEndPoint ep = new IPEndPoint(hostIP, ConfigReader.Port);
            ServerSocket.Bind(ep);
            Logger.DebugPrint("Binding to ip: " + ConfigReader.BindIP + " On port: " + ConfigReader.Port.ToString());
            ServerSocket.Listen(10000);

            serverTimer = new Timer(new TimerCallback(onTick), null, gameTickSpeed, gameTickSpeed);

            while (true)
            {
                Logger.DebugPrint("Waiting for new connections...");

                Socket cientSocket = ServerSocket.Accept();
                Client client = new Client(cientSocket);
                connectedClients.Add(client);
            }
        }
    }
}
