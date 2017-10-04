using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Targeting;
using Server.ContextMenus;
using Server.Gumps;
using Server.Misc;
using Server.Network;
using Server.Spells;
using Server.Accounting;

namespace Server.Mobiles
{
	[CorpseName( "a Blind Witch's corpse" )]
	public class Haggis : Mobile
	{
                public virtual bool IsInvulnerable{ get{ return true; } }
		[Constructable]
		public Haggis()
		{
			Hue = 0x83EA;

			Female = true;
			Body = 0x191;
			Name = "Haggis";
			Title = "the Blind Witch";
			
			AddItem( new Robe( 0x1 ) );
			AddItem( new Sandals() );
			AddItem( new GoldBracelet() );

			AddItem( new LongHair( 0x0 ) );

			Item staff = new GnarledStaff();
			staff.Movable = false;
			AddItem( staff );	
			
			Blessed = true;			
		}

		public Haggis( Serial serial ) : base( serial )
		{
		}

		public override void GetContextMenuEntries( Mobile from, List<ContextMenuEntry> list ) 
	        { 
	                base.GetContextMenuEntries( from, list ); 
        	        list.Add( new HaggisEntry( from, this ) ); 
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

		public class HaggisEntry : ContextMenuEntry
		{
			private Mobile m_Mobile;
			private Mobile m_Giver;
			
			public HaggisEntry( Mobile from, Mobile giver ) : base( 6146, 3 )
			{
				m_Mobile = from;
				m_Giver = giver;
			}

			public override void OnClick()
			{
				if( !( m_Mobile is PlayerMobile ) )
					return;
				
				PlayerMobile mobile = (PlayerMobile) m_Mobile;

				{
					if ( ! mobile.HasGump( typeof( HaggisGump ) ) )
					{
						mobile.SendGump( new HaggisGump( mobile ));
					} 
				}
			}
		}

		public override bool OnDragDrop( Mobile from, Item dropped )
		{          		
         	        Mobile m = from;
			PlayerMobile mobile = m as PlayerMobile;
			Account acct=(Account)from.Account;
			bool WitchPaymentReceived = Convert.ToBoolean( acct.GetTag("WitchPaymentReceived") );

			if ( mobile != null)
			{
				if( dropped is PumpkinheadRemains )
         			{
         				if(dropped.Amount!=1)
         				{
						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "That is not what I need to perform the ritual!", mobile.NetState );
         					return false;
         				}
					if ( WitchPaymentReceived ) //added account tag check
		                	{
						dropped.Delete(); 
						mobile.AddToBackpack( new PumpkinheadSummoner() );
						mobile.SendGump( new HaggisFinishGump( mobile ));
	                       	 	        acct.SetTag( "WitchPaymentReceived", "false" ); //reset quest
         		      		}
					else //what to do if account not been tagged
         				{
         					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I have no need of this without payment!", mobile.NetState );
         					dropped.Delete();
         					mobile.AddToBackpack( new PumpkinheadRemains( ) );
         				}
         			}				
				else if( dropped is Gold )
				{
         				if(dropped.Amount!=10000)
         				{
						this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "The payment is 10,000 gold!", mobile.NetState );
         					return false;
         				}
         				
					dropped.Delete();

					mobile.SendGump( new HaggisMidGump( mobile ));
                       	 	        acct.SetTag( "WitchPaymentReceived", "true" );
                       	 	        
                       	 	        mobile.AddToBackpack( new PumpkinheadShovel( ) );

					return true;
				} 
				else
         			{
					this.PrivateOverheadMessage( MessageType.Regular, 1153, false, "I have no need for this item.", mobile.NetState );
     				}
			}
			return false;
		}
	}
}