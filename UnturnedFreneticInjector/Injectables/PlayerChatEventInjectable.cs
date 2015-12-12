﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace UnturnedFreneticInjector.Injectables
{
    public class PlayerChatEventInjectable: Injectable
    {
        public override void InjectInto(ModuleDefinition gamedef, ModuleDefinition moddef)
        {
            // This injects a call to the mod's static PlayerChat method for the PlayerChatScriptEvent
            TypeDefinition modtype = moddef.GetType("UnturnedFrenetic.UnturnedFreneticMod");
            MethodReference eventmethod = gamedef.ImportReference(GetMethod(modtype, "PlayerChat", 5));
            TypeDefinition managertype = gamedef.GetType("SDG.Unturned.ChatManager");
            FieldDefinition managerfield = GetField(managertype, "manager");
            managerfield.IsPrivate = false;
            managerfield.IsPublic = true;
            MethodDefinition chatmethod = GetMethod(managertype, "askChat", 3);
            MethodBody chatbody = chatmethod.Body;
            // Remove old color handling
            for (int i = 105; i < 118; i++)
            {
                chatbody.Instructions.RemoveAt(105);
            }
            InjectInstructions(chatbody, 27, new Instruction[]
                {
                    // Load "steamPlayer" onto the stack.
                    Instruction.Create(OpCodes.Ldloc_0),
                    // Load "mode" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, chatmethod.Parameters[1]),
                    // Load "eChatMode" onto the stack.
                    Instruction.Create(OpCodes.Ldloca_S, chatbody.Variables[1]),
                    // Load "color" onto the stack.
                    Instruction.Create(OpCodes.Ldloca_S, chatbody.Variables[2]),
                    // Load "text" onto the stack.
                    Instruction.Create(OpCodes.Ldarga_S, chatmethod.Parameters[2]),
                    // Call the PlayerChat method with the above parameters and return a bool.
                    Instruction.Create(OpCodes.Call, eventmethod),
                    // If the return is true, jump ahead to the original 27th instruction.
                    Instruction.Create(OpCodes.Brfalse, chatbody.Instructions[27]),
                    // Otherwise,return now.
                    Instruction.Create(OpCodes.Ret)
                });
            chatbody.Instructions[59] = Instruction.Create(OpCodes.Br, chatbody.Instructions[113]);
            chatbody.Instructions[85] = Instruction.Create(OpCodes.Br, chatbody.Instructions[113]);
            chatbody.Instructions[111] = Instruction.Create(OpCodes.Br, chatbody.Instructions[113]);
        }
    }
}