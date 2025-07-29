using Terraria.ModLoader;

namespace FishOverhaul.Content;

public class FishOverhaulPlayer : ModPlayer
{
    public bool CanFishInShimmer;

    public override void ResetEffects()
    {
        CanFishInShimmer = false;
    }
}
