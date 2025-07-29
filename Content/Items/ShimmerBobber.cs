using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishOverhaul.Content.Items;

public class ShimmerBobber : ModItem
{
    public static void _AddBuffPatch(
        Player player,
        int type,
        int timeToAdd,
        bool quiet,
        bool foodHack,
        Projectile projectile
    )
    {
        // TODO: IMPLEMENT
    }

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.LavaFishingHook);
        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(gold: 3);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<FishOverhaulPlayer>().CanFishInShimmer = true;
    }
}
