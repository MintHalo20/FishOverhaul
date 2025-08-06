using FishOverhaul.Content.ShimmeredDuke;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using YourMod.Utilities; // Replace with your actual mod's namespace

namespace CalamityMod.Items.TreasureBags
{
    public class ShimmeredDukeLootbag : ModItem, ILocalizedModType
    {
        public new string LocalizationCategory => "Items.TreasureBags";

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 3;
            ItemID.Sets.BossBag[Item.type] = true;

            // Optional glow and animation
            ItemID.Sets.ItemIconPulse[Item.type] = true;
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 24;
            Item.maxStack = 9999;
            Item.consumable = true;
            Item.expert = true;
            Item.rare = ItemRarityID.Red;
        }

        public override void ModifyResearchSorting(
            ref ContentSamples.CreativeHelper.ItemGroup itemGroup
        )
        {
            itemGroup = ContentSamples.CreativeHelper.ItemGroup.BossBags;
        }

        public override bool CanRightClick() => true;

        public override Color? GetAlpha(Color lightColor) =>
            Color.Lerp(lightColor, Color.White, 0.4f);

        public override void PostUpdate()
        {
            // If this line was just to keep it floating, it's already handled in Draw
            // CustomDrawTreasureBag.ForceItemIntoWorld(Item);
            Item.TreasureBagLightAndDust(); // Keep this if you want light + dust FX
        }

        public override bool PreDrawInWorld(
            SpriteBatch spriteBatch,
            Color lightColor,
            Color alphaColor,
            ref float rotation,
            ref float scale,
            int whoAmI
        )
        {
            return ItemDrawHelper.DrawFloatingItemWithPulse(
                Item,
                spriteBatch,
                ref rotation,
                ref scale,
                whoAmI
            );
        }

        public override void ModifyItemLoot(ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.CoinsBasedOnNPCValue(ModContent.NPCType<ShimmeredDuke>()));

            // Example loot — change to your needs
            itemLoot.Add(ItemDropRule.NormalvsExpert(ItemID.MasterBait, 3, 2));
        }
    }
}
