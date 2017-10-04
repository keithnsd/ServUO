using System;
using Server;

namespace Server.Items
{
	public class InigoMontoyasSword : ThinLongsword
	{
		public override int ArtifactRarity{ get{ return 12; } }

		public override int InitMinHits{ get{ return 255; } }
		public override int InitMaxHits{ get{ return 255; } }

		[Constructable]
		public InigoMontoyasSword()
		{
			Name = "Diego Montoya's Finely Crafted Sword";
			Hue = 2433;

			SkillBonuses.SetValues( 0, SkillName.Swords, 10.0 );
			SkillBonuses.SetValues( 1, SkillName.Tactics, 10.0 );

			WeaponAttributes.HitLeechHits = 40;
			WeaponAttributes.HitLeechStam = 30;
			WeaponAttributes.ResistPhysicalBonus = 15;
			WeaponAttributes.SelfRepair = 5;
			WeaponAttributes.HitLowerDefend = 50;

			Attributes.DefendChance = 10;
			Attributes.AttackChance = 25;
			Attributes.RegenStam = 4;
			Attributes.WeaponSpeed = 30;
			Attributes.WeaponDamage = 40;
			Attributes.RegenHits = 4;
		}

		public InigoMontoyasSword( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}