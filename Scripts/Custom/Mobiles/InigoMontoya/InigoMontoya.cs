using System;
using System.Collections;
using Server;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;

namespace Server.Mobiles
{
	[CorpseName( "a corpse of a Spaniard" )]
	public class InigoMontoya : BaseCreature
	{
		private static bool m_Talked;
		string[] InigoMontoyaSay = new string[]
		{
			"Hello, my name is Inigo Montoya!",
			"You killed my father, prepare to die!",
            "I see you have studied your agrippa"
		};

		public override bool ShowFameTitle{ get{ return false; } }

		[Constructable]
		public InigoMontoya() : base( AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4 )
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = 0x83EA;

			Body = 0x190;
			Name = "Inigo Montoya";
			Title = "the Spaniard";
			HairItemID = 0x203C;
			HairHue = 0x497;
			FacialHairItemID = 0x2041;
			FacialHairHue = 0x497;


			AddItem( new FancyShirt( Utility.RandomNeutralHue()) );
			AddItem( new Doublet( 0x345 ) );
			AddItem( new LongPants( 0x345 ) );
			AddItem( new ThighBoots( Utility.RandomNeutralHue()) );
			//AddItem( new LongHair( 0x497 ) );
			//AddItem( new Mustache( 0x497 ) );

			InigoMontoyasSword weapon = new InigoMontoyasSword();
			weapon.Movable = false;
			AddItem( weapon );

			SetStr( 450, 550 );
			SetDex( 210, 285 );
			SetInt( 110, 135 );
			SetHits( 4250, 5450 );

			SetDamage( 25, 35 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 75 );
			SetResistance( ResistanceType.Fire, 50, 65 );
			SetResistance( ResistanceType.Cold, 60, 75 );
			SetResistance( ResistanceType.Poison, 65, 80 );
			SetResistance( ResistanceType.Energy, 50, 70 );

			SetSkill( SkillName.Anatomy, 90.0, 115.3 );
			SetSkill( SkillName.MagicResist, 65.0, 77.5 );
			SetSkill( SkillName.Tactics, 75.0, 109.5 );
			SetSkill( SkillName.Swords, 110.1, 140.0 );

			Fame = 15000;
			Karma = -15000;

			PackGem();
			//PackPotion();
			//PackMagicItems( 3, 5, 0.95, 0.95 );
		}

		public override int TreasureMapLevel{ get{ return 5; } }
		public override bool CanRummageCorpses{ get{ return true; } }

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}
		public override bool AlwaysAttackable{ get{ return true; } }

		public override bool OnBeforeDeath()
		{
			switch ( Utility.Random( 20 ) )
			{
				//case 0: PackItem( new RewardTicket() ); break;
				case 1: PackItem( new InigoMontoyasSword() ); break;
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
					SayRandom( InigoMontoyaSay, this );
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

		public InigoMontoya( Serial serial ) : base( serial )
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
