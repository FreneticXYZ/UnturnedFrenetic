﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;


namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class AnimalTag : TemplateObject
    {
        public Animal Internal;

        public AnimalTag(Animal animal)
        {
            Internal = animal;
        }

        public static AnimalTag For(int aID)
        {
            Animal animal = null;
            foreach (Animal anim in AnimalManager.animals)
            {
                if (anim.gameObject.GetInstanceID() == aID)
                {
                    animal = anim;
                }
            }
            if (animal == null)
            {
                return null;
            }
            return new AnimalTag(animal);
        }

        public override string Handle(TagData data)
        {
            if (data.Input.Count == 0)
            {
                return ToString();
            }
            switch (data.Input[0])
            {
                // <--[tag]
                // @Name AnimalTag.name
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the name of the animal's type.
                // @Example "2" .name returns "Cow".
                // -->
                case "name":
                    return new TextTag(Internal.asset.animalName).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.aid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns the animal ID number of the animal.
                // @Example "2" .aid returns "1".
                // -->
                case "aid":
                    return new TextTag(Internal.index).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.iid
                // @Group General Information
                // @ReturnType TextTag
                // @Returns this animal's instance ID number.
                // @Example "2" .iid returns "2".
                // -->
                case "iid":
                    return new TextTag(Internal.gameObject.GetInstanceID()).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.asset
                // @Group General Information
                // @ReturnType AnimalAssetTag
                // @Returns the animal asset that this animal is based off.
                // @Example "2" .asset returns "Cow".
                // -->
                case "asset":
                    return new AnimalAssetTag(Internal.asset).Handle(data.Shrink());
                // <--[tag]
                // @Name AnimalTag.location
                // @Group Status
                // @ReturnType LocationTag
                // @Returns the animal's current world position.
                // @Example "2" .location returns "(5, 10, 15)".
                // -->
                case "location":
                    return new LocationTag(Internal.transform.position).Handle(data.Shrink());
                default:
                    return new TextTag(ToString()).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.gameObject.GetInstanceID().ToString();
        }
    }
}