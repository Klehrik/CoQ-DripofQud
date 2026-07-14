using System;
using System.Collections.Generic;
using XRL.UI;
using XRL.World;
using XRL.World.Parts;

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
            if (!base.WantEvent(ID, cascade))
            {
                return ID == InventoryActionEvent.ID;
            }
            return true;
        }

        public override bool HandleEvent(InventoryActionEvent E)
        {
            if (E.Command == "Apply" && AttemptApply(E))
            {
                E.Actor.UseEnergy(5000, "Item DoQ_ItemDye");
                // E.RequestInterfaceExit();
            }
            return base.HandleEvent(E);
        }

        public bool AttemptApply(InventoryActionEvent E)
        {
            if (!E.Actor.CheckFrozen(Telepathic: false, Telekinetic: true))
            {
                return false;
            }
            if (E.Actor.AreHostilesNearby())
            {
                E.Actor.Fail("You can't dye with hostiles nearby.");
                return false;
            }
            if (E.Item.IsBroken() || E.Item.IsRusted())
            {
                return E.Actor.Fail("The sprayer head won't move.");
            }
            List<GameObject> list = E.Actor.GetInventoryAndEquipment();
			list.Remove(ParentObject);
			GameObject gameObject = PickItem.ShowPicker(list, null, PickItem.PickItemDialogStyle.SelectItemDialog, E.Actor);
            if (gameObject == null)
            {
                return false;
            }
            string text = Popup.ShowColorPicker("Choose a primary color.", includeNone: false);
            if (text == null)
            {
                return false;
            }
            string text2 = Popup.ShowColorPicker("Choose a secondary color.", includeNone: false);
            if (text2 == null)
            {
                return false;
            }
            gameObject.SplitStack(1, E.Actor);
            gameObject.Render.SetForegroundColor(text);
            gameObject.Render.DetailColor = text2;
            if (E.Actor.IsPlayer())
            {
                Popup.Show("You dye " + gameObject.t() + ".");
            }
            ParentObject.Destroy();
            return true;
        }
    }
}