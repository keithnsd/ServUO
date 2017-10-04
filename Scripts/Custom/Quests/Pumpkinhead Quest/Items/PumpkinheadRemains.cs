using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PumpkinheadRemains : Item
	{
		[Constructable]
		public PumpkinheadRemains() : base( 0xECA )
		{
			Weight = 30;
			Hue = 1023;
			Name = "the Remains of Pumpkinhead";
		}

		public PumpkinheadRemains( Serial serial ) : base( serial )
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