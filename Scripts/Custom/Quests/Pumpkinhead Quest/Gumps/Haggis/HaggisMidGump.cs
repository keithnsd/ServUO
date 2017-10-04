using System; 
using Server; 
using System.Collections.Generic;
using Server.Commands;
using Server.Network;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{ 
	public class HaggisMidGump : Gump 
	{ 
		public static void Initialize() 
		{ 
			CommandSystem.Register( "HaggisMidGump", AccessLevel.GameMaster, new CommandEventHandler( HaggisMidGump_OnCommand ) ); 
		} 

		private static void HaggisMidGump_OnCommand( CommandEventArgs e ) 
		{ 
			e.Mobile.SendGump( new HaggisMidGump( e.Mobile ) ); 
		} 

		public HaggisMidGump( Mobile owner ) : base( 50,50 ) 
		{ 
//--------------------------------------------------------------------
			AddPage( 0 );
			AddImageTiled(  54, 33, 369, 400, 2624 );
			AddAlphaRegion( 54, 33, 369, 400 );

			AddImageTiled( 416, 39, 44, 389, 203 );
//---------------------------Window size bar--------------------------
			
			AddImage( 97, 49, 9005 );
			AddImageTiled( 58, 39, 29, 390, 10460 );
			AddImageTiled( 412, 37, 31, 389, 10460 );
			AddLabel( 140, 60, 0x34, "Revenge: the Demon" );
			
			AddHtml( 107, 140, 300, 230, "<BODY>" +
//--------------------------------------------------------------------/
			"<BASEFONT COLOR=YELLOW>Ah, you're a bigger fool than I thought!<BR><BR>" +
			"<BASEFONT COLOR=YELLOW>Very well, I will help you, but there is something I need you to do first.<BR><BR>" +
			"<BASEFONT COLOR=YELLOW>In the woods north of here, there is an old graveyard.  Man-folk used to bury their kin in there... kin they's ashamed of.<BR><BR>" +
			"<BASEFONT COLOR=YELLOW>Bring this here shovel... the thing you are looking for is in that graveyard.<BR><BR>" +
			"<BASEFONT COLOR=YELLOW>Bring it back here, some things I gotta do to it before it will be of any use to ya.<BR><BR>" +
			"<BASEFONT COLOR=YELLOW>What?  How will you know which grave?  You'll know "+owner.Name+", you'll know!<BR><BR>" +
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
		} 

		public override void OnResponse( NetState state, RelayInfo info ) //Function for GumpButtonType.Reply Buttons 
		{ 
			Mobile from = state.Mobile; 

			switch ( info.ButtonID ) 
			{ 
				case 0: //Case uses the ActionIDs defenied above. Case 0 defenies the actions for the button with the action id 0 
				{ 
					//Cancel 
					from.SendMessage( "Dig up the remains of Pumpkinhead and return to the witch." );
					break; 
				} 
			}
		}
	}
}