using System;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName( "a pure evil corpse" )]
	public class MichaelMyers : BaseCreature
	{
		private SpawnTimer m_Timer;

		private bool i_Died;
		public bool died{ get{ return i_Died; } set { i_Died = value; InvalidateProperties(); } }

		public override bool ShowFameTitle{ get{ return false; } }

		public override int GetDeathSound()
		{
			return 0x438;
		}

		public override int GetAttackSound()
		{
			return 0x154;
		}

		public override int GetHurtSound()
		{
			return 0x435;
		}

		public override int GetAngerSound()
		{
			return 0x154;
		}

		[Constructable]
		public MichaelMyers() : this ( false )
		{
		}

		[Constructable]
		public MichaelMyers( bool i_Died ) : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			Name = "Michael Myers";
			Body = 185;
			BaseSoundID = 0x154;
			Hue = 1150;

			died = i_Died;

			SetStr( 330, 500 );
			SetDex( 220, 360 );
			SetInt( 100, 150 );

			SetHits( 3000, 6000 );

			SetDamage( 20, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 75, 90 );
			SetResistance( ResistanceType.Fire, 70, 85 );
			SetResistance( ResistanceType.Cold, 70, 85 );
			SetResistance( ResistanceType.Poison, 70, 85 );
			SetResistance( ResistanceType.Energy, 70, 85 );

			SetSkill( SkillName.Anatomy, 120.0 );
			SetSkill( SkillName.MagicResist, 100.0 );
			SetSkill( SkillName.Tactics, 120.0 );
			SetSkill( SkillName.Fencing, 120.0 );

			Fame = 30000;
			Karma = -30000;

			VirtualArmor = 90;

			FancyShirt shirt = new FancyShirt();
			shirt.Hue = 1175;
			shirt.Movable = false;
			AddItem( shirt );

			ClothNinjaJacket chest = new ClothNinjaJacket();
			chest.Hue = 1175;
			AddItem( chest );

			LeatherGloves gloves = new LeatherGloves();
			gloves.Hue = 1175;
			AddItem( gloves );

			LongPants legs = new LongPants();
			legs.Hue = 1175;
			AddItem( legs );

			HalloweenKnife weapon = new HalloweenKnife();
			weapon.Movable = false;
			AddItem( weapon );

			Boots boots = new Boots();
			boots.Hue = 1175;
			AddItem( boots );

			AddItem( new ShortHair( 1115 ) );
		}

		public override bool BardImmune{ get{ return true; } }
		public override bool AutoDispel{ get{ return true; } }
		public override Poison PoisonImmune{ get{ return Poison.Lethal; } }
		public override bool AlwaysMurderer{ get{ return true; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.UltraRich );
			AddLoot( LootPack.Average );
		}

		public override bool OnBeforeDeath()
		{
			if (died)
			{
				switch ( Utility.Random( 20 ) )
				{
					case 0: PackItem( new HalloweenKnife() ); break;
				}

				//PackItem( new RewardTicket(1) );
				PackItem( new Gold( 250, 750 ));
			}

			return true;
		}

		public override void OnDeath( Container c )
		{
			base.OnDeath( c );

			if ( !died )
			{
				Mobile lastkiller = this.LastKiller;
				m_Timer = new SpawnTimer( c, this, lastkiller );
				m_Timer.Start();
			}
		}

		public MichaelMyers( Serial serial ) : base( serial )
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
		}

		private class SpawnTimer : Timer
		{
			private Container m_Container;
			private Mobile m_From;
			private Mobile m_Combatant;

			public SpawnTimer( Container c, Mobile from, Mobile combatant) : base( TimeSpan.FromSeconds( Utility.RandomMinMax( 2, 4 ) ) )
			{
				Priority = TimerPriority.FiftyMS;

				m_Container = c;
				m_From = from;
				m_Combatant = combatant;
			}

			protected override void OnTick()
			{
				if ( m_Container.Deleted )
					return;

				Map map = m_Container.Map;
				Point3D loc = m_Container.Location;
				//Map map = m_Combatant.Map;
				//Point3D loc = m_Combatant.Location;

				if ( map == null || !map.CanFit( loc, 16, false, false ) )
					return;

				MichaelMyers mm = new MichaelMyers();

				if ( Utility.RandomDouble() <= 0.3 )
					mm.died = true;
				else
					mm.died = false;

				mm.SetHits( 1000 * Utility.RandomMinMax(6,15));
				mm.Combatant = m_Combatant;
				mm.FocusMob = m_Combatant;

				mm.MoveToWorld( loc, map );

				m_Container.Delete();
			}
		}

	}
}