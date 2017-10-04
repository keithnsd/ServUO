using System;
using Server;

namespace Server.Items
{
	public class MothersDress : PlainDress
	{
		public override int ArtifactRarity{ get{ return 13; } }

		[Constructable]
		public MothersDress()
		{

			Name = "Norman's Mother Dress";
			Hue = 903;

			Resistances.Cold = 8;
			Resistances.Physical = 10;

			SkillBonuses.SetValues( 0, SkillName.Fencing, 10.0 );

			Attributes.BonusDex = 4;
			Attributes.BonusHits = 5;
			Attributes.NightSight = 1;
			Attributes.DefendChance = 10;
			Attributes.ReflectPhysical = 10;
			Attributes.RegenHits = 5;
		}

		public MothersDress( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 2 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}