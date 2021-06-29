using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using Dumpostor.Dumpers;
using HarmonyLib;
using Reactor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Dumpostor
{
    [BepInPlugin(Id)]
    [BepInProcess("Among Us.exe")]
    [BepInDependency(ReactorPlugin.Id)]
    public class DumpostorPlugin : BasePlugin
    {
        public const string Id = "pl.js6pak.dumpostor";

        public Harmony Harmony { get; } = new Harmony(Id);

        public ConfigEntry<string> DumpedPath { get; private set; }

        public override void Load()
        {
            DumpedPath = Config.Bind("Settings", "DumpedPath", string.Empty);

            Harmony.PatchAll();

            if (string.IsNullOrEmpty(DumpedPath.Value) || !Directory.Exists(DumpedPath.Value))
            {
                Log.LogError("DumpedPath is empty or is not a directory");
                Application.Quit();
            }

            EntrypointPatch.Initialized += () =>
            {
                var dumpers = new IDumper[]
                {
                    new EnumDumper<StringNames>(),
                    new EnumDumper<SystemTypes>(),
                    new EnumDumper<TaskTypes>(),

                    new TaskDumper()
                };

                foreach (var dumper in dumpers)
                {
                    var dump = dumper.Dump();
                    if (dump != null)
                    {
                        File.WriteAllText(Path.Combine(DumpedPath.Value, dumper.FileName + ".json"), dump);
                    }
                }

                foreach (var mapType in (MapTypes[]) Enum.GetValues(typeof(MapTypes)))
                {
                    var shipPrefab = AmongUsClient.Instance.ShipPrefabs[(int) mapType];
                    Coroutines.Start(DumpCoroutine(mapType, shipPrefab, dumpers.OfType<IMapDumper>()));
                }
            };
        }

        public IEnumerator DumpCoroutine(MapTypes mapType, AssetReference assetReference, IEnumerable<IMapDumper> dumpers)
        {
            var shipPrefab = assetReference.Instantiate();

            while (!shipPrefab.IsDone)
            {
                yield return null;
            }

            var shipStatus = shipPrefab.Result.GetComponent<ShipStatus>();

            var directory = Path.Combine(DumpedPath.Value, mapType.ToString());
            Directory.CreateDirectory(directory);

            foreach (var dumper in dumpers)
            {
                File.WriteAllText(Path.Combine(directory, dumper.FileName + ".json"), dumper.Dump(mapType, shipStatus));
            }

            assetReference.ReleaseInstance(shipPrefab.Result);
        }

        [HarmonyPatch(typeof(AmongUsClient), nameof(AmongUsClient.Awake))]
        private static class EntrypointPatch
        {
            public static event Action Initialized;

            public static void Postfix(AmongUsClient __instance)
            {
                if (!AmongUsClient.Instance.Equals(__instance))
                {
                    return;
                }

                Initialized?.Invoke();
            }
        }
    }
}
