﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreneticScript.CommandSystem;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript.TagHandlers;
using SDG.Unturned;

namespace UnturnedFrenetic.CommandSystems.EntityCommands
{
    class WalkCommand : AbstractCommand
    {
        // <--[command]
        // @Name walk
        // @Arguments <entity> <location>
        // @Short Makes an entity walk to the given location.
        // @Updated 2016/04/28
        // @Authors mcmonkey
        // @Group Entity
        // @Minimum 2
        // @Maximum 2
        // @Description
        // Makes an entity walk to the given location.
        // Note that this does not override AI - meaning the AI will sometimes cause the entity to change path.
        // TODO: Explain more!
        // @Example
        // // This makes the entity with ID 1 walk to the location (50, 50, 50).
        // walk 1 50,50,50;
        // -->
        public WalkCommand()
        {
            Name = "walk";
            Arguments = "<entity> <location>";
            Description = "Teleports the entity to the location.";
            MinimumArguments = 2;
            MaximumArguments = 2;
            ObjectTypes = new List<Func<TemplateObject, TemplateObject>>()
            {
                TemplateObject.Basic_For,
                LocationTag.For
            };
        }

        public override void Execute(FreneticScript.CommandSystem.CommandQueue queue, CommandEntry entry)
        {
            LocationTag loc = LocationTag.For(entry.GetArgument(queue, 1));
            if (loc == null)
            {
                queue.HandleError(entry, "Invalid location!");
                return;
            }
            EntityTag entity = EntityTag.For(entry.GetArgumentObject(queue, 0));
            if (entity == null)
            {
                queue.HandleError(entry, "Invalid entity!");
                return;
            }
            ZombieTag zombie;
            if (entity.TryGetZombie(out zombie))
            {
                zombie.Internal.target.position = loc.ToVector3();
                zombie.Internal.seeker.canMove = true;
                zombie.Internal.seeker.canSearch = true;
                zombie.Internal.path = EZombiePath.RUSH; // TODO: Option for this?
                if (!zombie.Internal.isTicking)
                {
                    zombie.Internal.isTicking = true;
                    ZombieManager.tickingZombies.Add(zombie.Internal);
                }
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully started a zombie walking to " + TagParser.Escape(loc.ToString()) + "!");
                }
                return;
            }
            AnimalTag animal;
            if (entity.TryGetAnimal(out animal))
            {
                animal.Internal.target = loc.ToVector3();
                if (!animal.Internal.isTicking)
                {
                    animal.Internal.isTicking = true;
                    AnimalManager.tickingAnimals.Add(animal.Internal);
                }
                if (entry.ShouldShowGood(queue))
                {
                    entry.Good(queue, "Successfully started an animal walking to " + TagParser.Escape(loc.ToString()) + "!");
                }
                return;
            }
            queue.HandleError(entry, "That entity can't be made to walk!");
        }
    }
}
