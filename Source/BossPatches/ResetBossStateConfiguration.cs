﻿using BossChallengeMod.CustomMonsterStates;
using BossChallengeMod.CustomMonsterStates.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.BossPatches {
    public class ResetBossStateConfiguration : StateConfiguration {
        public List<string> Animations { get; set; } = new();
        public List<string> TargetDamageReceivers { get; set; } = new();
        public float PauseTime { get; set; } = 0f;
        public bool UseFlashing { get; set; } = false;
        public MonsterBase.States ExitState;
        public MonsterBase.States StateType;

        public Action? StateEnterEvents;
        public Action? StateExitEvents;

        public override void ConfigureComponent(Component component) {
            if (component is ResetBossState state) {
                if (ExitState != default)
                    state.exitState = ExitState;

                if (StateType != default)
                    state.StateType = StateType;

                if (TargetDamageReceivers.Any())
                    state.TargetDamageReceivers = TargetDamageReceivers.ToArray();

                if (Animations.Any())
                    state.Animations = Animations.ToArray();

                if (StateEnterEvents != null)
                    state.stateEvents.StateEnterEvent.AddListener(() => StateEnterEvents.Invoke());

                if (StateExitEvents != null)
                    state.stateEvents.StateExitEvent.AddListener(() => StateExitEvents.Invoke());

                state.PauseTime = PauseTime;
                state.UseFlashing = UseFlashing;
            }
        }
    }
}
