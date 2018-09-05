using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class KunaiMaster : ModNPC
	{
		private int updateTime = 0;
		private int attackTime = -80;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kunai Master");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 800;
			npc.damage = 20;
			npc.defense = 12;
			npc.knockBackResist = 0.0125f;
			npc.width = 34;
			npc.height = 46;
			npc.aiStyle = 14;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 8000;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 4;
		}
		
		public void fireKunai()
		{
			Player target = Main.player[Main.myPlayer];
			Vector2 off = new Vector2(0,0);
			float speed = 15.0f;
			double dire = Math.Atan2((target.Center.Y - npc.Center.Y), (target.Center.X - npc.Center.X));
			Projectile.NewProjectile(npc.Center.X + off.X, npc.Center.Y + off.Y, (float)Math.Cos(dire) * speed, (float)Math.Sin(dire) * speed, mod.ProjectileType("ExpertKunai"), (int)(npc.damage), 0f, Main.myPlayer, npc.whoAmI, 0.25f);		
		}
		
		public override void PostAI()
		{
			++attackTime;
			
			if (attackTime >= 0 && attackTime % 15 == 0)
			{
				fireKunai();
			}
			
			if (attackTime >= 44)
			{
				attackTime = -150;
			}
			
			npc.velocity *= 1.0f;
		}
		
		public override void FindFrame(int frameHeight)
		{
			++updateTime;
			npc.spriteDirection = npc.direction;
			if(attackTime > 0 && attackTime % 15 < 7)
			{
				npc.frame.Y = frameHeight * 1;			
			}
			else
			{
				npc.frame.Y = frameHeight * ((int)(updateTime / 6) % 3);			
			}
		}
		
		public override void NPCLoot()
		{
			if (Main.rand.Next(50) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NinjasKunai"), 1);
			}
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float ret = 1.0f / 300.0f;
			if (Main.bloodMoon) {ret += 0.0125f;}
			if (Main.hardMode) {ret += 0.0125f;}
			if (Main.expertMode) {ret += 19.0f / 300.0f;}
			ret /= 2.0f;
            return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime && NPC.downedMechBossAny && Main.hardMode ? ret * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0f;
        }
	}
}