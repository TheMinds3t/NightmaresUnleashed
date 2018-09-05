using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Buffs
{
	public class HallowPoison : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Hallow Poison");
			Description.SetDefault("Damage multiplier - x0%");
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			TruePlayer tru = player.GetModPlayer<TruePlayer>(mod);
            tru.cometBlast = true;
			
			if (tru.updateTime % 30 == 0)
			{
				Description.SetDefault("Damage multiplier - x" + (tru.poisonLevel * 100.0f) + "%");
			}
		}
	}
}