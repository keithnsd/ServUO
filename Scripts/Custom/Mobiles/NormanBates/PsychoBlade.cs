using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute( 0xEC4, 0xEC5 )]
	public class PsychoBlade : BaseKnife
	{
		public override int ArtifactRarity{ get{ return 12; } }

		public override WeaponAbility PrimaryAbility{ get{ return WeaponAbility.ParalyzingBlow; } }
		public override WeaponAbility SecondaryAbility{ get{ return WeaponAbility.BleedAttack; } }

		public override int AosStrengthReq{ get{ return 10; } }
		public override int AosMinDamage{ get{ return 12; } }
		public override int AosMaxDamage{ get{ return 20; } }
		public override int AosSpeed{ get{ return 60; } }

		public override int OldStrengthReq{ get{ return 10; } }
		public override int OldMinDamage{ get{ return 8; } }
		public override int OldMaxDamage{ get{ return 16; } }
		public override int OldSpeed{ get{ return 40; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public PsychoBlade() : base( 0xEC4 )
		{
			Weight = 6.0;
			Name = "Psycho Blade";
			Hue = 1157;

			WeaponAttributes.HitHarm = 75;
			WeaponAttributes.HitLeechHits = 60;
			WeaponAttributes.HitLowerDefend = 40;
			WeaponAttributes.SelfRepair = 5;

			Attributes.AttackChance = 30;
			Attributes.BonusHits = 10;
			Attributes.BonusStam = 5;
			Attributes.ReflectPhysical = 15;
			Attributes.RegenHits = 5;
			Attributes.RegenStam = 4;
			Attributes.WeaponDamage = 30;
			Attributes.WeaponSpeed = 40;
		}

		public PsychoBlade( Serial serial ) : base( serial )
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