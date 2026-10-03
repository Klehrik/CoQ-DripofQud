using System;
using System.Collections.Generic;
using HarmonyLib;
using XRL.UI;
using XRL.World;
using XRL.World.Parts;
using ConsoleLib.Console;
using Klehrik_DripofQud;

namespace XRL.World.Parts
{
    public class DoQ_ItemDye : IPart
    {
        public override bool SameAs(IPart p)
        {
            return true;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == InventoryActionEvent.ID;
        }

        public override bool HandleEvent(InventoryActionEvent E)
        {
            if (E.Command == "Apply" && AttemptApply(E))
            {
                E.Actor.UseEnergy(5000, "Item DoQ_ItemDye");
            }
            return base.HandleEvent(E);
        }

        public bool AttemptApply(InventoryActionEvent E)
        {
            if (!E.Actor.CheckFrozen(Telepathic: false, Telekinetic: true)) return false;
            if (E.Actor.AreHostilesNearby())                                return E.Actor.Fail("You can't dye with hostiles nearby.");
            if (E.Item.IsBroken() || E.Item.IsRusted())                     return E.Actor.Fail("The sprayer head won't move.");

            List<GameObject> list = E.Actor.GetInventoryAndEquipment();
			list.Remove(ParentObject);
			GameObject gameObject = PickItem.ShowPicker(list, null, PickItem.PickItemDialogStyle.SelectItemDialog, E.Actor);
            if (gameObject == null) return false;

            ColorPickerPatch.Enable = true;
            string text = Popup.ShowColorPicker("Choose a primary color.", AllowEscape: true);
            if (text == null) return false;

            ColorPickerPatch.Enable = true;
            string text2 = Popup.ShowColorPicker("Choose a secondary color.", AllowEscape: true);
            if (text2 == null) return false;

            if (string.IsNullOrEmpty(text) && string.IsNullOrEmpty(text2)) return false;

            gameObject.SplitStack(1, E.Actor);
            if (!string.IsNullOrEmpty(text))
            {
                gameObject.Render.SetForegroundColor(text);
            }
            if (!string.IsNullOrEmpty(text2))
            {
                gameObject.Render.DetailColor = text2;
            }
            if (E.Actor.IsPlayer())
            {
                Popup.Show("You dye " + gameObject.t() + ".");
            }
            ParentObject.Destroy();
            return true;
        }
    }
}

namespace Klehrik_DripofQud
{
    [HarmonyPatch(typeof(Popup))]
    public class ColorPickerPatch
    {
        static internal bool Enable = false;

        private static readonly List<string> ColorPickerOptionStrings = AccessTools.StaticFieldRefAccess<Popup, List<string>>("ColorPickerOptionStrings");
        private static readonly List<string> ColorPickerOptions = AccessTools.StaticFieldRefAccess<Popup, List<string>>("ColorPickerOptions");
        private static readonly List<char> ColorPickerKeymap = AccessTools.StaticFieldRefAccess<Popup, List<char>>("ColorPickerKeymap");

        [HarmonyPostfix]
        [HarmonyPatch("SetupColorPickers")] // Can't use nameof since it's private
        static void SetupColorPickers()
        {
            if (!Enable) return;
            Enable = false;

            // Add both oranges (after reds)
            char lastUsedKey = ColorPickerKeymap[^1];
            int pos = 10;
            foreach (char c in new[] {'o', 'O'})
            {
                var col     = MarkupShaders.GetSolidColor(c);
                var colCode = col.Foreground.ToString();
                ColorPickerOptionStrings.Insert(pos, "&" + colCode + col.GetDisplayName());
                ColorPickerOptions.Insert(pos, colCode);
                ColorPickerKeymap.Add(++lastUsedKey);
                pos++;
            }
        }
    }
}