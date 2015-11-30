﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.CommandSystem;
using SDG.Unturned;
using UnturnedFrenetic.TagSystems.TagObjects;
using Frenetic.TagHandlers;
using System.Reflection;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    class SpawnCommand: AbstractCommand
    {
        public SpawnCommand()
        {
            Name = "spawn";
            Arguments = "<entity type> <location>";
            Description = "Spawns an entity at the location.";
        }

        public override void Execute(CommandEntry entry)
        {
            if (entry.Arguments.Count < 2)
            {
                ShowUsage(entry);
                return;
            }
            LocationTag loc = LocationTag.For(entry.GetArgument(1));
            if (loc == null)
            {
                entry.Bad("Invalid location!");
                return;
            }
            string targetAssetType = entry.GetArgument(0).ToLower();
            EntityType etype = EntityType.ValueOf(targetAssetType);
            if (etype == null)
            {
                entry.Bad("Invalid entity type!");
                return;
            }
            if (etype.Type == EntityAssetType.ZOMBIE)
            {
                UnityEngine.Vector3 vec3 = loc.ToVector3();
                byte reg = 0; // TODO: Optionally specifiable
                float closest = float.MaxValue;
                for (int r = 0; r < LevelZombies.zombies.Length; r++)
                {
                    for (int i = 0; i < LevelZombies.zombies[r].Count; i++)
                    {
                        float dist = (LevelZombies.zombies[r][i].point - vec3).sqrMagnitude;
                        if (dist < closest)
                        {
                            closest = dist;
                            reg = (byte)r;
                        }
                    }
                }
                ZombieManager.manager.addZombie(reg, 0, 0, 0, 0, 0, 0, 0, 0, vec3, 0, false);
                Zombie zombie = ZombieManager.regions[reg].zombies[ZombieManager.regions[reg].zombies.Count - 1];
                // TODO: Make this actually work!
                /*
                foreach (SteamPlayer player in PlayerTool.getSteamPlayers())
                {
                    ZombieManager.manager.channel.openWrite();
                    ZombieManager.manager.channel.write(reg);
                    ZombieManager.manager.channel.write((ushort)1);
                    ZombieManager.manager.channel.write(new object[]
                        {
                            zombie.type,
                            (byte)zombie.speciality,
                            zombie.shirt,
                            zombie.pants,
                            zombie.hat,
                            zombie.gear,
                            zombie.move,
                            zombie.idle,
                            zombie.transform.position,
                            MeasurementTool.angleToByte(zombie.transform.rotation.eulerAngles.y),
                            zombie.isDead
                        });
                    ZombieManager.manager.channel.closeWrite("tellZombies", player.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
                }
                */
                entry.Good("Successfully spawned a zombie at " + TagParser.Escape(loc.ToString()) + "! (WARNING: IT WILL BE INVISIBLE CURRENTLY - SEE THE COMPLAINTS FILE)");
                return;
            }
            else if (etype.Type == EntityAssetType.ANIMAL)
            {
                AnimalAssetTag asset = AnimalAssetTag.For(targetAssetType);
                if (asset == null)
                {
                    entry.Bad("Invalid animal type!");
                    return;
                }
                AnimalManager.manager.addAnimal(asset.Internal.id, loc.ToVector3(), 0, false);
                Animal animal = AnimalManager.animals[AnimalManager.animals.Count - 1];
                foreach (SteamPlayer player in PlayerTool.getSteamPlayers())
                {
                    AnimalManager.manager.channel.openWrite();
                    AnimalManager.manager.channel.write((ushort)AnimalManager.animals.Count);
                    AnimalManager.manager.channel.write(new object[]
                    {
                    animal.id,
                    animal.transform.position,
                    MeasurementTool.angleToByte(animal.transform.rotation.eulerAngles.y),
                    animal.isDead
                    });
                    AnimalManager.manager.channel.closeWrite("tellAnimals", player.playerID.steamID, ESteamPacket.UPDATE_RELIABLE_CHUNK_BUFFER);
                }
                // TODO: Maybe add the animal as a var to the current commandqueue.
                entry.Good("Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
                return;
            }
            else if (etype.Type == EntityAssetType.ITEM)
            {
                ItemAssetTag asset = ItemAssetTag.For(targetAssetType);
                if (asset == null)
                {
                    entry.Bad("Invalid item type!");
                    return;
                }
                byte x = (byte)((loc.X + 4096f) / Regions.REGION_SIZE);
                byte y = (byte)((loc.Y + 4096f) / Regions.REGION_SIZE);
                ItemManager.manager.spawnItem(x, y, asset.Internal.id, 1, asset.Internal.quality, asset.Internal.getState(), loc.ToVector3());
                // TODO: make this work :(
                ItemManager.manager.channel.send("tellItem", ESteamCall.CLIENTS, x, y, ItemManager.ITEM_REGIONS, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                {
                        x,
                        y,
                        asset.Internal.id,
                        1,
                        asset.Internal.quality,
                        asset.Internal.getState(),
                        loc.ToVector3()
                });
                entry.Good("Successfully spawned a " + TagParser.Escape(asset.ToString()) + " at " + TagParser.Escape(loc.ToString()) + "!");
            }
            else
            {
                entry.Bad("Invalid or unspawnable entity type!");
            }
        }
    }
}
