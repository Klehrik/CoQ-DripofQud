using System;
using HarmonyLib;
using XRL;
using XRL.World;
using XRL.World.Parts;
using XRL.World.Capabilities;

namespace XRL.World.Parts
{
    public class DoQ_TastyBonus : IPart
    {
        public int Value;

        public override bool SameAs(IPart p)
        {
            return Value == (p as DoQ_TastyBonus).Value;
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
            E.Actor.ModIntProperty("DoQ_TastyBonus", Value);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(UnequippedEvent E)
        {
            E.Actor.ModIntProperty("DoQ_TastyBonus", 0);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.AppendRules("Your chance to cook a tasty meal while hungry is increased to " + (10 + Value) + "%.");
            return base.HandleEvent(E);
        }
    }
}

namespace Klehrik_DripofQud
{
    [HarmonyPatch(typeof(Campfire), nameof(Campfire.RollTasty))]
    public class RollTastyPatch
    {
        static void Prefix(ref int Bonus)
        {
            Bonus += The.Player.GetIntProperty("DoQ_TastyBonus", 0);
        }
    }
}