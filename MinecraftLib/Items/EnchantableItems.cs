using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib.Items
{
    public class EnchantableItems
    {
        static List<short> table = new List<short>
        {
            256,        // Iron Shovel
            257,        // Iron Pickaxe
            258,        // Iron Axe
            267,        // Iron Sword
            268,        // Wooden Sword
            269,        // Wooden Shovel
            270,        // Wooden Pickaxe
            271,        // Wooden Axe
            272,        // Stone Sword
            273,        // Stone Shovel
            274,        // Stone Pickaxe
            275,        // Stone Axe
            276,        // Diamond Sword
            277,        // Diamond Shovel
            278,        // Diamond Pickaxe
            279,        // Diamond Axe
            283,        // Gold Sword
            284,        // Gold Shovel
            285,        // Gold Pickaxe
            286         // Gold Axe     
        };

        public static bool CanEnchant(short itemId)
        {
            return table.Contains(itemId);
        }
        public static void SetEnchantable(short itemId, bool value)
        {
            if (value)
            {
                if (!table.Contains(itemId))
                    table.Add(itemId);
            }
            else
            {
                if (table.Contains(itemId))
                    table.Add(itemId);
            }
        }
    }
}
