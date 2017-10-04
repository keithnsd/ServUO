using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xEC4, 0xEC5 )]
	public class HalloweenKnife : BaseKnife
	{
		public override int ArtifactRarity{ get{ return 31; } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override int AosStrengthReq{ get{ return 10; } }
		public override int AosMinDamage{ get{ return 10; } }
		public override int AosMaxDamage{ get{ return 15; } }
		public override int AosSpeed{ get{ return 50; } }

		public override int OldStrengthReq{ get{ return 10; } }
		public override int OldMinDamage{ get{ return 5; } }
		public override int OldMaxDamage{ get{ return 10; } }
		public override int OldSpeed{ get{ return 40; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public HalloweenKnife() : base( 0xEC4 )
		{
			Weight = 6.0;
			Name = "Michael Myer's Halloween Knife";
			Hue = 1157;

			WeaponAttributes.HitLeechHits = 50;
			WeaponAttributes.HitLowerDefend = 50;

			Attributes.AttackChance = 25;
			Attributes.Luck = 80;
			Attributes.ReflectPhysical = 10;
			Attributes.RegenHits = 5;
			Attributes.RegenStam = 3;
			Attributes.WeaponDamage = 30;
			Attributes.WeaponSpeed = 50;
		}

		public HalloweenKnife( Serial serial ) : base( serial )
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