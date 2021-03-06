﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class StartTheModInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static Init() method into the top of the game's central Provider Awake method.
            // Our mod init will run before anything else in the assembly due to this.
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference initmethod = gamedef.ImportReference(GetMethod(modtype, "Init", 0));
            MethodReference initmethod2 = gamedef.ImportReference(GetMethod(modtype, "InitSecondary", 0));
            TypeDefinition providertype = gamedef.GetType("SDG.Unturned.Provider");
            MethodDefinition awakemethod = GetMethod(providertype, "awake");
            MethodBody awakebody = awakemethod.Body;
            // Call: the mod initialization.
            awakebody.Instructions.Insert(0, Instruction.Create(OpCodes.Call, initmethod));
            // Call: the mod secondary initialization.
            awakebody.Instructions.Insert(176, Instruction.Create(OpCodes.Call, initmethod2));
        }
    }
}
