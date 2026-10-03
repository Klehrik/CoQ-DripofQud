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
            
            string[] items = {
                "DoQ_ItemDye",
                "DoQ_Transmog",
            };
            string[] clothes = {
                "DoQ_PointedHat",
                "DoQ_TopHat",
                "DoQ_Bandana",
                "DoQ_Rangefinder",
                "DoQ_Balaclava",
                "DoQ_Eyepatch",
                "DoQ_Jacket",
                "DoQ_AmberShawl",
                "DoQ_Poncho",
                "DoQ_Cuff",
                "DoQ_RubberGloves",
                "DoQ_FingerlessGloves",
                "DoQ_PowerBoots",
                "DoQ_HikingBoots",
            };
            foreach (string i in items) p.ReceiveObject(i, 20);
            foreach (string c in clothes) p.ReceiveObject(c, 1);
        }
    }
}