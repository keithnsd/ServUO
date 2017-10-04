using System;
using System.Collections.Generic;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Spells.Custom
{
	public class BanishEvilSpell : ClericSpell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Banish Evil", "Abigo Malus",
				212,
				9041
			);

		public BanishEvilSpell( Mobile caster, Item scroll ) 
			: base( caster, scroll, m_Info )
		{
		}
		
        public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds(0.5); } }
		
		public override int RequiredTithing{ get{ return 30; } }
        public override int RequiredMana { get { return 15; } }
		public override double RequiredSkill{ get{ return 60.0; } }
		public override bool BlocksMovement { get{ return false;} }


		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
            //Console.WriteLine(" On Cast Working ");
		}

		public void Target( Mobile m )
		{
			SlayerEntry undead = SlayerGroup.GetEntryByName( SlayerName.Silver );
			SlayerEntry demon = SlayerGroup.GetEntryByName( SlayerName.Exorcism );
           
            if (m == null)
                return;

			else if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( !undead.Slays( m ) && !demon.Slays( m ) )
			{
				Caster.SendMessage( "This spell cannot be used on this type of creature." );
			}
			else if ( CheckHSequence( m ) )
			{
				SpellHelper.Turn( Caster, m );

                if (m == null)
                    return;
                
				m.FixedParticles( 0x3709, 10, 30, 5052, 0x480, 0, EffectLayer.LeftFoot );
				m.PlaySound( 0x208 );

				m.Say( "No! I musn't be banished!"  );
				new InternalTimer( m ).Start();
                //Console.WriteLine(" checkhsequence passed ");
			}

			FinishSequence();
		}

		private class InternalTimer : Timer
		{
			Mobile m_Owner;

			public InternalTimer( Mobile owner ) : base( TimeSpan.FromSeconds( 1.5 ) )
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ( m_Owner != null) 
				{
                    if( m_Owner.CheckAlive() )
					m_Owner.Delete();
				}
			}
		}

		private class InternalTarget : Target
		{
			private BanishEvilSpell m_Owner;
			// from target.cs
			// public target ( int range, bool allowGround, TargetFlags flags)
			//
			// is perhaps the allowground bool causing the error?
			public InternalTarget( BanishEvilSpell owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
                 if ( m_Owner !=null && o is Mobile )
				{
                     m_Owner.Target( (Mobile)o );
				}
                //else
                //{
                   // m_Owner.SendMessage("This spell can only be used on Evil Monsters!");
                //}
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}