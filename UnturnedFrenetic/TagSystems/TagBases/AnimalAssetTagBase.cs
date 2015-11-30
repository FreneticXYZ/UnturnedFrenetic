﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class AnimalAssetTagBase : TemplateTags
    {
        // <--[tag]
        // @Base animal_asset[<TextTag>]
        // @Group Entities
        // @ReturnType AnimalTag
        // @Returns the animal asset corresponding to the given animal type name.
        // -->
        public AnimalAssetTagBase()
        {
            Name = "animal_asset";
        }

        public override string Handle(TagData data)
        {
            AnimalAssetTag atag = AnimalAssetTag.For(data.GetModifier(0));
            if (atag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return atag.Handle(data.Shrink());
        }
    }
}