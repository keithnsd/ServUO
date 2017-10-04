using System;
using Server;
using System.Text;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Custom;
using Server.Items;

namespace Server.Commands
{
	public class CastClericSpells
	{
		public static void Initialize()
		{
			CommandSystem.Prefix = "[";

			CommandSystem.Register( "AngelicFaith", AccessLevel.Player, new CommandEventHandler( AngelicFaith_OnCommand ) );

			CommandSystem.Register( "BanishEvil", AccessLevel.Player, new CommandEventHandler( BanishEvil_OnCommand ) );

			CommandSystem.Register( "DampenSpirit", AccessLevel.Player, new CommandEventHandler( DampenSpirit_OnCommand ) );

			CommandSystem.Register( "DivineFocus", AccessLevel.Player, new CommandEventHandler( DivineFocus_OnCommand ) );

			CommandSystem.Register( "HammerOfFaith", AccessLevel.Player, new CommandEventHandler( HammerOfFaith_OnCommand ) );

			CommandSystem.Register( "Purge", AccessLevel.Player, new CommandEventHandler( Purge_OnCommand ) );

			CommandSystem.Register( "Restoration", AccessLevel.Player, new CommandEventHandler( Restoration_OnCommand ) );

			CommandSystem.Register( "SacredBoon", AccessLevel.Player, new CommandEventHandler( SacredBoon_OnCommand ) );

			CommandSystem.Register( "Sacrifice", AccessLevel.Player, new CommandEventHandler( Sacrifice_OnCommand ) );

			CommandSystem.Register( "Smite", AccessLevel.Player, new CommandEventHandler( Smite_OnCommand ) );

			CommandSystem.Register( "TouchOfLife", AccessLevel.Player, new CommandEventHandler( TouchOfLife_OnCommand ) );

			CommandSystem.Register( "TrialByFire", AccessLevel.Player, new CommandEventHandler( TrialByFire_OnCommand ) );

		}

		public static void Register( string command, AccessLevel access, CommandEventHandler handler )
		{
			Server.Commands.CommandSystem.Register( command, access, handler );
		}

		public static bool HasSpell( Mobile from, int spellID )
		{
			Spellbook book = Spellbook.Find( from, spellID );

			return ( book != null && book.HasSpell( spellID ) );
		}

		[Usage( "AngelicFaith" )]
		[Description( "Casts AngelicFaith spell." )]
		public static void AngelicFaith_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new AngelicFaithSpell( e.Mobile, null ).Cast(); 
					}			
		}
		[Usage( "BanishEvil" )]
		[Description( "Casts Banish Evil spell." )]
		public static void BanishEvil_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;
			
         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new BanishEvilSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "DampenSpirit" )]
		[Description( "Casts Dampen Spirit spell." )]
		public static void DampenSpirit_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new DampenSpiritSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "DivineFocus" )]
		[Description( "Casts Divine Focus spell." )]
		public static void DivineFocus_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new DivineFocusSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "HammerOfFaith" )]
		[Description( "Casts Hammer Of Faith spell." )]
		public static void HammerOfFaith_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new HammerOfFaithSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "Purge" )]
		[Description( "Casts Purge spell." )]
		public static void Purge_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new  PurgeSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "Restoration" )]
		[Description( "Casts Restoration spell." )]
		public static void Restoration_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new RestorationSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "SacredBoon" )]
		[Description( "Casts Sacred Boon spell." )]
		public static void SacredBoon_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new SacredBoonSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "Sacrifice" )]
		[Description( "Casts Sacrifice spell." )]
		public static void Sacrifice_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing

				//if ( HasSpell( from, 459 ) )
					{
					new SacrificeSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "Smite" )]
		[Description( "Casts Smite spell." )]
		public static void Smite_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new SmiteSpell( e.Mobile, null ).Cast(); 
					}
		}

		[Usage( "TouchOfLife" )]
		[Description( "Casts Touch Of Life spell." )]
		public static void TouchOfLife_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new TouchOfLifeSpell( e.Mobile, null ).Cast(); 
					}			
		}

		[Usage( "TrialByFire" )]
		[Description( "Casts Trial By Fire spell." )]
		public static void TrialByFire_OnCommand( CommandEventArgs e )
		{
				Mobile from = e.Mobile;

         			if ( !Multis.DesignContext.Check( e.Mobile ) )
            				return; // They are customizing
					{
					new TrialByFireSpell( e.Mobile, null ).Cast(); 
					}			
		}
		

	}
}
