﻿using System;
using System.Collections.Generic;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using SDG.Unturned;
using FreneticScript.TagHandlers.Objects;
using FreneticScript.TagHandlers;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    public class BleedingCommand : AbstractCommand
    {
        // <--[command]
        // @Name bleeding
        // @Arguments <player> <boolean>
        // @Short Starts or stops a player's bleeding.
        // @Updated 2016/04/27
        // @Authors Morphan1
        // @Group Player
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Specify a boolean value that when true makes a player
        // bleed. When false, it stops their bleeding.
        // TODO: Explain more!
        // @Example
        // // This makes the player bleed.
        // bleeding <{var[player]}> true;
        // -->
        public BleedingCommand()
        {
            Name = "bleeding";
            Arguments = "<player> <boolean>";
            Description = "Starts or stops a player's bleeding.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                BooleanTag.TryFor
            };
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            try
            {
                BooleanTag boolean = BooleanTag.TryFor(entry.GetArgumentObject(queue, 1));
                if (boolean == null)
                {
                    queue.HandleError(entry, "Invalid boolean!");
                    return;
                }
                PlayerTag player = PlayerTag.For(entry.GetArgument(queue, 0));
                if (player == null)
                {
                    queue.HandleError(entry, "Invalid player!");
                    return;
                }
                bool value = boolean.Internal;
                PlayerLife life = player.Internal.player.life;
                if (life._isBleeding != value)
                {
                    life._isBleeding = value;
                    if (value)
                    {
                        uint sim = life.player.input.simulation;
                        life.lastBleeding = sim;
                        life.lastBleed = sim;
                    }
                    life.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[]
                    {
                        life.isBleeding
                    });
                    entry.Good(queue, "Successfully adjusted the bleeding of player " + TagParser.Escape(player.ToString()) + " to " + TagParser.Escape(boolean.ToString()) + "!");
                }
                else
                {
                    entry.Good(queue, "Player " + TagParser.Escape(player.ToString()) + " already has their bleeding set to " + TagParser.Escape(boolean.ToString()) + "!");
                }
            }
            catch (Exception ex) // TODO: Necessity?
            {
                queue.HandleError(entry, "Failed to adjust player's bleeding state: " + ex.ToString());
            }
        }
    }
}
