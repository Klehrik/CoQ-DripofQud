using System;
using HarmonyLib;
using XRL;
using XRL.World;
using XRL.World.Parts;
using Effects = XRL.World.Effects;

namespace XRL.World.Parts
{
    public class DoQ_LiquidProof : IPart
    {
        /*
        Chance of *not* being covered/stained when entering a pool.
        This should be the sqrt of the actual desired decimal chance
        since entering a pool procs LiquidCovered twice for some reason
        (except for when it doesn't???)
        */
        public int Chance;

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
                E.Actor.ApplyEffect(new Effects.DoQ_LiquidProof(ParentObject, Chance));
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(UnequippedEvent E)
        {
            Effects.DoQ_LiquidProof effect = E.Actor.GetEffect((Effects.DoQ_LiquidProof fx) => fx.Source == ParentObject);
            if (effect != null)
            {
                E.Actor.RemoveEffect(effect);
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.AppendRules("Chance of becoming covered or stained by liquids reduced by " + Math.Round(Chance * Chance / 100f) + "%.");
            return base.HandleEvent(E);
        }
    }
}

namespace XRL.World.Effects
{
    public class DoQ_LiquidProof : Effect, ITierInitialized
    {
        public GameObject Source;
        public int Chance;

        public DoQ_LiquidProof()
        {
            DisplayName = "liquid-resistant";
		    Duration = 9999;
            Chance = 0;
        }

        public DoQ_LiquidProof(GameObject Source, int Chance = 0) : this()
        {
            this.Source = Source;
            this.Chance = Chance;
        }

        public override int GetEffectType()
        {
            return Effect.TYPE_MINOR | Effect.TYPE_EQUIPMENT;
        }

        public override bool SameAs(Effect e)
        {
            return false;
        }

        public override bool UseStandardDurationCountdown()
        {
            return true;
        }

        public override string GetDescription()
        {
            return "liquid-resistant";
        }

        public override string GetDetails()
        {
            return "Chance of becoming covered or stained by liquids reduced by " + Math.Round(Chance * Chance / 100f) + "%.";
        }

        public override bool Apply(GameObject Object)
        {
            return true;
        }
    }
}

namespace Klehrik_DripofQud
{
    [HarmonyPatch(typeof(GameObject))]
    public class GameObjectPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GameObject.ApplyEffect))]
        static bool ApplyEffect(GameObject __instance, ref bool __result, Effect E)
        {
            Effects.DoQ_LiquidProof effect = __instance.GetEffect<Effects.DoQ_LiquidProof>();
            if (effect != null
                && (E is Effects.LiquidCovered
                 || E is Effects.LiquidStained)
            )
            {
                if (effect.Chance.in100())
                {
                    __result = false;
                    return false;
                }
            }
            return true;
        }
    }
}