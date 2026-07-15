using System;
using HarmonyLib;
using XRL;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    public class DoQ_LiquidProof : IPart
    {
        public override bool SameAs(IPart p)
        {
            return false;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == EquippedEvent.ID
                || ID == UnequippedEvent.ID
                || ID == GetShortDescriptionEvent.ID;
        }

        public override bool HandleEvent(EquippedEvent E)
        {
            if (ParentObject.IsWorn())
            {
                LiquidRepellent p = E.Actor.GetPart<LiquidRepellent>();
                if (p == null)
                {
                    p = E.Actor.AddPart<LiquidRepellent>();
                    p.Liquids = "water,cider,goo,oil,ooze,proteangunk,slime,sludge,wine";
                        // Same batch of liquids as torches
                }
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(UnequippedEvent E)
        {
            LiquidRepellent p = E.Actor.GetPart<LiquidRepellent>();
            if (p != null)
            {
                E.Actor.RemovePart(p);
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.AppendRules("You cannot be covered or stained by several common liquids.");
            return base.HandleEvent(E);
        }
    }
}