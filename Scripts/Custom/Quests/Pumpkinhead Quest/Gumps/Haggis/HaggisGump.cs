using System; 
using Server; 
using System.Collections.Generic;
using Server.Commands;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{ 
	public class HaggisGump : Gump 
	{ 
		public static void Initialize() 
		{ 
			CommandSystem.Register( "HaggisGump", AccessLevel.GameMaster, new CommandEventHandler( HaggisGump_OnCommand ) ); 
		} 

		private static void HaggisGump_OnCommand( CommandEventArgs e ) 
		{ 
			e.Mobile.SendGump( new HaggisGump( e.Mobile ) ); 
		} 

		public HaggisGump( Mobile owner ) : base( 50,50 ) 
		{ 
//----------------------------------------------------------------------------------------------------
			AddPage( 0 );
			AddImageTiled(  54, 33, 369, 400, 2624 );
			AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//--------------------------------------Window size bar--------------------------------------------
			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "Revenge: the Demon" );
			
			AddHtml( 107, 140, 300, 230, "<BODY>" +
//----------------------/----------------------------------------------/
			"<BASEFONT COLOR=YELLOW>*Raises her head and sniffs the air as you enter*<BR>" +
			"<BASEFONT COLOR=YELLOW><BR>Best be on your way... nothing I can do for ya.<BR>" +
			"<BASEFONT COLOR=YELLOW><BR>Wait... I sense the anger in ya, ya come seeking vengeance.  Well what ya ask got a powerful price.<BR>" +
			"<BASEFONT COLOR=YELLOW><BR>Not to mention a high price in gold!  10,000 to be exact.  Are ya still interested?<BR>" +
			"<BASEFONT COLOR=YELLOW><BR>If ya are, then hand me the gold and I will continue." +
			"</BODY>", false, true);
			
			AddImage( 430, 9, 10441);
			AddImageTiled( 40, 38, 17, 391, 9263 );
			AddImage( 6, 25, 10421 );
			AddImage( 34, 12, 10420 );
			AddImageTiled( 94, 25, 342, 15, 10304 );
			AddImageTiled( 40, 427, 415, 16, 10304 );
			AddImage( -10, 314, 10402 );
			AddImage( 56, 150, 10411 );
			AddImage( 155, 120, 2103 );
			AddImage( 136, 84, 96 );

			AddButton( 225, 390, 0xF7, 0xF8, 0, GumpButtonType.Reply, 0 ); 

//--------------------------------------------------------------------------------------------------------------
		} 

		public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
		{ 
			Mobile from = state.Mobile; 

			switch ( info.ButtonID ) 
			{ 
				case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
				{ 
					//Cancel 
					from.SendMessage( "Pay the witch for more information." );
					break; 
				} 
			}
		}
	}
}