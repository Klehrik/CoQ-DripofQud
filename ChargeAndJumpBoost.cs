using System;
using XRL;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    public class DoQ_ChargeAndJumpBoost : IPart
    {
        public int Jump;
        public int Charge;

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
	            E.Actor.ModIntProperty("JumpRangeModifier", Jump);
                E.Actor.ModIntProperty("ChargeRangeModifier", Charge);
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(UnequippedEvent E)
        {
            E.Actor.ModIntProperty("JumpRangeModifier", -Jump, RemoveIfZero: true);
            E.Actor.ModIntProperty("ChargeRangeModifier", -Charge, RemoveIfZero: true);
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.AppendRules("You can jump " + Jump + " and charge " + Charge + " squares farther.");
            return base.HandleEvent(E);
        }
    }
}