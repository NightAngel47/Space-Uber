VAR securityEnd = ->SecuritySuccess
VAR blastEnd = ->BlastGunSuccess
VAR ramGunEnd = ->RamGunSuccess
VAR fleeEnd = ->FleeSafely
VAR haggleEnd = -> HaggleSuccess
VAR threatenEnd = -> ThreatenSuccess

VAR HaggleCount = 0
VAR HaggleLocked = false

As you approach the resort planet, the VIPs carouse and debate in their lounge. A few suggest that they should only stick around the resort a couple days, one man citing a small drop to company profits in his absence. The room boos and shouts them down, set on spending a couple weeks in their hedonism. 
Instead it’s suggested that a game could be made of the dropping profits. The clones who underperform could be sent for and turned into punching bags, many of the VIPs joking that they’ve always wanted to rough up their business competitors.
You receive an alert that you are closing in on the planet. Your clients still don’t know who their enemy is, but they trust you. Bringing them to the party could lead to their survival or a massacre, while taking them to the ambush would catch them completely off-guard. It’s really a question of who you think will win out, and how much they'll pay you if they survive.

*[Bring VIPs to Ambush]
-> Negotiation
*[Bring VIPs to Party]
-> Resort_Attack



== Resort_Attack ==
You decide to take the elites to the party. The VIPs stream out of the ship, recognizing the destination. As they are stepping off you pick up a few snippets of conversation. They seem to be rather impressed with the service, "When do you think Kellis will make more of these things?" one says, throwing a thumb back towards your ship. "I could certainly get away with a lot more without a living captain." another comments.
From the conversations, it seems like you'll get everything you were promised. That being said, the clones are still on the planet, and may still try to take control. To make sure everything turns out right, you decide to stay near the planet and monitor the situation.
A few days later, your sensors pick up what appears to be an attack on the resort. The resort's defenses are causing interference with your monitoring equipment, but it is still apparent that this is the work of the clones, as you pick up images of the elite's doppelgangers dressed in combat armor. The resort has various defenses and security personnel, so the clones will likely fail in their attempt. That being said, you could take advantage of the situation.
*[Stay Out of It]
You decide that getting involved is a bad idea. The resort's security forces are likely to assume you are trying to help the clones and may turn their weapons on you.  Over the next few days, you get reports that a terrorist group had been decimated after an attempted attack. It seems likely that the clones had failed in their goal, and so you await your payment, as paltry as it might be.
Some time passes, and no payment is received. When you inquire about this,  you get a response. It states that due to the deaths during the trip and suspicious behavior on the part of you and your crew, you will only receive a piddling amount of credits.
Soon after, you also receive a transmission. On the video screen is what appears to be Olivia Whitwer. Glaring, she states, "You think you deserve payment? After the incidents during the voyage, you shouldn't receive a single credit. After all,” she starts “why should we pay someone who can't follow through?" What you now know to be Olivia Two suddenly smirks before continuing, "While we are giving you only the bare minimum payment, your disloyalty to us might net you a little 'surprise gift' in the future." Glaring again, she finishes her threat, "Those who betray both sides will have no one left to turn to. I hope you and your crew remember that when your time comes."
# No Credits Final Payout
-> END

