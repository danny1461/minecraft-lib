using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib.Entities
{
    public class NamedEntity : Entity
    {
        public String Name { get; set; }
        public Slot[] Inventory { get; set; }
        public Slot InHand
        {
            get
            {
                return Inventory[0];
            }
            set
            {
                Inventory[0] = value;
            }
        }
        public Slot HeadPiece
        {
            get
            {
                return Inventory[5];
            }
            set
            {
                Inventory[5] = value;
            }
        }
        public Slot ChestPiece
        {
            get
            {
                return Inventory[6];
            }
            set
            {
                Inventory[6] = value;
            }
        }
        public Slot LegPiece
        {
            get
            {
                return Inventory[7];
            }
            set
            {
                Inventory[7] = value;
            }
        }
        public Slot ShoePiece
        {
            get
            {
                return Inventory[8];
            }
            set
            {
                Inventory[8] = value;
            }
        }

        public NamedEntity()
            : base()
        {
            Inventory = new Slot[5];
            for (int x = 0; x < 5; x++)
                Inventory[x] = new Slot();
        }
    }
}
