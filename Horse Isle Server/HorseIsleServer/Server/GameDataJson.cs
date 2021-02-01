﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using HISP.Game;
using HISP.Game.Chat;
using HISP.Player;
using HISP.Game.Services;
using HISP.Game.SwfModules;
using HISP.Game.Horse;
using HISP.Game.Items;

namespace HISP.Server
{
    public class GameDataJson
    {
        
        public static void ReadGamedata()
        {
            if(!File.Exists(ConfigReader.GameDataFile))
            {
                Logger.ErrorPrint("Game Data JSON File: " + ConfigReader.GameDataFile + " Does not exist!");
                throw new FileNotFoundException(ConfigReader.GameDataFile + " Not found :(");
            }
            string jsonData = File.ReadAllText(ConfigReader.GameDataFile);
            dynamic gameData = JsonConvert.DeserializeObject(jsonData);

            // Register Towns
            int totalTowns = gameData.places.towns.Count;
            for (int i = 0; i < totalTowns; i++)
            {

                World.Town town = new World.Town();
                town.StartX = gameData.places.towns[i].start_x;
                town.StartY = gameData.places.towns[i].start_y;
                town.EndX = gameData.places.towns[i].end_x;
                town.EndY = gameData.places.towns[i].end_y;
                town.Name = gameData.places.towns[i].name;

                Logger.DebugPrint("Registered Town: " + town.Name + " X " + town.StartX + "-" + town.EndX + " Y " + town.StartY + "-" + town.EndY);
                World.Towns.Add(town);
            }

            // Register Zones
            int totalZones = gameData.places.zones.Count;
            for (int i = 0; i < totalZones; i++)
            {

                World.Zone zone = new World.Zone();
                zone.StartX = gameData.places.zones[i].start_x;
                zone.StartY = gameData.places.zones[i].start_y;
                zone.EndX = gameData.places.zones[i].end_x;
                zone.EndY = gameData.places.zones[i].end_y;
                zone.Name = gameData.places.zones[i].name;

                Logger.DebugPrint("Registered Zone: " + zone.Name + " X " + zone.StartX + "-" + zone.EndX + " Y " + zone.StartY + "-" + zone.EndY);
                World.Zones.Add(zone);
            }

            // Register Areas
            int totalAreas = gameData.places.areas.Count;
            for (int i = 0; i < totalAreas; i++)
            {

                World.Area area = new World.Area();
                area.StartX = gameData.places.areas[i].start_x;
                area.StartY = gameData.places.areas[i].start_y;
                area.EndX = gameData.places.areas[i].end_x;
                area.EndY = gameData.places.areas[i].end_y;
                area.Name = gameData.places.areas[i].name;

                Logger.DebugPrint("Registered Area: " + area.Name + " X " + area.StartX + "-" + area.EndX + " Y " + area.StartY + "-" + area.EndY);
                World.Areas.Add(area);
            }

            // Register Isles
            int totalIsles = gameData.places.isles.Count;
            for(int i = 0; i < totalIsles; i++)
            {

                World.Isle isle = new World.Isle();
                isle.StartX = gameData.places.isles[i].start_x;
                isle.StartY = gameData.places.isles[i].start_y;
                isle.EndX = gameData.places.isles[i].end_x;
                isle.EndY = gameData.places.isles[i].end_y;
                isle.Tileset = gameData.places.isles[i].tileset;
                isle.Name = gameData.places.isles[i].name;

                Logger.DebugPrint("Registered Isle: " + isle.Name + " X " + isle.StartX + "-" + isle.EndX + " Y " + isle.StartY + "-" + isle.EndY + " tileset: " + isle.Tileset);
                World.Isles.Add(isle);
            }

            // Register Special Tiles
            int totalSpecialTiles = gameData.places.special_tiles.Count;
            for (int i = 0; i < totalSpecialTiles; i++)
            {

                World.SpecialTile specialTile = new World.SpecialTile();
                specialTile.X = gameData.places.special_tiles[i].x;
                specialTile.Y = gameData.places.special_tiles[i].y;
                specialTile.Title = gameData.places.special_tiles[i].title;
                specialTile.Description = gameData.places.special_tiles[i].description;
                specialTile.Code = gameData.places.special_tiles[i].code;
                if(gameData.places.special_tiles[i].exit_x != null)
                    specialTile.ExitX = gameData.places.special_tiles[i].exit_x;
                if(gameData.places.special_tiles[i].exit_x != null)
                    specialTile.ExitY = gameData.places.special_tiles[i].exit_y;
                specialTile.AutoplaySwf = gameData.places.special_tiles[i].autoplay_swf;
                specialTile.TypeFlag = gameData.places.special_tiles[i].type_flag;

                Logger.DebugPrint("Registered Special Tile: " + specialTile.Title + " X " + specialTile.X + " Y: " + specialTile.Y);
                World.SpecialTiles.Add(specialTile);
            }

            // Register Filter Reasons
            int totalReasons = gameData.messages.chat.reason_messages.Count;
            for(int i = 0; i < totalReasons; i++)
            {
                Chat.Reason reason = new Chat.Reason();
                reason.Name = gameData.messages.chat.reason_messages[i].name;
                reason.Message = gameData.messages.chat.reason_messages[i].message;
                Chat.Reasons.Add(reason);

                Logger.DebugPrint("Registered Chat Warning Reason: " + reason.Name + " (Message: " + reason.Message + ")");
            }
            // Register Filters

            int totalFilters = gameData.messages.chat.filter.Count;
            for(int i = 0; i < totalFilters; i++)
            {
                Chat.Filter filter = new Chat.Filter();
                filter.FilteredWord = gameData.messages.chat.filter[i].word;
                filter.MatchAll = gameData.messages.chat.filter[i].match_all;
                filter.Reason = Chat.GetReason((string)gameData.messages.chat.filter[i].reason_type);
                Chat.FilteredWords.Add(filter);

                Logger.DebugPrint("Registered Filtered Word: " + filter.FilteredWord + " With reason: "+filter.Reason.Name+" (Matching all: " + filter.MatchAll + ")");
            }

            // Register Corrections
            int totalCorrections = gameData.messages.chat.correct.Count;
            for (int i = 0; i < totalCorrections; i++)
            {
                Chat.Correction correction = new Chat.Correction();
                correction.FilteredWord = gameData.messages.chat.correct[i].word;
                correction.ReplacedWord = gameData.messages.chat.correct[i].new_word;
                Chat.CorrectedWords.Add(correction);

                Logger.DebugPrint("Registered Word Correction: " + correction.FilteredWord + " to "+correction.ReplacedWord);
            }

            // Register Transports

            int totalTransportPoints = gameData.transport.transport_points.Count;
            for (int i = 0; i < totalTransportPoints; i++)
            {
                Transport.TransportPoint transportPoint = new Transport.TransportPoint();
                transportPoint.X = gameData.transport.transport_points[i].x;
                transportPoint.Y = gameData.transport.transport_points[i].y;
                transportPoint.Locations = gameData.transport.transport_points[i].places.ToObject<int[]>();
                Transport.TransportPoints.Add(transportPoint);

                Logger.DebugPrint("Registered Transport Point: At X: " + transportPoint.X + " Y: " + transportPoint.Y);
            }

            int totalTransportPlaces = gameData.transport.transport_places.Count;
            for (int i = 0; i < totalTransportPlaces; i++)
            {
                Transport.TransportLocation transportPlace = new Transport.TransportLocation();
                transportPlace.Id = gameData.transport.transport_places[i].id;
                transportPlace.Cost = gameData.transport.transport_places[i].cost;
                transportPlace.GotoX = gameData.transport.transport_places[i].goto_x;
                transportPlace.GotoY = gameData.transport.transport_places[i].goto_y;
                transportPlace.Type = gameData.transport.transport_places[i].type;
                transportPlace.LocationTitle = gameData.transport.transport_places[i].place_title;
                Transport.TransportLocations.Add(transportPlace);

                Logger.DebugPrint("Registered Transport Location: "+ transportPlace.LocationTitle+" To Goto X: " + transportPlace.GotoX + " Y: " + transportPlace.GotoY);
            }

            // Register Items
            int totalItems = gameData.item.item_list.Count;
            for (int i = 0; i < totalItems; i++)
            {
                Item.ItemInformation item = new Item.ItemInformation();
                item.Id = gameData.item.item_list[i].id;
                item.Name = gameData.item.item_list[i].name;
                item.PluralName = gameData.item.item_list[i].plural_name;
                item.Description = gameData.item.item_list[i].description;
                item.IconId = gameData.item.item_list[i].icon_id;
                item.SortBy = gameData.item.item_list[i].sort_by;
                item.SellPrice = gameData.item.item_list[i].sell_price;
                item.EmbedSwf = gameData.item.item_list[i].embed_swf;
                item.WishingWell = gameData.item.item_list[i].wishing_well;
                item.Type = gameData.item.item_list[i].type;
                item.MiscFlags = gameData.item.item_list[i].misc_flags.ToObject<int[]>();
                int effectsCount = gameData.item.item_list[i].effects.Count;

                Item.Effects[] effectsList = new Item.Effects[effectsCount];
                for(int ii = 0; ii < effectsCount; ii++)
                {
                    effectsList[ii] = new Item.Effects();
                    effectsList[ii].EffectsWhat = gameData.item.item_list[i].effects[ii].effect_what;
                    effectsList[ii].EffectAmount = gameData.item.item_list[i].effects[ii].effect_amount;
                }

                item.Effects = effectsList;
                item.SpawnParamaters = new Item.SpawnRules();
                item.SpawnParamaters.SpawnCap = gameData.item.item_list[i].spawn_parameters.spawn_cap;
                item.SpawnParamaters.SpawnInZone = gameData.item.item_list[i].spawn_parameters.spawn_in_zone;
                item.SpawnParamaters.SpawnOnTileType = gameData.item.item_list[i].spawn_parameters.spawn_on_tile_type;
                item.SpawnParamaters.SpawnOnSpecialTile = gameData.item.item_list[i].spawn_parameters.spawn_on_special_tile;
                item.SpawnParamaters.SpawnNearSpecialTile = gameData.item.item_list[i].spawn_parameters.spawn_near_special_tile;

                Logger.DebugPrint("Registered Item ID: " + item.Id + " Name: " + item.Name + " spawns on: "+item.SpawnParamaters.SpawnOnTileType);
                Item.Items.Add(item);
            }

            int totalThrowable = gameData.item.throwable.Count;
            for(int i = 0; i < totalThrowable; i++)
            {
                Item.ThrowableItem throwableItem = new Item.ThrowableItem();
                throwableItem.Id = gameData.item.throwable[i].id;
                throwableItem.Message = gameData.item.throwable[i].message;
                Item.ThrowableItems.Add(throwableItem);
            }

            // Register NPCs
            Logger.DebugPrint("Registering NPCS: ");
            int totalNpcs = gameData.npc_list.Count;
            for(int i = 0; i < totalNpcs; i++)
            {
                Npc.NpcEntry npcEntry = new Npc.NpcEntry();
                npcEntry.Id = gameData.npc_list[i].id;
                npcEntry.Name = gameData.npc_list[i].name;
                npcEntry.AdminDescription = gameData.npc_list[i].admin_description;
                npcEntry.ShortDescription = gameData.npc_list[i].short_description;
                npcEntry.LongDescription = gameData.npc_list[i].long_description;
                npcEntry.Moves = gameData.npc_list[i].moves;
                npcEntry.X = gameData.npc_list[i].x;
                npcEntry.Y = gameData.npc_list[i].y;
                if (gameData.npc_list[i].stay_on != null)
                    npcEntry.StayOn = gameData.npc_list[i].stay_on;
                if (gameData.npc_list[i].requires_questid_completed != null)
                    npcEntry.RequiresQuestIdCompleted = gameData.npc_list[i].requires_questid_completed;
                if (gameData.npc_list[i].requires_questid_not_completed != null)
                    npcEntry.RequiresQuestIdNotCompleted = gameData.npc_list[i].requires_questid_not_completed;
                if (gameData.npc_list[i].udlr_script != null)
                    npcEntry.UDLRScript = gameData.npc_list[i].udlr_script;
                if (gameData.npc_list[i].udlr_start_x != null)
                    npcEntry.UDLRStartX = gameData.npc_list[i].udlr_start_x;
                if (gameData.npc_list[i].udlr_start_y != null)
                    npcEntry.UDLRStartY = gameData.npc_list[i].udlr_start_y;
                npcEntry.AdminOnly = gameData.npc_list[i].admin_only;
                npcEntry.LibarySearchable = gameData.npc_list[i].libary_searchable;
                npcEntry.IconId = gameData.npc_list[i].icon_id;

                Logger.DebugPrint("NPC ID:" + npcEntry.Id.ToString() + " NAME: " + npcEntry.Name);
                List<Npc.NpcChat> chats = new List<Npc.NpcChat>();
                int totalNpcChat = gameData.npc_list[i].chatpoints.Count;
                for (int ii = 0; ii < totalNpcChat; ii++)
                {
                    Npc.NpcChat npcChat = new Npc.NpcChat();
                    npcChat.Id = gameData.npc_list[i].chatpoints[ii].chatpoint_id;
                    npcChat.ChatText = gameData.npc_list[i].chatpoints[ii].chat_text;
                    npcChat.ActivateQuestId = gameData.npc_list[i].chatpoints[ii].activate_questid;

                    Logger.DebugPrint("CHATPOINT ID: " + npcChat.Id.ToString() + " TEXT: " + npcChat.ChatText);
                    int totalNpcReply = gameData.npc_list[i].chatpoints[ii].replies.Count;
                    List<Npc.NpcReply> replys = new List<Npc.NpcReply>();
                    for (int iii = 0; iii < totalNpcReply; iii++)
                    {
                        Npc.NpcReply npcReply = new Npc.NpcReply();
                        npcReply.Id = gameData.npc_list[i].chatpoints[ii].replies[iii].reply_id;
                        npcReply.ReplyText = gameData.npc_list[i].chatpoints[ii].replies[iii].reply_text;
                        npcReply.GotoChatpoint = gameData.npc_list[i].chatpoints[ii].replies[iii].goto_chatpoint;

                        if (gameData.npc_list[i].chatpoints[ii].replies[iii].requires_questid_completed != null)
                            npcReply.RequiresQuestIdCompleted = gameData.npc_list[i].chatpoints[ii].replies[iii].requires_questid_completed;

                        if (gameData.npc_list[i].chatpoints[ii].replies[iii].requires_questid_not_completed != null)
                            npcReply.RequiresQuestIdNotCompleted = gameData.npc_list[i].chatpoints[ii].replies[iii].requires_questid_not_completed;

                        Logger.DebugPrint("REPLY ID: " + npcReply.Id.ToString() + " TEXT: " + npcReply.ReplyText);
                        replys.Add(npcReply);

                    }
                    npcChat.Replies = replys.ToArray();
                    chats.Add(npcChat);
                }
                npcEntry.Chatpoints = chats.ToArray();
                Npc.NpcList.Add(npcEntry);
            }

            // Register Quests

            Logger.DebugPrint("Registering Quests: ");
            int totalQuests = gameData.quest_list.Count;
            for(int i = 0; i < totalQuests; i++)
            {
                Quest.QuestEntry quest = new Quest.QuestEntry();
                quest.Id = gameData.quest_list[i].id;
                quest.Notes = gameData.quest_list[i].notes;
                if(gameData.quest_list[i].title != null)
                    quest.Title = gameData.quest_list[i].title;
                quest.RequiresQuestIdCompleteStatsMenu = gameData.quest_list[i].requires_questid_statsmenu.ToObject<int[]>();
                if (gameData.quest_list[i].alt_activation != null)
                {
                    quest.AltActivation = new Quest.QuestAltActivation();
                    quest.AltActivation.Type = gameData.quest_list[i].alt_activation.type;
                    quest.AltActivation.ActivateX = gameData.quest_list[i].alt_activation.x;
                    quest.AltActivation.ActivateY = gameData.quest_list[i].alt_activation.y;
                }
                quest.Tracked = gameData.quest_list[i].tracked;
                quest.MaxRepeats = gameData.quest_list[i].max_repeats;
                quest.MoneyCost = gameData.quest_list[i].money_cost;
                int itemsRequiredCount = gameData.quest_list[i].items_required.Count;

                List<Quest.QuestItemInfo> itmInfo = new List<Quest.QuestItemInfo>();
                for(int ii = 0; ii < itemsRequiredCount; ii++)
                {
                    Quest.QuestItemInfo itemInfo = new Quest.QuestItemInfo();
                    itemInfo.ItemId = gameData.quest_list[i].items_required[ii].item_id;
                    itemInfo.Quantity = gameData.quest_list[i].items_required[ii].quantity;
                    itmInfo.Add(itemInfo);
                }
                quest.ItemsRequired = itmInfo.ToArray();
                if(gameData.quest_list[i].fail_npc_chat != null)
                    quest.FailNpcChat = gameData.quest_list[i].fail_npc_chat;
                quest.MoneyEarned = gameData.quest_list[i].money_gained;

                int itemsGainedCount = gameData.quest_list[i].items_gained.Count;
                itmInfo = new List<Quest.QuestItemInfo>();
                for (int ii = 0; ii < itemsGainedCount; ii++)
                {
                    Quest.QuestItemInfo itemInfo = new Quest.QuestItemInfo();
                    itemInfo.ItemId = gameData.quest_list[i].items_gained[ii].item_id;
                    itemInfo.Quantity = gameData.quest_list[i].items_gained[ii].quantity;
                    itmInfo.Add(itemInfo);
                }
                quest.ItemsEarned = itmInfo.ToArray();

                quest.QuestPointsEarned = gameData.quest_list[i].quest_points;
                quest.SetNpcChatpoint = gameData.quest_list[i].set_npc_chatpoint;
                quest.GotoNpcChatpoint = gameData.quest_list[i].goto_npc_chatpoint;
                if(gameData.quest_list[i].warp_x != null)
                    quest.WarpX = gameData.quest_list[i].warp_x;
                if(gameData.quest_list[i].warp_y != null)
                    quest.WarpY = gameData.quest_list[i].warp_y;
                if(gameData.quest_list[i].success_message != null)
                    quest.SuccessMessage = gameData.quest_list[i].success_message;
                if(gameData.quest_list[i].success_npc_chat != null)
                    quest.SuccessNpcChat = gameData.quest_list[i].success_npc_chat;
                if (gameData.quest_list[i].requires_awardid != null)
                    quest.AwardRequired = gameData.quest_list[i].requires_awardid;
                quest.RequiresQuestIdCompleted = gameData.quest_list[i].requires_questid_completed.ToObject<int[]>();
                quest.RequiresQuestIdNotCompleted = gameData.quest_list[i].requires_questid_not_completed.ToObject<int[]>();
                quest.HideReplyOnFail = gameData.quest_list[i].hide_reply_on_fail;
                if (gameData.quest_list[i].difficulty != null)
                    quest.Difficulty = gameData.quest_list[i].difficulty;
                if (gameData.quest_list[i].author != null)
                    quest.Author = gameData.quest_list[i].author;
                if (gameData.quest_list[i].chained_questid != null)
                    quest.ChainedQuestId = gameData.quest_list[i].chained_questid;
                quest.Minigame = gameData.quest_list[i].minigame;
                Logger.DebugPrint("Registered Quest: " + quest.Id +" - "+ quest.Title);
                Quest.QuestList.Add(quest);
            }

            int totalShops = gameData.shop_list.Count;
            for(int i = 0; i < totalShops; i++)
            {

                Shop shop = new Shop(gameData.shop_list[i].stocks_itemids.ToObject<int[]>());
                shop.BuyPricePercentage = gameData.shop_list[i].buy_percent;
                shop.SellPricePercentage = gameData.shop_list[i].sell_percent;
                shop.BuysItemTypes = gameData.shop_list[i].buys_item_types.ToObject<string[]>();
                
                Logger.DebugPrint("Registered Shop ID: "+ shop.Id + " Selling items at " + shop.SellPricePercentage + "% and buying at " + shop.BuyPricePercentage);
            }

            // Register awards

            int totalAwards = gameData.award_list.Count;
            Award.GlobalAwardList = new Award.AwardEntry[totalAwards];
            for (int i = 0; i < totalAwards; i++)
            {

                Award.AwardEntry award = new Award.AwardEntry();
                award.Id = i+1;
                award.Sort = gameData.award_list[i].sort_by;
                award.Title = gameData.award_list[i].title;
                award.IconId = gameData.award_list[i].icon_id;
                award.MoneyBonus = gameData.award_list[i].earn_money;
                award.CompletionText = gameData.award_list[i].on_complete_text;
                award.Description = gameData.award_list[i].description;

                Award.GlobalAwardList[i] = award;

                Logger.DebugPrint("Registered Award ID: " + award.Id + " - " + award.Title);
            }

            // Register Abuse Report Reasons

            int totalAbuseReportReasons = gameData.messages.meta.abuse_report.reasons.Count;
            for(int i = 0; i < totalAbuseReportReasons; i++)
            {
                AbuseReport.ReportReason reason = new AbuseReport.ReportReason();
                reason.Id = gameData.messages.meta.abuse_report.reasons[i].id;
                reason.Name = gameData.messages.meta.abuse_report.reasons[i].name;
                reason.Meta = gameData.messages.meta.abuse_report.reasons[i].meta;
                AbuseReport.AddReason(reason);
                Logger.DebugPrint("Registered Abuse Report Reason: " + reason.Name);
            }

            // Map Data

            Map.OverlayTileDepth = gameData.tile_paramaters.overlay_tiles.tile_depth.ToObject<int[]>();

            List<Map.TerrainTile> terrainTiles = new List<Map.TerrainTile>();
            int totalTerrainTiles = gameData.tile_paramaters.terrain_tiles.Count;
            for (int i = 0; i < totalTerrainTiles; i++)
            {
                Map.TerrainTile tile = new Map.TerrainTile();
                tile.Passable = gameData.tile_paramaters.terrain_tiles[i].passable;
                tile.Type = gameData.tile_paramaters.terrain_tiles[i].tile_type;
                Logger.DebugPrint("Registered Tile: " + i + " Passable: " + tile.Passable + " Type: " + tile.Type);
                terrainTiles.Add(tile);
            }
            Map.TerrainTiles = terrainTiles.ToArray();

            // Register Abuse Report Reasons

            int totalInns = gameData.inns.Count;
            for (int i = 0; i < totalInns; i++)
            {
                int id = gameData.inns[i].id;
                int[] restsOffered = gameData.inns[i].rests_offered.ToObject<int[]>();
                int[] mealsOffered = gameData.inns[i].meals_offered.ToObject<int[]>();
                int buyPercent = gameData.inns[i].buy_percent;
                Inn inn = new Inn(id, restsOffered, mealsOffered, buyPercent);

                Logger.DebugPrint("Registered Inn: " + inn.Id + " Buying at: " + inn.BuyPercentage.ToString() + "%!");
            }

            int totalPoets = gameData.poetry.Count;
            for(int i = 0; i < totalPoets; i++)
            {
                Brickpoet.PoetryEntry entry = new Brickpoet.PoetryEntry();
                entry.Id = gameData.poetry[i].id;
                entry.Word = gameData.poetry[i].word;
                entry.Room = gameData.poetry[i].room_id;
                Brickpoet.PoetList.Add(entry);

                Logger.DebugPrint("Registered poet: " + entry.Id.ToString() + " word: " + entry.Word + " in room " + entry.Room.ToString());
            }

            // Register Horse Breeds
            int totalBreeds = gameData.horses.breeds.Count;
            for(int i = 0; i < totalBreeds; i++)
            {
                HorseInfo.Breed horseBreed = new HorseInfo.Breed();

                horseBreed.Id = gameData.horses.breeds[i].id;
                horseBreed.Name = gameData.horses.breeds[i].name;
                horseBreed.Description = gameData.horses.breeds[i].description;

                int speed = gameData.horses.breeds[i].base_stats.speed;
                int strength = gameData.horses.breeds[i].base_stats.strength;
                int conformation = gameData.horses.breeds[i].base_stats.conformation;
                int agility = gameData.horses.breeds[i].base_stats.agility;
                int inteligence = gameData.horses.breeds[i].base_stats.inteligence;
                int endurance = gameData.horses.breeds[i].base_stats.endurance;
                int personality = gameData.horses.breeds[i].base_stats.personality;
                int height = gameData.horses.breeds[i].base_stats.height;
                horseBreed.BaseStats = new HorseInfo.AdvancedStats(null, speed, strength, conformation, agility, inteligence, endurance, personality, height);
                horseBreed.BaseStats.MinHeight = gameData.horses.breeds[i].base_stats.min_height;
                horseBreed.BaseStats.MaxHeight = gameData.horses.breeds[i].base_stats.max_height;

                horseBreed.Colors = gameData.horses.breeds[i].colors.ToObject<string[]>();
                horseBreed.SpawnOn = gameData.horses.breeds[i].spawn_on;
                horseBreed.SpawnInArea = gameData.horses.breeds[i].spawn_area;
                horseBreed.Swf = gameData.horses.breeds[i].swf;
                horseBreed.Type = gameData.horses.breeds[i].type;

                HorseInfo.Breeds.Add(horseBreed);
                Logger.DebugPrint("Registered Horse Breed: #" + horseBreed.Id + ": " + horseBreed.Name);
            }
            int totalCategories = gameData.horses.categorys.Count;
            for(int i = 0; i < totalCategories; i++)
            {
                HorseInfo.Category category = new HorseInfo.Category();
                category.Name = gameData.horses.categorys[i].name;
                category.Meta = gameData.horses.categorys[i].message;
                HorseInfo.HorseCategories.Add(category);
                Logger.DebugPrint("Registered horse category type: " + category.Name);
            }
            int totalTrackedItems = gameData.messages.meta.misc_stats.tracked_items.Count;
            for(int i = 0; i < totalTrackedItems; i++)
            {
                Tracking.TrackedItemStatsMenu trackedItem = new Tracking.TrackedItemStatsMenu();
                trackedItem.What = gameData.messages.meta.misc_stats.tracked_items[i].id;
                trackedItem.Value = gameData.messages.meta.misc_stats.tracked_items[i].value;
                Tracking.TrackedItemsStatsMenu.Add(trackedItem);
                Logger.DebugPrint("Registered Tracked Item: " + trackedItem.What + " value: " + trackedItem.Value);
            }
            int totalVets = gameData.vet.price_multipliers.Count;
            for (int i = 0; i < totalVets; i++)
            {
                double cost = gameData.vet.price_multipliers[i].cost;
                int id = gameData.vet.price_multipliers[i].id;
                Vet vet = new Vet(id, cost);
                Logger.DebugPrint("Registered Vet: " + vet.Id + " selling at: " + vet.PriceMultiplier);
            }
            HorseInfo.HorseNames = gameData.horses.names.ToObject<string[]>();

            Item.Present = gameData.item.special.present;
            Item.MailMessage = gameData.item.special.mail_message;
            Item.DorothyShoes = gameData.item.special.dorothy_shoes;
            Item.PawneerOrder = gameData.item.special.pawneer_order;
            Item.Telescope = gameData.item.special.telescope;
            Item.Pitchfork = gameData.item.special.pitchfork;
            Item.WishingCoin = gameData.item.special.wishing_coin;
            Item.FishingPole = gameData.item.special.fishing_poll;
            Item.Earthworm = gameData.item.special.earthworm;

            // New Users
            Messages.NewUserMessage = gameData.messages.new_user.starting_message;
            Map.NewUserStartX = gameData.messages.new_user.starting_x;
            Map.NewUserStartY = gameData.messages.new_user.starting_y;

            // Records
            Messages.ProfileSavedMessage = gameData.messages.profile_save;
            Messages.PrivateNotesSavedMessage = gameData.messages.private_notes_save;
            Messages.PrivateNotesMetaFormat = gameData.messages.meta.private_notes_format;


            // Announcements

            Messages.WelcomeFormat = gameData.messages.welcome_format;
            Messages.MotdFormat = gameData.messages.motd_format;
            Messages.LoginMessageFormat = gameData.messages.login_format;
            Messages.LogoutMessageFormat = gameData.messages.logout_format;

            // Stats
            Messages.StatsBarFormat = gameData.messages.meta.stats_page.stats_bar_format;
            Messages.StatsAreaFormat = gameData.messages.meta.stats_page.stats_area_format;
            Messages.StatsMoneyFormat = gameData.messages.meta.stats_page.stats_money_format;
            Messages.StatsFreeTimeFormat = gameData.messages.meta.stats_page.stats_freetime_format;
            Messages.StatsDescriptionFormat = gameData.messages.meta.stats_page.stats_description_format;
            Messages.StatsExpFormat = gameData.messages.meta.stats_page.stats_experience;
            Messages.StatsQuestpointsFormat = gameData.messages.meta.stats_page.stats_questpoints;
            Messages.StatsHungerFormat = gameData.messages.meta.stats_page.stats_hunger;
            Messages.StatsThirstFormat = gameData.messages.meta.stats_page.stats_thisrt;
            Messages.StatsTiredFormat = gameData.messages.meta.stats_page.stats_tiredness;
            Messages.StatsGenderFormat = gameData.messages.meta.stats_page.stats_gender;
            Messages.StatsJewelFormat = gameData.messages.meta.stats_page.stats_equipped;
            Messages.StatsCompetitionGearFormat = gameData.messages.meta.stats_page.stats_competion_gear;

            Messages.JewelrySlot1Format = gameData.messages.meta.stats_page.jewelry.slot_1;
            Messages.JewelrySlot2Format = gameData.messages.meta.stats_page.jewelry.slot_2;
            Messages.JewelrySlot3Format = gameData.messages.meta.stats_page.jewelry.slot_3;
            Messages.JewelrySlot4Format = gameData.messages.meta.stats_page.jewelry.slot_4;

            Messages.CompetitionGearHeadFormat = gameData.messages.meta.stats_page.competition_gear.head_format;
            Messages.CompetitionGearBodyFormat = gameData.messages.meta.stats_page.competition_gear.body_format;
            Messages.CompetitionGearLegsFormat = gameData.messages.meta.stats_page.competition_gear.legs_format;
            Messages.CompetitionGearFeetFormat = gameData.messages.meta.stats_page.competition_gear.feet_format;

            Messages.StatsPrivateNotesButton = gameData.messages.meta.stats_page.stats_private_notes;
            Messages.StatsQuestsButton = gameData.messages.meta.stats_page.stats_quests;
            Messages.StatsMinigameRankingButton = gameData.messages.meta.stats_page.stats_minigame_ranking;
            Messages.StatsAwardsButton = gameData.messages.meta.stats_page.stats_awards;
            Messages.StatsMiscButton = gameData.messages.meta.stats_page.stats_misc;

            Messages.JewelrySelected = gameData.messages.meta.stats_page.msg.jewelry_selected;
            Messages.NoJewerlyEquipped = gameData.messages.meta.stats_page.msg.no_jewelry_equipped;
            Messages.NoCompetitionGear = gameData.messages.meta.stats_page.msg.no_competition_gear;
            Messages.CompetitionGearSelected = gameData.messages.meta.stats_page.msg.competition_gear_selected;

            Messages.StatHunger = gameData.messages.meta.stats_page.hunger_stat_name;
            Messages.StatThirst = gameData.messages.meta.stats_page.thirst_stat_name;
            Messages.StatTired = gameData.messages.meta.stats_page.tired_stat_name;

            Messages.StatPlayerFormats = gameData.messages.meta.stats_page.player_stats.ToObject<string[]>();

            // Misc Stats
            Messages.StatMiscHeader = gameData.messages.meta.misc_stats.header;
            Messages.StatMiscNoneRecorded = gameData.messages.meta.misc_stats.no_stats_recorded;
            Messages.StatMiscEntryFormat = gameData.messages.meta.misc_stats.stat_format;

            // Minigame (Libary)
            Messages.MinigameSingleplayer = gameData.messages.meta.libary.minigames.singleplayer;
            Messages.MinigameTwoplayer = gameData.messages.meta.libary.minigames.twoplayer;
            Messages.MinigameMultiplayer = gameData.messages.meta.libary.minigames.multiplayer;
            Messages.MinigameCompetitions = gameData.messages.meta.libary.minigames.competitions;
            Messages.MinigameEntryFormat = gameData.messages.meta.libary.minigames.minigame_entry;

            // Companion (Libary)
            Messages.CompanionViewFormat = gameData.messages.meta.libary.companion.view_button;
            Messages.CompanionEntryFormat = gameData.messages.meta.libary.companion.entry_format;

            // Tack (Libary)
            Messages.TackViewSetFormat = gameData.messages.meta.libary.tack.view_tack_set;
            Messages.TackSetPeiceFormat = gameData.messages.meta.libary.tack.set_peice_format;

            // Vet
            Messages.VetServiceHorseFormat = gameData.messages.meta.vet.service_horse;
            Messages.VetSerivcesNotNeeded = gameData.messages.meta.vet.not_needed;
            Messages.VetApplyServicesFormat = gameData.messages.meta.vet.apply;

            Messages.VetApplyServicesForAllFormat = gameData.messages.meta.vet.apply_all;
            Messages.VetFullHealthRecoveredMessageFormat = gameData.messages.meta.vet.now_full_health;
            Messages.VetServicesNotNeededAll = gameData.messages.meta.vet.not_needed_all;
            Messages.VetAllFullHealthRecoveredMessage = gameData.messages.meta.vet.all_full;
            Messages.VetCannotAffordMessage = gameData.messages.meta.vet.cant_afford;

            // Pond
            Messages.PondHeader = gameData.messages.meta.pond.header;
            Messages.PondGoFishing = gameData.messages.meta.pond.go_fishing;
            Messages.PondNoFishingPole = gameData.messages.meta.pond.no_fishing_pole;
            Messages.PondDrinkHereIfSafe = gameData.messages.meta.pond.drink_here;
            Messages.PondHorseDrinkFormat = gameData.messages.meta.pond.horse_drink_format;
            Messages.PondNoEarthWorms = gameData.messages.meta.pond.no_earth_worms;

            Messages.PondDrinkFullFormat = gameData.messages.meta.pond.drank_full;
            Messages.PondCantDrinkHpLowFormat = gameData.messages.meta.pond.cant_drink_hp_low;
            Messages.PondDrinkOhNoesFormat = gameData.messages.meta.pond.drank_something_bad;
            Messages.PondNotThirstyFormat = gameData.messages.meta.pond.not_thirsty;

            // Horse Whisperer
            Messages.WhispererHorseLocateButtonFormat = gameData.messages.meta.whisperer.horse_locate_meta;
            Messages.WhispererServiceCostYouFormat = gameData.messages.meta.whisperer.service_cost;
            Messages.WhispererServiceCannotAfford = gameData.messages.meta.whisperer.cant_afford;
            Messages.WhispererSearchingAmoungHorses = gameData.messages.meta.whisperer.searching_amoung_horses;
            Messages.WhispererNoneFound = gameData.messages.meta.whisperer.none_found_meta;
            Messages.WhispererHorsesFoundFormat = gameData.messages.meta.whisperer.horse_found_meta;

            // Mud Hole
            Messages.MudHoleNoHorses = gameData.messages.meta.mud_hole.no_horses;
            Messages.MudHoleRuinedGroomFormat = gameData.messages.meta.mud_hole.ruined_groom;

            // Movement
            Messages.RandomMovement = gameData.messages.random_movement;

            // Quests Log
            Messages.QuestLogHeader = gameData.messages.meta.quest_log.header_meta;
            Messages.QuestFormat = gameData.messages.meta.quest_log.quest_format;

            Messages.QuestNotCompleted = gameData.messages.meta.quest_log.not_complete;
            Messages.QuestNotAvalible = gameData.messages.meta.quest_log.not_avalible;
            Messages.QuestCompleted = gameData.messages.meta.quest_log.completed;

            Messages.QuestFooterFormat = gameData.messages.meta.quest_log.footer_format;
            // Transport

            Messages.CantAffordTransport = gameData.messages.transport.not_enough_money;
            Messages.WelcomeToAreaFormat = gameData.messages.transport.welcome_to_format;

            // Abuse Reports
            Messages.AbuseReportMetaFormat = gameData.messages.meta.abuse_report.options_format;
            Messages.AbuseReportReasonFormat = gameData.messages.meta.abuse_report.report_reason_format;

            Messages.AbuseReportPlayerNotFoundFormat = gameData.messages.abuse_report.player_not_found_format;
            Messages.AbuseReportFiled = gameData.messages.abuse_report.report_filed;
            Messages.AbuseReportProvideValidReason = gameData.messages.abuse_report.valid_reason;

            // Bank
            Messages.BankMadeInIntrestFormat = gameData.messages.meta.bank.made_interest;
            Messages.BankCarryingFormat = gameData.messages.meta.bank.carrying_message;
            Messages.BankWhatToDo = gameData.messages.meta.bank.what_to_do;
            Messages.BankOptionsFormat = gameData.messages.meta.bank.options;


            Messages.BankDepositedMoneyFormat = gameData.messages.bank.deposit_format;
            Messages.BankWithdrewMoneyFormat = gameData.messages.bank.withdraw_format;

            // Horses
            Messages.AdvancedStatFormat = gameData.messages.meta.horse.stat_format;
            Messages.BasicStatFormat = gameData.messages.meta.horse.basic_stat_format;
            Messages.HorsesHere = gameData.messages.meta.horse.horses_here;
            Messages.WildHorseFormat = gameData.messages.meta.horse.wild_horse;
            Messages.HorseCaptureTimer = gameData.messages.meta.horse.horse_timer;
            Messages.YouCapturedTheHorse = gameData.messages.meta.horse.hore_caught;
            Messages.HorseEvadedCapture = gameData.messages.meta.horse.horse_escaped;

            Messages.HorsesMenuHeader = gameData.messages.meta.horse.horses_menu;
            Messages.TooManyHorses = gameData.messages.meta.horse.too_many_horses;
            Messages.UpdateHorseCategory = gameData.messages.meta.horse.update_category;
            Messages.HorseEntryFormat = gameData.messages.meta.horse.horse_format;
            Messages.ViewBaiscStats = gameData.messages.meta.horse.view_basic_stats;
            Messages.ViewAdvancedStats = gameData.messages.meta.horse.view_advanced_stats;

            Messages.HorseRidingMessageFormat = gameData.messages.meta.horse.riding_message;
            Messages.HorseNameFormat = gameData.messages.meta.horse.horse_inventory.your_horse_format;
            Messages.HorseDescriptionFormat = gameData.messages.meta.horse.horse_inventory.description_format;
            Messages.HorseHandsHeightFormat = gameData.messages.meta.horse.horse_inventory.hands_high;
            Messages.HorseExperienceEarnedFormat = gameData.messages.meta.horse.horse_inventory.experience;
            
            Messages.HorseTrainableInFormat = gameData.messages.meta.horse.horse_inventory.trainable_in;
            Messages.HorseIsTrainable = gameData.messages.meta.horse.horse_inventory.currently_trainable;

            Messages.HorseMountButtonFormat = gameData.messages.meta.horse.horse_inventory.mount_button;
            Messages.HorseDisMountButtonFormat = gameData.messages.meta.horse.horse_inventory.dismount_button;
            Messages.HorseFeedButtonFormat = gameData.messages.meta.horse.horse_inventory.feed_button;
            Messages.HorseTackButtonFormat = gameData.messages.meta.horse.horse_inventory.tack_button;
            Messages.HorsePetButtonFormat = gameData.messages.meta.horse.horse_inventory.pet_button;
            Messages.HorseProfileButtonFormat = gameData.messages.meta.horse.horse_inventory.profile_button;
            Messages.HorseSavedProfileMessageFormat = gameData.messages.meta.horse.saved_profile;

            Messages.HorseNoAutoSell = gameData.messages.meta.horse.horse_inventory.no_auto_sell;
            Messages.HorseAutoSellPriceFormat = gameData.messages.meta.horse.horse_inventory.auto_sell_format;
            Messages.HorseAutoSellFormat = gameData.messages.meta.horse.horse_inventory.auto_sell;
            Messages.HorseCantAutoSellTacked = gameData.messages.meta.horse.horse_inventory.cannot_auto_sell_tacked;

            Messages.HorseCurrentlyCategoryFormat = gameData.messages.meta.horse.horse_inventory.marked_as;
            Messages.HorseStats = gameData.messages.meta.horse.horse_inventory.horse_stats;
            Messages.HorseTacked = gameData.messages.meta.horse.horse_inventory.wearing_tacked;
            Messages.HorseTackFormat = gameData.messages.meta.horse.horse_inventory.tacked_format;

            Messages.HorseCompanion = gameData.messages.meta.horse.horse_inventory.companion;
            Messages.HorseCompanionFormat = gameData.messages.meta.horse.horse_inventory.companion_selected;
            Messages.HorseNoCompanion = gameData.messages.meta.horse.horse_inventory.no_companion;

            Messages.HorseAdvancedStatsFormat = gameData.messages.meta.horse.horse_inventory.advanced_stats;
            Messages.HorseBreedDetailsFormat = gameData.messages.meta.horse.horse_inventory.breed_details;
            Messages.HorseHeightRangeFormat = gameData.messages.meta.horse.horse_inventory.height_range;
            Messages.HorsePossibleColorsFormat = gameData.messages.meta.horse.horse_inventory.possible_colors;
            Messages.HorseReleaseButton = gameData.messages.meta.horse.horse_inventory.release_horse;
            Messages.HorseOthers = gameData.messages.meta.horse.horse_inventory.other_horses;

            Messages.HorseDescriptionEditFormat = gameData.messages.meta.horse.description_edit;
            Messages.HorseEquipTackMessageFormat = gameData.messages.meta.horse.equip_tack_message;
            Messages.HorseUnEquipTackMessageFormat = gameData.messages.meta.horse.unequip_tack_message;
            Messages.HorseStopRidingMessage = gameData.messages.meta.horse.stop_riding_message;

            Messages.HorsePetMessageFormat = gameData.messages.meta.horse.pet_horse;
            Messages.HorsePetTooHappyFormat = gameData.messages.meta.horse.pet_horse_too_happy;
            Messages.HorseSetNewCategoryMessageFormat = gameData.messages.meta.horse.horse_set_new_category;

            Messages.HorseAutoSellMenuFormat = gameData.messages.meta.horse.auto_sell.auto_sell_meta;
            Messages.HorseIsAutoSell = gameData.messages.meta.horse.auto_sell.is_auto_sell;
            Messages.HorseAutoSellConfirmedFormat = gameData.messages.meta.horse.auto_sell.auto_sell_confirmed;
            Messages.HorseAutoSellRemoved = gameData.messages.meta.horse.auto_sell.auto_sell_remove;

            Messages.HorseSetAutoSell = gameData.messages.meta.horse.horse_inventory.set_auto_sell;
            Messages.HorseChangeAutoSell = gameData.messages.meta.horse.horse_inventory.change_auto_sell;
            Messages.HorseTackFailAutoSell = gameData.messages.meta.horse.tack_fail_autosell;

            Messages.HorseAreYouSureYouWantToReleaseFormat = gameData.messages.meta.horse.horse_release;
            Messages.HorseCantReleaseTheHorseYourRidingOn = gameData.messages.meta.horse.cant_release_currently_riding;
            Messages.HorseReleasedMeta = gameData.messages.meta.horse.released_horse;
            Messages.HorseReleasedBy = gameData.messages.meta.horse.released_by_message;

            // All Stats (basic)

            Messages.HorseAllBasicStats = gameData.messages.meta.horse.allstats_basic.all_baisc_stats;
            Messages.HorseBasicStatEntryFormat = gameData.messages.meta.horse.allstats_basic.horse_entry;

            // All Stats (all)
            Messages.HorseAllStatsHeader = gameData.messages.meta.horse.allstats.all_stats_header;
            Messages.HorseNameEntryFormat = gameData.messages.meta.horse.allstats.horse_name_entry;
            Messages.HorseBasicStatsCompactedFormat = gameData.messages.meta.horse.allstats.basic_stats_compact;
            Messages.HorseAdvancedStatsCompactedFormat = gameData.messages.meta.horse.allstats.advanced_stats_compact;
            Messages.HorseAllStatsLegend = gameData.messages.meta.horse.allstats.legend;


            // Horse companion menu
            Messages.HorseCompanionMenuHeaderFormat = gameData.messages.meta.horse.companion_menu.menu_header;
            Messages.HorseCompnaionMenuCurrentCompanionFormat = gameData.messages.meta.horse.companion_menu.selected_companion;
            Messages.HorseCompanionEntryFormat = gameData.messages.meta.horse.companion_menu.companion_entry;
            Messages.HorseCompanionEquipMessageFormat = gameData.messages.meta.horse.companion_menu.companion_equip_message;
            Messages.HorseCompanionRemoveMessageFormat = gameData.messages.meta.horse.companion_menu.companion_remove_message;
            Messages.HorseCompanionMenuCurrentlyAvalibleCompanions = gameData.messages.meta.horse.companion_menu.companions_avalible;

            // Horse Feed Menu
            Messages.HorseCurrentStatusFormat = gameData.messages.meta.horse.feed_horse.current_status;
            Messages.HorseHoldingHorseFeed = gameData.messages.meta.horse.feed_horse.holding_horse_feed;
            Messages.HorsefeedFormat = gameData.messages.meta.horse.feed_horse.horsefeed_format;
            Messages.HorseNeighsThanks = gameData.messages.meta.horse.feed_horse.horse_neigh;
            Messages.HorseCouldNotFinish = gameData.messages.meta.horse.feed_horse.horse_could_not_finish;

            // Tack menu (horses)
            Messages.HorseTackedAsFollowsFormat = gameData.messages.meta.horse.tack_menu.tacked_as_follows;
            Messages.HorseUnEquipSaddleFormat = gameData.messages.meta.horse.tack_menu.dequip_saddle;
            Messages.HorseUnEquipSaddlePadFormat = gameData.messages.meta.horse.tack_menu.dequip_saddle_pad;
            Messages.HorseUnEquipBridleFormat = gameData.messages.meta.horse.tack_menu.dequip_bridle;
            Messages.HorseTackInInventory = gameData.messages.meta.horse.tack_menu.you_have_following_tack;
            Messages.HorseEquipFormat = gameData.messages.meta.horse.tack_menu.equip_tack;
            Messages.BackToHorse = gameData.messages.meta.horse.back_to_horse;


            // Libary
            Messages.LibaryMainMenu = gameData.messages.meta.libary.main_menu;
            Messages.LibaryFindNpc = gameData.messages.meta.libary.find_npc;
            Messages.LibaryFindNpcSearchResultsHeader = gameData.messages.meta.libary.find_npc_results_header;
            Messages.LibaryFindNpcSearchResultFormat = gameData.messages.meta.libary.find_npc_results_format;
            Messages.LibaryFindNpcSearchNoResults = gameData.messages.meta.libary.find_npc_no_results;
            Messages.LibaryFindNpcLimit5 = gameData.messages.meta.libary.find_npc_limit5;

            Messages.HorseBreedFormat = gameData.messages.meta.libary.horse_breed_format;
            Messages.HorseRelativeFormat = gameData.messages.meta.libary.horse_relative_format;
            Messages.BreedViewerFormat = gameData.messages.meta.libary.breed_preview_format;
            Messages.BreedViewerMaximumStats = gameData.messages.meta.libary.maximum_stats;

            // Chat

            Messages.ChatViolationMessageFormat = gameData.messages.chat.violation_format;
            Messages.RequiredChatViolations = gameData.messages.chat.violation_points_required;

            Messages.GlobalChatFormatForModerators = gameData.messages.chat.for_others.global_format_moderator;
            Messages.DirectChatFormatForModerators = gameData.messages.chat.for_others.dm_format_moderator;


            Messages.HereChatFormat = gameData.messages.chat.for_others.here_format;
            Messages.IsleChatFormat = gameData.messages.chat.for_others.isle_format;
            Messages.NearChatFormat = gameData.messages.chat.for_others.near_format;
            Messages.GlobalChatFormat = gameData.messages.chat.for_others.global_format;
            Messages.AdsChatFormat = gameData.messages.chat.for_others.ads_format;
            Messages.DirectChatFormat = gameData.messages.chat.for_others.dm_format;
            Messages.BuddyChatFormat = gameData.messages.chat.for_others.friend_format;
            Messages.ModChatFormat = gameData.messages.chat.for_others.mod_format;
            Messages.AdminChatFormat = gameData.messages.chat.for_others.admin_format;

            Messages.HereChatFormatForSender = gameData.messages.chat.for_sender.here_format;
            Messages.IsleChatFormatForSender = gameData.messages.chat.for_sender.isle_format;
            Messages.NearChatFormatForSender = gameData.messages.chat.for_sender.near_format;
            Messages.BuddyChatFormatForSender = gameData.messages.chat.for_sender.friend_format;
            Messages.DirectChatFormatForSender = gameData.messages.chat.for_sender.dm_format;
            Messages.ModChatFormatForSender = gameData.messages.chat.for_sender.mod_format;
            Messages.AdsChatFormatForSender = gameData.messages.chat.for_sender.ads_format;
            Messages.AdminChatFormatForSender = gameData.messages.chat.for_sender.admin_format;

            Messages.AdminCommandFormat = gameData.messages.commands.admin_command_completed;
            Messages.PlayerCommandFormat = gameData.messages.commands.player_command_completed;
            Messages.MuteHelp = gameData.messages.commands.mute_help;


            Messages.PasswordNotice = gameData.messages.chat.password_included;
            Messages.CapsNotice = gameData.messages.chat.caps_notice;

            // Brickpoet
            Messages.LastPoetFormat = gameData.messages.meta.last_poet;

            // Mutliroom
            Messages.MultiroomParticipentFormat = gameData.messages.meta.multiroom.partcipent_format;
            Messages.MultiroomPlayersParticipating = gameData.messages.meta.multiroom.other_players_participating;

            // Dropped Items

            Messages.NothingMessage = gameData.messages.meta.dropped_items.nothing_message;
            Messages.ItemsOnGroundMessage = gameData.messages.meta.dropped_items.items_message;
            Messages.GrabItemFormat = gameData.messages.meta.dropped_items.item_format;
            Messages.ItemInformationFormat = gameData.messages.meta.dropped_items.item_information_format;
            Messages.GrabAllItemsButton = gameData.messages.meta.dropped_items.grab_all;
            Messages.DroppedAnItemMessage = gameData.messages.dropped_items.dropped_item_message;
            Messages.GrabbedAllItemsMessage = gameData.messages.dropped_items.grab_all_message;
            Messages.GrabbedItemMessage = gameData.messages.dropped_items.grab_message;
            Messages.GrabAllItemsMessage = gameData.messages.dropped_items.grab_all_message;

            Messages.GrabbedAllItemsButInventoryFull = gameData.messages.dropped_items.grab_all_but_inv_full;
            Messages.GrabbedItemButInventoryFull = gameData.messages.dropped_items.grab_but_inv_full;

            // Tools
            Messages.BinocularsNothing = gameData.messages.tools.binoculars;
            Messages.MagnifyNothing = gameData.messages.tools.magnify;
            Messages.RakeNothing = gameData.messages.tools.rake;
            Messages.ShovelNothing = gameData.messages.tools.shovel;

            // Shop
            Messages.ThingsIAmSelling = gameData.messages.meta.shop.selling;
            Messages.ThingsYouSellMe = gameData.messages.meta.shop.sell_me;
            Messages.InfinitySign = gameData.messages.meta.shop.infinity;

            Messages.CantAfford1 = gameData.messages.shop.cant_afford_1;
            Messages.CantAfford5 = gameData.messages.shop.cant_afford_5;
            Messages.CantAfford25 = gameData.messages.shop.cant_afford_25;
            Messages.Brought1Format = gameData.messages.shop.brought_1;
            Messages.Brought5Format = gameData.messages.shop.brought_5;
            Messages.Brought25Format = gameData.messages.shop.brought_25;
            Messages.Sold1Format = gameData.messages.shop.sold_1;
            Messages.SoldAllFormat = gameData.messages.shop.sold_all;

            Messages.Brought1ButInventoryFull = gameData.messages.shop.brought_1_but_inv_full;
            Messages.Brought5ButInventoryFull = gameData.messages.shop.brought_5_but_inv_full;
            Messages.Brought25ButInventoryFull = gameData.messages.shop.brought_25_but_inv_full;

            // Player List

            Messages.PlayerListHeader = gameData.messages.meta.player_list.playerlist_header;
            Messages.PlayerListSelectFromFollowing = gameData.messages.meta.player_list.select_from_following;
            Messages.PlayerListOfBuddiesFormat = gameData.messages.meta.player_list.list_of_buddies_format;
            Messages.PlayerListOfNearby = gameData.messages.meta.player_list.list_of_players_nearby;
            Messages.PlayerListOfPlayersFormat = gameData.messages.meta.player_list.list_of_all_players_format;
            Messages.PlayerListOfPlayersAlphabetically = gameData.messages.meta.player_list.list_of_all_players_alphabetically;
            Messages.PlayerListMapAllBuddiesForamt = gameData.messages.meta.player_list.map_all_buddies_format;
            Messages.PlayerListMapAllPlayersFormat = gameData.messages.meta.player_list.map_all_players_format;
            Messages.PlayerListAbuseReport = gameData.messages.meta.player_list.abuse_report;

            Messages.ThreeMonthSubscripitionIcon = gameData.messages.meta.player_list.icon_subbed_3month;
            Messages.YearSubscriptionIcon = gameData.messages.meta.player_list.icon_subbed_year;
            Messages.NewUserIcon = gameData.messages.meta.player_list.icon_new;
            Messages.MonthSubscriptionIcon = gameData.messages.meta.player_list.icon_subbed_month;
            Messages.AdminIcon = gameData.messages.meta.player_list.icon_admin;
            Messages.ModeratorIcon = gameData.messages.meta.player_list.icon_mod;

            Messages.BuddyListHeader = gameData.messages.meta.player_list.online_buddy_header;
            Messages.BuddyListOnlineBuddyEntryFormat = gameData.messages.meta.player_list.online_buddy_format;
            Messages.BuddyListOfflineBuddys = gameData.messages.meta.player_list.offline_buddys;
            Messages.BuddyListOfflineBuddyEntryFormat = gameData.messages.meta.player_list.offline_buddy_format;

            Messages.NearbyPlayersListHeader = gameData.messages.meta.player_list.nearby_player_header;
            Messages.PlayerListAllAlphabeticalHeader = gameData.messages.meta.player_list.all_players_alphabetical_header;

            Messages.PlayerListEntryFormat = gameData.messages.meta.player_list.player_format;

            Messages.PlayerListIdle = gameData.messages.meta.player_list.idle_text;
            Messages.PlayerListAllHeader = gameData.messages.meta.player_list.all_players_header;
            Messages.PlayerListIconFormat = gameData.messages.meta.player_list.icon_format;
            Messages.PlayerListIconInformation = gameData.messages.meta.player_list.icon_info;
            // Consume

            Messages.ConsumeItemFormat = gameData.messages.consume.consumed_item_format;
            Messages.ConsumedButMaxReached = gameData.messages.consume.consumed_but_max_reached;

            // Meta Format

            Messages.LocationFormat = gameData.messages.meta.location_format;
            Messages.IsleFormat = gameData.messages.meta.isle_format;
            Messages.TownFormat = gameData.messages.meta.town_format;
            Messages.AreaFormat = gameData.messages.meta.area_format;
            Messages.Seperator = gameData.messages.meta.seperator;
            Messages.TileFormat = gameData.messages.meta.tile_format;
            Messages.TransportFormat = gameData.messages.meta.transport_format;
            Messages.ExitThisPlace = gameData.messages.meta.exit_this_place;
            Messages.BackToMap = gameData.messages.meta.back_to_map;
            Messages.BackToMapHorse = gameData.messages.meta.back_to_map_horse;
            Messages.LongFullLine = gameData.messages.meta.long_full_line;
            Messages.MetaTerminator = gameData.messages.meta.end_of_meta;

            Messages.NearbyPlayers = gameData.messages.meta.nearby.players_nearby;
            Messages.North = gameData.messages.meta.nearby.north;
            Messages.East = gameData.messages.meta.nearby.east;
            Messages.South = gameData.messages.meta.nearby.south;
            Messages.West = gameData.messages.meta.nearby.west;

            Messages.NoPitchforkMeta = gameData.messages.meta.hay_pile.no_pitchfork;
            Messages.HasPitchforkMeta = gameData.messages.meta.hay_pile.pitchfork;

            Messages.PasswordEntry = gameData.messages.meta.password_input;
            Messages.VenusFlyTrapFormat = gameData.messages.meta.venus_flytrap_format;

            // Inn
            Messages.InnBuyMeal = gameData.messages.meta.inn.buy_meal;
            Messages.InnBuyRest = gameData.messages.meta.inn.buy_rest;
            Messages.InnItemEntryFormat = gameData.messages.meta.inn.inn_entry;
            Messages.InnEnjoyedServiceFormat = gameData.messages.inn.enjoyed_service;
            Messages.InnCannotAffordService = gameData.messages.inn.cant_afford;
            Messages.InnFullyRested = gameData.messages.inn.fully_rested;

            // Fountain
            Messages.FountainMeta = gameData.messages.meta.fountain;
            Messages.FountainDrankYourFull = gameData.messages.fountain.drank_your_fill;
            Messages.FountainDroppedMoneyFormat = gameData.messages.fountain.dropped_money;

            // Highscore

            Messages.HighscoreHeaderMeta = gameData.messages.meta.highscores.header_meta;
            Messages.HighscoreFormat = gameData.messages.meta.highscores.highscore_format;
            Messages.BestTimeFormat = gameData.messages.meta.highscores.besttime_format;

            Messages.GameHighScoreHeaderFormat = gameData.messages.meta.highscores.game_highscore_header;
            Messages.GameHighScoreFormat = gameData.messages.meta.highscores.game_highscore_format;

            Messages.GameBestTimeHeaderFormat = gameData.messages.meta.highscores.game_besttime_header;
            Messages.GameBestTimeFormat = gameData.messages.meta.highscores.game_besttime_format;

            // Awards

            Messages.AwardHeader = gameData.messages.meta.awards_page.awards_header;
            Messages.NoAwards = gameData.messages.meta.awards_page.no_awards;
            Messages.AwardFormat = gameData.messages.meta.awards_page.award_format;

            // World Peace
            Messages.NoWishingCoins = gameData.messages.meta.wishing_well.no_coins;
            Messages.YouHaveWishingCoinsFormat = gameData.messages.meta.wishing_well.wish_coins;
            Messages.WishItemsFormat = gameData.messages.meta.wishing_well.wish_things;
            Messages.WishMoneyFormat = gameData.messages.meta.wishing_well.wish_money;
            Messages.WishWorldPeaceFormat = gameData.messages.meta.wishing_well.wish_worldpeace;

            Messages.TossedCoin = gameData.messages.meta.wishing_well.make_wish;
            Messages.WorldPeaceOnlySoDeep = gameData.messages.meta.wishing_well.world_peace_message;
            Messages.WishingWellMeta = gameData.messages.meta.wishing_well.wish_meta;
            // Sec Codes

            Messages.InvalidSecCodeError = gameData.messages.sec_code.invalid_sec_code;
            Messages.YouEarnedAnItemFormat = gameData.messages.sec_code.item_earned;
            Messages.YouLostAnItemFormat = gameData.messages.sec_code.item_deleted;
            Messages.YouEarnedMoneyFormat = gameData.messages.sec_code.money_earned;
            Messages.BeatHighscoreFormat = gameData.messages.sec_code.highscore_beaten;
            Messages.BeatBestTimeFormat = gameData.messages.sec_code.best_time_beaten;

            // Inventory

            Messages.InventoryHeaderFormat = gameData.messages.meta.inventory.header_format;
            Messages.InventoryItemFormat = gameData.messages.meta.inventory.item_entry;
            Messages.ShopEntryFormat = gameData.messages.meta.inventory.shop_entry;
            Messages.ItemInformationButton = gameData.messages.meta.inventory.item_info_button;
            Messages.ItemInformationByIdButton = gameData.messages.meta.inventory.item_info_itemid_button;

            Messages.ItemDropButton = gameData.messages.meta.inventory.item_drop_button;
            Messages.ItemThrowButton = gameData.messages.meta.inventory.item_throw_button;
            Messages.ItemConsumeButton = gameData.messages.meta.inventory.item_consume_button;
            Messages.ItemUseButton = gameData.messages.meta.inventory.item_use_button;
            Messages.ItemWearButton = gameData.messages.meta.inventory.item_wear_button;
            Messages.ItemReadButton = gameData.messages.meta.inventory.item_read_button;

            Messages.ShopBuyButton = gameData.messages.meta.inventory.buy_button;
            Messages.ShopBuy5Button = gameData.messages.meta.inventory.buy_5_button;
            Messages.ShopBuy25Button = gameData.messages.meta.inventory.buy_25_button;

            Messages.SellButton = gameData.messages.meta.inventory.sell_button;
            Messages.SellAllButton = gameData.messages.meta.inventory.sell_all_button;
            // Npc

            Messages.NpcStartChatFormat = gameData.messages.meta.npc.start_chat_format;
            Messages.NpcChatpointFormat = gameData.messages.meta.npc.chatpoint_format;
            Messages.NpcReplyFormat = gameData.messages.meta.npc.reply_format;
            Messages.NpcTalkButton = gameData.messages.meta.npc.npc_talk_button;
            Messages.NpcInformationButton = gameData.messages.meta.npc.npc_information_button;
            Messages.NpcInformationFormat = gameData.messages.meta.npc.npc_information_format;

            // Disconnect Reasons

            Messages.KickReasonKicked = gameData.messages.disconnect.kicked;
            Messages.KickReasonBanned = gameData.messages.disconnect.banned;
            Messages.KickReasonIdleFormat = gameData.messages.disconnect.client_timeout.kick_message;
            Messages.KickReasonNoTime = gameData.messages.disconnect.no_playtime;
            Messages.IdleWarningFormat = gameData.messages.disconnect.client_timeout.warn_message;
            Messages.KickReasonDuplicateLogin = gameData.messages.disconnect.dupe_login;

            Chat.PrivateMessageSound = gameData.messages.chat.pm_sound;

            GameServer.IdleWarning = gameData.messages.disconnect.client_timeout.warn_after;
            GameServer.IdleTimeout = gameData.messages.disconnect.client_timeout.kick_after;

            // Inventory

            Messages.DefaultInventoryMax = gameData.item.max_carryable;

            // Competition Gear

            Messages.EquipCompetitionGearFormat = gameData.messages.equips.equip_competition_gear_format;
            Messages.RemoveCompetitionGear = gameData.messages.equips.removed_competition_gear;

            // Jewerly
            Messages.EquipJewelryFormat = gameData.messages.equips.equip_jewelry;
            Messages.MaxJewelryMessage = gameData.messages.equips.max_jewelry;
            Messages.RemoveJewelry = gameData.messages.equips.removed_jewelry;

            // Click
            Messages.NothingInterestingHere = gameData.messages.click_nothing_message;

            // Swf
            Messages.WagonCutscene = gameData.transport.wagon_cutscene;
            Messages.BoatCutscene = gameData.transport.boat_cutscene;
            Messages.BallonCutscene = gameData.transport.ballon_cutscene;

            gameData = null;
        }

    }
}