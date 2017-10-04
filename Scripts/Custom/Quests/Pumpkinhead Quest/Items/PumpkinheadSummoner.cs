using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class PumpkinheadSummoner : Item
	{
		private int m_Charges;

		[CommandProperty( AccessLevel.GameMaster )]
		public int Charges
		{
			get{ return m_Charges; }
			set{ m_Charges = value; InvalidateProperties(); }
		}

		[Constructable]
		public PumpkinheadSummoner() : base( 0xC6A )
		{
			Weight = 3.0;
			Hue = 1523;
			LootType = LootType.Blessed;
			Name = "Pumpkinhead Summoner";
			Charges = 5;
		}

		public PumpkinheadSummoner( Serial serial ) : base( serial )
		{
		}
		
		public override void AddNameProperties( ObjectPropertyList list )
 		{
 			base.AddNameProperties( list );
 			list.Add( 1060658, "Charges\t{0}", m_Charges.ToString() );
 		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_Charges );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			switch ( version )
			{
				case 0:
				{
					m_Charges = (int)reader.ReadInt();

					break;
				}
			}
		}
		public override void OnDoubleClick( Mobile from )
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage( 1042010 ); //You must have the object in your backpack to use it.
				return;
			}
			else
			{
				if ( from.BeginAction( typeof( PumpkinheadSummoner ) ) )
				{
					from.Target = new PumpkinheadTarget( from, this );
					
					new InternalTimer( from ).Start();
				}
				else
				{
					from.SendMessage( "You cannot summon Pumpkinhead yet" );
				}
			}
		}
	
		public void ConsumeCharge( Mobile from )
		{
			--Charges;
			
			if ( Charges == 0 )
			{
				from.SendLocalizedMessage( 1019073 ); // This item is out of charges.
				this.Delete();
			}
		}
		
		private class InternalTimer : Timer
		{
			private Mobile m_From;

			public InternalTimer( Mobile from ) : base( TimeSpan.FromSeconds( 5.0 ) )
			{
				m_From = from;
			}

			protected override void OnTick()
			{
				m_From.EndAction( typeof( PumpkinheadSummoner ) );
			}
		}

		private class PumpkinheadTarget : Target
		{
			private Mobile m_Summoner;
			private PumpkinheadSummoner m_Item;
 
			public PumpkinheadTarget( Mobile summoner, PumpkinheadSummoner item ) : base ( 12, false, TargetFlags.Harmful )
			{
				m_Summoner = summoner;
				m_Item = item;
			}

			protected override void OnTarget( Mobile from, object target )
			{
				
				if( target == from )
				{
					from.SendLocalizedMessage( 1061832 ); // You cannot exact vengeance on yourself.
				}
				else if( target is Mobile )
				{
					Mobile m = (Mobile)target;
					
					if ( !m.Alive )
					{
						//Target is already dead, don't summon.
						from.SendMessage( "They are already dead" );
					}
					else if ( Core.AOS && (from.Frozen || from.Paralyzed) )
					{
						//Summoner is frozen, can't summon at this time.
						from.SendMessage( "You can't do this while frozen" );
					}
					else if ( from.CanBeHarmful( m ) && from.Alive )
					{
						//OK to summon
						from.SendMessage( "You summon Pumpkinhead to exact your revenge!" );

						TimeSpan duration = TimeSpan.FromSeconds( 6000 );

						QuestPumpkinhead ph = new QuestPumpkinhead( m_Summoner, m, duration );

						if ( BaseCreature.Summon( ph, false, m_Summoner, m.Location, 0x81, TimeSpan.FromSeconds( duration.TotalSeconds + 2.0 ) ) )
							ph.FixedParticles( 0x373A, 1, 15, 9909, EffectLayer.Waist );
						
						//Gotta lose alot of Karma for this one!
						Misc.Titles.AwardKarma( (Mobile)from, -150, true );
						
						//Consume a charge from the summoner
						m_Item.ConsumeCharge( from );							
					}
					else
					{
						//target is protected (i.e. player in Trammel, etc.)
						from.SendMessage( "You cannot summon Pumpkinhead against them here." );
					}
				}
				else
				{
					//Not a valid target
					from.SendMessage( "You cannot summon Pumpkinhead against that." );
				}
			}
		}
	}
}