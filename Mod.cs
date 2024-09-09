﻿using System;
using System.Linq;
using System.Reflection;
using Unity.Entities;
using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.Prefabs;
using Game.Economy;
using Game.Common;
using Game.Input;
using HarmonyLib;
using UnityEngine;

namespace InfoLoom_Deluxe
{
    public class Mod : IMod
    {
        public static readonly string HarmonyId = "Infixo." + nameof(InfoLoom_Deluxe);

        // Mod's instance and asset
        public static Mod Instance { get; private set; }
        public static ExecutableAsset ModAsset { get; private set; }
        
        // Logging
        public static ILog Log = LogManager.GetLogger($"{nameof(InfoLoom_Deluxe)}").SetShowsErrorsInUI(false);
        
        // Setting
        public static Setting Setting { get; private set; }

        // New properties from CS2_InfoLoom_Deluxe
        public static ProxyAction ButtonAction { get; private set; }
        public static ProxyAction AxisAction { get; private set; }
        public static ProxyAction VectorAction { get; private set; }

        public const string ButtonActionName = "ButtonBinding";
        public const string AxisActionName = "FloatBinding";
        public const string VectorActionName = "Vector2Binding";

        public void OnLoad(UpdateSystem updateSystem)
        {
            Log.Info(nameof(OnLoad));
            Instance = this;

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
            {
                Log.Info($"Current mod asset at {asset.path}");
                ModAsset = asset;
            }

            InitializeSetting();
            InitializeLocalization();
            ApplyHarmonyPatches();
            RegisterSystems(updateSystem);
            SetupInputActions();
        }

        private void InitializeSetting()
        {
            Setting = new Setting(this);
            Setting.RegisterInOptionsUI();
            Setting._Hidden = false;
            AssetDatabase.global.LoadSettings(nameof(InfoLoom_Deluxe), Setting, new Setting(this));
        }

        private void InitializeLocalization()
        {
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(Setting));
        }

        private void ApplyHarmonyPatches()
        {
            var harmony = new Harmony(HarmonyId);
            harmony.PatchAll(typeof(Mod).Assembly);
            var patchedMethods = harmony.GetPatchedMethods().ToArray();
            Log.Info($"Plugin {HarmonyId} made patches! Patched methods: {patchedMethods.Length}");
            foreach (var patchedMethod in patchedMethods)
            {
                Log.Info($"Patched method: {patchedMethod.Module.Name}:{patchedMethod.DeclaringType.Name}.{patchedMethod.Name}");
            }
        }

        private void RegisterSystems(UpdateSystem updateSystem)
        {
            updateSystem.UpdateAt<Systems.BuildingDemandUISystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<Systems.PopulationStructureUISystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<Systems.WorkplacesInfoLoomUISystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<Systems.WorkforceInfoLoomUISystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<Systems.CommercialDemandUISystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<Systems.ResidentialDemandUISystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<Systems.IndustrialDemandUISystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<Systems.TradeInfoSystem>(SystemUpdatePhase.UIUpdate);
            updateSystem.UpdateAt<Systems.SeparateConsumptionSystem>(SystemUpdatePhase.UIUpdate);
        }

        private void SetupInputActions()
        {
            Setting.RegisterKeyBindings();

            ButtonAction = Setting.GetAction(ButtonActionName);
            AxisAction = Setting.GetAction(AxisActionName);
            VectorAction = Setting.GetAction(VectorActionName);

            ButtonAction.shouldBeEnabled = true;
            AxisAction.shouldBeEnabled = true;
            VectorAction.shouldBeEnabled = true;

            ButtonAction.onInteraction += (_, phase) => Log.Info($"[{ButtonAction.name}] On{phase} {ButtonAction.ReadValue<float>()}");
            AxisAction.onInteraction += (_, phase) => Log.Info($"[{AxisAction.name}] On{phase} {AxisAction.ReadValue<float>()}");
            VectorAction.onInteraction += (_, phase) => Log.Info($"[{VectorAction.name}] On{phase} {VectorAction.ReadValue<Vector2>()}");
        }

        public void OnDispose()
        {
            Log.Info(nameof(OnDispose));
            if (Setting != null)
            {
                Setting.UnregisterInOptionsUI();
                Setting = null;
            }

            var harmony = new Harmony(HarmonyId);
            harmony.UnpatchAll(HarmonyId);

            Instance = null;
        }
    }
}
