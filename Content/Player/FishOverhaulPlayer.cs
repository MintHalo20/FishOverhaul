using Terraria.ModLoader;

namespace FishOverhaul
{
    public class FishOverhaulPlayer : ModPlayer
    {
        public bool canFishInShimmer;

        public override void ResetEffects()
        {
            canFishInShimmer = false;
        }
    }
}
