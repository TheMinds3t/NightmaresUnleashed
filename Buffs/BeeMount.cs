using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Buffs
{
	public class BeeMount : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mechanical Drone");
			Description.SetDefault("*BZZT*");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(mod.MountType("MechaBee"), player);
			player.buffTime[buffIndex] = 10;
		}
	}
}
