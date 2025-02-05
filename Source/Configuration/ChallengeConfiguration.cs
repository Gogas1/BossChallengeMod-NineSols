using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration {
    public struct ChallengeConfiguration {
        #region General
        public bool EnableMod;
        public bool UseSingleRecordKey;
        public int MaxBossCycles;
        public int MaxMinibossCycles;
        public int MaxEnemyCycles;

        public bool AffectBosses;
        public bool AffectMiniBosses;
        public bool AffectRegularEnemies;

        public bool RandomizeBossCyclesNumber;//
        public int MinRandomBossCycles;//
        public int MaxRandomBossCycles;//

        public bool RandomizeMiniBossCyclesNumber;//
        public int MinRandomMiniBossCycles;//
        public int MaxRandomMiniBossCycles;//
        
        public bool RandomizeEnemyCyclesNumber;//
        public int MinRandomEnemyCycles;//
        public int MaxRandomEnemyCycles;//
        #endregion General

        #region Scaling
        public bool EnableSpeedScaling;
        public float MinSpeedScalingValue;
        public float MaxSpeedScalingValue;
        public int MaxSpeedScalingCycle;

        public bool EnableModifiersScaling;
        public int MaxModifiersNumber;
        public int MaxModifiersScalingCycle;

        public bool EnableRandomSpeedScaling;//
        public int RandomSpeedScalingStartDeath;//
        public float MinRandomSpeedScalingValue;//
        public float MaxRandomSpeedScalingValue;//

        public bool EnableRandomModifiersScaling;//
        public int RandomModifiersScalingStartDeath;//
        public int MinRandomModifiersNumber;//
        public int MaxRandomModifiersNumber;//
        #endregion Scaling

        public bool ModifiersEnabled;
        public int ModifiersStartFromDeath;
        public bool AllowRepeatModifiers;
        public bool SpeedModifierEnabled;
        public bool TimerModifierEnabled;
        public bool ParryDirectDamageModifierEnabled;
        public bool DamageBuildupModifierEnabled;
        public bool RegenerationModifierEnabled;
        public bool KnockbackModifierEnabled;
        public bool RandomArrowModifierEnabled;
        public bool RandomTalismanModifierEnabled;
        public bool EnduranceModifierEnabled;
        public bool QiShieldModifierEnabled;
        public bool TimedShieldModifierEnabled;
        public bool QiOverloadModifierEnabled;
        public bool DistanceShieldModifierEnabled;
        public bool YanlaoGunModifierEnabled;
    }
}
