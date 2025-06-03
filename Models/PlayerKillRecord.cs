using System.Numerics;
using OpenMod.Unturned.Players.Life.Events;
using SDG.Unturned;
using Steamworks;

namespace Scitalis.Analytics.FileWriter
{
    public struct PlayerKillRecord
    {
        public CSteamID KillerID;
        public CSteamID VictimID;
        public ELimb HitLimb;
        public UnturnedPlayerDeathEvent DamageSource;
        public Vector3 DeathPosition;
        public EDeathCause Cause;

        public PlayerKillRecord(CSteamID killerID, CSteamID victimID, ELimb hitLimb, 
            UnturnedPlayerDeathEvent damageSource, Vector3 deathPosition, EDeathCause cause)
        {
            KillerID = killerID;
            VictimID = victimID;
            HitLimb = hitLimb;
            DamageSource = damageSource;
            DeathPosition = deathPosition;
            Cause = cause;
        }
    }
}