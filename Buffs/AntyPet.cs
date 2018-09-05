using Terraria;
using Terraria.ModLoader;

namespace TrueEater.Buffs
{
	public class AntyPet : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Anty");
			Description.SetDefault("\"It loves you!\"");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<TruePlayer>(mod).antyPet = true;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("AntyPet")] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("AntyPet"), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}