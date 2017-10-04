// Created by Xavier a.k.a. Wangchung
using System; 
using System.Collections; 
using Server.Accounting;
using Server.Items; 
using Server.Misc; 
using Server.Mobiles;
using Server.Network; 
using Server.Multis; 

namespace Server.Items 
{ 
public class BankStone : Item
	{ 

      		[Constructable] 
      		public BankStone() : base( 0x1870 ) 
      		{ 
         		Name = "a bank Stone"; 
         		Movable = true;
                  LootType = LootType.Blessed;
	 		Hue = 1161; 
      		} 

      		public BankStone( Serial serial ) : base( serial ) 
      		{ 
      		} 

      		public override bool HandlesOnSpeech{ get{ return true; } } 

      		public override void OnSpeech( SpeechEventArgs e ) 
      		{ 
         		if ( !e.Handled && e.Mobile.InLOS( this ) ) 
				{ 
					foreach (var keyword in e.Keywords)
					{
						switch (keyword)
						{
							case 0x0000: // *withdraw*
							{
								e.Handled = true;
								if (e.Mobile.Criminal)
								{
									// I will not do business with a criminal!
									this.Say(500389);
									break;
								}

								var split = e.Speech.Split(' ');

								if (split.Length >= 2)
								{
									int amount;

									var pack = e.Mobile.Backpack;

									if (!int.TryParse(split[1], out amount))
									{
										break;
									}

									if ((!Core.ML && amount > 5000) || (Core.ML && amount > 60000))
									{
										// Thou canst not withdraw so much at one time!
										this.Say(500381);
									}
									else if (pack == null || pack.Deleted || !(pack.TotalWeight < pack.MaxWeight) ||
											 !(pack.TotalItems < pack.MaxItems))
									{
										// Your backpack can't hold anything else.
										this.Say(1048147);
									}
									else if (amount > 0)
									{
										var box = e.Mobile.FindBankNoCreate();

										if (box == null || !box.ConsumeTotal( typeof( Gold ), amount ))
										{
											// Ah, art thou trying to fool me? Thou hast not so much gold!
											this.Say(500384);
										}
										else
										{
											pack.DropItem(new Gold(amount));

											// Thou hast withdrawn gold from thy account.
											this.Say(1010005);
										}
									}
								}
							}
							break;
							
							case 0x0001: // *balance*
							{
								e.Handled = true;

								//var box = e.Mobile.FindBankNoCreate();
								
								if (e.Mobile.Criminal)
								{
									// I will not do business with a criminal!
									this.Say(500389);
									break;
								}

								if (AccountGold.Enabled && e.Mobile.Account != null)
								{
									this.Say(String.Format(
										"Thy current bank balance is {0:#,0} platinum and {1:#,0} gold.",
										e.Mobile.Account.TotalPlat,
										e.Mobile.Account.TotalGold));
								}
								else
								{
									this.Say(String.Format(
										"Thy current bank balance is {0:#,0} gold.", Banker.GetBalance(e.Mobile).ToString("#,0")));
								}
							}
							break;
							
							case 0x0002: // *bank*
							{
								e.Handled = true;

								if (e.Mobile.Criminal)
								{
									// Thou art a criminal and cannot access thy bank box.
									this.Say(500378);
									break;
								}

								e.Mobile.BankBox.Open();
							}
							break;
						
							case 0x0003: // *check*
							{
								e.Handled = true;

								if (e.Mobile.Criminal)
								{
									// I will not do business with a criminal!
									this.Say(500389);
									break;
								}

								if (AccountGold.Enabled && e.Mobile.Account != null)
								{
									this.Say("We no longer offer a checking service.");
									break;
								}

								var split = e.Speech.Split(' ');

								if (split.Length >= 2)
								{
									int amount;

									if (!int.TryParse(split[1], out amount))
									{
										break;
									}

									if (amount < 5000)
									{
										// We cannot create checks for such a paltry amount of gold!
										this.Say(1010006);
									}
									else if (amount > 1000000)
									{
										// Our policies prevent us from creating checks worth that much!
										this.Say(1010007);
									}
									else
									{
										var check = new BankCheck(amount);

										var box = e.Mobile.BankBox;

										if (!box.TryDropItem(e.Mobile, check, false))
										{
											// There's not enough room in your bankbox for the check!
											this.Say(500386);
											check.Delete();
										}
										else if (!box.ConsumeTotal(typeof(Gold), amount))
										{
											// Ah, art thou trying to fool me? Thou hast not so much gold!
											this.Say(500384);
											check.Delete();
										}
										else
										{
											// Into your bank box I have placed a check in the amount of:
											this.Say(String.Format("Into your bank box I have placed a check in the amount of: {0}", AffixType.Append, amount.ToString("#,0"), ""));
										}
									}
								}
							}
							break;
						}
					}
				} 

         		base.OnSpeech( e ); 
      		} 

      		public void Say( int number ) 
      		{ 
         	PublicOverheadMessage( MessageType.Regular, 0x3B2, number ); 
      		} 

      		public void Say( string args ) 
      		{ 
         	PublicOverheadMessage( MessageType.Regular, 0x3B2, false, args ); 
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