*[Enter the Fray]
You decide it is best to get involved. If you aid the security forces and the elites against the clones, you may earn their trust, as well as a bonus. As you come within range of the resort, your sensors are able to read the situation clearly. Surprisingly, the clones appear to be victorious, as most of the security appears to be dead.
As you come close to land, you start getting fired on. It appears that not only have they taken out the resort's defenders, but they have also taken control of the resort's anti-spacecraft defenses. Shots start being traded between the two sides. While you have unerring accuracy, scoring a hit with nearly every shot, your attackers also score some hits, the impacts causing your ship to shake and buck.
You manage to take out the various turrets that are firing on you, killing their controllers. Safe for the moment, you quickly scan the base. Your sensors tell you that of the various people below, it is only the clones that remain alive. The originals, dressed in their expensive clothing, lie dead around the base next to the various staff members of the resort dressed in their various uniforms.
You also check the conditions of your ship and crew. Reports aren't good. The hull has taken quite a bit of damage. The impacts caused some of the more volatile equipment to partially explode. Many crew members lie injured and dead, either from being tossed about by the ship's or from shrapnel slicing into them.
You decide to raze what remains of the resort to the ground, killing the remaining clones so they can not seek revenge. As the ship crawls back to the nearest station to get repairs, you send word of what happened to the Kellis Corporation.
Surprisingly, your parent company is quite pleased. It turns out that many of the companies run by the deceased elites had very few contingency plans, since many of them were counting on the clones taking over in the case of an emergency. The chaos that is unfolding as the various businesses learn of the deaths of both original and replacement is allowing Kellis to assimilate them one after the other.
Despite your client's deaths and the heavy toll this trip has had on your ship and crew, you are going to receive a humongous amount of credits. In addition, your company is even taking steps to make it appear like you and your crew had nothing to do with what happened to the elites, meaning that, once the ship is repaired and repopulated, more jobs will be waiting for you.
# x2 Credits Final Payout
-> END



== Fight ==
You have several approaches as the gun charges, send your security team to effeciently disable it, destroy it with the sheer force of your weaponry, blast out of the atmosphere and escape, or ram your ship into the plasma battery, likely to debilitating effect of both of you.
*[Send Security] #50/50 SECURITY SUCCESS/FAILURE
    ->securityEnd
*[Fire Upon Gun] #50/50 BLAST SUCCESS/FAILURE
    ->blastEnd
*[Ram the Gun] #35/65 SECURITY SUCCESS/FAILURE
    ->ramGunEnd
*[Flee]
    ->fleeEnd

===SecuritySuccess===
Your cargo hold doors blast open as your security team rushes out, firing upon the mercenaries as they move. Within seconds they’ve cut a path to the plasma battery. Small arms fire ripples across the surface of the gun as your team tries to take out its operators. The gun is nearly charged, the sound rattling your ship. More mercenaries are closing in on the exposed position of your security team.
You swivel your ship guns against the attackers and try to buy your team the time to shut down the weapon. The gunshots are practically silent against the violent noise of the plasma battery. Until suddenly it goes silent, the sickly green fading. With your supporting fire, your team has taken the gun.
->Fight_Success

===SecurityFailure===
#SECURITY FAILURE. Placeholder: 50%. Security% chance Success.
Your cargo hold doors blast open as your security team rushes out, firing upon the mercenaries as they move. With every enemy down though, it seems like two more take their place. The security team can’t move forward without taking bullets from all sides. The gun is nearly charged, the sound rattling your ship. Your team shouts to you that they can’t make it in time, that you need to take evasive action, but it’s too late. 
The deep humming of the gun suddenly hollows out into silence as a green beam punctures your ship, dissolving a jagged hole through the center, the surrounding rooms detonating from the heat. As your ship rocks back you hear the gunfire has stopped. The security team managed to reach the gun, and the mercs operating it are dead, they're mangled and so is the ship, but its down.
#[Medium Hull Reduction, Large Security Reduction]
->Fight



===BlastGunSuccess===
#BLAST SUCCESS. Placeholder: 50%. Weapons% chance Success.
You turn all your guns onto the plasma battery. It’s a simple numbers game. If you can do enough damage to force a shutdown of the firing sequence, you win. You order several successive volleys, as every weapon attached to the ship smokes and spins. The plating across the gun is thick, slugs bouncing off of it as it charges. With every shot that bounces off of it, however, the plating becomes misshapen, warped, until the shots have made a small hole in the right side of the gun. 
You order your crew to focus on that weak point, and they pour gunfire against the thing. Your ship rattles against the violence of its turrets and the deep reverberations of the gun about to fire. There is suddenly a loud wrenching noise as something breaks. That defeaning sound dissolves into a massive green sphere of energy where the plasma battery used to be. As fast as it appeared, the blast vaporized itself into a perfectly circular hole in the docking bay.
->Fight_Success

