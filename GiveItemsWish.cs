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
            
            string[] n20 = {
                "DoQ_ItemDye",
            };
            string[] n1 = {
                "DoQ_Transmog",
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
            foreach (string i in n20) p.ReceiveObject(i, 20);
            foreach (string i in n1)  p.ReceiveObject(i, 1);
        }
    }
}