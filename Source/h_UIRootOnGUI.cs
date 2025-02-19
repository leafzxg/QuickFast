﻿using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace QuickFast
{
    [HarmonyPatch(typeof(UIRoot_Play))]
    [HarmonyPatch(nameof(UIRoot_Play.UIRootUpdate))]
    public static class h_UIRootOnGUI
    {
        public static void Postfix()
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                foreach (var pawn in Find.Selector.SelectedObjects.OfType<Pawn>())
                {
                    if (Settings.hairfilter.Contains(pawn.story.hairDef))
                    {
                        Settings.hairfilter.Remove(pawn.story.hairDef);
                        Messages.Message("Hair_Filter_Remove".Translate(pawn.story.hairDef.defName),
                            MessageTypeDefOf.NeutralEvent);
                    }
                    else
                    {
                        Settings.hairfilter.Add(pawn.story.hairDef);
                        Messages.Message("Hair_Filter_Add".Translate(pawn.story.hairDef.defName),
                            MessageTypeDefOf.NeutralEvent);
                    }
                    pawn.apparel.Notify_ApparelChanged();
                }

                Settings.DefToStrings = new List<string>();
                foreach (var s in Settings.hairfilter)
                {
                    Settings.DefToStrings.Add(s.defName);
                }

                DubsApparelTweaks.Settings.Write();
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                foreach (var pawn in Find.Selector.SelectedObjects.OfType<Pawn>())
                {
                    var hat = pawn.apparel.WornApparel.FirstOrDefault(x =>
                        x.def.apparel.layers.Any(z => z == bs.Overhead));
                    if (hat == null)
                    {
                        return;
                    }

                    if (Settings.hatfilter.Contains(hat.def))
                    {
                        Settings.hatfilter.Remove(hat.def);
                        Messages.Message("Hat_Filter_Remove".Translate(hat.def.defName), MessageTypeDefOf.NeutralEvent);
                    }
                    else
                    {
                        Settings.hatfilter.Add(hat.def);
                        Messages.Message("Hat_Filter_Add".Translate(hat.def.defName), MessageTypeDefOf.NeutralEvent);
                    }
                }

                Settings.HatDefToStrings = new List<string>();
                foreach (var s in Settings.hatfilter)
                {
                    Settings.HatDefToStrings.Add(s.defName);
                }


                DubsApparelTweaks.Settings.Write();
            }
        }
    }
}