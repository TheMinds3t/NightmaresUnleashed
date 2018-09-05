using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Buffs
{
	public class CometBlast : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Asteroid Belt");
			Description.SetDefault("Asteroids swarm around you\nOne for every 600 damage, or one for every thrown if using thrown weapon");
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			TruePlayer tru = player.GetModPlayer<TruePlayer>(mod);
            tru.asteroidBelt = true;
		}
	}
}