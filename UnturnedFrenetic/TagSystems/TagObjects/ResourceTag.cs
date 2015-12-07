﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Frenetic.TagHandlers;
using Frenetic.TagHandlers.Objects;
using SDG.Unturned;

namespace UnturnedFrenetic.TagSystems.TagObjects
{
    public class ResourceTag : TemplateObject
    {
        public ResourceSpawnpoint Internal;

        public ResourceTag(ResourceSpawnpoint resource)
        {
            Internal = resource;
        }

        public static ResourceTag For(int instanceID)
        {
            for (byte x = 0; x < Regions.WORLD_SIZE; x++)
            {
                for (byte y = 0; y < Regions.WORLD_SIZE; y++)
                {
                    foreach (ResourceSpawnpoint resource in LevelGround.trees[x, y])
                    {
                        if (instanceID == resource.model.gameObject.GetInstanceID())
                        {
                            return new ResourceTag(resource);
                        }
                    }
                }
            }
            return null;
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
                // @Name ResourceTag.asset
                // @Group General Information
                // @ReturnType ResourceAssetTag
                // @Returns the resource asset that this resource is based off.
                // @Example "2" .asset returns "Bush_Jade".
                // -->
                case "asset":
                    return new ResourceAssetTag(Internal.asset).Handle(data.Shrink());
                default:
                    return new EntityTag(Internal.model.gameObject).Handle(data);
            }
        }

        public override string ToString()
        {
            return Internal.model.gameObject.GetInstanceID().ToString();
        }
    }
}
