
# About

This plugin is an adaption of Bazz3l's Tree Planter plugin for Rust (https://umod.org/plugins/tree-planter) and includes some extended features like:

 - give the ability to plant any type of tree
 - give the ability to gather saplings from chopped trees (require permission to gather)
 - configure whether building permissions are required
 - configure whether protected trees can be planted (require permission to chop)

# Permissions

This plugin uses Oxide's permission system.

 - To assign a permission, use `oxide.grant <user or group> <name or steam id> <permission>`.
 - To revoke a permission, use `oxide.revoke <user or group> <name or steam id> <permission>`.

Permissions that are used:
- `advancedtreeplanter.buysapling`
- `advancedtreeplanter.gathersapling`
- `advancedtreeplanter.plantsapling`
- `advancedtreeplanter.chopprotected`

# Currency

In contrast to the original Tree Planter plugin by Bazz3l, the Economics plugin is currently not supported. Instead scrap in the player's inventory is used as currency.

# Commands

 - `/tree <environment> <type> <variant> <optional: amount>` - get a sapling for the selected tree
 - `/tree <environment> <type> <variant> <amount> prot` - get a sapling for the selected tree, which will be protected against chopping
 - `/tree <environment>` - list all available trees and variants for the selected environment

# Configuration

The settings and options can be configured in the `AdvancedTreePlanter` file under the `config` directory. The use of a JSON editor or validation site such as [jsonlint.com](https://jsonlint.com/) is recommended to avoid formatting issues and syntax errors.

    {
      "AllowProtectedTrees": true,
      "GatherSaplingChance": 0.33,
      "MaxSaplingGather": 2,
      "MinSaplingGather": 1,
      "RandomizeSaplingGather": true,
      "RequireBuildPermission": true,
      "Trees": [
        {
          "Env": "Temperate",
          "PlantCost": 10,
          "Prefab": "assets/bundled/prefabs/autospawn/resource/v2_temp_forest/birch_big_temp.prefab",
          "ProtCost": 20,
          "Type": "Birch",
          "Variant": "Big"
        }
      ]
    }

## Configuration explanations

- `AllowProtectedTrees` - trees protected from chopping can be placed, if enabled; if disabled: no tree is longer protected until option is re-enabled
- `GatherSaplingChance` - the chance to get a sapling, when chopping a tree
- `MaxSaplingGather` - maximum amount of saplings a player can get when chopping a tree
- `MinSaplingGather` - minimum amount of saplings a player can get when chopping a tree
- `RandomizeSaplingGather` - saplings gathered from chopping will be for a random variant of type (e.g. oak a, oak b, ...), if enabled
- `RequireBuildPermission` - player must have building privilege for planting trees, if enabled