﻿using FreneticScript.CommandSystem;
using FreneticScript.TagHandlers;
using FreneticScript.TagHandlers.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnturnedFrenetic.TagSystems.TagObjects;

namespace UnturnedFrenetic.EventSystems.EntityEvents
{
    // <--[event]
    // @Name StructureDeathEvent
    // @Fired When a structure dies.
    // @Updated 2016/04/29
    // @Authors Morphan1
    // @Group Player
    // @Cancellable true
    // @Description
    // This event will fire when a structure dies for any reason.
    // @Context structure StructureTag returns the structure that is dying.
    // @Context amount TextTag returns the amount of damage being done to kill the structure. Editable.
    // -->

    public class StructureDeathScriptEvent : ScriptEvent
    {
        /// <summary>
        /// Constructs the StructureDeath script event.
        /// </summary>
        /// <param name="system">The relevant command system.</param>
        public StructureDeathScriptEvent(Commands system)
            : base(system, "structuredeathevent", true)
        {
        }

        /// <summary>
        /// Register a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void RegisterPriority(int prio)
        {
            if (!UnturnedFreneticEvents.OnStructureDeath.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnStructureDeath.Add(Run, prio);
            }
        }

        /// <summary>
        /// Deregister a specific priority with the underlying event.
        /// </summary>
        /// <param name="prio">The priority.</param>
        public override void DeregisterPriority(int prio)
        {
            if (UnturnedFreneticEvents.OnStructureDeath.Contains(Run, prio))
            {
                UnturnedFreneticEvents.OnStructureDeath.Remove(Run, prio);
            }
        }

        /// <summary>
        /// Runs the script event with the given input.
        /// </summary>
        /// <param name="prio">The priority to run with.</param>
        /// <param name="oevt">The details of the script to be ran.</param>
        /// <returns>The event details after firing.</returns>
        public void Run(int prio, StructureDeathEventArgs oevt)
        {
            StructureDeathScriptEvent evt = (StructureDeathScriptEvent)Duplicate();
            evt.Cancelled = oevt.Cancelled;
            evt.Structure = oevt.Structure;
            evt.Amount = oevt.Amount;
            evt.Call(prio);
            oevt.Amount = evt.Amount;
            oevt.Cancelled = evt.Cancelled;
        }

        /// <summary>
        /// The structure that is dying.
        /// </summary>
        public StructureTag Structure;

        /// <summary>
        /// The amount of damage being done.
        /// </summary>
        public NumberTag Amount;

        /// <summary>
        /// Get all variables according the script event's current values.
        /// </summary>
        public override Dictionary<string, TemplateObject> GetVariables()
        {
            Dictionary<string, TemplateObject> vars = base.GetVariables();
            vars.Add("structure", Structure);
            vars.Add("amount", Amount);
            return vars;
        }

        public override void UpdateVariables(Dictionary<string, TemplateObject> vars)
        {
            Amount = NumberTag.TryFor(vars["amount"]);
            base.UpdateVariables(vars);
        }
    }

    public class StructureDeathEventArgs : EventArgs
    {
        public StructureTag Structure;
        public NumberTag Amount;

        public bool Cancelled = false;
    }
}
