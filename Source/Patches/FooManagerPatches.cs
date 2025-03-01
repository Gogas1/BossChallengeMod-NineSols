﻿using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BossChallengeMod.Patches {

    [HarmonyPatch(typeof(FooManager))]
    public class FooManagerPatches {

        [HarmonyPatch("ExplodeWithDealer")]
        [HarmonyPostfix]
        private static void ExplodeWithDealer_Postfix() {
            try {
                if (BossChallengeMod.Instance.GlobalModifiersFlags.BlockTalismanVotes.Any()) {
                    string path = "GameCore(Clone)/RCG LifeCycle/UIManager/GameplayUICamera/UI-Canvas/[Tab] MenuTab/CursorProvider/Menu Vertical Layout/Panels/PlayerStatus Panel/Description Provider/LeftPart/PlayerStatusSelectableButton_ControlStyle";
                    GameObject talismanSelectorGO = GameObject.Find(path);

                    var selectorComp = talismanSelectorGO.GetComponent<CollectionRotateSelectorButton>();
                    if (selectorComp != null) {
                        var collection = selectorComp.collection;
                        int talismansNum = collection.AcquiredCount;
                        int variantsNum = BossChallengeMod.Random.Next(1, talismansNum);
                        for (int i = 0; i < variantsNum; i++) {
                            collection.Next();
                        }

                        selectorComp.UpdateView();
                        BossChallengeMod.Instance.UIController.UpdateTalisman(selectorComp.image.sprite);
                    }

                }

            } catch (Exception e) {
                Log.Error($"{e.Message}, {e.StackTrace}");
            }
        }
    }
}
