using System;
using System.Collections.Generic;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Spells;

namespace Server.Spells.Custom
{
	public class SmiteSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Smite", "Ferio",
				212,
				9041
			);

		public SmiteSpell( Mobile caster, Item scroll ) 
			: base( caster, scroll, m_Info )
		{
		}
		
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(1.5); } }
		
		public override int RequiredTithing{ get{ return 60; } }
        public override int RequiredMana { get { return 50; } }
		public override double RequiredSkill{ get{ return 80.0; } }

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );
				
				double damage = Caster.Skills[SkillName.SpiritSpeak].Value * DivineFocusSpell.GetScalar( Caster );

                Effects.SendBoltEffect(m, true, 2569);
				
				if ( Core.AOS )
				{
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage, 0, 0, 0, 0, 100 );
				}
				else
				{
					SpellHelper.Damage( TimeSpan.Zero, m, Caster, damage );
				}
			}

			FinishSequence();
		}


		private class InternalTarget : Target
		{
			private SmiteSpell m_Owner;

			public InternalTarget( SmiteSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
				{
					m_Owner.Target( (Mobile)o );
				}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}