using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;

namespace Server.Items
{
	public class InstantGoldShower : Item
	{

		private int m_MinGold = 500;
		private int m_MaxGold = 2000;

		[CommandProperty( AccessLevel.GameMaster )]
		public int MinGold
		{
			get{ return m_MinGold; }
			set{ m_MinGold = value; InvalidateProperties(); }
		}

		[CommandProperty( AccessLevel.GameMaster )]
		public int MaxGold
		{
			get{ return m_MaxGold; }
			set{ m_MaxGold = value; InvalidateProperties(); }
		}

		[Constructable]
		public InstantGoldShower() :  base( 0x1870 )
		{
			Hue = 0x0;
			Name = "Golden Shower";
			LootType = LootType.Blessed;
			Visible = false;
			Movable = false;
		}

		public override void OnDoubleClick( Mobile m )
		{
			Map map = this.Map;

			if ( map != null )
			{
				for ( int x = -12; x <= 12; ++x )
				{
					for ( int y = -12; y <= 12; ++y )
					{
						double dist = Math.Sqrt(x*x+y*y);

						if ( dist <= 12 )
							new GoodiesTimer( map, X + x, Y + y, this ).Start();
					}
				}
			}
		}

		private class GoodiesTimer : Timer
		{
			private Map m_Map;
			private int m_X, m_Y;
			private InstantGoldShower m_GS;

			public GoodiesTimer( Map map, int x, int y, InstantGoldShower gs ) : base( TimeSpan.FromSeconds( Utility.RandomDouble() * 10.0 ) )
			{
				m_Map = map;
				m_X = x;
				m_Y = y;
				m_GS = gs;
			}

			protected override void OnTick()
			{
				int z = m_Map.GetAverageZ( m_X, m_Y );
				bool canFit = m_Map.CanFit( m_X, m_Y, z, 6, false, false );

				for ( int i = -3; !canFit && i <= 3; ++i )
				{
					canFit = m_Map.CanFit( m_X, m_Y, z + i, 6, false, false );

					if ( canFit )
						z += i;
				}

				if ( !canFit )
					return;

				Gold g = new Gold(m_GS.MinGold, m_GS.MaxGold );

				g.MoveToWorld( new Point3D( m_X, m_Y, z ), m_Map );

				if ( 0.5 >= Utility.RandomDouble() )
				{
					switch ( Utility.Random( 3 ) )
					{
						case 0: // Fire column
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 5052 );
							Effects.PlaySound( g, g.Map, 0x208 );

							break;
						}
						case 1: // Explosion
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36BD, 20, 10, 5044 );
							Effects.PlaySound( g, g.Map, 0x307 );

							break;
						}
						case 2: // Ball of fire
						{
							Effects.SendLocationParticles( EffectItem.Create( g.Location, g.Map, EffectItem.DefaultDuration ), 0x36FE, 10, 10, 5052 );

							break;
						}
					}
				}
			}
		}

		public InstantGoldShower( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 ); // version

			writer.Write( (int) m_MinGold );
			writer.Write( (int) m_MaxGold );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();

			m_MinGold = reader.ReadInt();
			m_MaxGold = reader.ReadInt();
		}
	}
}
