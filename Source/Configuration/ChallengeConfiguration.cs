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

        public bool EnableRandomSpeedScaling;
        public int RandomSpeedScalingStartDeath;
        public float MinRandomSpeedScalingValue;
        public float MaxRandomSpeedScalingValue;

        public bool EnableRandomModifiersScaling;
        public int RandomModifiersScalingStartDeath;
        public int MinRandomModifiersNumber;
        public int MaxRandomModifiersNumber;

        #region Bosses scaling
        public bool BossesEnableSpeedScaling;
        public float BossesMinSpeedScalingValue;
        public float BossesMaxSpeedScalingValue;
        public int BossesMaxSpeedScalingCycle;

        public bool BossesEnableModifiersScaling;
        public int BossesMaxModifiersNumber;
        public int BossesMaxModifiersScalingCycle;

        public bool BossesEnableRandomSpeedScaling;
        public int BossesRandomSpeedScalingStartDeath;
        public float BossesMinRandomSpeedScalingValue;
        public float BossesMaxRandomSpeedScalingValue;

        public bool BossesEnableRandomModifiersScaling;
        public int BossesRandomModifiersScalingStartDeath;
        public int BossesMinRandomModifiersNumber;
        public int BossesMaxRandomModifiersNumber;
        #endregion Bosses scaling

        #region Minibosses scaling
        public bool MinibossesEnableSpeedScaling;
        public float MinibossesMinSpeedScalingValue;
        public float MinibossesMaxSpeedScalingValue;
        public int MinibossesMaxSpeedScalingCycle;

        public bool MinibossesEnableModifiersScaling;
        public int MinibossesMaxModifiersNumber;
        public int MinibossesMaxModifiersScalingCycle;

        public bool MinibossesEnableRandomSpeedScaling;
        public int MinibossesRandomSpeedScalingStartDeath;
        public float MinibossesMinRandomSpeedScalingValue;
        public float MinibossesMaxRandomSpeedScalingValue;

        public bool MinibossesEnableRandomModifiersScaling;
        public int MinibossesRandomModifiersScalingStartDeath;
        public int MinibossesMinRandomModifiersNumber;
        public int MinibossesMaxRandomModifiersNumber;
        #endregion Bosses scaling

        #region Enemies scaling
        public bool EnemiesEnableSpeedScaling;
        public float EnemiesMinSpeedScalingValue;
        public float EnemiesMaxSpeedScalingValue;
        public int EnemiesMaxSpeedScalingCycle;

        public bool EnemiesEnableModifiersScaling;
        public int EnemiesMaxModifiersNumber;
        public int EnemiesMaxModifiersScalingCycle;

        public bool EnemiesEnableRandomSpeedScaling;
        public int EnemiesRandomSpeedScalingStartDeath;
        public float EnemiesMinRandomSpeedScalingValue;
        public float EnemiesMaxRandomSpeedScalingValue;

        public bool EnemiesEnableRandomModifiersScaling;
        public int EnemiesRandomModifiersScalingStartDeath;
        public int EnemiesMinRandomModifiersNumber;
        public int EnemiesMaxRandomModifiersNumber;
        #endregion Bosses scaling

        #endregion Scaling

        #region Modifiers
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
        public bool QiBombModifierEnabled;
        public bool ShieldBreakBombModifierEnabled;
        public bool QiOverloadBombModifierEnabled;
        public bool QiDepletionBombModifierEnabled;
        #endregion Modifiers
    }
}
