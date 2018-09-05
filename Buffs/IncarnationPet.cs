using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Buffs
{
	public class IncarnationPet : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Incarnation");
			Description.SetDefault("Summons an Incarnation to fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			TruePlayer modPlayer = player.GetModPlayer<TruePlayer>(mod);
			if (player.ownedProjectileCounts[mod.ProjectileType("IncarnationPet")] > 0)
			{
				modPlayer.incarnationPet = true;
			}
			if (!modPlayer.incarnationPet)
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