using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class FleshyZombie : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Zombie");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 56;
			npc.damage = 10;
			npc.defense = 2;
			npc.knockBackResist = 0.1f;
			npc.width = 34;
			npc.height = 46;
			npc.aiStyle = 3;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 150;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 4;
		}
		
		private int updateTime = 0;
		
		public override void FindFrame(int frameHeight)
		{
			++updateTime;
			npc.spriteDirection = npc.direction;
			npc.frame.Y = frameHeight * (int)((updateTime % 40) / 10);
		}
		
		 public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float ret = 1.0f / 90.0f;
			if (Main.bloodMoon) {ret += 0.1f;}
			if (Main.hardMode) {ret += 0.1f;}
			if (Main.expertMode) {ret += 10.0f / 90.0f;}
			ret /= 2.0f;
            return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? ret * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0f;
        }
	}
}