using System;
using System.Collections;
using Server;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a corpse of a salty old sailor" )]
	public class PoopdeckPappy : BaseCreature
	{
		private static bool m_Talked;
		string[] PoopdeckPappySay = new string[]
		{
			"Ooh, Can'st ya take it?",
			"Look who forgots ta eats theirs spinach!",
			"Olive Oil is tougher than ya... Hell, so is Sweet-pea!"
		};

		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public PoopdeckPappy() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = Utility.RandomSkinHue();

			Body = 0x190;
			Name = "Poopdeck Pappy";

			AddItem( new Cap( 0x12F ) );
			AddItem( new LongPants( 0x12F ) );
			AddItem( new Boots( Utility.RandomNeutralHue()) );

			SetStr( 350, 550 );
			SetDex( 150, 175 );
			SetInt( 110, 135 );
			SetHits( 3050, 4250 );

			SetDamage( 15, 25 );

			SetDamageType( ResistanceType.Physical, 75 );
			SetDamageType( ResistanceType.Energy, 25 );

			SetResistance( ResistanceType.Physical, 75 );
			SetResistance( ResistanceType.Fire, 50, 65 );
			SetResistance( ResistanceType.Cold, 60, 75 );
			SetResistance( ResistanceType.Poison, 65, 80 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.Anatomy, 80.0, 100.3 );
			SetSkill( SkillName.MagicResist, 65.0, 77.5 );
			SetSkill( SkillName.Tactics, 75.0, 99.5 );
			SetSkill( SkillName.Wrestling, 100.1, 120.0 );

			Fame = 21000;
			Karma = -21000;

			PackGem();
			//PackPotion();
			//PackMagicItems( 3, 5, 0.95, 0.95 );
		}

		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool CanRummageCorpses{ get{ return true; } }
		public override bool AlwaysAttackable{ get{ return true; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override bool OnBeforeDeath()
		{
			switch ( Utility.Random( 20 ) )
			{
				//case 0: PackItem( new RewardTicket() ); break;
				case 1: PackItem( new CanOfSpinach( ) ); break;
			}

			return base.OnBeforeDeath();

		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			if( m_Talked == false )
			{
				if ( m.InRange( this, 3 ) && m is PlayerMobile)
				{
					m_Talked = true;
					SayRandom( PoopdeckPappySay, this );
					this.Move( GetDirectionTo( m.Location ) );
					SpamTimer t = new SpamTimer();
					t.Start();
				}
			}
		}

		private class SpamTimer : Timer
		{
			public SpamTimer() : base( TimeSpan.FromSeconds( 12 ) )
				{
					Priority = TimerPriority.OneSecond;
				}

				protected override void OnTick()
				{
					m_Talked = false;
				}
			}

		private static void SayRandom( string[] say, Mobile m )
		{
				m.Say( say[Utility.Random( say.Length )] );
		}

		public PoopdeckPappy( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}