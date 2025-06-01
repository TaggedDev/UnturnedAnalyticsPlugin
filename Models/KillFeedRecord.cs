using OpenMod.Extensions.Games.Abstractions.Entities;
using SDG.Unturned;
using Steamworks;

namespace Scitalis.Analytics.Models
{
    public struct KillFeedRecord
    {
        public CSteamID KillerID;
        public CSteamID VictimID;
        public ELimb HitLimb;
        public IDamageSource? DamageSource;
        public byte DamageAmount;
        public EDeathCause Cause;
        public string DamageSourceName;

        public KillFeedRecord(CSteamID killerID, CSteamID victimID, ELimb hitLimb, IDamageSource? damageSource, byte damageAmount, EDeathCause cause)
        {
            KillerID = killerID;
            VictimID = victimID;
            HitLimb = hitLimb;
            DamageSource = damageSource;
            DamageAmount = damageAmount;
            Cause = cause;
            DamageSourceName = damageSource is null ? string.Empty : damageSource.DamageSourceName;
        }

    }
}