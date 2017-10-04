using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName( "a psycho killer corpse" )]
	public class NormanBates : BaseCreature
	{
		private bool m_ShowHours;
		private int m_NormanBeginHour;
		private int m_NormanEndHour;

		public bool mother;
		public int lastminute = 0;

		public override bool ShowFameTitle{ get{ return false; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public bool ShowHours
		{
			get { return m_ShowHours; }
			set { m_ShowHours = value; InvalidateProperties(); }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int NormanBeginHour
		{
			get{ return m_NormanBeginHour; }
			set{ m_NormanBeginHour = value; InvalidateProperties(); }
		}
		[CommandProperty( AccessLevel.GameMaster )]
		public int NormanEndHour
		{
			get{ return m_NormanEndHour; }
			set{ m_NormanEndHour = value; InvalidateProperties(); }
		}

		private static bool m_Talked;
		string[] NormanSay = new string[]
		{
			"A boy's best friend is his mother!",
			"We all go a little mad... somtimes.",
			"Mother!"
		};
		string[] MotherSay = new string[]
		{
			"Norman!  Where is that filthy boy?  Guess I have to do this myself.",
			"Why can't you leave my son, my poor Norman alone?",
			"I know what you're thinking with thay filthy mind!"
		};

		[Constructable]
		public NormanBates() : base( AIType.AI_Melee, FightMode.None, 10, 1, 0.2, 0.4 )
		{
			Name = "Norman Bates";
			Body = 185;

			SpeechHue = Utility.RandomDyedHue();
			Hue = 0x83EA;

			mother = false;
			m_NormanBeginHour = 7;
			m_NormanEndHour = 20;

			SetStr( 420, 500 );
			SetDex( 300, 360 );
			SetInt( 200, 250 );

			SetHits( 5000, 8000 );

			SetDamage( 15, 32 );

			SetDamageType( ResistanceType.Physical, 100 );

			SetResistance( ResistanceType.Physical, 75, 90 );
			SetResistance( ResistanceType.Fire, 70, 85 );
			SetResistance( ResistanceType.Cold, 70, 85 );
			SetResistance( ResistanceType.Poison, 70, 85 );
			SetResistance( ResistanceType.Energy, 70, 85 );

			SetSkill( SkillName.Anatomy, 110.0, 120.0 );
			SetSkill( SkillName.MagicResist, 100.0, 110.0 );
			SetSkill( SkillName.Tactics, 110.0, 120.0 );
			SetSkill( SkillName.Fencing, 120.0, 140.0 );
			SetSkill( SkillName.Wrestling, 120.0, 140.0 );

			Fame = 30000;
			Karma = -30000;

			VirtualArmor = 90;

			FancyShirt shirt = new FancyShirt();
			shirt.Hue = 1175;
			AddItem( shirt );

			Doublet doublet = new Doublet();
			doublet.Hue = 1175;
			doublet.Movable = false;
			AddItem( doublet );

			LongPants legs = new LongPants();
			legs.Hue = 646;
			AddItem( legs );

			Shoes shoes = new Shoes();
			shoes.Hue = 1175;
			AddItem( shoes );

			AddItem( new ShortHair( 1109 ) );

			AddItem( new Gold( 500, 1000 ));
		}

		public override void GenerateLoot()
		{
			AddLoot( LootPack.Rich );
		}

		public override void OnMovement( Mobile m, Point3D oldLocation )
		{
			base.OnMovement( m, oldLocation );
			int hours, minutes;

			Server.Items.Clock.GetTime( m.Map, m.X, m.Y, out hours, out minutes );

			if ( (m_ShowHours) && (m.AccessLevel > AccessLevel.Player) )
			{
				if (minutes > lastminute)
				{
					m.SendMessage( "Hour: "+hours+" Minute: "+minutes);
					lastminute = minutes;
				}
			}

			if ( ((hours >= m_NormanBeginHour) && (hours <= m_NormanEndHour)) )
			{
				if ( mother )
				{

					if ( this.Combatant != null )
						return;

					UnEquip ( this, Layer.Hair );
					UnEquip( this, Layer.OuterTorso );
					UnEquip ( this, Layer.OneHanded );

					this.AddItem( new ShortHair( 1175 ) );

					this.FightMode = FightMode.None;
					this.Kills = 0;

					mother = false;
				}
			}
			else
			{
				if ( !mother )
				{
					UnEquip( this, Layer.Hair );
					UnEquip( this, Layer.OuterTorso );

					this.AddItem( new BunsHair( 936 ) );

					PlainDress dress = new PlainDress();
					dress.Hue = 903;
					AddItem( dress );

					PsychoBlade weapon = new PsychoBlade();
					weapon.Movable = false;
					this.AddItem( weapon );

					this.FightMode = FightMode.Closest;
					this.Kills = 10;

					mother = true;
				}
			}

      		  	if( m_Talked == false )
        	  	{
      		      		if ( m.InRange( this, 3 ) && m is PlayerMobile)
        			{
            				m_Talked = true;

            				if ( mother )
            				{
            					SayRandom( MotherSay, this );
            				}
            				else
            				{
            					SayRandom( NormanSay, this );
            				}

           				this.Move( GetDirectionTo( m.Location ) );
             				SpamTimer t = new SpamTimer();
           				t.Start();
            			}
        	  	}
		}

		public override Poison PoisonImmune{ get{ return Poison.Regular; } }

		public override bool OnBeforeDeath()
		{
			if (mother)
			{
				switch ( Utility.Random( 25 ) )
				{
					case 0: PackItem( new PsychoBlade() ); break;
					case 1: PackItem( new MothersDress() ); break;
					case 2: PackItem( new ShowerLeftWallAddonDeed() ); break;
					case 3: PackItem( new ShowerRightWallAddonDeed() ); break;
					case 4: PackItem( new PsychoShowerLeftWallAddonDeed() ); break;
					case 5: PackItem( new PsychoShowerRightWallAddonDeed() ); break;
				}

			}

			return true;
		}


		public static void UnEquip( Mobile m_from, Layer layer )
		{
			if ( m_from.FindItemOnLayer( layer ) != null )
			{
				Item item = m_from.FindItemOnLayer( layer );
				item.Delete();
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

		public NormanBates( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int)2 ); // version

			writer.Write( (bool) m_ShowHours );
			writer.Write( (int) m_NormanBeginHour );
			writer.Write( (int) m_NormanEndHour );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			switch ( version )
			{
				case 2:
				{
					m_NormanBeginHour = reader.ReadInt();
					m_NormanEndHour = reader.ReadInt();

					goto case 1;
				}
				case 1:
				{
					m_ShowHours = reader.ReadBool();

					break;
				}
			}
		}
	}
}