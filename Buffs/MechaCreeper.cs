using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Buffs
{
	public class MechaCreeper : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mechacreep");
			Description.SetDefault("A Mechacreep to swarm around");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<TruePlayer>(mod).mechaprobePet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("MechaProbePet")] < player.maxMinions;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 10f, 10f, mod.ProjectileType("MechaProbePet"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}