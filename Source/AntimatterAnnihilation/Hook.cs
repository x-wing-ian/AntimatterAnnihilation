﻿using System;
using System.IO;
using AntimatterAnnihilation.Utils;
using InGameWiki;
using UnityEngine;
using Verse;
using Object = UnityEngine.Object;

namespace AntimatterAnnihilation
{
    public class Hook : MonoBehaviour
    {
        public static Hook Instance { get; private set; }

        private ModWiki wiki;

        private void Awake()
        {
            Instance = this;
        }

        private void OnGUI()
        {
            try
            {
                GUILayout.Space(120);
                bool spawn = GUILayout.Button("Spawn");
                bool openWiki = GUILayout.Button("Show wiki window");

                if (spawn)
                {
                    Vector3 pos = new Vector3(15, 0, 15);
                    Log.Message("Spawning at " + pos);

                    var beam = Object.Instantiate(Content.EnergyBeamInPrefab).transform;
                    beam.transform.position = pos;
                    beam.localScale = Vector3.one;
                    beam.eulerAngles = new Vector3(90f, 90f, 0f);
                }

                if (openWiki)
                {
                    if (wiki == null)
                    {
                        wiki = new ModWiki();
                        wiki.ModTitle = "Antimatter Annihilation";
                        wiki.GenerateFromMod(ModCore.Instance);
                    }

                    WikiWindow.Open(this.wiki);
                }
            }
            catch (Exception e)
            {
                Log.Error($"GUI draw error: {e}");
            }
        }
    }
}
