﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;
using FreneticScript;

namespace UnturnedFrenetic
{
    public class EntityType
    {
        public static EntityType BEAR = new EntityType("Bear", EntityAssetType.ANIMAL);
        public static EntityType COW = new EntityType("Cow", EntityAssetType.ANIMAL);
        public static EntityType DEER = new EntityType("Deer", EntityAssetType.ANIMAL);
        public static EntityType MOOSE = new EntityType("Moose", EntityAssetType.ANIMAL);
        public static EntityType PIG = new EntityType("Pig", EntityAssetType.ANIMAL);
        public static EntityType REINDEER = new EntityType("Reindeer", EntityAssetType.ANIMAL);
        public static EntityType WOLF = new EntityType("Wolf", EntityAssetType.ANIMAL);
        public static EntityType ZOMBIE = new EntityType("Zombie", EntityAssetType.ZOMBIE);

        public static Dictionary<string, EntityType> ITEMS = new Dictionary<string, EntityType>();
        public static Dictionary<string, EntityType> VEHICLES = new Dictionary<string, EntityType>();
        public static Dictionary<string, EntityType> WORLD_OBJECTS = new Dictionary<string, EntityType>();
        public static Dictionary<string, EntityType> RESOURCES = new Dictionary<string, EntityType>();
        public static Dictionary<string, EntityType> BARRICADES = new Dictionary<string, EntityType>();
        public static Dictionary<string, EntityType> STRUCTURES = new Dictionary<string, EntityType>();

        public static EntityType ValueOf(string name)
        {
            name = name.ToLowerFast();
            switch (name)
            {
                case "bear":
                    return BEAR;
                case "cow":
                    return COW;
                case "deer":
                    return DEER;
                case "moose":
                    return MOOSE;
                case "pig":
                    return PIG;
                case "reindeer":
                    return REINDEER;
                case "wolf":
                    return WOLF;
                case "zombie":
                    return ZOMBIE;
                default:
                    {
                        EntityType et;
                        if (ITEMS.TryGetValue(name, out et))
                        {
                            return et;
                        }
                        if (VEHICLES.TryGetValue(name, out et))
                        {
                            return et;
                        }
                        if (WORLD_OBJECTS.TryGetValue(name, out et))
                        {
                            return et;
                        }
                        if (RESOURCES.TryGetValue(name, out et))
                        {
                            return et;
                        }
                        if (BARRICADES.TryGetValue(name, out et))
                        {
                            return et;
                        }
                        if (STRUCTURES.TryGetValue(name, out et))
                        {
                            return et;
                        }
                        return null;
                    }
            }
        }

        public string AssetName;

        public EntityAssetType Type;

        public EntityType(string assetname, EntityAssetType type)
        {
            AssetName = assetname;
            Type = type;
        }
    }

    public enum EntityAssetType
    {
        ANIMAL = 0,
        ZOMBIE = 1,
        ITEM = 2,
        VEHICLE = 3,
        WORLD_OBJECT = 4,
        RESOURCE = 5,
        BARRICADE = 6,
        STRUCTURE
    }
}
