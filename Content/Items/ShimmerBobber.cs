using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace FishOverhaul.Items
{
    public class ShimmerBobber : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.accessory = true;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 3);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<FishOverhaulPlayer>().canFishInShimmer = true;
        }
    }
}
