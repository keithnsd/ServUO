using System;
using System.Collections;
using Server.Multis;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class PirateBag : BaseContainer
    {
        private Mobile m_Owner;
        private string m_PlayerTitle;
        private int m_PlayerHue;
        private int _playerBankMaxItems;
        private int _playerBackpackMaxItems;
        [CommandProperty(AccessLevel.GameMaster)]
        public Mobile Owner
        {
            get { return m_Owner; }
            set { m_Owner = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string PlayerTitle
        {
            get { return m_PlayerTitle; }
            set { m_PlayerTitle = value; InvalidateProperties(); }
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public int PlayerHue
        {
            get { return m_PlayerHue; }
            set { m_PlayerHue = value; InvalidateProperties(); }
        }

        //[CommandProperty(AccessLevel.Developer)]
        public int PlayerBankMaxItems
        {
            get { return _playerBankMaxItems; }
            set { _playerBankMaxItems = value; }
        }

        //[CommandProperty(AccessLevel.Developer)]
        public int PlayerBackpackMaxItems
        {
            get { return _playerBackpackMaxItems; }
            set { _playerBackpackMaxItems = value;}
        }
        public override int DefaultGumpID { get { return 0x3D; } }
        public override int DefaultDropSound { get { return 0x48; } }

        public override Rectangle2D Bounds
        {
            get { return new Rectangle2D(29, 34, 108, 94); }
        }

        [Constructable]
        public PirateBag(Mobile m): base(0xE76)
        {
            Movable = false;
            Visible = false;
            Weight = 2.0;
            Name = "Pirate Curse Bag";

            m_Owner = m;
            m_PlayerTitle = m.Title;
            m_PlayerHue = m.Hue;
            _playerBankMaxItems = m_Owner.BankBox.MaxItems;
            _playerBackpackMaxItems = m_Owner.Backpack.MaxItems;
        }

        public PirateBag(Serial serial): base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
  
            writer.Write(m_Owner);
            writer.Write((string)m_PlayerTitle);
            writer.WriteEncodedInt((int)m_PlayerHue);
            writer.Write((int)_playerBankMaxItems);
            writer.Write((int)_playerBackpackMaxItems);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_PlayerTitle = reader.ReadString();
            m_PlayerHue = reader.ReadEncodedInt();
            _playerBankMaxItems = reader.ReadInt();
            _playerBackpackMaxItems = reader.ReadInt();
        }
    }
}
