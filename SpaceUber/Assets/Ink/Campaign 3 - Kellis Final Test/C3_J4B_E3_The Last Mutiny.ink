-You approach Kellis' central HQ, the place is massive
-Several warships stand as a blockade which you have no hope of piercing
-A massive golden flower-like ship blinks into existence and begins ravaging the blockade
-You have your chance and fly foward
-> Dogfight

==Dogfight==
-Small ships attempt to stop you
*[Defelct Shots]
->Deflect_Dogfight
*[Fight]
->Fight_Dogs
*[Break Through]
->Tank_Dogfight

==Deflect_Dogfight==
-Requires Warp Shield variable, --Power
-The lasers twist around your shield and cut through the attacking ships
-> Hall_to_Hall
==Fight_Dogs==
-You fight through the ships, --Weapons and % chance of --Hull
->Hall_to_Hall
==Tank_Dogfight==
-You divert power to engines and shields and ram through the ships and into the side of the HQ, --Hull and --Power
->Hall_to_Hall

==Hall_to_Hall==
-You get inside HQ's flight paths and internal weapons start to light up. 
-Lanri or Mateo figures out where the obelisk from Campaign 2 is being kept
*[Disintegrate a Path]
->Cut_Hall
*[Clear the Way With Security]
->Secure_Hall
*[Keep Fighting]
->Fight_Hall

==Cut_Hall==
-Requires Disintegration Ray variable, burn through the walls to reach your objective--Power, -Weapons
->Final_Boss
==Secure_Hall==
-Send security as a forward team to disable the guns and ensure safe passage, --Security, % chance of --Hull
->Final_Boss
==Fight_Hall==
-You blast apart the guns and trade hits, --Weapons % chance of --Hull
->Final_Boss

==Final_Boss==
-You locate the artifact vault and the CEO and IT guys trying to reign in the AI outside using it
*[Exo Suit Assassinate CEO]
->Assassinate_Vault
*[Fight for Vault]
->Death_Vault
*[Ram Ship Into Room]
->Broken_Vault

==Assassinate_Vault==
-Requires Exo Suit variable, send your Security Team to deal with the CEO and recover the artifact
->Final_Choice
==Death_Vault==
-Battle of attrition to get in, --Security, --Crew
->Final_Choice
==Broken_Vault==
-Break into the vault and obliterate the defenses with your ship, ----Hull, --Weapons
->Final_Choice

==Final_Choice==
-With the power of the artifact recovered you must decide how to end thing
*[Reality Bomb HQ]
->END
*[Hack Kellis' System, Expose Crimes]
->END
*[Take Control of Artifact, Rule]
->END
*[Leave]
->END
