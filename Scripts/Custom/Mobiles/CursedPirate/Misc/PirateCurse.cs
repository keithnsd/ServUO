using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
   
    public abstract class PirateCurse
    {

        private static bool _isRunning;     

        public static bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }


        public static void CursedPirateLoot(BaseCreature pirate, int chance)
        {

            switch (Utility.Random(chance))
            {
                case 0: pirate.PackItem(new CursedJugOfRum()); break;
            }
        }

        public static void SummonPirate(Mobile m, int i_type)
        {

            Map map = m.Map;
            if (map == null)
                return;

            bool validLocation = false;
            Point3D loc = m.Location;

            for (int j = 0; !validLocation && j < 10; ++j)
            {
                int x = loc.X + Utility.Random(3) - 1;
                int y = loc.Y + Utility.Random(3) - 1;
                int z = map.GetAverageZ(x, y);

                if (validLocation = map.CanFit(x, y, loc.Z, 16, false, false))
                    loc = new Point3D(x, y, loc.Z);
                else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                    loc = new Point3D(x, y, z);
            }

            if (!validLocation)
                return;

            BaseCreature spawn;

            switch (i_type)
            {
                default: return;
                case 0:
                    {
                        m.SendMessage(2211, "You have summoned a cursed pirate!");

                        spawn = new CursedPirate();
                        break;
                    }
                case 1:
                    {
                        m.SendMessage(2211, "Uh oh, you have summoned the cursed pirate king");

                        spawn = new CursedPirateKing();
                        break;
                    }
            }

            spawn.FightMode = FightMode.Closest;

            spawn.MoveToWorld(loc, map);
            spawn.Combatant = m;
        }

        public static void HurtPlayer(Mobile m)
        {

            m.SendMessage(2211, "Ouch, that hurts");

            AOS.Damage(m, m, Utility.RandomMinMax(30, 150), 0, 100, 0, 0, 0);

            if (m.Alive && m.Body.IsHuman && !m.Mounted)
                m.Animate(20, 7, 1, true, false, 0); // take hit
        }

      
        public static void CursePlayer(Mobile m)
        {

            if (IsCursed(m))
            {
                m.SendMessage(2211, "You are already cursed!");
                return;
            }

            if (!UnEquipPlayer(m))
            {
                return;
            }
            
            ExpireTimer _timer = (ExpireTimer)m_Table[m];

            if (_timer != null)
            {
                _timer.DoExpire();
            }
            else
            {
                m.SendMessage(2211, "You feel yourself transform into a cursed pirate");
            }

            Effects.SendLocationEffect(m.Location, m.Map, 0x3709, 28, 10, 0x1D3, 5);
            Effects.PlaySound(m.Location, m.Map, 0x182); //0x183 Ghost5.wav

            TimeSpan duration = TimeSpan.FromSeconds(240.0); 
            TimeSpan intervalDuration = TimeSpan.FromSeconds(30.0);

            List<ResistanceMod> _mods = new List<ResistanceMod>(4);

            _mods.Add(new ResistanceMod(ResistanceType.Fire, -10));
            _mods.Add(new ResistanceMod(ResistanceType.Poison, -10));
            _mods.Add(new ResistanceMod(ResistanceType.Cold, +10));
            _mods.Add(new ResistanceMod(ResistanceType.Physical, +10));

            _timer = new ExpireTimer(m, _mods, duration, intervalDuration);
            _timer.Start();
            IsRunning = (_timer.Running = true);
            m_Table[m] = _timer;

            for (int i = 0; i < _mods.Count; ++i)
                m.AddResistanceMod(_mods[i]);

            m.ApplyPoison(m, Poison.Greater);
            m.Criminal = true;

        }

        public static bool IsCursed(Mobile m)
        {

            BankBox bankBox = m.BankBox;

            if (bankBox == null)
            {
                return false;
            }

            Item piratebag = bankBox.FindItemByType(typeof(PirateBag));
            if (piratebag == null)
            {
                return false;
            }

            return true;
        }

        public static bool UnEquipPlayer(Mobile m)
        {

            ArrayList ItemsToMove = new ArrayList();
            PirateBag pirateBag = new PirateBag(m);
            BankBox bankBox = m.BankBox;
           
            if (bankBox == null)
            {               
                pirateBag.Delete();
                return false;
            }
            else
            {             
                //if MaxItems is greater than TotalItems increase MaxItems by 1
                //else: Make max items equal to TotalItems + 1
                m.BankBox.MaxItems = m.BankBox.MaxItems > m.BankBox.TotalItems
                    ?  m.BankBox.MaxItems + 1 : m.BankBox.TotalItems + 1;

                m.BankBox.AddItem(pirateBag);

                foreach (Item item in m.Items)
                {
                    if (item.Layer != Layer.Bank &&
                        item.Layer != Layer.Hair &&
                        item.Layer != Layer.FacialHair &&
                        item.Layer != Layer.Mount &&
                        item.Layer != Layer.Backpack)
                    {
                        ItemsToMove.Add(item);
                    }
                }

                foreach (Item item in ItemsToMove)
                {
                    m.BankBox.MaxItems += 1; 
                    pirateBag.AddItem(item);
                }
               
                m.Title = "the Cursed Pirate";
                m.Hue = Utility.RandomMinMax(0x8596, 0x8599);
                EquipPirateItems(m);
            }

            return true;
        }

        private static void EquipPirateItems(Mobile player)
        {

            //Equip the cursed pirate garb.
            //Cutlass
            Cutlass cutlass = new Cutlass();
            cutlass.Movable = false;
            cutlass.Skill = SkillName.Swords;
            cutlass.Layer = Layer.OneHanded;
            player.AddItem(cutlass);

            //Fancy Shirt
            FancyShirt shirt = new FancyShirt(Utility.RandomNeutralHue());
            shirt.Movable = false;
            player.AddItem(shirt);

            //Long Pants
            LongPants pants = new LongPants(Utility.RandomNeutralHue());
            pants.Movable = false;
            player.AddItem(pants);

            //Tricorne Hat
            TricorneHat hat = new TricorneHat(Utility.RandomNeutralHue());
            hat.Movable = false;
            player.AddItem(hat);

            //Thigh Boots
            ThighBoots boots = new ThighBoots();
            boots.Movable = false;
            player.AddItem(boots);
        }

        public static bool UnequipPirateItems(Mobile m)
        {

            ArrayList ItemsToMove = new ArrayList();
            ArrayList ItemsToDelete = new ArrayList();
            Bag deletePirateGearBag = new Bag();
            BankBox bankBox = m.BankBox; 

            if (bankBox == null)
            {
                deletePirateGearBag.Delete();
                return false;
            }
            else
            {
                bankBox.MaxItems = bankBox.TotalItems > bankBox.MaxItems ?
                bankBox.TotalItems + 1 : bankBox.MaxItems + 1;
                bankBox.AddItem(deletePirateGearBag);
            }

            Bag playerUnequippedItems = new Bag();
            playerUnequippedItems.Name = "Pirate Curse Unequiped Items";
            playerUnequippedItems.Hue = m.Hue;

            Container pack = m.Backpack;
            
            if (pack == null)
            {
                playerUnequippedItems.Delete();
                return false;
            }
            
            pack.MaxItems = pack.TotalItems > pack.MaxItems ?
                 pack.TotalItems + 1 : pack.MaxItems + 1;

            pack.AddItem(playerUnequippedItems);

            foreach (Item item in m.Items)
            {
                if (item.Layer != Layer.Bank && item.Layer != Layer.Hair && item.Layer != Layer.FacialHair && item.Layer != Layer.Mount && item.Layer != Layer.Backpack)
                {
                    if (item.Layer == Layer.OneHanded || item.Layer == Layer.Shirt || item.Layer == Layer.Pants || item.Layer == Layer.Helm || item.Layer == Layer.Shoes)
                        ItemsToDelete.Add(item);
                    else
                        ItemsToMove.Add(item);
                }
            }

            foreach (Item item in ItemsToDelete)
            {
                bankBox.MaxItems = bankBox.TotalItems > bankBox.MaxItems ?
                bankBox.TotalItems + 1 : bankBox.MaxItems + 1;
                deletePirateGearBag.AddItem(item);
            }

            deletePirateGearBag.Delete();
           
            foreach (Item item in ItemsToMove)
            {               
                pack.MaxItems = pack.TotalItems > pack.MaxItems ?
                    pack.TotalItems + 1 : pack.MaxItems + 1;
                playerUnequippedItems.AddItem(item);
            }

            RestorePlayerItems(m);
            playerUnequippedItems.Delete();

            return true;
        }

        public static bool RestorePlayerItems(Mobile player)
        {

            ArrayList ItemsToEquip = new ArrayList();
            BankBox bankBox = player.BankBox;

            if (bankBox == null)
            {
                return false;
            }

            PirateBag pirateBag = (PirateBag)bankBox.FindItemByType(typeof(PirateBag));

            if (pirateBag == null)
            {
                return false;
            }

            foreach (Item item in pirateBag.Items)
            {
                ItemsToEquip.Add(item);
            }

            Container pack = player.Backpack;

            foreach (Item item in ItemsToEquip)
            {
                pack.MaxItems = pack.TotalItems > pack.MaxItems ?
                    pack.TotalItems + 1 : pack.MaxItems + 1;
                player.AddItem(item);
            }
            
            player.Title = pirateBag.PlayerTitle;
            player.Hue = pirateBag.PlayerHue;
            //reset players containers MaxItems 
           bankBox.MaxItems = pirateBag.PlayerBankMaxItems;
            pack.MaxItems = pirateBag.PlayerBackpackMaxItems;

            pirateBag.Delete();

            return true;
        }

        public static void RemoveCurseTimerExpired(Mobile m)
        {

            UnequipPirateItems(m);
           
            m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Fire, -10));
            m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, -10));
            m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Cold, +10));
            m.RemoveResistanceMod(new ResistanceMod(ResistanceType.Physical, +10));

            if (m.Criminal)
            {
                m.Criminal = false;
            } 
            
            if (m.Poisoned)
            {
                m.CurePoison(m);
            }

            
            Effects.PlaySound(m.Location, m.Map, 0x381); //0x382 Ghost5.wav     
            m.SendMessage(2211, "The effects of the pirate curse have been removed");
        }
      
        public static bool RemoveCurse(Mobile m)
        {

            ExpireTimer t = (ExpireTimer)m_Table[m];

            if (t == null || m == null)
            {               
                if (IsCursed(m) )
                {
                    RemoveCurseTimerExpired(m);
                }
                return false;
            }

            m.SendMessage(2211, "The effects of the pirate curse have been removed");           
            t.DoExpire();

            return true;
        }

        private static Hashtable m_Table = new Hashtable();

        private class ExpireTimer : Timer
        {

            private Mobile m_Mobile;
            private List<ResistanceMod> m_Mods;

            public ExpireTimer(Mobile m, List<ResistanceMod> mods,
                TimeSpan delay, TimeSpan interval) : base(delay, interval)
            {
                m_Mobile = m; 
                m_Mods = mods;
            }

            public void DoExpire()
            {

                UnequipPirateItems(m_Mobile);

                for (int i = 0; i < m_Mods.Count; ++i)
                {
                    m_Mobile.RemoveResistanceMod(m_Mods[i]);
                }                    

                if (m_Mobile.Criminal)
                {
                    m_Mobile.Criminal = false;
                }
                    
                if (m_Mobile.Poisoned)
                { 
                    m_Mobile.CurePoison(m_Mobile);
                }

                Effects.PlaySound(m_Mobile.Location, m_Mobile.Map, 0x381); //0x382 Ghost5.wav

                if (this.Running)
                {
                    this.Stop();
                    IsRunning = (this.Running = false);
                    
                    m_Table.Remove(m_Mobile);
                }               
            }

            protected override void OnTick()
            {

                if (m_Mobile == null)
                {
                    return;
                }

                if (!m_Mobile.Alive || m_Mobile.Blessed)
                {
                    if (m_Mobile.Alive && !m_Mobile.Blessed)
                    {
                        DoExpire();
                    }
                }
                else
                {
                    m_Mobile.SendMessage(2211, "You feel the effects of the pirate curse wear off");
                    DoExpire();
                }
            }
        }
    }
}