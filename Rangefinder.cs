using System;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Capabilities;

namespace XRL.World.Parts
{
    public class DoQ_Rangefinder : IPoweredPart
    {
        public int Value;
        public bool Enabled;

        public DoQ_Rangefinder()
        {
            WorksOnEquipper = true;
		    // IsPowerLoadSensitive = true;  Too lazy to figure this one out
            Enabled = false;
        }

        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == EquippedEvent.ID
                || ID == UnequippedEvent.ID
                || ID == EarlyBeforeBeginTakeActionEvent.ID
                || ID == GetShortDescriptionEvent.ID;
        }

        public override bool HandleEvent(EquippedEvent E)
        {
            if (!base.OnWorldMap)
            {
                ConsumeChargeIfOperational();
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(UnequippedEvent E)
        {
            TryDisable(E.Actor);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EarlyBeforeBeginTakeActionEvent E)
        {
            GameObject equipped = ParentObject.Equipped;
            if (equipped != null)
            {
                if (!base.OnWorldMap && ConsumeChargeIfOperational())
                {
                    TryEnable(equipped);
                    
                    if (equipped.IsPlayer() && AutoAct.IsInterruptable() && !IsReady(UseCharge: false, IgnoreCharge: false, IgnoreLiquid: false, IgnoreBootSequence: false, IgnoreBreakage: false, IgnoreRust: false, IgnoreEMP: false, IgnoreRealityStabilization: false, IgnoreSubject: false, IgnoreLocallyDefinedFailure: false, 1, null, UseChargeIfUnpowered: false, 0L))
                    {
                        AutoAct.Interrupt(equipped.poss(ParentObject) + ParentObject.GetVerb("have") + " stopped working");
                        TryDisable(equipped);
                    }
                }
                else
                {
                    TryDisable(equipped);
                }
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.AppendRules("For the purpose of determining your accuracy with ranged attacks, your agility is treated as if it were " + Math.Round(Value * 2f) + " points higher.");
            return base.HandleEvent(E);
        }

        public void TryEnable(GameObject Actor)
        {
            if (!Enabled)
            {
                Enabled = true;
                Actor.ModIntProperty("MissileWeaponAccuracyBonus", Value);
                XRL.Messages.MessageQueue.AddPlayerMessage(Actor.GetIntProperty("MissileWeaponAccuracyBonus").ToString());
            }
        }

        public void TryDisable(GameObject Actor)
        {
            if (Enabled)
            {
                Enabled = false;
                Actor.ModIntProperty("MissileWeaponAccuracyBonus", -Value, RemoveIfZero: true);
                XRL.Messages.MessageQueue.AddPlayerMessage(Actor.GetIntProperty("MissileWeaponAccuracyBonus").ToString());
            }
        }
    }
}