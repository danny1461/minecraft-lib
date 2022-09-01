using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib.Blocks
{
    public class Block
    {
        #region Block List

        private static Block[] Blocks = new Block[]
        {

        };

        #endregion

        //public abstract short Id { get; }
        public short Id { get; set; }
        public virtual String Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
        public virtual byte Metadata { get; set; }
        public virtual byte SkyLight { get; set; }

        public Block() { }
        public static implicit operator short(Block b)
        {
            return b.Id;
        }
        public static implicit operator Block(short b)
        {
            //Block bl = Blocks[b];
            //return (Block)Activator.CreateInstance(bl.GetType());
            Block bl = new Block();
            bl.Id = b;
            return bl;
        }
    }
}