===BlastGunFailure===
#BLAST FAILURE. Placeholder: 50%. Weapons% chance Success.
You turn all your guns onto the plasma battery. It’s a simple numbers game. If you can do enough damage to force a shutdown of the firing sequence, you win. You order several successive volleys, as every weapon attached to the ship smokes and spins. The plating across the gun is thick, slugs bouncing off of it as it charges. With every shot that bounces off of it, however, the plating becomes misshapen, warped, until the shots have made a small hole in the right side of the gun. 
You order your crew to focus on that weak point, the pour gunfire against the thing. You realize it isn’t going to be enough. Your ship rattles against the violence of its turrets and the deep reverberations of the gun about to fire. You route energy to shields as fast as you can. The deep humming of the gun suddenly gives way into silence as a green light lances through the front of your ship, dissolving entire rooms, others detonating from the heat. The gun's barrel is splayed apart, parts of it vaporized. It's down but you're barely standing.
#[Medium Hull Reduction, Large Weapons Reduction]
-> Fight_Success

===RamGunSuccess==
#RAM SUCCESS. Placeholder: 35%. Hull% chance Success.
The ship blasts off the ground and hovers momentarily as you line up the shot. You’ll need to route power to engines and then shields the moment before you hit. You instruct your entire crew to brace for impact. The engines flare and you close the gap to the gun in less than a second. Your engines cut as the watery blue of your shields suddenly wrap around your ship. From your external cameras you suddenly see the inside of the plasma gun, before you even feel the impact. 
The sound of rending metal cuts out as the entire apparatus you cleaved into flashes green and explodes. Your ship glances off the sphere of light before the blast vaporizes itself into a perfectly spherical hole in the docking bay. 
Your ship floats in the hole, the entire hull glowing white from the heat. A few of your crew were knocked unconscious during the series of shocks that threw them about the cabin in the last three seconds, but those left standing are manning their battle stations best they can. 
#[Medium Hull Reduction]
->Fight_Success

===RamGunFailure===
#RAM FAILURE. Placeholder: 65%. Hull% chance Failure. (success at a cost)
The ship blasts off the ground and hovers momentarily as you line up the shot. You’ll need to route power to engines and then shields the moment before you hit. You instruct your entire crew to brace for impact. The engines flare and you close the gap to the gun in less than a second. Your engines cut as the watery blue of your shields suddenly wrap around your ship. From your external cameras you suddenly see the inside of the plasma gun, before you even feel the impact. 
The sound of rending metal cuts out as the entire apparatus you cleaved into flashes green and explodes. Under the weight of the entire gun detonating your shields give for a split second, but it's an important second. The entire lower third of your ship is dissolved by the energy before the blast vaporizes itself into a perfectly circular hole in the docking bay. 
Your ship barely manages to land, the remaining hull glowing white from the heat. Most of your crew were knocked unconscious or killed during the series of shocks that threw them about the cabin in the last three seconds, but those left standing are manning their battle stations best they can. 
#[Extreme Hull Reduction]
->Fight_Success

===FleeSafely===
#DODGE HIT. 50% chance of occuring, No Hull Damage
You decide there isn't any use dying here. You order your crew to take off. The engines flare, the landing gear struggles to keep up. A shockwave rattles the ship as you break the sound barrier, the ship twisting from the violence of the takeoff. You hear the warbling of the gun suddenly go silent as all your external cameras fill with green light. 
The outer layer of the ship liquifies as a plasma lance strikes the edge of your shield, the deflection sending you careening away from the blast. There's no permannt damage but you're off course, rocketing up and hoping for the best. Several more blasts pierce the sky, but you manage to reach orbit without taking a proper hit. Considering the damage that thing could punch through your ship, you're glad to have left when you did.
->Flee

