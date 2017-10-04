///Based on RoninGT's Grave Digger Shovel.  Thanks RoninGT for the great Grave Digger Quest!

using System;
using Server;
using Server.Gumps; 
using Server.Network; 
using Server.Misc; 
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Items
{
	public class PumpkinheadShovel : Item
	{
		private bool m_IsDigging;


		[CommandProperty( AccessLevel.GameMaster )]
		public bool IsDigging
		{
			get{ return m_IsDigging; }
			set{ m_IsDigging = value; }
		}

		[Constructable]
		public PumpkinheadShovel() : base( 0xF39 )
		{
			Weight = 0.0;
			Hue = 1894;
			Name = "a blind witch's shovel";
		}

		public PumpkinheadShovel( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf (from.Backpack))
			{
				from.SendLocalizedMessage( 1042001 ); // That must be in your pack for you to use it.
			}
			else if ( from.Mounted )
			{
				from.SendLocalizedMessage( 501864 ); // You can't mine while riding.
			}
			else if ( from.IsBodyMod && !from.Body.IsHuman )
			{
				from.SendLocalizedMessage( 501865 ); // You can't mine while polymorphed.
			}
			else 
			{
				if ( IsDigging != true )
				{
					from.Target = new PumpkinTarget( this, from );
					from.SendMessage( "What grave would you like it dig in." );
				}
				else
				{
					from.SendMessage( "You must wait to use this item again." );
				}
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version 

			writer.Write( m_IsDigging );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

			m_IsDigging = reader.ReadBool();
		}

		private class DigTimer : Timer
		{ 
			private Mobile m_From;
			private PumpkinheadShovel m_Item;

			public DigTimer( Mobile from, PumpkinheadShovel shovel, TimeSpan duration ) : base( duration ) 
			{ 
				Priority = TimerPriority.OneSecond;
				m_From = from;
				m_Item = shovel;
			}

			protected override void OnTick() 
			{
				if ( m_Item != null )
					m_Item.IsDigging = false;
					
				if ( !m_From.Alive )
				{
					m_From.SendMessage( "You cannot continue digging in this state." );
					Stop();
				}
				else
				{
					if ( Utility.Random( 100 ) < 35 )
					{
						switch ( Utility.Random ( 10 ) )
						{
							case 0:
							Skeleton skel = new Skeleton();
							skel.Location = m_From.Location;
							skel.Map = m_From.Map;
							skel.Combatant = m_From;

							if ( Utility.Random( 100 ) < 50 )
								skel.IsParagon = true;

	        					World.AddMobile( skel );
							break;

							case 1:
							Ghoul ghoul = new Ghoul();
							ghoul.Location = m_From.Location;
							ghoul.Map = m_From.Map;
							ghoul.Combatant = m_From;

							if ( Utility.Random( 100 ) < 50 )
								ghoul.IsParagon = true;
	
	        					World.AddMobile( ghoul );
							break;

							case 2:
							Lich lich = new Lich();
							lich.Location = m_From.Location;
							lich.Map = m_From.Map;
							lich.Combatant = m_From;
	
							if ( Utility.Random( 100 ) < 50 )
								lich.IsParagon = true;

	        					World.AddMobile( lich );
							break;

							case 3:
							LichLord lichl = new LichLord();
							lichl.Location = m_From.Location;
							lichl.Map = m_From.Map;
							lichl.Combatant = m_From;

							if ( Utility.Random( 100 ) < 50 )
								lichl.IsParagon = true;
	
	        					World.AddMobile( lichl );
							break;
	
							case 4:
							AncientLich alich = new AncientLich();
							alich.Location = m_From.Location;
							alich.Map = m_From.Map;
							alich.Combatant = m_From;
	
							if ( Utility.Random( 100 ) < 50 )
								alich.IsParagon = true;
	
	        					World.AddMobile( alich );
							break;
	
							case 5:
							Zombie zom = new Zombie();
							zom.Location = m_From.Location;
							zom.Map = m_From.Map;
							zom.Combatant = m_From;
	
							if ( Utility.Random( 100 ) < 50 )
								zom.IsParagon = true;
	
	        					World.AddMobile( zom );
							break;
	
							case 6:
							SkeletalKnight sk = new SkeletalKnight();
							sk.Location = m_From.Location;
							sk.Map = m_From.Map;
							sk.Combatant = m_From;
	
							if ( Utility.Random( 100 ) < 50 )
								sk.IsParagon = true;
	
	        					World.AddMobile( sk );
							break;

							case 7:
							SkeletalMage sm = new SkeletalMage();
							sm.Location = m_From.Location;
							sm.Map = m_From.Map;
							sm.Combatant = m_From;
	
							if ( Utility.Random( 100 ) < 50 )
								sm.IsParagon = true;
	
	        					World.AddMobile( sm );
							break;
	
							case 8:
							Spectre spec = new Spectre();
							spec.Location = m_From.Location;
							spec.Map = m_From.Map;
							spec.Combatant = m_From;
	
							if ( Utility.Random( 100 ) < 50 )
								spec.IsParagon = true;

	        					World.AddMobile( spec );
							break;
	
							case 9:
							Shade shade = new Shade();
							shade.Location = m_From.Location;
							shade.Map = m_From.Map;
							shade.Combatant = m_From;
	
							if ( Utility.Random( 100 ) < 50 )
								shade.IsParagon = true;
	
	        					World.AddMobile( shade );
							break;
						}
						m_From.SendMessage( "You have angered the spirits." );
					}
					else if ( m_From.Skills[SkillName.Mining].Base > 10 )
					{
						if ( Utility.Random( 120 ) >= (m_From.Skills[SkillName.Mining].Base) )
						{
							m_From.SendMessage( "You fail to dig anything up." );
						}
						else
						{
							m_From.SendMessage( "You dig up the hideous remains of the demon.  Unfortunately your shovel breaks in the process." );
							m_From.AddToBackpack( new PumpkinheadRemains() );
							m_Item.Delete();
							
						}
					}
					else if ( m_From.Skills[SkillName.Mining].Base <= 10 )
					{
						if ( Utility.Random( 120 ) > 10 )
						{
							m_From.SendMessage( "You fail to dig anything up." );
						}
						else
						{
							m_From.SendMessage( "You dig up the hideous remains of the demon.  Unfortunately your shovel breaks in the process." );
							m_From.AddToBackpack( new PumpkinheadRemains() );
							m_Item.Delete();
							
						}
					}
					else
					{
						m_From.SendMessage( "You fail to dig anything up." );
					}
		
					Stop();
				}
			}
		}

		private class PumpkinTarget : Target
		{

			//Grave ItemIDs
			public static int[] m_Grave = new int[]
			{
				3795,
				3807,
				3808,
				3809,
				3810,
				3816
			
			};

			private PumpkinheadShovel m_Item;
			private Mobile m_From;

			public PumpkinTarget( PumpkinheadShovel item, Mobile from ) : base( 12, false, TargetFlags.None )
			{
				m_Item = item;
				m_From = from;
			}

			protected override void OnTarget( Mobile from, object targeted )
			{
				if ( targeted is Item )
				{
					Item i = (Item)targeted;

					bool isGrave = false;

					foreach ( int check in m_Grave )
					{
  						if ( check == i.ItemID )
    							isGrave = true;
					}
				
					if ( isGrave == true )
					{
						bool isNearPumpkins = false;

						foreach ( Item item in i.GetItemsInRange( 2 ) )
						{
							if ( item is PumpkinheadGraveAddon )
								isNearPumpkins = true;
						}	

						if ( isNearPumpkins == true )
						{
							if ( m_From != null )
								m_From.SendMessage( "You start to dig." );

							DigTimer dt = new DigTimer( m_From, m_Item, TimeSpan.FromSeconds( 5.0 ) );
							dt.Start();
							m_From.PlaySound( Utility.RandomList( 0x125, 0x126 ) );
							m_From.Animate( 11, 5, 1, true, false, 0 );
							m_Item.IsDigging = true;
						}
						else
						{
							if ( m_From != null )
								m_From.SendMessage( "That is not Pumpkinhead's grave." );
						}

					}
					else
					{
						if ( m_From != null )
							m_From.SendMessage( "That is not a grave." );
					}

				}
				else if ( targeted is StaticTarget )
				{
					StaticTarget i = (StaticTarget)targeted;

					bool isGrave = false;

					foreach ( int check in m_Grave )
					{
  						if ( check == i.ItemID )
    							isGrave = true;
					}

					if ( isGrave == true )
					{
						bool isNearPumpkins = false;
						foreach ( Item item in m_From.GetItemsInRange( 2 ) )
						{
							if ( item is PumpkinheadGraveAddon )
								isNearPumpkins = true;
						}	

						if ( isNearPumpkins == true )
						{
							if ( m_From != null )
								m_From.SendMessage( "You start to dig." );

							DigTimer dt = new DigTimer( m_From, m_Item, TimeSpan.FromSeconds( 10.0 ) );
							dt.Start();
							m_From.PlaySound( Utility.RandomList( 0x125, 0x126 ) );
							m_From.Animate( 11, 5, 1, true, false, 0 );
							m_Item.IsDigging = true;
						}
						else
						{
							if ( m_From != null )
								m_From.SendMessage( "That is not Pumpkinhead's grave." );
						}
					}
					else
					{
						if ( m_From != null )
							m_From.SendMessage( "That is not a grave." );
					}
				}
				else
				{
					m_From.SendMessage( "That is not a grave." );
				}
			}
		}
	}
}
