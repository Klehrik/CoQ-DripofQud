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

                // Head
                "DoQ_PointedHat",
                "DoQ_TopHat",
                "DoQ_Toque",

                // Face
                "DoQ_Bandana",
                "DoQ_Rangefinder",
                "DoQ_Balaclava",
                "DoQ_Eyepatch",

                // Body
                "DoQ_Jacket",
                "DoQ_Plaid",

                // Back
                "DoQ_AmberShawl",
                "DoQ_Poncho",

                // Arm
                "DoQ_Cuff",

                // Hands
                "DoQ_RubberGloves",
                "DoQ_FingerlessGloves",

                // Feet
                "DoQ_PowerBoots",
                "DoQ_HikingBoots",
            };
            foreach (string i in n20) p.ReceiveObject(i, 20);
            foreach (string i in n1)  p.ReceiveObject(i, 1);
        }
    }
}