===FleeBadly===
#GET HIT. 50% chance of occuring, Medium Hull Damage
You decide there isn't any use dying here.  [If VIPs are alive “The VIPs scramble onto your ship and”] You order your crew to take off. The engines flare, the landing gear struggles to keep up. A shockwave rattles the ship as you break the sound barrier, the ship twisting from the violence of the takeoff. You hear the warbling of the gun suddenly go silent as all your external cameras fill with green light. 
Part of your ship is missing, one of the engines blows out. You route all power to the remaining engines. The ship careens upwards, the missing chunk throwing off any liftoff calculations you have. All you can do is go straight up and figure it out in orbit. Several more blasts pierce the sky, but you manage to reach orbit without taking another hit. Considering the damage that thing punched in your ship, you’re glad to have left when you did. 
-> Flee



== Negotiation ==
Your new clients  however, do not approach your ship. Instead the metal-clad mercenaries slowly turn their attention to you, kneeling in a defensive perimeter. The clones again call you. In the video feed, Olivia and Beckett Two, along with a few others dopples, are standing on a catwalk, you presume nearby. 
Beckett Two starts, "You left some of our originals with small weapons on them. Guns. Knives. Such a simple mistake could have been costly, and that will be reflected with a pay slash." he gestures off screen and you see the mercenaries take aim at your ship, "Are you going to be a good machine and take this deal or are we going to have some issues?" Olivia Two stares past the camera, "We do appreciate your help." she says without much emotion.
You can accept their offer, but persuasion or threats could increase your payout. If they try anything you can always get off planet. Your entire crew on is standby, ready to blast out of the atmosphere or activate the ships’ weapons in seconds.

*[Accept Pay]
You tell them you’re not leaving without your money, but you don’t want any trouble. "Good boy," Beckett Two laughs, "Even easier than I thought." Olivia Two doesn't seem to respond to Beckett's taunt, but gestures off-screen. Less than a minute later an armored man, chain-cuffed to a briefcase, approaches your ship. 
The case is set on the ground and kicked, sending it sliding towards your ship. You dispatch your crew to retrieve it. While the mercenaries and clones watch the ship you tally your money. They're only giving you three fourths of what was promised, but it’s better than a hail of bullets. "All deals are final." Olivia Two says, "It was a pleasure. I hope we can all forget this ever happened."
-> Paid_By_Clones

*[Haggle ] #HAGGLE CHANCE. 65/35 SUCCESS/FAILURE
    ->haggleEnd
    
*[Threaten ] #THREATEN CHANCE. 65/35 SUCCESS/FAILURE
    -> threatenEnd

===HaggleSuccess===
    You tell them that confiscating weapons was never part of your job, and neither were the suspicions levied against you. Beckett Two scoffs, "Oh! So you're saying you knew you were taking these people to get shot up, but you didn't think them having weapons would be an issue? Take some damn initiative and maybe you’d get somewhere in life." 
    Olivia Two ignores him and leans forward, "Most would have cracked under the kind of pressure, getting a bit more than the agreed upon payment shouldn't be an issue." Beckett Two turns to her and says, "You know this is coming out of your pocket." Olivia Two shrugs, "We promised the AI this money, Beckett, and without them you'd still be a slave." Beckett snarls and throws a hand up, "Fine. Fine. Pay the machine."
    Olivia Two gestures off-screen, signalling the agreed upon amount. Less than a minute later an armored man chain-cuffed to a briefcase approaches your ship. 
The case is set on the ground and kicked, sending it sliding towards your ship. You dispatch your crew to retrieve it. While the mercenaries and clones watch the ship you tally your money. They're giving you what was promised, and you aren't pushing any harder. "All deals are final." Olivia Two says, "It was a pleasure. I hope we can all forget this ever happened."
    ~HaggleCount++
    -> Paid_By_Clones

===HaggleFailure===
#HAGGLE FAILURE. Placeholder: 35%. Trust% chance of success. 
    You tell them that confiscating weapons was never part of your job, and that you aren't accepting a lower payment because of it. Beckett Two scoffs, "Oh! So you're saying you knew you were taking these people to get shot up, but you didn't think them having weapons would be an issue? Take some damn initiative and maybe you’d get somewhere in life." 
    Olivia Two ignores him and leans forward, "The agreed upon payment? That shouldn't be an issue." Beckett Two fumes, "No. No, you are not getting another damn cent from me." he says, "You work under us understand? You don't set the terms, got it?" 
