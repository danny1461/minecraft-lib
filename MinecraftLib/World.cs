using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using MinecraftLib.Entities;
using MinecraftLib.Blocks;
using MinecraftLib.Packets;

namespace MinecraftLib
{
    public enum Dimension : sbyte
    {
        Nether = -1,
        Overworld = 0,
        End = 1
    }
    public enum Difficulty : sbyte
    {
        Peaceful = 0,
        Easy,
        Normal,
        Hard
    }
    public enum WorldMode : int
    {
        Survival = 0,
        Creative = 1,
        Hardcore = 2,
    }
    public class World
    {
        public List<Entity> Entities { get; set; }
        public Vector3 Spawn { get; set; }
        public WorldMode Mode { get; set; }
        public List<Region> Regions { get; set; }
        public long Time { get; set; }
        public String LevelType { get; set; }
        public Difficulty Difficulty { get; set; }

        public World()
        {
            Regions = new List<Region>();
            Entities = new List<Entity>();
        }
        public Region GetRegion(Vector3 position)
        {
            Vector3 offset = position.Floor();
            offset.X = (int)Math.Floor(offset.X / 32) * 32;
            offset.Y = 0;
            offset.Z = (int)Math.Floor(offset.Z / 32) * 32;
            foreach (Region r in Regions)
            {
                if (r.Location.Equals(offset))
                    return r;
            }
            Region region = new Region(offset);
            Regions.Add(region);
            return region;
        }
        public Chunk GetChunk(Vector3 position)
        {
            Region r = GetRegion(position);
            position -= r.Location;
            return r.GetChunk(position);
        }
        public Block GetBlock(Vector3 position)
        {
            Region r = GetRegion(position);
            position -= r.Location;
            return r.GetBlock(position);
        }
        public void SetBlock(Vector3 position, Block value)
        {
            Region r = GetRegion(position);
            position -= r.Location;
            r.SetBlock(position, value);
        }
        public Entity GetEntity(int Id)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                if (Entities[i].Id == Id)
                    return Entities[i];
            }
            return null;
        }
        public void DestroyEntity(int Id)
        {
            for (int i = 0; i < Entities.Count; i++)
            {
                if (Entities[i].Id == Id)
                {
                    Entities.RemoveAt(i);
                    break;
                }
            }
        }

        public List<Vector3> GetOccupiedBlocks(Entity e)
        {
            Vector3 Pos = e.Location;
            Vector3 occupied;
            List<Vector3> result = new List<Vector3>();
            for (double x = Pos.X - e.BoundingBox.Width; x < Pos.X + e.BoundingBox.Width; x = x + 0.5)
            {
                for (double z = Pos.X - e.BoundingBox.Width; z < Pos.X + e.BoundingBox.Width; z = z + 0.5)
                {
                    for (double y = Pos.Y; y < Pos.Y + e.BoundingBox.Height; y++)
                    {
                        occupied = new Vector3(x, y, z);
                        occupied = occupied.FixToBlock();
                        if (!result.Contains(occupied))
                            result.Add(occupied);
                    }
                }
            }
            result.TrimExcess();
            return result;
        }
        public List<Vector3> GetOccupiedBlocksAtFeet(Entity e)
        {
            Vector3 Pos = e.Location;
            Vector3 occupied;
            List<Vector3> result = new List<Vector3>();
            for (double x = Pos.X - e.BoundingBox.Width; x < Pos.X + e.BoundingBox.Width; x = x + 0.5)
            {
                for (double z = Pos.X - e.BoundingBox.Width; z < Pos.X + e.BoundingBox.Width; z = z + 0.5)
                {
                    occupied = new Vector3(x, Pos.Y, z);
                    occupied = occupied.FixToBlock();
                    if (!result.Contains(occupied))
                        result.Add(occupied);
                }
            }
            result.TrimExcess();
            return result;
        }
        public List<Vector3> GetOccupiedBlocksAtHead(Entity e)
        {
            Vector3 Pos = e.Location.Clone();
            Pos.Y += e.BoundingBox.Height;
            Vector3 occupied;
            List<Vector3> result = new List<Vector3>();
            for (double x = Pos.X - e.BoundingBox.Width; x < Pos.X + e.BoundingBox.Width; x = x + 0.5)
            {
                for (double z = Pos.X - e.BoundingBox.Width; z < Pos.X + e.BoundingBox.Width; z = z + 0.5)
                {
                    occupied = new Vector3(x, Pos.Y, z);
                    occupied = occupied.FixToBlock();
                    if (!result.Contains(occupied))
                        result.Add(occupied);
                }
            }
            result.TrimExcess();
            return result;
        }
        public List<Entity> GetEntitiesFromArea(Vector3 Pos, double Radius)
        {
            Radius *= 2;
            List<Entity> result = new List<Entity>();
            foreach (Entity e in Entities)
                if (Pos.DistanceSquaredTo(e.Location) <= Radius)
                    result.Add(e);
            return result;
        }
        public List<Mob> GetMobsFromArea(Vector3 Pos, double Radius)
        {
            Radius *= 2;
            List<Mob> result = new List<Mob>();
            foreach (Entity e in Entities)
                if (e is Mob)
                    if (Pos.DistanceSquaredTo(e.Location) <= Radius)
                        result.Add((Mob)e);
            return result;
        }
        public List<Mob> GetHostileMobsFromArea(Vector3 Pos, double Radius)
        {
            Radius *= 2;
            List<Mob> result = new List<Mob>();
            foreach (Entity e in Entities)
                if (e is Mob)
                    if (((Mob)e).IsHostile)
                        if (Pos.DistanceSquaredTo(e.Location) <= Radius)
                            result.Add((Mob)e);
            return result;
        }
        public List<Mob> GetPassiveMobsFromArea(Vector3 Pos, double Radius)
        {
            Radius *= 2;
            List<Mob> result = new List<Mob>();
            foreach (Entity e in Entities)
                if (e is Mob)
                    if (!((Mob)e).IsHostile)
                        if (Pos.DistanceSquaredTo(e.Location) <= Radius)
                            result.Add((Mob)e);
            return result;
        }
        public Mob GetNearestMob(Vector3 Pos)
        {
            double shortestDist = double.MaxValue;
            Entity result = null;
            double dist = 0;
            foreach (Entity e in Entities)
            {
                if (e is Mob)
                {
                    dist = Pos.DistanceSquaredTo(e.Location);
                    if (dist < shortestDist)
                    {
                        shortestDist = dist;
                        result = e;
                    }
                }
            }
            return (Mob)result;
        }
        public Mob GetNearestHostileMob(Vector3 Pos)
        {
            double shortestDist = double.MaxValue;
            Entity result = null;
            double dist = 0;
            foreach (Entity e in Entities)
            {
                if (e is Mob)
                {
                    if (((Mob)e).IsHostile)
                    {
                        dist = Pos.DistanceSquaredTo(e.Location);
                        if (dist < shortestDist)
                        {
                            shortestDist = dist;
                            result = e;
                        }
                    }
                }
            }
            return (Mob)result;
        }
        public Mob GetNearestPassiveMob(Vector3 Pos)
        {
            double shortestDist = double.MaxValue;
            Entity result = null;
            double dist = 0;
            foreach (Entity e in Entities)
            {
                if (e is Mob)
                {
                    if (!((Mob)e).IsHostile)
                    {
                        dist = Pos.DistanceSquaredTo(e.Location);
                        if (dist < shortestDist)
                        {
                            shortestDist = dist;
                            result = e;
                        }
                    }
                }
            }
            return (Mob)result;
        }
    }
    public class Region
    {
        // 32x16x32 Chunks
        public MapColumn[,] Columns { get; set; }
        public Vector3 Location;

        public Region(Vector3 Location)
        {
            Columns = new MapColumn[32, 32];
            this.Location = Location;
        }
        public void DeleteColumn(Vector3 position)
        {
            Columns[(int)position.X, (int)position.Z] = null;
        }
        public MapColumn GetColumn(Vector3 position)
        {
            int x = (int)position.X;
            int z = (int)position.Z;
            if (Columns[x, z] == null)
                Columns[x, z] = new MapColumn(new Vector3(x * 16, 0, z * 16));
            return Columns[x, z];
        }
        public Chunk GetChunk(Vector3 position)
        {
            MapColumn column = GetColumn(position);
            return column.GetChunk(position);
        }
        public Block GetBlock(Vector3 position)
        {
            MapColumn column = GetColumn(position);
            Vector3 offset = position.Floor();
            offset -= column.Location;
            Chunk c = GetChunk(position);
            offset -= c.Location;
            return c.GetBlock(offset);
        }
        public void SetBlock(Vector3 position, Block value)
        {
            MapColumn column = GetColumn(position);
            Vector3 offset = position.Floor();
            offset -= column.Location;
            Chunk c = GetChunk(position);
            offset -= c.Location;
            c.SetBlock(offset, value);
        }
    }
    public class MapColumn
    {
        public Chunk[] Chunks { get; set; }
        public Vector3 Location { get; set; }
        public byte[] Biome { get; set; }

        public MapColumn(Vector3 location)
        {
            this.Location = location;
            Chunks = new Chunk[16];
            for (int i = 0; i < 16; i++)
                Chunks[i] = new Chunk(Location + new Vector3(0, 16 * i, 0));
        }
        public Chunk GetChunk(Vector3 position)
        {
            return Chunks[(int)(position.Y / 16)];
        }
        public Block GetBlock(Vector3 position)
        {
            Chunk c = GetChunk(position);
            Vector3 offset = position.Floor();
            offset -= c.Location;
            return c.GetBlock(offset);
        }
        public void SetBlock(Vector3 position, Block value)
        {
            Chunk c = GetChunk(position);
            Vector3 offset = position.Floor();
            offset -= c.Location;
            c.SetBlock(offset, value);
        }
    }
    public class Chunk
    {
        // Usually 16x16x16
        public short[] Blocks { get; set; }
        public NibbleArray Metadata { get; set; }
        public NibbleArray BlockLight { get; set; }
        public NibbleArray SkyLight { get; set; }
        public bool IsAir { get; set; }
        public Vector3 Location { get; set; }

        public Chunk(Vector3 location)
        {
            Blocks = new short[4096];
            Metadata = new NibbleArray(4096);
            BlockLight = new NibbleArray(4096);
            SkyLight = new NibbleArray(4096);
            Location = location;
        }
        public Block GetBlock(Vector3 pos)
        {
            if (IsAir)
                return 0;

            int index = (int)pos.X + ((int)pos.Z * 16) + ((int)pos.Y * 256);
            //int index = (int)pos.Z + ((int)pos.X * 16) + ((int)pos.Y * 256);
            Block b = Blocks[index];
            b.Metadata = Metadata[index];
            //b.SkyLight = SkyLight[index];
            return b;
        }
        public void SetBlock(Vector3 pos, Block value)
        {
            //int index = (int)pos.X + ((int)pos.Z * 16) + ((int)pos.Y * 256);
            int index = (int)pos.Y + ((int)pos.Z * 16) + ((int)pos.X * 256);
            Blocks[index] = value.Id;
            Metadata[index] = value.Metadata;

            if (value.Id != 0)
                IsAir = false;
            else
            {
                IsAir = true;
                for (int i = 0; i < Blocks.Length; i++)
                {
                    if (Blocks[i] != 0)
                    {
                        IsAir = false;
                        break;
                    }
                }
            }
        }
    }
}
