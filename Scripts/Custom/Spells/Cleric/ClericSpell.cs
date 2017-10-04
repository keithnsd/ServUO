using System;
using Server;
using Server.Spells;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.Custom
{
	public abstract class ClericSpell : Spell
	{
		public ClericSpell( Mobile caster, Item scroll, SpellInfo info ) 
			: base( caster, scroll, info )
		{}
		
		public abstract double RequiredSkill{ get; }
		public abstract int RequiredMana { get; }
		public abstract int RequiredTithing{ get; }

		public override SkillName CastSkill{ get{ return SkillName.SpiritSpeak; } }
		public override bool ClearHandsOnCast{ get{ return false; } }
		//public override int CastDelayBase{ get{ return 1; } }
		public override int CastRecoveryBase { get { return 5; } }

		public override bool CheckCast()
		{
			int mana = ScaleMana(RequiredMana);
		
			if (!base.CheckCast())
			{
				return false;
			}

			if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "You must have at least " + RequiredSkill + " Spirit Speak to invoke this prayer" );
				return false;
			}
			else if ( Caster.TithingPoints < RequiredTithing )
			{
				Caster.SendLocalizedMessage(1060173, RequiredTithing.ToString());
					// You must have at least ~1_TITHE_REQUIREMENT~ Tithing Points to use this ability,
				return false;
			}
			else if (Caster.Mana < mana)
			{
				Caster.SendLocalizedMessage(1060174, mana.ToString());
					// You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			return true;
		}

		public override bool CheckFizzle()
		{
			int tithing = RequiredTithing;

			if (AosAttributes.GetValue(Caster, AosAttribute.LowerRegCost) > Utility.Random(100))
			{
				tithing = 0;
			}

			int mana = ScaleMana(RequiredMana);

			if ( Caster.Skills[CastSkill].Value < RequiredSkill )
			{
				Caster.SendMessage( "You must have at least " + RequiredSkill + " Spirit Speak to invoke this prayer." );
				return false;
			}
			else if ( Caster.TithingPoints < tithing )
			{
				Caster.SendLocalizedMessage(1060173, tithing.ToString());
					// You must have at least ~1_TITHE_REQUIREMENT~ Tithing Points to use this ability,
				return false;
			}
			else if ( Caster.Mana < mana )
			{
				Caster.SendLocalizedMessage(1060174, mana.ToString());
					// You must have at least ~1_MANA_REQUIREMENT~ Mana to use this ability.
				return false;
			}

			Caster.TithingPoints -= tithing;

			if (!base.CheckFizzle())
			{
				return false;
			}

			Caster.Mana -= mana;

			return true;
		}

		public override void SayMantra()
		{
			Caster.PublicOverheadMessage( MessageType.Regular, 0x3B2, false, Info.Mantra );
			Caster.PlaySound( 0x24A );
		}

		public override void DoFizzle()
		{
			Caster.PlaySound( 0x1D6 );
			Caster.NextSpellTime = Core.TickCount;
		}

		public override void DoHurtFizzle()
		{
			Caster.PlaySound( 0x1D6 );
		}

		public override void OnDisturb( DisturbType type, bool message )
		{
			base.OnDisturb( type, message );

			if ( message )
				Caster.PlaySound( 0x1D6 );
		}

		public override void OnBeginCast()
		{
			base.OnBeginCast();

			Caster.FixedEffect( 0x37C4, 10, 42, 4, 3 );
		}

		public override void GetCastSkills( out double min, out double max )
		{
			min = RequiredSkill;
			max = RequiredSkill + 40.0;
		}
		
		public override int GetMana()
		{
			return 0;
		}
	}
}