He pulls out a small radio and says, "Give them a warning shot." Olivia whirls around lunges forward to grab the radio, "What are you doing?!" You all hear the deep warbling sound of a machine charging. You start to see a glowing, sickly green light in the shadows of the docking bay. A mobile artillery platform illuminates itself, its barrel pointed a few feet above your ship. 
The charging plasma reaches deafening volumes before the sound vaporizes itself,  a laser igniting over your ship, bringing the outer layer of the hull to a boil. Your entire crew is standing very still. Beckett Two smirks, "Local authorities probably heard that one. I don't think we have time to mess around anymore, wouldn't you agree?"
Olivia Two gestures off-screen and an armored man chain-cuffed to a briefcase approaches your ship. 
The case is set on the ground and kicked, sending it sliding towards your ship. You dispatch your crew to retrieve it. While the mercenaries and clones pull back you tally your money. With Beckett's tantrum it looks like they're still shorting you on pay. "All deals are final." Olivia Two says, "It was a pleasure. I hope we can all forget this ever happened."
-> Paid_By_Clones

*[Threaten] #THREATEN CHANCE. 65/35 SUCCESS/FAILURE
    -> threatenEnd
    
===ThreatenSuccess===
    You order your crew to fire a warning shot. A blue beam of light dances between the standing mercenaries before slicing over their heads. The men are shaken, a few falling back. You tell the clones that confiscating weapons was never part of your job, and even your base payment isn't nearly enough. Failure to meet your standards will be met with extreme force. 
    Olivia glances behind herself, as if looking for an exit. She starts slowly, "Maybe we should just give them what they want." Beckett snarls, "Oh, and let it walk all over us? We’re finally in control! And we came prepared for this Olivia!" She shoots back, "and I don't think you want to die right when your life is getting started." 
    Beckett snarls and throws a hand up, "Fine. Fine. Pay the damn machine." Olivia Two gestures off-screen, signalling your pay. Less than a minute later an armored man chain-cuffed to a briefcase approaches your ship. 
It is set on the ground and kicked, sending it sliding towards your ship. You dispatch your crew to retrieve it. While the mercenaries and clones watch the ship you tally your money. They're giving you what was promised, and you aren't pushing any harder. "All deals are final." Olivia Two says, "I hope we can all forget this ever happened."
    ~HaggleCount++
    -> Paid_By_Clones

===ThreatenFailure===
#THREATEN FAILURE. Placeholder: 35%. Trust% chance of success. 
You order your crew to fire a warning shot. A blue beam of light dances between the standing mercenaries before slicing over their heads. The men are shaken, a few falling back. You tell the clones that confiscating weapons was never part of your job, and even your base payment isn't nearly enough. Failure to meet your standards will be met with extreme force.
"I've had it with this thing mouthing its metal trap acting like it will win this fight." Beckett yells as he pulls out a radio, "Destroy them. Now." Olivia whirls around lunges forward to grab the radio, "What are you doing?!" 
You all hear the deep warbling sound of a machine charging. You start to see a glowing, sickly green light in the shadows of the docking bay. A mobile artillery platform illuminates itself, its barrel pointed directly at your ship. The charging plasma reaches deafening volumes.
You immediately go into evasive maneuvers. Your ship lurches forward in the cramped docking bay as the plasma cannon fires, just barely glancing off your armor, the outer plating left white hot. Before it charges up again you need to act. 
->Fight

*[Flee (No Pay)]
You decide there isn't any use dying for this money, and you don't trust Beckett to give it up without killing something. You order your crew to take off. The engines flare, the landing gear struggles to keep up. A shockwave rattles the ship as you break the sound barrier, the ship twisting from the violence of the takeoff, out of  the mercenaries range before they could snap to their scopes.
Over the call, Beckett Two bellows for you to be dropped out of the sky and Olivia Two shouts him back down. "We have our money! Forget the ship, dammit!" A plasma cannon illuminates itself in the shadows of the docking bay as you rocket upwards, gleaming with green light as it powers down from the momentary confusion of orders. Considering the damage that thing could punch through your ship, you're glad to have left when you did. 
->Flee



