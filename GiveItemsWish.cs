using System;
using XRL;
using XRL.Wish;
using XRL.World;

namespace Klehrik_DripofQud
{
    [HasWishCommand]
    class Klehrik_DripofQud
    {
        [WishCommand("DoQ_giveitems", null)]
        public static void GiveItems()
        {
            GameObject p = The.Player;
            p.ReceiveObject("DoQ_ItemDye", 20);
            p.ReceiveObject("DoQ_PointedHat", 1);
            p.ReceiveObject("DoQ_TopHat", 1);
            p.ReceiveObject("DoQ_Bandana", 1);
            p.ReceiveObject("DoQ_Rangefinder", 1);
            p.ReceiveObject("DoQ_Balaclava", 1);
            p.ReceiveObject("DoQ_Jacket", 1);
            p.ReceiveObject("DoQ_AmberShawl", 1);
            p.ReceiveObject("DoQ_Poncho", 1);
            p.ReceiveObject("DoQ_Cuff", 2);
            p.ReceiveObject("DoQ_RubberGloves", 1);
            p.ReceiveObject("DoQ_FingerlessGloves", 1);
            p.ReceiveObject("DoQ_PowerBoots", 1);
            p.ReceiveObject("DoQ_HikingBoots", 1);
        }
    }
}