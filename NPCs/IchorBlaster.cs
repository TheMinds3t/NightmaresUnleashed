using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace TrueEater.NPCs
{
	[AutoloadBossHead]
	public class IchorBlaster : ModNPC
	{
		private bool hasInitialized = false;
		private int leftHand = -1;
		private int rightHand = -1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Blaster");
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((short)leftHand);
			writer.Write((short)rightHand);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			leftHand = reader.ReadInt16();
			rightHand = reader.ReadInt16();
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 30000;
			npc.damage = 0;
			npc.defense = 30;
			npc.knockBackResist = 0.2f;
			npc.width = 120;
			npc.height = 120;
			npc.aiStyle = -1;//14;
			npc.HitSound = SoundID.NPCHit13;
			npc.DeathSound = SoundID.NPCDeath19;
			npc.value = 70000;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.scale = 1.0f;
			npc.visualOffset = new Vector2(0,-45);
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
			Main.npcFrameCount[npc.type] = 4;
			npc.timeLeft = 22500;
			music = MusicID.Boss2;
			npc.netAlways = true;
			npc.boss = true;
		}
		
		public override bool CheckDead()
		{
			return true;
		}

		public int getClosestPlayer()
		{
			Player ret = null;
			int rret = -1;
			for (int k = 0; k < 255; k++)
			{
				Player p = Main.player[k];
				if (p.active)
				{
					Vector2 distance = (npc.Center - p.Center);
					if (ret == null)
					{
						ret = p;
						rret = k;
					}
					else
					{
						Vector2 distanceRet = (npc.Center - ret.Center);
						if ((Math.Abs(distanceRet.X) + Math.Abs(distanceRet.Y)) > (Math.Abs(distance.X) + Math.Abs(distance.Y)))
						{
							ret = p;
							rret = k;
						}
					}
				}
			}
			
			return rret;
		}

		float curRight = 0;
		float curLeft = 180;
		int updateTime = 0;
		public override void PostAI()
		{
			npc.velocity *= 0.975f;
			Vector2 Rpos = npc.Center + new Vector2(32,-30);
			Vector2 Lpos = npc.Center + new Vector2(-32-44,-30);

			npc.defense = 30 + (isRightHandAlive() ? 50 : 0) + (isLeftHandAlive() ? 50 : 0);
			float rightAngle = (float)Math.Cos(npc.localAI[2] / 30.0) * 20 - 25;
			float leftAngle = 180 + (float)Math.Cos((npc.localAI[2]+7) / 30.0) * 20 + 25;
			npc.ai[2] = getClosestPlayer();

			if (!hasInitialized) 
			{
				hasInitialized = true;
				int left = NPC.NewNPC((int)Lpos.X-90, (int)Lpos.Y - 40, mod.NPCType("IchorBlasterHand"), 0, npc.whoAmI, Lpos.X,Lpos.Y);
				leftHand = left;
				Main.npc[left].localAI[0] = 1;
				Main.npc[left].ai[0] = npc.whoAmI;
				int right = NPC.NewNPC((int)Rpos.X+90, (int)Rpos.Y - 40, mod.NPCType("IchorBlasterHand"), 0, npc.whoAmI, Rpos.X,Rpos.Y);
				rightHand = right;
				Main.npc[right].ai[0] = npc.whoAmI;
			}
			else
			{
				if ((int)npc.ai[2] != -1 && Main.player[(int)npc.ai[2]] != null)
				{
					Player target = Main.player[(int)npc.ai[2]];
					++npc.localAI[3];
					float off = (float)Math.Cos(npc.localAI[3] / (45.0f - (30.0f - (float)npc.defense / 130.0f * 30.0f)));
					float off2 = (float)Math.Sin(npc.localAI[3] / (45.0f - (30.0f - (float)npc.defense / 130.0f * 30.0f)));
					Vector2 targ = (target.Center + new Vector2(off*90.0f,off2*30.0f-180.0f)) - (npc.Center);	

					if (npc.ai[3] == 0 && Main.rand.Next(30) == 0 && target.position.Y - npc.position.Y > 160.0 && Math.Abs(target.position.X - npc.position.X) < 64)
					{
						npc.ai[3] = Main.rand.Next(3) + 1;
						TrueEater.PlaySound(SoundID.Roar, npc);
					}

					if (npc.ai[3] == 1 || npc.ai[3] == 2)
					{
						targ = new Vector2(0,0);
						npc.velocity = new Vector2(0.0f, updateTime < 90 ? 0.5f : 0.0f);
						++updateTime;
						float ang = updateTime < 45 ? -updateTime*1.3f : updateTime > 90 ? 360 - (updateTime - 45) * 4 : (updateTime - 45) * 6 - 60;

						if(isLeftHandAlive() && npc.ai[3] == 1) 
						{
							leftAngle = 180 - ang;								
						}
						else if(npc.ai[3] == 1)
						{
							npc.ai[3] = 3;
							updateTime = 0;
						}

						if(isRightHandAlive() && npc.ai[3] == 2)
						{
							rightAngle = ang;								
						}
						else if(npc.ai[3] == 2)
						{
							npc.ai[3] = 3;
							updateTime = 0;
						}

						if(updateTime >= 120)
						{
							npc.ai[3] = 0;
							updateTime = 0;
						}
					}

					if(npc.ai[3] == 3)
					{
						++updateTime;
						if (updateTime % 20 == 0 && updateTime <= 100)
						{
							int space = Main.rand.Next(5);
							for(int i = 0; i < 4; ++i)
							{
								if (i != space)
								{
									float ang = DtoR(250.0f + (110.0f / 4.0f) * i);
									float speed = 3.0f+Main.rand.NextFloat() * 2.0f;
									int p = Projectile.NewProjectile(npc.Center.X, npc.Center.Y-32, (float)Math.Cos(ang) * speed, (float)Math.Sin(ang) * speed, 288, 50 + Main.rand.Next(26), 4.0f, Main.myPlayer, npc.whoAmI, 0.25f);																
								}
							}
						}

						if(updateTime >= 120)
						{
							npc.ai[3] = 0;
							updateTime = 0;
						}						
					}

					if (target.statLife <= 0)
					{
						targ = new Vector2(0.0f, 3.0f * target.respawnTimer);
						npc.timeLeft = 1;
					}

					npc.velocity = npc.velocity * new Vector2(0.9f,0.9f) + targ * new Vector2((1 / 120.0f),(1 / 120.0f));								
				}
				else
				{
					npc.ai[2] = getClosestPlayer();
				}
			}

			if (isRightHandAlive())
			{
				NPC right = Main.npc[(int)rightHand];
				if (right != null)
				{
					++npc.localAI[2];
					curRight = curRight + rightAngle;
					curRight /= 2.0f;
					float off = (float)(DtoR(curRight));
					right.position = Rpos + new Vector2(145*(float)Math.Cos(off),160*(float)Math.Sin(off));
				}
			}
			else 
			{
				npc.localAI[0] += 0.1f;
				if (npc.localAI[0] >= 1.0f && TrueEater.FireHostileProj())
				{
					npc.localAI[0] -= 1.0f;
					float speed = 2.0f + Main.rand.NextFloat() * 10.0f;
					int p = Projectile.NewProjectile(Rpos.X, Rpos.Y, (float)Math.Cos(DtoR(0 + 30 - Main.rand.Next(60))) * speed, (float)Math.Sin(DtoR(0 + 30 - Main.rand.Next(60))) * speed, 288, 70 + Main.rand.Next(10), 4.0f, Main.myPlayer, npc.whoAmI, 0.25f);														
				}
			}

			if (isLeftHandAlive())
			{
				NPC left = Main.npc[(int)leftHand];
				if (left != null)
				{
					curLeft = curLeft + leftAngle;
					curLeft /= 2.0f;
					float off = (float)(DtoR(curLeft));
					left.position = Lpos + new Vector2(145*(float)Math.Cos(off),160*(float)Math.Sin(off));
				}
			}
			else
			{
				npc.localAI[1] += 0.1f;
				if (npc.localAI[1] >= 1.0f && TrueEater.FireHostileProj())
				{
					npc.localAI[1] -= 1.0f;
					float speed = 2.0f + Main.rand.NextFloat() * 10.0f;
					Lpos.X += 44;
					int p = Projectile.NewProjectile(Lpos.X, Lpos.Y, (float)Math.Cos(DtoR(180 + 30 - Main.rand.Next(60))) * speed, (float)Math.Sin(DtoR(180 + 30 - Main.rand.Next(60))) * speed, ProjectileID.GoldenShowerHostile, 70 + Main.rand.Next(10), 4.0f, Main.myPlayer, npc.whoAmI, 0.25f);						
				}
			}
		}
		
		private int spriteTime = 0;
		
		public override void FindFrame(int frameHeight)
		{
			++spriteTime;
			npc.spriteDirection = npc.direction;
			npc.frame.Y = frameHeight * (int)((spriteTime % 40) / 10);
		}
		
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0.0f;//return spawnInfo.spawnTileY < Main.rockLayer && !Main.dayTime ? ret : 0f;
        }

        private float DtoR(double angle)
		{
		   return (float)(Math.PI * angle / 180.0);
		}

		private float RtoD(double angle)
		{
		   return (float)(angle * (180.0 / Math.PI));
		}

		public bool isRightHandAlive()
		{
			return rightHand == -1 ? false : Main.npc[(int)rightHand] != null ? Main.npc[(int)rightHand].life > 0 : false;
		}
		public bool isLeftHandAlive()
		{
			return leftHand == -1 ? false : Main.npc[(int)leftHand] != null ? Main.npc[(int)leftHand].life > 0 : false;
		}

        int render_time = 0;
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			++render_time;
			SpriteEffects effects = SpriteEffects.None;
			Texture2D image = mod.GetTexture("NPCs/IchorBlasterArm");
			Color color = npc.GetAlpha(new Color(255,255,255,255));
			if (isRightHandAlive())
			{
				NPC tent = Main.npc[rightHand];
				Vector2 pos = npc.Center + new Vector2(32,-30);//Main.screenWidth / 2 - image.Width / 2, Main.screenHeight - image.Height * 2);
				Vector2 origin = new Vector2(8,14);
				float rot = (float)Math.Atan2((tent.Center.Y - pos.Y),(tent.Center.X - pos.X));

				spriteBatch.Draw(image, pos - Main.screenPosition, new Rectangle(0,0,image.Width,image.Height/4), color, rot+DtoR(rot / 10.0), origin, 1.0f, effects, 0f);

				Vector2 offset = new Vector2((float)(Math.Cos(rot+DtoR(rot / 10.0)) * 80.0),(float)(Math.Sin(rot+DtoR(rot / 7.0)) * 78.0f+6));
				pos = npc.Center + offset + new Vector2(32,-30);
				rot = (float)Math.Atan2((tent.Center.Y - pos.Y),(tent.Center.X - pos.X));
				spriteBatch.Draw(image, pos - Main.screenPosition, new Rectangle(0,image.Height/4,image.Width,image.Height/4), color, rot, origin, 1.0f, effects, 0f);
			}
			if (isLeftHandAlive())
			{
				NPC tent = Main.npc[leftHand];
				Vector2 pos = npc.Center + new Vector2(-32,-30);//Main.screenWidth / 2 - image.Width / 2, Main.screenHeight - image.Height * 2);
				Vector2 origin = new Vector2(8,14);
				float rot = (float)Math.Atan2((tent.Center.Y - pos.Y),(tent.Center.X - pos.X));

				spriteBatch.Draw(image, pos - Main.screenPosition, new Rectangle(0,image.Height/4,image.Width,image.Height/4), color, rot+DtoR(rot / 10.0), origin, 1.0f, effects, 0f);

				Vector2 offset = new Vector2((float)(Math.Cos(rot+DtoR(rot / 10.0)) * 80.0),(float)(Math.Sin(rot+DtoR(rot / 7.0)) * 78.0f+6));
				pos = npc.Center + offset + new Vector2(-32,-30);
				rot = (float)Math.Atan2((tent.Center.Y - pos.Y),(tent.Center.X - pos.X));
				spriteBatch.Draw(image, pos - Main.screenPosition, new Rectangle(0,0,image.Width,image.Height/4), color, rot, origin, 1.0f, effects, 0f);
			}
			
			return true;
		}


		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
			TrueWorld.downedIchorBlaster = true;
			if(isRightHandAlive())
			{
				Main.npc[rightHand].life = 0;
			}
			if(isLeftHandAlive())
			{
				Main.npc[leftHand].life = 0;
			}
			if (!Main.expertMode)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("BladderBomb"), 80 + Main.rand.Next(71));				
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Ichor, 20 + Main.rand.Next(21));
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SoulofFlight, 5 + Main.rand.Next(5));
				if (Main.rand.Next(10) == 0)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IchorBlasterTrophy"), 1);
				}
			}
			else
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("IchorBlasterBag"), 1);				
			}
		}
	}

	public class IchorBlasterHand : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Blaster Tentacle");
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 6000;
			npc.damage = 58;
			npc.defense = 30;
			npc.knockBackResist = 0.1f;
			npc.width = 40;
			npc.height = 40;
			npc.aiStyle = -1;
			npc.value = 0;
			npc.HitSound = SoundID.NPCHit13;
			npc.DeathSound = SoundID.NPCDeath19;			
			npc.noGravity = true;
			npc.noTileCollide = true;
			Main.npcFrameCount[npc.type] = 2;
			//banner = npc.type;
			//bannerItem = mod.ItemType("TrueEaterBanner");
		}

		public override void PostAI()
		{
			if(Main.npc[(int)npc.ai[0]] != null && Main.npc[(int)npc.ai[0]].life > 0)
			{
			}
			else
			{
			}
		}

		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)	
		{
			if (npc.life - damage <= 0 && Main.npc[(int)npc.ai[0]] != null && Main.npc[(int)npc.ai[0]].type == mod.NPCType("IchorBlaster"))
			{
				Main.npc[(int)npc.ai[0]].ai[(int)(0 + npc.localAI[0])] = -1.0f;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
			npc.frame.Y = frameHeight * (int)npc.localAI[0];
		}
	}
}