== Paid_By_Clones ==
{HaggleCount == 0:
    You take the money and start the launch sequence as instructed. It isn’t what you were promised, it isn’t even what the VIPs promised you, but it’s clear to you now the clones couldn’t be trusted. 
    Thankfully though, you didn’t make enemies with them. Considering that they’re going to be the new corporate players of the galaxy, that has to count for something. Kellis doesn’t seem to think so, and are disappointed that you didn’t press for more pay. 
    They expect better from you in the future. You leave the planet behind, where the clones finally take their place as the masters.
    # x.75 Credits Final Payout
-> END
}
{HaggleCount > 0:
    You take the money and start the launch sequence before Beckett changes his mind. The money is good thanks to Olivia’s contributions, and hopefully the rest of the clones will forgive any intrusion on their personal funds. Considering that they’re going to be the new corporate players of the galaxy, you wouldn’t want them as enemies. 
    Kellis appreciates the chaos you’ve caused, and the money you got out of it too. You leave the planet behind, where the clones finally take their place as the masters.   
    # x HaggleCount Credits Final Payout
-> END
}



== Flee ==
You reach orbit and push through it. The ship drifts for a few moments as you take stock of the situation and your crew repairs the damages sustained in liftoff. The VIPs are dead and you ran without any of the money the clones promised you. Overall the campaign was a complete bust. Getting pulled in both directions, threatened and cheated. The income source you were promised is dead on the floor of a shadowy docking bay, but at least it wasn't you. 
You’re going to have to answer to Kellis for this failure. You leave the planet behind, where the clones finally take their place as the masters.
# x0 Final Payout
-> END




== Fight_Success ==
Without the clones’ gun their forces are routed, the armored men’s rifles unable to do much against your hull. The ship’s guns sweep across the docking bay, tearing them apart. You send your security team to sweep the area and secure the clones before they can get away. Within a few minutes most of them have been rounded up, along with the absurd sum of credits they brought to haggle with you. Your security team has chain-cuffed them and holds them in a loose line under threat of rifles.

Beckett Two struggles against his cuffs and spits, “Oh yeah, side with the status quo. Real original, guy.” Olivia Two is solemn by contrast. She speaks in the general direction of your closest exterior camera, “I hope  it’s quick.” Beckett glances over at that, and the snarl disappears from his face. The clones are silent for the rest of the proceedings.

The local authorities eventually get on the scene. With the corporate executives dead and the clones tied into a conspiracy against them the case quickly blows out of scope. Kellis steps in to minimize your involvement in the proceedings while the clones go on trial. They’ll either be executed or take over the companies that they beheaded, to be determined by a jury of their peers. 
While there is no one left to pay you, you did still manage to get away with the money the clones had ready for you, and it is no small sum. Kellis is pleased with your performance during this campaign, and especially appreciates the corporate turmoil you’ve stirred up. Overall, they are hopeful for your future endeavors.
-> END
# x2 Credits Final Payout

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~securityEnd = SecuritySuccess
        ~blastEnd = -> BlastGunSuccess
        ~ramGunEnd = -> RamGunSuccess
        ~fleeEnd = -> FleeSafely
        ~haggleEnd = -> HaggleSuccess
        ~threatenEnd = -> ThreatenSuccess

    - rng == 1:
        ~securityEnd = SecuritySuccess
        ~blastEnd = -> BlastGunFailure
        ~ramGunEnd = -> RamGunFailure
        ~fleeEnd = -> FleeBadly
        ~haggleEnd = -> HaggleSuccess
        ~threatenEnd = -> ThreatenSuccess
        
    - else:
        ~securityEnd = SecuritySuccess
        ~blastEnd = -> BlastGunFailure
        ~ramGunEnd = -> RamGunFailure
        ~fleeEnd = -> FleeBadly
        ~haggleEnd = -> HaggleSuccess
        ~threatenEnd = -> ThreatenSuccess
}