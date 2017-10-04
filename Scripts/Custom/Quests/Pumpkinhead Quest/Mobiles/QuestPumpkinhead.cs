using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Mobiles
{
	public class QuestPumpkinhead : BaseCreature
	{
		private Mobile m_Summoner;
		private Mobile m_Target;
		private DateTime m_ExpireTime;
		private bool m_Stunning;
		
		public override double DispelDifficulty{ get{ return 200.0; } }
		public override double DispelFocus{ get{ return 10.0; } }
		public override Mobile ConstantFocus{ get{ return m_Target; } }
		public override bool NoHouseRestrictions{ get{ return true; } }
		public override bool IsScaredOfScaryThings{ get{ return false; } }
		public override bool IsScaryToPets{ get{ return true; } }
		public override bool ShowFameTitle{ get{ return false; } }
		
		public override int GetIdleSound()
		{
			return 0xDC;
		}

		public override int GetDeathSound()
		{
			return 0x283;
		}

		public override int GetAttackSound()
		{
			return 0x281;
		}

		public override int GetHurtSound()
		{
			if (this.Hits > 1000) 
			{
				return 0x27C; //play hurt sound
			}
			else if (this.Hits > 500)
			{
				return 0x27B; //play hurt sound
			}
			else if (this.Hits > 100)
			{
				return 0x282; //play hurt sound
			}
			else 
			{
				return 0xDD; //play hurt sound
			}
		}

		public override int GetAngerSound()
		{
			return 0x282;
		}

		public override void DisplayPaperdollTo( Mobile to )
		{
			// Do nothing
		}

		public QuestPumpkinhead( Mobile summoner, Mobile target, TimeSpan duration ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.18, 0.36 )
		{
			Name = "Pumpkinhead";
			Body = 309;
			BaseSoundID = 0x280;
			Hue = 1023;

			m_Summoner = summoner;
			m_Target = target;
			m_ExpireTime = DateTime.Now + duration;

			SetStr( 500 );
			SetDex( 350 );
			SetInt( 150 );

			SetHits( 800, 1200 );
			SetDamage( 12, 25 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetSkill( SkillName.MagicResist, 140.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Wrestling, 140.0 );
			SetSkill( SkillName.DetectHidden, 120 );

			
			SetResistance( ResistanceType.Physical, 100, 120 );
			SetResistance( ResistanceType.Fire, 75, 100 );
			SetResistance( ResistanceType.Cold, 75, 90 );
			SetResistance( ResistanceType.Poison, 75,90 );
			SetResistance( ResistanceType.Energy, 75, 90 );

			Fame = 0;
			Karma = 0;

			ControlSlots = 3;

			VirtualArmor = 80;
		}

		public override bool AlwaysMurderer{ get{ return true; } }
		public override bool BardImmune{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AutoDispel{ get{ return true; } }
		
		public override void OnGaveMeleeAttack( Mobile defender )
		{
			base.OnGaveMeleeAttack( defender );

			if ( this.Hits < (this.HitsMax) )
				this.Hits += Utility.RandomMinMax( 1, 5 );
				
			if ( !m_Stunning && 0.1 > Utility.RandomDouble() )
			{
				m_Stunning = true;

				defender.Animate( 21, 6, 1, true, false, 0 );
				this.PlaySound( 0xEE );
				defender.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "You are frozen with fear!" );

				if ( defender.Alive )
				{
					defender.Frozen = true;
					Timer.DelayCall( TimeSpan.FromSeconds( 5.0 ), new TimerStateCallback( Recover_Callback ), defender );
				}
			}
		}

		public override void OnDamage( int amount, Mobile from, bool willKill )
		{
			if ( m_Summoner != null && m_Summoner.Alive )
			{
				int hitdrain = Utility.RandomMinMax( 5, 10 );
				AOS.Damage( m_Summoner, this, hitdrain, 100, 0, 0, 0, 0 );

				int stamdrain = Utility.RandomMinMax( 10, 25 );
				m_Summoner.Stam -= stamdrain;
			}
		}

		public override void OnThink()
		{
			if ( !m_Target.Alive || DateTime.Now > m_ExpireTime || !m_Summoner.Alive )
			{
				Kill();
				return;
			}
			else if ( Map != m_Target.Map || !InRange( m_Target, 15 ) )
			{
				Map fromMap = Map;
				Point3D from = Location;

				Map toMap = m_Target.Map;
				Point3D to = m_Target.Location;

				if ( toMap != null )
				{
					for ( int i = 0; i < 5; ++i )
					{
						Point3D loc = new Point3D( to.X - 4 + Utility.Random( 9 ), to.Y - 4 + Utility.Random( 9 ), to.Z );

						if ( toMap.CanSpawnMobile( loc ) )
						{
							to = loc;
							break;
						}
						else
						{
							loc.Z = toMap.GetAverageZ( loc.X, loc.Y );

							if ( toMap.CanSpawnMobile( loc ) )
							{
								to = loc;
								break;
							}
						}
					}
				}

				Map = toMap;
				Location = to;

				ProcessDelta();

				Effects.SendLocationParticles( EffectItem.Create( from, fromMap, EffectItem.DefaultDuration ), 0x3728, 1, 13, 37, 7, 5023, 0 );
				FixedParticles( 0x3728, 1, 13, 5023, 37, 7, EffectLayer.Waist );

				PlaySound( 0x37D );
			}

			if ( m_Target.Hidden && InRange( m_Target, 3 ) && Core.TickCount >= this.NextSkillTime && UseSkill( SkillName.DetectHidden ) )
			{
				Target targ = this.Target;

				if ( targ != null )
					targ.Invoke( this, this );
			}

			Combatant = m_Target;
			FocusMob = m_Target;

			if ( AIObject != null )
				AIObject.Action = ActionType.Combat;

			base.OnThink();
		}

		public override bool OnBeforeDeath()
		{
			Effects.PlaySound( Location, Map, 0x10B );
			Effects.SendLocationParticles( EffectItem.Create( Location, Map, TimeSpan.FromSeconds( 10.0 ) ), 0x37CC, 1, 50, 2101, 7, 9909, 0 );

			Delete();
			return false;
		}
		
		private void Recover_Callback( object state )
		{
			Mobile defender = state as Mobile;

			if ( defender != null )
			{
				defender.Frozen = false;
				defender.Combatant = null;
				defender.LocalOverheadMessage( MessageType.Regular, 0x3B2, false, "You recover your senses." );
			}

			m_Stunning = false;
		}

		public QuestPumpkinhead( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			Delete();
		}
	}
}