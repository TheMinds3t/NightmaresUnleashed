using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Buffs
{
	public class AntyMinion : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Winged Anty");
			Description.SetDefault("The anty will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			TruePlayer modPlayer = player.GetModPlayer<TruePlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("AntyMinion")] > 0)
			{
				modPlayer.antyMinion = true;
			}
			if (!modPlayer.antyMinion)
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
			else
			{
				player.buffTime[buffIndex] = 18000;
			}
		}
	}
}