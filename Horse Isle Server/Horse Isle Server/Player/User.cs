﻿using System;
using System.Collections.Generic;
using HISP.Game;
using HISP.Server;
using HISP.Player.Equips;
using HISP.Game.Services;
using HISP.Game.Inventory;

namespace HISP.Player
{
    class User
    {

        public int Id;
        public string Username;
        public bool Administrator;
        public bool Moderator;
        public bool NewPlayer = false;
        public GameClient LoggedinClient;
        public CompetitionGear EquipedCompetitionGear;
        public Jewelry EquipedJewelry;
        public bool MuteAds = false;
        public bool MuteGlobal = false;
        public bool MuteIsland = false;
        public bool MuteNear = false;
        public bool MuteHere = false;
        public bool MuteBuddy = false;
        public bool MutePrivateMessage = false;
        public bool MuteBuddyRequests = false;
        public bool MuteSocials = false;
        public bool MuteAll = false;
        public bool MuteLogins = false;
        public string Gender;
        public bool MetaPriority = false;

        public bool Idle;
        public int Facing;
        public Mailbox MailBox;
        public Friends Friends;
        public string Password; // For chat filter.
        public PlayerInventory Inventory;
        public Npc.NpcEntry LastTalkedToNpc;
        public Shop LastShoppedAt;
        public Inn LastVisitedInn;
        public PlayerQuests Quests;
        public Highscore Highscores;
        public Award Awards;
        public DateTime LoginTime;

        public DateTime SubscribedUntil
        {
            get
            {
                return Converters.UnixTimeStampToDateTime(subscribedUntil);
            }
        }
        public int FreeMinutes
        {
            get
            {
                int freeTime = Database.GetFreeTime(Id);
                return freeTime;
            }
            set
            {
                Database.SetFreeTime(Id, value);
            }
        }
        public bool Subscribed
        { 
            get
            {
                if (ConfigReader.AllUsersSubbed)
                    return true;
                
                int Timestamp = Convert.ToInt32(new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());
                if(Timestamp > subscribedUntil && subscribed) // sub expired.
                {
                    Logger.InfoPrint(Username + "'s Subscription expired. (timestamp now: " + Timestamp + " exp date: " + subscribedUntil+" )");
                    Database.SetUserSubscriptionStatus(this.Id, false);
                    subscribed = false;
                }

                return subscribed;
            }
            set
            {
                Database.SetUserSubscriptionStatus(this.Id, value);
            }
        }
        public bool Stealth
        {
            get
            {
                return stealth;
            }
            set
            {
                if (value)
                    Database.RemoveOnlineUser(this.Id);
                else
                    Database.AddOnlineUser(this.Id, this.Administrator, this.Moderator, this.Subscribed);

                stealth = value;
            }
        }
        public int ChatViolations
        {
            get
            {
                return chatViolations;
            }
            set
            {
                Database.SetChatViolations(value,Id);
                chatViolations = value;
            }
        }

        public string PrivateNotes
        {
            get
            {
                return privateNotes;
            }
            set
            {

                Database.SetPlayerNotes(Id, value);
                privateNotes = value;
            }
        }
        public string ProfilePage { 
            get 
            { 
                return profilePage; 
            }
            set 
            { 
                
                Database.SetPlayerProfile(value, Id);
                profilePage = value;
            } 
        }

        public int Money
        {
            get
            {
                return money;
            }
            set
            {
                money = value;
                Database.SetPlayerMoney(value, Id);
                GameServer.UpdatePlayer(LoggedinClient);
            }
        }


        public int Experience
        {
            get
            {
                return experience;
            }
            set
            {
                Database.SetExperience(Id, value);
                experience = value;
            }
        }
        public int QuestPoints
        {
            get
            {
                return questPoints;
            }
            set
            {
                Database.SetPlayerQuestPoints(value, Id);
                questPoints = value;
            }
        }

        public UInt64 BankMoney
        {
            get
            {
                return bankMoney;
            }
            set
            {
                Database.SetPlayerBankMoney(value, Id);
                bankMoney = value;
            }
        }

