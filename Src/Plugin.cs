using BepInEx;
using BepInEx.Logging;

using System;
using System.IO;
using System.Reflection;
using System.Linq;

using UnityEngine;


namespace OpalTools
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        AssetBundle _assetBundle;

        public static Plugin Instance;

        public InventoryItem Inv_OpalShovel;
        bool bDisabled;

        private void Awake()
        {
            // Plugin startup logic
            Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        void Start()
        {
            //BundleLoader();
            LoadAssetBundle("octr_opaltools");
            BuildItem();
        }

        void Update()
        {
            ArrayBuilder();
        }

        public void CreateLog(string msg)
        {
            Logger.LogInfo(msg);
        }

        

        void BuildItem()
        {
            var prefab = _assetBundle.LoadAsset<GameObject>("Inv_OpalShovel");
            Instantiate(prefab);
            Inv_OpalShovel = prefab.GetComponent<InventoryItem>();
            _assetBundle.Unload(false);
            Logger.LogInfo("Asset Bundle Loaded: " + prefab.name);
        }

        private Stream GetEmbeddedAssetBundle(string name)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var presumed = assembly.GetManifestResourceNames().ToList().Find(resource => resource.Contains(name));

            if (!string.IsNullOrEmpty(presumed)) return assembly.GetManifestResourceStream(presumed);

            Logger.LogError($"Unable to find any embedded resource with name: {name}.");
            return null;
        }

        private void LoadAssetBundle(string name)
        {
            var resource = GetEmbeddedAssetBundle(name);
            _assetBundle = AssetBundle.LoadFromStream(resource);

            if (_assetBundle != null) return;

            Logger.LogError("Unable to load embedded asset bundle.");
            return;
        }

        private void UnloadAssetBundle()
        {
            _assetBundle.Unload(true);
        }

        void ArrayBuilder()
        {
            if (Inventory.inv.allItems == null)
            {
                CreateLog("Couldn't find allItems array.");
                return;
            }

            if (bDisabled)
            {
                return;
            }
            else
            {
                Array.Resize<InventoryItem>(ref Inventory.inv.allItems, Inventory.inv.allItems.Length + 1);
                Inventory.inv.allItems[905] = Inv_OpalShovel;
                CreateLog("Inserted Inv_OpalShovel");
                CreateLog("allItems Array Resized " + Inventory.inv.allItems.Length);
                bDisabled = true;

            }
        }

    }
}
