using System;
using System.Collections.Generic;
using XRL.UI;
using XRL.World;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    public class DoQ_Transmog : IPart
    {
        public override bool SameAs(IPart p)
        {
            return true;
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == GetInventoryActionsEvent.ID
                || ID == InventoryActionEvent.ID;
        }

        public override bool HandleEvent(GetInventoryActionsEvent E)
        {
            E.AddAction("Restyle", "restyle", "DoQ_Transmog", null, 'r');
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(InventoryActionEvent E)
        {
            if (E.Command == "DoQ_Transmog" && AttemptTransmog(E))
            {
                E.Actor.UseEnergy(10000, "Item DoQ_Transmog");
            }
            return base.HandleEvent(E);
        }

        public bool AttemptTransmog(InventoryActionEvent E)
        {
            if (!E.Actor.CheckFrozen(Telepathic: false, Telekinetic: true)) return false;
            if (E.Actor.AreHostilesNearby())                                return E.Actor.Fail("You can't style with hostiles nearby.");
            if (E.Item.IsBroken() || E.Item.IsRusted())                     return E.Actor.Fail("The styling kit is damaged.");

            List<GameObject> list = E.Actor.GetInventoryAndEquipment();
			list.Remove(ParentObject);
			GameObject gameObject = PickItem.ShowPicker(list, null, PickItem.PickItemDialogStyle.SelectItemDialog, E.Actor, Title: "Select an item to restyle");
            if (gameObject == null) return false;
            list.Remove(gameObject);
			GameObject gameObject2 = PickItem.ShowPicker(list, null, PickItem.PickItemDialogStyle.SelectItemDialog, E.Actor, Title: "Select a new texture (will be consumed)");
            if (gameObject2 == null) return false;

            gameObject.SplitStack(1, E.Actor);
            gameObject2.SplitStack(1, E.Actor);

            DoQ_OriginalItemTexture og = gameObject.RequirePart<DoQ_OriginalItemTexture>();

            gameObject.Render.Tile         = gameObject2.Render.Tile;
            gameObject.Render.RenderString = gameObject2.Render.RenderString;
            gameObject.Render.ColorString  = gameObject2.Render.ColorString;
            gameObject.Render.DetailColor  = gameObject2.Render.DetailColor;
            gameObject.Render.TileColor    = gameObject2.Render.TileColor;

            Description desc1 = gameObject.GetPart<Description>();
            Description desc2 = gameObject2.GetPart<Description>();
            if (desc1 != null && desc2 != null)
            {
                desc1._Short = desc2._Short;
            }

            gameObject.Render.DisplayName = og.DescriptionInject;
            gameObject.RemovePart<DoQ_TransmogMark>();
            if (E.Actor.IsPlayer())
            {
                Popup.Show("You restyle " + gameObject.t() + ".");
            }
            gameObject.Render.DisplayName = gameObject2.Render.DisplayName;
            gameObject.AddPart<DoQ_TransmogMark>();

            gameObject2.Destroy();
            // ParentObject.Destroy();
            return true;
        }
    }

    public class DoQ_OriginalItemTexture : IPart
    {
        public string DescriptionInject;

        public override void Initialize()
        {
            base.Initialize();
            DescriptionInject = ParentObject.GetDisplayName(int.MaxValue, null, null, AsIfKnown: true, Single: false, NoConfusion: true, NoColor: false, Stripped: false, ColorOnly: false, Visible: true, WithoutTitles: false, ForSort: false, Short: true, BaseOnly: false, WithIndefiniteArticle: false, WithDefiniteArticle: false, null, IndicateHidden: false, Capitalize: false, SecondPerson: false, Reflexive: false, true);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == GetShortDescriptionEvent.ID;
        }

        public override bool HandleEvent(GetShortDescriptionEvent E)
        {
            E.Postfix.Insert(0, "\n{{M|▲}} This item is a restyled " + DescriptionInject + ".\n");
            return base.HandleEvent(E);
        }
    }

    public class DoQ_TransmogMark : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == GetDisplayNameEvent.ID;
        }

        public override bool HandleEvent(GetDisplayNameEvent E)
        {
            E.AddMark("{{M|▲}}");
            return base.HandleEvent(E);
        }
    }
}