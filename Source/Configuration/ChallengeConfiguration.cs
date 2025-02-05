using System;
using System.Collections.Generic;
using System.Text;

namespace BossChallengeMod.Configuration {
    public struct ChallengeConfiguration {
        public bool EnableRestoration;
        public bool UseSingleRecordKey;
        public int MaxCycles;

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

        public bool ModifiersEnabled;
        public bool AllowRepeatModifiers;
        public bool SpeedModifierEnabled;
        public bool TimerModifierEnabled;
        public bool ParryDirectDamageModifierEnabled;
        public bool DamageBuildupModifierEnabled;
        public bool RegenerationModifierEnabled;
        public bool KnockbackModifierEnabled;
        //public bool KnockoutModifierEnabled;
        public bool RandomArrowModifierEnabled;
        public bool RandomTalismanModifierEnabled;
        public bool EnduranceModifierEnabled;
        public bool QiShieldModifierEnabled;
        public bool TimedShieldModifierEnabled;
        public bool QiOverloadModifierEnabled;
        public bool DistanceShieldModifierEnabled;
        public bool YanlaoGunModifierEnabled;

        public int ModifiersStartFromDeath;
    }
}
