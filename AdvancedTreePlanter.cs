﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Oxide.Plugins
{
    [Info("Advanced Tree Planter", "shaqnic", "1.1.6")]
    [Description("Allow planting specific and protected trees. Adaption of Bazz3l's \"Tree Planter\" plugin.")]
    /*
     * Adaption of Bazz3l's "Tree Planter" plugin (https://umod.org/plugins/tree-planter)
     * Extended features:
     * - ability to plant any type of tree
     * - configure whether building permissions are required
     * - configure whether protected trees can be planted (require permission to chop)
     */
    public class AdvancedTreePlanter : RustPlugin
    {
        #region Fields

        private const string PermUse = "advancedtreeplanter.use";
        private const string PermChop = "advancedtreeplanter.chop";
        private ConfigData _config;

        #endregion


        #region Config

        private ConfigData GetDefaultConfig()
        {
            return new ConfigData
            {
                RequireBuildPermission = true,
                AllowProtectedTrees = true,
                Trees = new List<TreeConfig>
                {
                    /* TEMPERATE ENVIRONMENT */
                    new TreeConfig("Birch", "Big",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/birch_big_temp.prefab", "Temperate"),
                    new TreeConfig("Birch", "Large",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/birch_large_temp.prefab",
                        "Temperate"),
                    new TreeConfig("Birch", "Medium",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/birch_medium_temp.prefab",
                        "Temperate"),
                    new TreeConfig("Birch", "Small",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/birch_small_temp.prefab",
                        "Temperate"),
                    new TreeConfig("Birch", "Tiny",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/birch_tiny_temp.prefab", "Temperate"),
                    new TreeConfig("Douglas", "A",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/douglas_fir_a.prefab", "Temperate"),
                    new TreeConfig("Douglas", "B",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/douglas_fir_b.prefab", "Temperate"),
                    new TreeConfig("Douglas", "C",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/douglas_fir_c.prefab", "Temperate"),
                    new TreeConfig("Douglas", "D",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_small/douglas_fir_d.prefab",
                        "Temperate"),
                    new TreeConfig("Pine", "A",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/pine_a.prefab", "Temperate"),
                    new TreeConfig("Pine", "B",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/pine_b.prefab", "Temperate"),
                    new TreeConfig("Pine", "C",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/pine_c.prefab", "Temperate"),
                    new TreeConfig("Pine", "D",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_small/pine_d.prefab", "Temperate"),
                    new TreeConfig("Beech", "A",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_deciduous_large/american_beech_a.prefab",
                        "Temperate"),
                    new TreeConfig("Beech", "B",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_deciduous_large/american_beech_b.prefab",
                        "Temperate"),
                    new TreeConfig("Beech", "C",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_deciduous_large/american_beech_c.prefab",
                        "Temperate"),
                    new TreeConfig("Beech", "D",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_deciduous_small/american_beech_d.prefab",
                        "Temperate"),
                    new TreeConfig("Beech", "E",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_deciduous_small/american_beech_e.prefab",
                        "Temperate"),
                    new TreeConfig("Oak", "A",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_field_large/oak_a.prefab", "Temperate"),
                    new TreeConfig("Oak", "B",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_field_large/oak_b.prefab", "Temperate"),
                    new TreeConfig("Oak", "C",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_field_large/oak_c.prefab", "Temperate"),
                    new TreeConfig("Oak", "D",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_field_large/oak_d.prefab", "Temperate"),
                    new TreeConfig("Oak", "E",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_field_small/oak_e.prefab", "Temperate"),
                    new TreeConfig("Oak", "F",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_field_small/oak_f.prefab", "Temperate"),
                    new TreeConfig("Beech", "A-dead",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_deciduous_large/american_beech_a_dead.prefab",
                        "Temperate"),
                    new TreeConfig("Beech", "D-dead",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_field/american_beech_d_dead.prefab",
                        "Temperate"),
                    new TreeConfig("Beech", "E-dead",
                        "assets/bundled/prefabs/autospawn/resource/v2_temp_forest_deciduous_small/american_beech_e_dead.prefab",
                        "Temperate"),
                    /* ARCTIC ENVIRONMENT */
                    new TreeConfig("Douglas", "A",
                        "assets/bundled/prefabs/autospawn/resource/v2_arctic_forest/douglas_fir_a_snow.prefab",
                        "Arctic"),
                    new TreeConfig("Douglas", "B",
                        "assets/bundled/prefabs/autospawn/resource/v2_arctic_forest/douglas_fir_b_snow.prefab",
                        "Arctic"),
                    new TreeConfig("Douglas", "C",
                        "assets/bundled/prefabs/autospawn/resource/v2_arctic_forest/douglas_fir_c_snow.prefab",
                        "Arctic"),
                    new TreeConfig("Pine", "A",
                        "assets/bundled/prefabs/autospawn/resource/v2_arctic_forest_snow/pine_a_snow.prefab", "Arctic"),
                    new TreeConfig("Pine", "B",
                        "assets/bundled/prefabs/autospawn/resource/v2_arctic_forest_snow/pine_b snow.prefab", "Arctic"),
                    new TreeConfig("Pine", "C",
                        "assets/bundled/prefabs/autospawn/resource/v2_arctic_forest_snow/pine_c_snow.prefab", "Arctic"),
                    new TreeConfig("Pine", "D",
                        "assets/bundled/prefabs/autospawn/resource/v2_arctic_forest_snow/pine_d_snow.prefab", "Arctic"),
                    /* TUNDRA ENVIRONMENT */
                    new TreeConfig("Birch", "Big",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_forest/birch_big_tundra.prefab", "Tundra"),
                    new TreeConfig("Birch", "Large",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_forest/birch_large_tundra.prefab",
                        "Tundra"),
                    new TreeConfig("Birch", "Medium",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_forest/birch_medium_tundra.prefab",
                        "Tundra"),
                    new TreeConfig("Birch", "Small",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_forest/birch_small_tundra.prefab",
                        "Tundra"),
                    new TreeConfig("Birch", "Tiny",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_forest/birch_tiny_tundra.prefab",
                        "Tundra"),
                    new TreeConfig("Oak", "A",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_forest/oak_a_tundra.prefab", "Tundra"),
                    new TreeConfig("Oak", "B",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_forest/oak_b_tundra.prefab", "Tundra"),
                    new TreeConfig("Oak", "F",
                        "assets/bundled/prefabs/autospawn/resource/v2_tundra_forest_small/oak_f_tundra.prefab",
                        "Tundra"),
                    /* SWAMP ENVIRONMENT */
                    new TreeConfig("Tree", "A",
                        "assets/bundled/prefabs/autospawn/resource/swamp-trees/swamp_tree_a.prefab", "Swamp"),
                    new TreeConfig("Tree", "B",
                        "assets/bundled/prefabs/autospawn/resource/swamp-trees/swamp_tree_b.prefab", "Swamp"),
                    new TreeConfig("Tree", "C",
                        "assets/bundled/prefabs/autospawn/resource/swamp-trees/swamp_tree_c.prefab", "Swamp"),
                    new TreeConfig("Tree", "D",
                        "assets/bundled/prefabs/autospawn/resource/swamp-trees/swamp_tree_d.prefab", "Swamp"),
                    new TreeConfig("Tree", "E",
                        "assets/bundled/prefabs/autospawn/resource/swamp-trees/swamp_tree_e.prefab", "Swamp"),
                    new TreeConfig("Tree", "F",
                        "assets/bundled/prefabs/autospawn/resource/swamp-trees/swamp_tree_f.prefab", "Swamp"),
                    /* ARID ENVIRONMENT */
                    new TreeConfig("Palm", "Med-a",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_forest/palm_tree_med_a_entity.prefab",
                        "Arid"),
                    new TreeConfig("Palm", "Med-b",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_forest/palm_tree_med_b_entity.prefab",
                        "Arid"),
                    new TreeConfig("Palm", "Small-a",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_forest/palm_tree_small_a_entity.prefab",
                        "Arid"),
                    new TreeConfig("Palm", "Small-b",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_forest/palm_tree_small_b_entity.prefab",
                        "Arid"),
                    new TreeConfig("Palm", "Small-c",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_forest/palm_tree_small_c_entity.prefab",
                        "Arid"),
                    new TreeConfig("Palm", "Tall-a",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_forest/palm_tree_tall_a_entity.prefab",
                        "Arid"),
                    new TreeConfig("Palm", "Tall-b",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_forest/palm_tree_tall_b_entity.prefab",
                        "Arid"),
                    new TreeConfig("Cactus", "1",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_cactus/cactus-1.prefab", "Arid"),
                    new TreeConfig("Cactus", "2",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_cactus/cactus-2.prefab", "Arid"),
                    new TreeConfig("Cactus", "3",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_cactus/cactus-3.prefab", "Arid"),
                    new TreeConfig("Cactus", "4",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_cactus/cactus-4.prefab", "Arid"),
                    new TreeConfig("Cactus", "5",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_cactus/cactus-5.prefab", "Arid"),
                    new TreeConfig("Cactus", "6",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_cactus/cactus-6.prefab", "Arid"),
                    new TreeConfig("Cactus", "7",
                        "assets/bundled/prefabs/autospawn/resource/v2_arid_cactus/cactus-7.prefab", "Arid")
                }
            };
        }

        public class ConfigData
        {
            public bool AllowProtectedTrees;
            public bool RequireBuildPermission;
            public List<TreeConfig> Trees;
        }

        public class TreeConfig
        {
            public string Env;
            public int PlantCost;
            public string Prefab;
            public int ProtCost;
            public string Type;
            public string Variant;

            public TreeConfig(string type, string variant, string prefab, string env, int plantCost = 10,
                int protCost = 20)
            {
                Type = type;
                Variant = variant;
                Prefab = prefab;
                Env = env;
                PlantCost = plantCost;
                ProtCost = protCost;
            }
        }

        #endregion


        #region Oxide

        protected override void LoadDefaultConfig()
        {
            Config.WriteObject(GetDefaultConfig(), true);
        }

        protected override void LoadDefaultMessages()
        {
            lang.RegisterMessages(new Dictionary<string, string>
            {
                {"NoPermission", "No permission."},
                {"Balance", "You cannot afford this ({0} Scrap required)."},
                {"Given", "You received a sapling for '{0}'."},
                {"MissingAuth", "You must have building privilege."},
                {"Planter", "Cannot be placed in a planter."},
                {"Planted", "You planted a tree."},
                {"Error", "Something went wrong."},
                {"Invalid", "Invalid type."},
                {"CmdTree", "Get saplings for tree type:"},
                {"CmdTreeProt", "Get saplings for tree type (protected from chopping):"},
                {"CmdTreeList", "List of trees for selected environment:"},
                {"EnvList", "List of available environments:"},
                {"EnvTreeList", "List of trees for environment '{0}':"},
                {"EnvNotFound", "Environment '{0}' is not available."},
                {"VariantMissing", "No variant for this tree type was chosen."},
                {"AvailableVariants", "List of available variants"},
                {"InvalidTree", "Tree type '{0}' is not valid for environment '{1}'."},
                {"InvalidVariant", "Tree variant '{0}' is not valid for tree '{1}' and environment '{2}'."},
                {"InvalidAmount", "Amount must be a number greater than 0."},
                {"GetSapling", "You get {0}x sapling of '{1}'."},
                {"ProtectedTree", "This tree seems to be protected."},
                {"ProtectionDisabled", "Tree protection is not enabled."},
                {"BracketExplanation", "In brackets"},
                {"PricePerSapling", "price per sapling"},
                {"PricePerProtSapling", "price per protected sapling"}
            }, this);
        }

        private void OnServerInitialized()
        {
            permission.RegisterPermission(PermUse, this);
            permission.RegisterPermission(PermChop, this);
        }

        private void Init()
        {
            _config = Config.ReadObject<ConfigData>();
        }

        private object OnMeleeAttack(BasePlayer player, HitInfo info)
        {
            var ent = info?.HitEntity;

            if (ent == null || !IsTree(ent.ShortPrefabName)) return null;

            if (_config.AllowProtectedTrees && ent.OwnerID != 0UL && ent.OwnerID != player.userID &&
                !permission.UserHasPermission(player.UserIDString, PermChop))
            {
                info.damageTypes.ScaleAll(0.0f);

                player.ChatMessage(Lang("ProtectedTree", player.UserIDString));

                return false;
            }

            return null;
        }

        private void OnEntityBuilt(Planner plan, GameObject seed)
        {
            var player = plan.GetOwnerPlayer();
            if (player == null || !permission.UserHasPermission(player.UserIDString, PermUse)) return;

            var plant = seed.GetComponent<GrowableEntity>();
            if (plant == null) return;

            var item = player.GetActiveItem();
            if (item == null) return;

            var pattern = @"([a-zA-Z]*)\s([a-zA-Z]*),\sVariant\s([a-zA-Z0-9\-]*)";
            var attributes = Regex.Match(item.name, pattern).Groups;

            var tree = _config.Trees.FirstOrDefault(tre =>
                tre.Env == attributes[1].Value && tre.Type == attributes[2].Value &&
                tre.Variant == attributes[3].Value);
            if (tree == null) return;

            var prot = item.name.Contains("protected") ? true : false;

            NextTick(() =>
            {
                if (plant.GetParentEntity() is PlanterBox)
                {
                    RefundItem(player, item.name);

                    plant?.Kill();

                    player.ChatMessage(Lang("Planter", player.UserIDString));
                    return;
                }

                if (_config.RequireBuildPermission && !player.IsBuildingAuthed())
                {
                    RefundItem(player, item.name);

                    plant?.Kill();

                    player.ChatMessage(Lang("MissingAuth", player.UserIDString));
                    return;
                }

                PlantTree(player, plant, tree.Prefab, prot);
            });
        }

        [ChatCommand("tree")]
        private void TreeCommand(BasePlayer player, string cmd, string[] args)
        {
            if (!permission.UserHasPermission(player.UserIDString, PermUse))
            {
                player.ChatMessage(Lang("NoPermission", player.UserIDString));
                return;
            }

            var sb = new StringBuilder();
            sb.Append("<color=#6699ff>Advanced Tree Planter</color>\n\n");

            if (args.Length == 0 || args.Length > 5)
            {
                sb.Append("<color=#66ff99>" + Lang("CmdTree", player.UserIDString) + "</color>\n");
                sb.Append("/tree <environment> <tree> <variant> <color=#cccccc><amount></color>\n\n");
                if (_config.AllowProtectedTrees)
                {
                    sb.Append("<color=#66ff99>" + Lang("CmdTreeProt", player.UserIDString) + "</color>\n");
                    sb.Append("/tree <environment> <tree> <variant> <amount> prot\n\n");
                }

                sb.Append("<color=#66ff99>" + Lang("CmdTreeList", player.UserIDString) + "</color>\n");
                sb.Append("/tree <environment>\n\n");
                sb.Append("<color=#66ff99>" + Lang("EnvList", player.UserIDString) + "</color>\n");
                sb.Append(string.Join(", ", GetAvailableEnvironments()));

                player.ChatMessage(sb.ToString());
                return;
            }

            var environment = GetAvailableEnvironments().FirstOrDefault(env =>
                string.Equals(env, args[0], StringComparison.InvariantCultureIgnoreCase));
            if (environment == null)
            {
                sb.Append($"/tree <color=#ff3333>{args[0]}</color>\n\n");
                sb.Append("<color=#ff3333>" + Lang("EnvNotFound", player.UserIDString, args[0]) + "</color>");

                player.ChatMessage(sb.ToString());
                return;
            }

            if (args.Length == 1)
            {
                sb.Append($"/tree {environment} <tree> <variant> <color=#cccccc><amount></color>\n\n");
                sb.Append("<color=#66ff99>" + Lang("EnvTreeList", player.UserIDString, environment) + "</color>\n");
                var envTrees = new List<string>();
                foreach (var envTree in GetTreesForEnvironment(environment))
                {
                    var sbTree = new StringBuilder();
                    sbTree.Append(envTree.Type + " " + envTree.Variant + " <color=#cccccc>(");
                    sbTree.Append(envTree.PlantCost);
                    if (_config.AllowProtectedTrees)
                        sbTree.Append("/" + (envTree.PlantCost + envTree.ProtCost));
                    sbTree.Append(")</color>");
                    envTrees.Add(sbTree.ToString());
                }

                sb.Append(string.Join(", ", envTrees) + "\n\n");
                sb.Append("<color=#dddddd>" + Lang("BracketExplanation", player.UserIDString) + ": " +
                          Lang("PricePerSapling", player.UserIDString));
                if (_config.AllowProtectedTrees)
                    sb.Append(" / " + Lang("PricePerProtSapling", player.UserIDString));
                sb.Append("</color>");

                player.ChatMessage(sb.ToString());
            }
            else
            {
                if (!IsValidTree(args[0], args[1]))
                {
                    var args2 = args.Length > 2 ? args[2] : "<variant>";
                    var args3 = args.Length > 3 ? args[3] : "<amount>";
                    sb.Append(
                        $"/tree {args[0]} <color=#ff3333>{args[1]}</color> {args2} <color=#cccccc>{args3}</color>\n\n");
                    sb.Append($"<color=#ff3333>{Lang("InvalidTree", player.UserIDString, args[1], args[0])}</color>");
                    player.ChatMessage(sb.ToString());
                    return;
                }

                var treeVariants = GetTreeVariantsFromArray(GetTreesForEnvironment(environment).Where(tree =>
                    string.Equals(tree.Type, args[1], StringComparison.InvariantCultureIgnoreCase)).ToArray());

                if (args.Length == 2)
                {
                    sb.Append(
                        $"/tree {args[0]} {args[1]} <color=#ff3333><variant></color> <color=#cccccc><amount></color>\n\n");
                    sb.Append($"<color=#ff3333>{Lang("VariantMissing", player.UserIDString)}</color>\n\n");
                    sb.Append($"<color=#66ff99>{Lang("AvailableVariants", player.UserIDString)}:</color>\n");
                    var variants = new List<string>();
                    foreach (var envTree in GetTreesForEnvironment(environment))
                        if (string.Equals(envTree.Type, args[1], StringComparison.InvariantCultureIgnoreCase))
                        {
                            var sbVariant = new StringBuilder();
                            sbVariant.Append(envTree.Variant + " <color=#cccccc>(");
                            sbVariant.Append(envTree.PlantCost);
                            if (_config.AllowProtectedTrees)
                                sbVariant.Append("/" + (envTree.PlantCost + envTree.ProtCost));
                            sbVariant.Append(")</color>");
                            variants.Add(sbVariant.ToString());
                        }

                    sb.Append(string.Join(", ", variants) + "\n\n");
                    sb.Append("<color=#dddddd>" + Lang("BracketExplanation", player.UserIDString) + ": " +
                              Lang("PricePerSapling", player.UserIDString));
                    if (_config.AllowProtectedTrees)
                        sb.Append(" / " + Lang("PricePerProtSapling", player.UserIDString));
                    sb.Append("</color>");

                    player.ChatMessage(sb.ToString());
                }
                else
                {
                    if (!IsValidVariant(args[0], args[1], args[2]))
                    {
                        var args3 = args.Length > 3 ? args[3] : "<amount>";
                        sb.Append(
                            $"/tree {args[0]} {args[1]} <color=#ff3333>{args[2]}</color> <color=#cccccc>{args3}</color>\n\n");
                        sb.Append(
                            $"<color=#ff3333>{Lang("InvalidVariant", player.UserIDString, args[2], args[1], args[0])}</color>\n\n");
                        sb.Append($"<color=#66ff99>{Lang("AvailableVariants", player.UserIDString)}:</color>\n");
                        sb.Append(string.Join(", ", treeVariants));
                        player.ChatMessage(sb.ToString());
                        return;
                    }

                    var amount = 1;
                    if (args.Length > 3)
                    {
                        var isNumeric = int.TryParse(args[3], out amount);
                        if (!isNumeric || amount < 1)
                        {
                            sb.Append($"/tree {args[0]} {args[1]} {args[2]} <color=#ff3333>{args[3]}</color>\n\n");
                            sb.Append($"<color=#ff3333>{Lang("InvalidAmount", player.UserIDString)}</color>");
                            player.ChatMessage(sb.ToString());
                            return;
                        }
                    }

                    var prot = false;
                    if (args.Length > 4)
                    {
                        if (!_config.AllowProtectedTrees)
                        {
                            sb.Append("<color=#ff3333>" + Lang("ProtectionDisabled", player.UserIDString) + "</color>");
                            player.ChatMessage(sb.ToString());
                            return;
                        }

                        prot = args[4].ToLower() == "prot" ? true : false;
                    }

                    var nameBuilder = new StringBuilder();
                    nameBuilder.Append(
                        $"{UniFormat(args[0])} {UniFormat(args[1])}, Variant {UniFormat(args[2])}");
                    if (prot)
                        nameBuilder.Append(" (protected)");
                    var saplingName = nameBuilder.ToString();

                    var tree = _config.Trees.FirstOrDefault(tre =>
                        string.Equals(tre.Env, args[0], StringComparison.InvariantCultureIgnoreCase) &&
                        string.Equals(tre.Type, args[1], StringComparison.InvariantCultureIgnoreCase) &&
                        string.Equals(tre.Variant, args[2], StringComparison.InvariantCultureIgnoreCase));

                    if (tree == null)
                    {
                        player.ChatMessage(Lang("Invalid", player.UserIDString));
                        return;
                    }

                    var cost = tree.PlantCost * amount;
                    if (prot)
                        cost += tree.ProtCost * amount;

                    if (!CheckBalance(player, cost))
                    {
                        player.ChatMessage($"<color=#ff3333>{Lang("Balance", player.UserIDString, cost)}</color>");
                        return;
                    }

                    var item = CreateItem(saplingName, amount);
                    if (item == null)
                    {
                        player.ChatMessage(Lang("Error", player.UserIDString));
                        return;
                    }

                    BalanceTake(player, cost);

                    player.GiveItem(item);

                    sb.Append($"{Lang("GetSapling", player.UserIDString, amount, saplingName)}");

                    player.ChatMessage(sb.ToString());
                }
            }
        }

        #endregion


        #region Core

        private void PlantTree(BasePlayer player, GrowableEntity plant, string prefabName, bool prot = false)
        {
            var entity = GameManager.server.CreateEntity(prefabName, plant.transform.position, Quaternion.identity);
            if (entity == null) return;

            entity.OwnerID = prot ? player.userID : 0;
            entity.Spawn();

            plant?.Kill();

            player.ChatMessage(Lang("Planted", player.UserIDString));
        }

        private bool CheckBalance(BasePlayer player, int cost)
        {
            if (player.inventory.GetAmount(-932201673) >= cost) return true;

            return false;
        }

        private void BalanceTake(BasePlayer player, int cost)
        {
            player.inventory.Take(new List<Item>(), -932201673, cost);
        }

        private Item CreateItem(string treeType, int treeAmount = 1)
        {
            var item = ItemManager.CreateByName("clone.hemp", treeAmount);
            item.name = treeType;
            item.info.stackable = 1;
            return item;
        }

        private void RefundItem(BasePlayer player, string treeType)
        {
            var refundItem = CreateItem(treeType);

            if (refundItem == null)
            {
                player.ChatMessage(Lang("Error", player.UserIDString));
                return;
            }

            player.GiveItem(refundItem);
        }

        private bool IsTree(string prefab)
        {
            if (prefab.Contains("oak_")
                || prefab.Contains("birch_")
                || prefab.Contains("douglas_")
                || prefab.Contains("beech_")
                || prefab.Contains("swamp_")
                || prefab.Contains("palm_")
                || prefab.Contains("pine_")
                || prefab.Contains("cactus-"))
                return true;

            return false;
        }

        #endregion


        #region Helpers

        private string Lang(string key, string id = null, params object[] args)
        {
            return string.Format(lang.GetMessage(key, this, id), args);
        }

        private List<string> GetAvailableEnvironments()
        {
            var environments = new List<string>();
            foreach (var tree in _config.Trees)
                if (environments.FirstOrDefault(env =>
                    string.Equals(env, tree.Env, StringComparison.InvariantCultureIgnoreCase)) == null)
                    environments.Add(tree.Env);

            return environments;
        }

        private TreeConfig[] GetTreesForEnvironment(string environment)
        {
            var trees = new List<TreeConfig>();
            foreach (var tree in _config.Trees.Where(tree =>
                string.Equals(tree.Env, environment, StringComparison.InvariantCultureIgnoreCase))) trees.Add(tree);

            return trees.ToArray();
        }

        private List<string> GetTreeNamesFromArray(TreeConfig[] treeArray)
        {
            var treeNames = new List<string>();
            foreach (var tree in treeArray) treeNames.Add(tree.Type + " " + tree.Variant);

            return treeNames;
        }

        private List<string> GetTreeVariantsFromArray(TreeConfig[] treeArray)
        {
            var variants = new List<string>();
            foreach (var tree in treeArray) variants.Add(tree.Variant);

            return variants;
        }

        private bool IsValidEnvironment(string environment)
        {
            if (GetAvailableEnvironments().FirstOrDefault(env =>
                string.Equals(env, environment, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;

            return true;
        }

        private bool IsValidTree(string environment, string tree)
        {
            if (!IsValidEnvironment(environment))
                return false;

            if (GetTreesForEnvironment(environment).FirstOrDefault(tre =>
                string.Equals(tre.Type, tree, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;

            return true;
        }

        private bool IsValidVariant(string environment, string tree, string variant)
        {
            if (!IsValidEnvironment(environment))
                return false;

            if (!IsValidTree(environment, tree))
                return false;

            if (GetTreesForEnvironment(environment).FirstOrDefault(tre =>
                string.Equals(tre.Type, tree, StringComparison.InvariantCultureIgnoreCase) &&
                string.Equals(tre.Variant, variant, StringComparison.InvariantCultureIgnoreCase)) == null)
                return false;

            return true;
        }

        private string UniFormat(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
                default: return input.First().ToString().ToUpper() + input.Substring(1).ToLower();
            }
        }

        #endregion
    }
}