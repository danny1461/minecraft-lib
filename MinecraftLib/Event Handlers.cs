using System;
using System.Collections.Generic;
using System.Text;

namespace MinecraftLib
{
    //public delegate void PlayerSpawnHandler(Player player);
    //public delegate void PlayerDespawnHandler(Player player);
    public delegate void DisconnectedHandler(String Reason);
    public delegate void PlayerDeathHandler(String PlayerName);
    public delegate void ChatHandler(ChatEventArgs e);
    public delegate void DebugHandler(String Message, int Code);
    public delegate void TickHandler(TimeSpan GameTime);
    public delegate void HealthHandler(HealthUpdateEventArgs e);
    public delegate void HungerHandler(HungerUpdateEventArgs e);
    public delegate void PlayerJoinHandler(String PlayerName);
    public delegate void PlayerLeftHandler(String PlayerName);
    public delegate void PlayerEnteredAreaHandler(String PlayerName);
    public delegate void PlayerLeftAreaHandler(String PlayerName);

    public class HealthUpdateEventArgs
    {
        public short OldHealth { get; set; }
        public short Health { get; set; }

        public HealthUpdateEventArgs(short OldHealth, short Health)
        {
            this.OldHealth = OldHealth;
            this.Health = Health;
        }
    }
    public class HungerUpdateEventArgs
    {
        public short OldHunger { get; set; }
        public short Hunger { get; set; }

        public HungerUpdateEventArgs(short OldHunger, short Hunger)
        {
            this.OldHunger = OldHunger;
            this.Hunger = Hunger;
        }
    }
    public class ChatEventArgs
    {
        public string FullMessage { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public bool Private { get; set; }

        public ChatEventArgs(String Raw)
        {
            try
            {
                FullMessage = Raw;
                Private = false;

                if (Raw.Contains("§"))
                {
                    // From the server
                    UserName = "Console";
                    // Dig out the color codes
                    while (Raw.IndexOf("§") >= 0)
                        Raw = Raw.Remove(Raw.IndexOf("§"), 2);
                    if (Raw.IndexOf('[') == 0)
                        Message = Raw.Substring(Raw.IndexOf(']') + 2);
                    else if (Raw.Contains(" whispers "))
                    {
                        //UserName = Raw.Substring(0, Raw.IndexOf(' '));
                        Message = Raw.Substring(17);
                        Private = true;
                    }
                    else
                        Message = Raw;
                }
                else
                {
                    if (Raw.IndexOf('<') == 0)
                    {
                        UserName = Raw.Substring(1, Raw.IndexOf('>') - 1);
                        Message = Raw.Substring(Raw.IndexOf('>') + 2);
                    }
                    else
                    {
                        UserName = "Console";
                        Message = Raw;
                    }
                }
            }
            catch
            {

            }
        }
    }

}
