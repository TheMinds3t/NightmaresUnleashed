using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	public class IceWisp : ModNPC
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frost Wisp");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 85;
			npc.damage = 20;
			npc.defense = 1;
			npc.knockBackResist = 0.6f;
			npc.width = 24;
			npc.height = 24;
			npc.aiStyle = 14;
			npc.HitSound = SoundID.NPCHit13;
			npc.DeathSound = SoundID.NPCDeath19;
			npc.value = 100;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.scale = 1.5f;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 10;
		}
		
		public override bool CheckDead()
		{
			return true;
		}

		public override void PostAI()
		{
			int dust = Dust.NewDust(npc.position, npc.width, npc.height, mod.DustType("AtomicDust"));
			Main.dust[dust].velocity *= 2f;
			npc.velocity *= 0.95f;
		}
		
		private int updateTime = 0;
		
		public override void FindFrame(int frameHeight)
		{
			++updateTime;
			npc.spriteDirection = npc.direction;
			npc.frame.Y = frameHeight * (int)((updateTime % 50) / 5);
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float ret = 1.0f / 400.0f;
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			bool flag = false;
			for(int l = -2;l < 2;++l)
			{
				for(int k = -2;k<2;++k)
				{
					if (Main.tile[x+l, y+k].active())
					{
						if (Main.tile[x+l, y+k].type == TileID.IceBlock || Main.tile[x+l, y+k].type == TileID.SnowBlock)
						{
							flag = true;
						}
					}
				}
			}
            return flag && Main.player[Main.myPlayer].statLifeMax > 160 ? ret * TrueEater.GetSpawnModifier(npc,spawnInfo) : 0.0f;//return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? ret : 0f;
        }

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, 5 + Main.rand.Next(6));

			if (Main.rand.Next(20) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Skull, 1);
			}

			if (Main.rand.Next(10) == 0)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IceWispOrb"), 1);
			}
		}
	}
}