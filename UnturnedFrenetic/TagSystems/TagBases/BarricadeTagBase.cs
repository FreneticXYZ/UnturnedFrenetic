﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.TagSystems.TagBases
{
    public class BarricadeTagBase : TemplateTags
    {
        // <--[tag]
        // @Base barricade[<TextTag>]
        // @SubType EntityTag
        // @Group Entities
        // @ReturnType BarricadeTag
        // @Returns the barricade entity corresponding to the given ID number.
        // -->
        public BarricadeTagBase()
        {
            Name = "barricade";
        }

        public override string Handle(TagData data)
        {
            BarricadeTag itag = BarricadeTag.For(Utilities.StringToInt(data.GetModifier(0)));
            if (itag == null)
            {
                return new TextTag("{NULL}").Handle(data.Shrink());
            }
            return itag.Handle(data.Shrink());
        }
    }
}