        public int X
        {
            get
            {
                return x;
            }
            set
            {
                Database.SetPlayerX(value, Id);
                x = value;
            }
        }

        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                Database.SetPlayerY(value, Id);
                y = value;
            }
        }

        public int CharacterId
        {
            get
            {
                return charId;
            }
            set
            {
                Database.SetPlayerCharId(value, Id);
                charId = value;
            }
        }


        public int Hunger
        {
            get
            {
                return hunger;
            }
            set
            {
                if (value >= 1000)
                    value = 1000;
                if (value <= 0)
                    value = 0;
                Database.SetPlayerHunger(Id, value);
                hunger = value;
            }
        }

        public int Thirst
        {
            get
            {
                return thirst;
            }
            set
            {
                if (value >= 1000)
                    value = 1000;
                if (value <= 0)
                    value = 0;
                Database.SetPlayerHunger(Id, value);
                thirst = value;
            }
        }

        public int Tiredness
        {
            get
            {
                return tired;
            }
            set
            {
                if (value >= 1000)
                    value = 1000;
                if (value <= 0)
                    value = 0;
                Database.SetPlayerTiredness(Id, value);
                tired = value;
            }
        }


        private int chatViolations;
        private int charId;
        private int subscribedUntil;
        private bool subscribed;
        private string profilePage;
        private string privateNotes;
        private int x;
        private bool stealth = false;
        private int y;
        private int money;
        private int questPoints;
        private UInt64 bankMoney;
        private int experience;
        private int hunger;
        private int thirst;
        private int tired;


        public byte[] SecCodeSeeds = new byte[3];
        public  int SecCodeInc = 0;
        public int SecCodeCount = 0;


        public int GetPlayerListIcon()
        {
            int icon = -1;
            if (NewPlayer)
                icon = Messages.NewUserIcon;
            if (Subscribed)
            {
                int months = (DateTime.UtcNow.Month - SubscribedUntil.Month) + 12 * (DateTime.UtcNow.Year - SubscribedUntil.Year);
                if (months <= 1)
                    icon = Messages.MonthSubscriptionIcon;
                else if (months <= 3)
                    icon = Messages.ThreeMonthSubscripitionIcon;
                else
                    icon = Messages.YearSubscriptionIcon;
            }
            if (Moderator)
                icon = Messages.ModeratorIcon;
            if (Administrator)
                icon = Messages.AdminIcon;

            return icon;
        }
        public void Teleport(int newX, int newY)
        {
            Logger.DebugPrint("Teleporting: " + Username + " to: " + newX.ToString() + "," + newY.ToString());

            X = newX;
            Y = newY;

            byte[] MovementPacket = PacketBuilder.CreateMovementPacket(X, Y, CharacterId, Facing, PacketBuilder.DIRECTION_TELEPORT, true);
            LoggedinClient.SendPacket(MovementPacket);
            GameServer.Update(LoggedinClient);
        }

        public byte[] GenerateSecCode()
        {
            var i = 0;
            SecCodeCount++;
            SecCodeSeeds[SecCodeCount % 3] = (byte)(SecCodeSeeds[SecCodeCount % 3] + SecCodeInc);
            SecCodeSeeds[SecCodeCount % 3] = (byte)(SecCodeSeeds[SecCodeCount % 3] % 92);
            i = SecCodeSeeds[0] + SecCodeSeeds[1] * SecCodeSeeds[2] - SecCodeSeeds[1];
            i = Math.Abs(i);
            i = i % 92;

            byte[] SecCode = new byte[4];
            SecCode[0] = (byte)(SecCodeSeeds[0] + 33);
            SecCode[1] = (byte)(SecCodeSeeds[1] + 33);
            SecCode[2] = (byte)(SecCodeSeeds[2] + 33);
            SecCode[3] = (byte)(i + 33);
            Logger.DebugPrint("Expecting "+Username+" To send Sec Code: "+BitConverter.ToString(SecCode).Replace("-", " "));
            return SecCode;
        }


        public User(GameClient baseClient, int UserId)
        {
            if (!Database.CheckUserExist(UserId))
                throw new KeyNotFoundException("User " + UserId + " not found in database!");

            if (!Database.CheckUserExtExists(UserId))
            {
                Database.CreateUserExt(UserId);
                NewPlayer = true;
            }


            EquipedCompetitionGear = new CompetitionGear(UserId);
            EquipedJewelry = new Jewelry(UserId);

            Id = UserId;
            Username = Database.GetUsername(UserId);
            
            Administrator = Database.CheckUserIsAdmin(Username);
            Moderator = Database.CheckUserIsModerator(Username);

            chatViolations = Database.GetChatViolations(UserId);
            x = Database.GetPlayerX(UserId);
            y = Database.GetPlayerY(UserId);
            charId = Database.GetPlayerCharId(UserId);

            Facing = PacketBuilder.DIRECTION_DOWN;
            experience = Database.GetExperience(UserId);
            money = Database.GetPlayerMoney(UserId);
            bankMoney = Database.GetPlayerBankMoney(UserId);
            questPoints = Database.GetPlayerQuestPoints(UserId);
            subscribed = Database.IsUserSubscribed(UserId);
            subscribedUntil = Database.GetUserSubscriptionExpireDate(UserId);
            profilePage = Database.GetPlayerProfile(UserId);
            privateNotes = Database.GetPlayerNotes(UserId);
            hunger = Database.GetPlayerHunger(UserId);
            thirst = Database.GetPlayerThirst(UserId);
            tired = Database.GetPlayerTiredness(UserId);

            Gender = Database.GetGender(UserId);
            MailBox = new Mailbox(this);
            Highscores = new Highscore(this);
            Awards = new Award(this);

            // Generate SecCodes


            SecCodeSeeds[0] = (byte)GameServer.RandomNumberGenerator.Next(40, 60);
            SecCodeSeeds[1] = (byte)GameServer.RandomNumberGenerator.Next(40, 60);
            SecCodeSeeds[2] = (byte)GameServer.RandomNumberGenerator.Next(40, 60);
            SecCodeInc = (byte)GameServer.RandomNumberGenerator.Next(40, 60);


            Friends = new Friends(this);
            LoginTime = DateTime.UtcNow;
            LoggedinClient = baseClient;
            Inventory = new PlayerInventory(this);
            Quests = new PlayerQuests(this);
        }
    }
}
