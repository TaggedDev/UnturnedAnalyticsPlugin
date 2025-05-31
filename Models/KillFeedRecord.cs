using System.Collections.Generic;
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
        public Dictionary<string, object> Arguments;
        public Dictionary<string, object> Data;
    }
}