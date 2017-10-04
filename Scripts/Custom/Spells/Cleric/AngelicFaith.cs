using System;
using System.Collections;
using System.Collections.Generic;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Spells;
using Server.Items;

namespace Server.Spells.Custom
{
	public class AngelicFaithSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Angelic Faith", "Angelus Terum",
				212,
				9041
			);
		public AngelicFaithSpell( Mobile caster, Item scroll ) 
			: base( caster, scroll, m_Info )
		{
		}
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(0.5); } }
		
		public override int RequiredTithing{ get{ return 100; } }
        public override int RequiredMana { get { return 20; } }
		public override double RequiredSkill{ get{ return 80.0; } }

		private static Hashtable m_Table = new Hashtable();

		public static bool HasEffect( Mobile m )
		{
			return ( m_Table[m] != null );
		}

		public static void RemoveEffect( Mobile m )
		{
			object[] mods = (object[])m_Table[m];
						
			if ( mods != null )
			{
				m.BodyMod = 0;

				m.RemoveStatMod( ((StatMod)mods[0]).Name );
				m.RemoveStatMod( ((StatMod)mods[1]).Name );
				m.RemoveStatMod( ((StatMod)mods[2]).Name );
				m.RemoveSkillMod( (SkillMod)mods[3] );
				m.RemoveSkillMod( (SkillMod)mods[4] );
				m.RemoveSkillMod( (SkillMod)mods[5] );
				
				m.EndAction( typeof( AngelicFaithSpell ) );
			}
			
			m_Table.Remove( m );
		}
		
		public override bool CheckCast()
		{
			if ( !base.CheckCast() )
			{
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( AngelicFaithSpell ) ) )
			{
				if (Core.ML)
                    RemoveEffect(Caster);
                else
					Caster.SendLocalizedMessage( 1005559 );
					return false;
			}
            else if (TransformationSpellHelper.UnderTransformation(this.Caster))
            {
				Caster.SendLocalizedMessage(1061091); // You cannot cast that spell in this form.
                return false;
            }
            else if (DisguiseTimers.IsDisguised(this.Caster))
            {
				Caster.SendMessage( "You cannot transform while disguised." );
                return false;
            }
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendMessage( "You cannot transform while wearing body paint." );
				return false;
			}
			else if ( !Caster.CanBeginAction( typeof( Seventh.PolymorphSpell ) ) )
			{
				Caster.SendMessage( "You cannot transform while polymorphed." );
				return false;
			}

			return true;
		}		

		public override void OnCast()
		{
			if ( !Caster.CanBeginAction( typeof( AngelicFaithSpell ) ) )
			{
				if (Core.ML)
                    RemoveEffect(Caster);
                else
					Caster.SendLocalizedMessage( 1005559 );
			}
            else if (TransformationSpellHelper.UnderTransformation(this.Caster))
            {
				Caster.SendLocalizedMessage(1061091); // You cannot cast that spell in this form.
            }
            else if (DisguiseTimers.IsDisguised(this.Caster))
            {
				Caster.SendMessage( "You cannot transform while disguised." );
            }
			else if ( Caster.BodyMod == 183 || Caster.BodyMod == 184 )
			{
				Caster.SendMessage( "You cannot transform while wearing body paint." );
			}
			else if ( !Caster.CanBeginAction( typeof( Seventh.PolymorphSpell ) ) )
			{
				Caster.SendMessage( "You cannot transform while polymorphed." );
			}
			else if ( CheckSequence() )
			{
				object[] mods = new object[]
				{
					new StatMod( StatType.Str, "[Cleric] Str Offset", 20, TimeSpan.Zero ),
					new StatMod( StatType.Dex, "[Cleric] Dex Offset", 20, TimeSpan.Zero ),
					new StatMod( StatType.Int, "[Cleric] Int Offset", 20, TimeSpan.Zero ),
					new DefaultSkillMod( SkillName.Macing, true, 20 ),
					new DefaultSkillMod( SkillName.Healing, true, 20 ),
					new DefaultSkillMod( SkillName.Anatomy, true, 20 )
				};

				m_Table[Caster] = mods;

				Caster.AddStatMod( (StatMod)mods[0] );
				Caster.AddStatMod( (StatMod)mods[1] );
				Caster.AddStatMod( (StatMod)mods[2] );
				Caster.AddSkillMod( (SkillMod)mods[3] );
				Caster.AddSkillMod( (SkillMod)mods[4] );
				Caster.AddSkillMod( (SkillMod)mods[5] );

				double span = 10.0 * DivineFocusSpell.GetScalar( Caster );
				new InternalTimer( Caster, TimeSpan.FromMinutes( (int)span ) ).Start();

				IMount mount = Caster.Mount;

				if ( mount != null )
					mount.Rider = null;

				Caster.BodyMod = 123;
				Caster.BeginAction( typeof( AngelicFaithSpell ) );
				Caster.PlaySound( 0x165 );
				Caster.FixedParticles( 0x3728, 1, 13, 0x480, 92, 3, EffectLayer.Head );
			}
		}


		private class InternalTimer : Timer
		{
			private Mobile m_Owner;
			private DateTime m_Expire;

			public InternalTimer( Mobile owner, TimeSpan duration ) : base( TimeSpan.Zero, TimeSpan.FromSeconds( 0.1 ) )
			{
				m_Owner = owner;
				m_Expire = DateTime.Now + duration;
			}

			protected override void OnTick()
			{
				if ( DateTime.Now >= m_Expire )
				{
					AngelicFaithSpell.RemoveEffect( m_Owner );
					Stop();
				}
			}
		}
	}
}