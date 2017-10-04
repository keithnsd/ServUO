using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Gumps;
using Server.Multis;

namespace Server.Items
{
	public class CanOfSpinach : Item
	{
		[Constructable]
		public CanOfSpinach() : base( 0x1006 )
		{
			Name = "a Can of Spinach";

			Weight = 1.0;
			Hue = 2296;
		}

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) )
			{
				from.SendLocalizedMessage( 1042038 ); // You must have the object in your backpack to use it.
			}
			else if ( from.GetStatMod( "CanOfSpinach" ) != null )
			{
				from.SendMessage( "You have eaten a can of Spinach recently and eating another will provide no benefit");
			}
			else
			{
				from.PlaySound( 0x1EE );
				from.AddStatMod( new StatMod( StatType.Str, "CanOfSpinach", 40, TimeSpan.FromMinutes( 5.0 ) ) );

				Delete();
			}
		}

		public CanOfSpinach( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.WriteEncodedInt( (int) 0 ); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadEncodedInt();
		}
	}
}