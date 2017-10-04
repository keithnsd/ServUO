/////////////////////////////////////////////////
//
// Automatically generated by the
// AddonGenerator script by Arya
//
/////////////////////////////////////////////////
using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PumpkinheadGraveAddon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new PumpkinheadGraveAddonDeed();
			}
		}

		[ Constructable ]
		public PumpkinheadGraveAddon()
		{
			AddComponent( new AddonComponent( 3178 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 3178 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 3168 ), -1, -1, 0 );
			AddComponent( new AddonComponent( 3168 ), -2, 1, 0 );
			AddComponent( new AddonComponent( 3168 ), -1, 1, 0 );
			AddComponent( new AddonComponent( 3168 ), 1, -1, 0 );
			AddComponent( new AddonComponent( 3178 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 3168 ), -2, 2, 0 );
			AddComponent( new AddonComponent( 3178 ), 1, 2, 0 );
			AddComponent( new AddonComponent( 3179 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 3178 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 3168 ), -1, 0, 0 );
			AddComponent( new AddonComponent( 3179 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 3168 ), -2, 0, 0 );
			AddComponent( new AddonComponent( 3168 ), 0, 2, 0 );
			AddComponent( new AddonComponent( 3179 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 3168 ), 0, 1, 0 );
			AddComponent( new AddonComponent( 3179 ), 0, 0, 0 );
			AddComponent( new AddonComponent( 3178 ), 0, -1, 0 );
			AddComponent( new AddonComponent( 3178 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 3168 ), 1, 0, 0 );
			AddComponent( new AddonComponent( 3179 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 3168 ), 1, 1, 0 );
			AddComponent( new AddonComponent( 3178 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 3168 ), 1, -2, 0 );
			AddComponent( new AddonComponent( 3168 ), 0, -2, 0 );
			AddComponent( new AddonComponent( 3179 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 3178 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 3168 ), 2, -1, 0 );
			AddComponent( new AddonComponent( 3179 ), 3, 1, 0 );
			AddComponent( new AddonComponent( 3168 ), 2, 1, 0 );
			AddComponent( new AddonComponent( 3179 ), 2, 0, 0 );
			AddComponent( new AddonComponent( 3168 ), 2, 0, 0 );
			AddonComponent ac = null;
			ac = new AddonComponent( 3179 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, -1, 0, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, -1, 1, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, -1, -1, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, -2, 1, 0 );
			ac = new AddonComponent( 3179 );
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, -2, 0, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 2, 1, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, 1, 2, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, -2, 2, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, -2, 2, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, 1, -2, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 1, -2, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 1, 0, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 1, -1, 0 );
			ac = new AddonComponent( 3179 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 1, 1, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 0, 2, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, 0, -1, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 0, -2, 0 );
			ac = new AddonComponent( 3179 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 0, 1, 0 );
			ac = new AddonComponent( 3179 );
			AddComponent( ac, 0, 0, 0 );
			ac = new AddonComponent( 3179 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 2, 0, 0 );
			ac = new AddonComponent( 3179 );
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 3178 );
			AddComponent( ac, 2, -1, 0 );
			ac = new AddonComponent( 3168 );
			AddComponent( ac, 2, -1, 0 );

		}

		public PumpkinheadGraveAddon( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}

	public class PumpkinheadGraveAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new PumpkinheadGraveAddon();
			}
		}

		[Constructable]
		public PumpkinheadGraveAddonDeed()
		{
			Name = "PumpkinheadGrave";
		}

		public PumpkinheadGraveAddonDeed( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( 0 ); // Version
		}

		public override void	Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}