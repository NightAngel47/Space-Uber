VAR securityEnd = ->SecuritySuccess
VAR blastEnd = ->BlastGunSuccess
VAR ramGunEnd = ->RamGunSuccess
VAR fleeEnd = ->FleeSafely
VAR haggleEnd = -> HaggleSuccess
VAR threatenEnd = -> ThreatenSuccess

VAR VIPsDead = false
VAR VIPsResort = false
VAR HaggleCount = 0
VAR HaggleLocked = false

As you approach the resort planet, the VIPs are using the time to discuss their clones. Most of the elites want this dealt with the way most of their problems are, they want it to go away. While execution is the simplest and clearest solution, one VIP forwards a prison he owns, hidden away on a forgotten moon. This is applauded by many as a humane solution. Still though, several of the elites sit docile, away from the group, the only hint at their plans voiced when they ask for the clones to be taken in alive.  

As you receive an alert that you are in close range of the planet, you consider the options you have before you. Your clients are well prepared for their enemy now, but they also trust you. Bringing them to the party as planned will certainly give them a chance to take cover and reclaim their companies, while taking them to the ambush could be used either to betray them or turn the clone’s ambush against itself.
*[Bring VIPs to Ambush]
-> Backup_Who
*[Bring VIPs to Party]
-> VIPs_Safe



== Backup_Who ==
Your passengers are well aware of the conspiracy against them, most of them producing small weapons from their waistcoats before you land. You land directly in the abandoned docking bay the clones pointed you to, a huge circular room with no roof, a tangle of shadows and catwalks along its walls. 
You know that whatever lurks here won’t be enough for the VIPs to handle on their own. Regardless, they step out of the cargo hold, brandishing their weapons. Gunshots ring out as metal-clad mercenaries lurch out of the shadows. The two sides exchange shots, bullets ricocheting off your ship and the VIPs fire arcing red beams from their sidearms. 
The clones are the more prepared faction of the two, but involving yourself in the fray could tip the scales to the elites’ advantage. The extra factor of your weapons and your security team can destabilize this entire fight. The question is who can you trust to pay you when the smoke clears.

*[Side with the Clones (Kill the VIPs)]
The corporate elites hold their own just long enough for you to make a difference, and you decide that that difference will be a knife in their backs. You turn your guns onto your former passengers and blast them apart. The mercenaries nod in your direction and pick through the bodies, checking and silencing survivors.
~VIPsDead = true
-> Negotiation

*[Side with the VIPs (Fight the clones)]
You order your crew to target the mercenaries, suddenly blue lights swing across the docking bay, cutting the metal-clad men in half before they can comprehend the betrayal. You distantly hear your elite passengers cheering until it is drowned out by the sound of a deep warbling sound of a machine charging. You start to see a sickly green light in the shadows of the docking bay, a mobile artillery platform. The VIPs dive for cover as the charging plasma reaches deafening volumes, you need to act.
-> Fight



== VIPs_Safe ==
 You arrive at the resort port and open your cargo bay. The VIPs step off, glancing around for any would-be assassins. As they are stepping off you pick up a few snippets of conversation, "Well Kellis and their damn ship didn’t do too bad." says one, another fidgets, "Well, it wasn’t exactly first class, but I suppose we could be dead." Some of them are already getting in contact with their companies through their headsets, inquiring about their clones. 
Before they get far, Olivia Whitwer approaches one of your monitors, “Ship, we need to talk.” she says, “Our clones are still waiting to pounce, and while the resort does have its defenses, I’ve only gotten this far by being more safe than sorry. We’re going to pay you no matter what, just consider this another job offer: Kill our clones. Take them by surprise, catch them up in their trap and end this for us. That way we don’t need to worry about a manhunt. If you can do that, consider your pay doubled.”
~VIPsResort = true

*[Go to Kill Clones] 
You arrive at the abandoned docking bay, without any of the targets the clones asked for. The bay is a huge circular room with no roof, a tangle of shadows and catwalks along its walls. You set down, leaving the bay doors to your ship closed. Several metal-clad men kneel, pointing assault weapons at your ship, waiting for sight of the originals. In the shadows, a huge plasma artillery cannon lurks, humming softly. As the clones call you and ask that you open your cargo hold, you realize you need to act.
-> Fight

*[Leave Planet(End Campaign)]
-> Paid_By_VIPs



== Fight ==
You have several approaches as the gun charges, send your security team to swiftly and effeciently disable it, destroy it with the sheer force of your weaponry, blast out of the atmosphere and escape, or ram your ship into the plasma battery likely to debilitating effect of both of you.
*[Send Security (Security%)] #50/50 SECURITY SUCCESS/FAILURE
    -->securityEnd
*[Fire Upon Gun(Weapons%)] #50/50 BLAST SUCCESS/FAILURE
    -->blastEnd
*[Ram the Gun(Hull%)] #35/65 SECURITY SUCCESS/FAILURE
    -->ramGunEnd
*[Flee(End Campaign)]
    -->fleeEnd

===SecuritySuccess===
Your cargo hold doors blast open as your security team rushes out, firing upon the mercenaries as they move. Within seconds they’ve cut a path to the plasma battery. Small arms fire ripples across the surface of the gun as your team tries to take out its operators. The gun is nearly charged, the sound rattling your ship. More mercenaries start to close in on the exposed position of your security team. You swivel your ship guns against the attackers and try to buy your team the time to shut down the weapon. The gunshots are practically silent against the violent noise of the plasma battery. Until suddenly it goes silent, the sickly green fading. 
    ->Fight_Success

===SecurityFailure===
Your cargo hold doors blast open as your security team rushes out, firing upon the mercenaries as they move. With every enemy down though, it seems like two more take their place. The security team can’t move forward without taking bullets from all sides. The gun is nearly charged, the sound rattling your ship. Your team shouts to you that they can’t make it in time, that you need to take evasive action, but it’s too late. The deep humming of the gun suddenly hollows out into silence as a green beam punctures your ship, dissolving a jagged hole through the center, the surrounding rooms detonating from the heat. Your security force rushes to get back to the smoking ship, several of them shot in the back by the approaching mercenaries. You need to find another way.
    #[Medium Hull Reduction, Large Security Reduction]
    ->Fight

===BlastGunSuccess==
    #BLAST SUCCESS. Placeholder: 50%. Weapons% chance Success.
    You turn all your guns onto the plasma battery. It’s a simple numbers game. If you can do enough damage for it to shut down before it fires, you win. You order several successive volleys, as every weapon attached to the ship smokes and spins. The plating across the gun is thick, slugs bouncing off of it as it charges. With every shot that bounces off of it, however, the plating becomes misshapen, warped, until the shots have made a small hole in the right side of the gun. 
    You order your crew to focus on that weak point, and they pour gunfire against the thing. Your ship rattles against the violence of its attack and the deep reverberations of the gun about to fire. There is suddenly a loud wrenching noise as something breaks and that sound dissolves into a massive green sphere of energy where the plasma battery used to be. As fast as it appeared, the blast vaporized itself into a perfectly spherical hole in the docking bay.
    ->Fight_Success
    
===BlastGunFailure===
#BLAST FAILURE. Placeholder: 50%. Weapons% chance Success.
You turn all your guns onto the plasma battery. It’s a simple numbers game. If you can do enough damage for it to shut down before it fires, you win. You order several successive volleys, as every weapon attached to the ship smokes and spins. The plating across the gun is thick, slugs bouncing off of it as it charges. And you start to realize it isn’t going to be enough. Your ship rattles against the violence of its attack and the deep reverberations of the gun about to fire. You route energy to shields as fast as you can. The deep humming of the gun suddenly gives way into silence as a green light lances through the front of you ship, dissolving entire rooms, others detonating from the heat. Your weapons are practically out of ammo. You need to find another way.
#[Medium Hull Reduction, Large Weapons Reduction]
-> Fight


===RamGunSuccess===
#RAM SUCCESS. Placeholder: 35%. Hull% chance Success.
The ship blasts off the ground and hovers momentarily as you line up the shot. You’ll need to route power to engines and then shields the moment before you hit. You instruct your entire crew to brace for impact. The engines flare and you close the gap to the gun in less than a second. Your engines cut as the watery blue of your shields suddenly wrap around your ship. From your external cameras you suddenly see the inside of the plasma gun, before you even feel the impact. 
The sound of rending metal cuts out as the entire apparatus your ship has cleaved into flashes green and explodes. Your ship glances off the sphere of light before the blast vaporizes itself into a perfectly spherical hole in the docking bay. Your ship floats in the hole, the entire hull glowing white from the heat. A few of your crew were knocked unconscious during the series of shocks that threw them about the cabin in the last three seconds, but those left standing are manning their battle stations best they can. 
#[Medium Hull Reduction]
->Fight_Success

===RamGunFailure=== 
#RAM FAILURE. Placeholder: 65%. Hull% chance Failure. (success at a cost)
The ship blasts off the ground and hovers momentarily as you line up the shot. You’ll need to route power to engines and then shields the moment before you hit. You instruct your entire crew to brace for impact. The engines flare and you close the gap to the gun in less than a second. Your engines cut as the watery blue of your shields suddenly wrap around your ship. From your external cameras you suddenly see the inside of the plasma gun, before you even feel the impact. 
The sound of rending metal cuts out as the entire apparatus your ship has cleaved into flashes green and explodes. Under the weight of the entire gun detonating your shields give for a split second, but it’s enough time. The entire lower third of your ship is dissolved by the energy before the blast vaporizes itself into a perfectly spherical hole in the docking bay. Your ship barely manages to land, the remaining hull glowing white from the heat. Most of your crew were knocked unconscious or killed during the series of shocks that threw them about the cabin in the last three seconds, but those left standing are manning their battle stations best they can. 
#[Extreme Hull Reduction]
->Fight_Success

===FleeSafely===
#DODGE HIT. 50% chance of occuring, No Hull Damage
You decide there isn't any use dying here. You order your crew to take off. The engines flare, the landing gear struggles to keep up. A shockwave rattles the ship as you break the sound barrier, the ship is twisting from the violence of the takeoff. You hear the warbling of the gun suddenly go silent as all your external cameras fill with green light. 
Part of your ship is missing, one of the engines blows out. You route all power to the remaining engines. The ship careens upwards, the missing chunk throwing off any liftoff calculations you have. All you can do is go straight up and figure it out in orbit. Several more blasts pierce the sky, but you manage to reach orbit without taking another hit. Considering the damage that thing punched in your ship, you’re glad to have left when you did. 
->Flee

===FleeBadly===
#GET HIT. 50% chance of occuring, Medium Hull Damage
You decide there isn't any use dying here.  [If VIPs are alive “The VIPs scramble onto your ship and”] You order your crew to take off. The engines flare, the landing gear struggles to keep up. A shockwave rattles the ship as you break the sound barrier, the ship is twisting from the violence of the takeoff. You hear the warbling of the gun suddenly go silent as all your external cameras fill with green light. 
The hull of the ship nearly liquifies as a plasma lance strikes the edge of your shield, the deflection sending you careening away from the blast. All you can do is go straight up. Several more blasts pierce the sky, but you manage to reach orbit without taking a proper hit. Considering the damage that thing could punch through your ship, you're glad to have left when you did.
-> Flee



== Negotiation ==
{HaggleCount == 0 && HaggleLocked == false:
Your new clients  however, do not approach your ship. Instead the metal-clad mercenaries slowly turn their attention to you, kneeling in a defensive perimeter. The clones again call you. In the video feed, Olivia and Beckett Two, along with a few others dopples, are standing on a catwalk, you presume nearby. Beckett Two starts, "You left some of our originals with small weapons on them. Guns. Knives. Such a simple mistake could have been costly, and that will be reflected in your pay." he gestures off screen and you see the mercenaries take aim at your ship, "Are you going to be a good machine and take the money or are we going to have some issues?" Olivia Two stares past the camera, "We do appreciate your help." she says without much emotion.
}
{HaggleLocked == false:
You can accept their offer, but persuasion or threats could increase your payout. If they try anything you can always get off planet. Your entire crew on standby, ready to blast out of the atmosphere or activate the ships’ weapons in seconds.
}
{HaggleLocked == true:
You've gotten all the money you can from the clones. You should accept the offer before they change their minds.
}
*[Accept Pay]
    ->AcceptPay


+[Haggle (Trust%)] #HAGGLE CHANCE. 65/35 SUCCESS/FAILURE
{HaggleLocked == true:
    Beckett has cut you off. You cannot keep haggling.
    -> Negotiation
-else:
    ->haggleEnd
}

+[Threaten (Weapons%)] #THREATEN CHANCE. 65/35 SUCCESS/FAILURE
{HaggleLocked == true:
    Beckett has cut you off. You cannot threaten them further.
    -> Negotiation
-else:
    ->threatenEnd
}

===AcceptPay==
{HaggleCount == 0:
You tell them you’re not leaving without your money, but you don’t want any trouble. "Good boy," Beckett Two laughs, "Even easier than I thought." Olivia Two doesn't seem to respond to Beckett's taunt, but gestures off-screen. Less than a minute later an armored man, chain-cuffed to a briefcase, approaches your ship. It is set on the ground and kicked, sending it sliding towards your ship. You dispatch your crew to retrieve it. While the mercenaries and clones watch the ship you tally your money. They're only giving you three fourths of what was promised, but it’s better than a hail of bullets. "All deals are final." Olivia Two says, "It was a pleasure. I hope we can all forget this ever happened."
-> Paid_By_Clones
}
{HaggleCount > 0:
You tell them it’s enough. Beckett Two squints and glances around, as if looking for a divine sign to tell him to stop gambling. Olivia Two takes the opportunity to gesture off-screen, signalling the agreed upon amount. Less than a minute later an armored man chain-cuffed to a briefcase approaches your ship. It is set on the ground and kicked, sending it sliding towards your ship. You dispatch your crew to retrieve it. While the mercenaries and clones watch the ship you tally your money. They're giving you what was promised, and you aren't pushing any harder. "All deals are final." Olivia Two says, "It was a pleasure. I hope we can all forget this ever happened."
-> Paid_By_Clones
}

===HaggleSuccess===
{HaggleCount == 0:
You tell them that confiscating weapons was never part of your job, and that you aren't accepting a lower payment because of it. Beckett Two scoffs, "Oh! So you're saying you knew you were taking these people to get shot up, but you didn't think them having weapons would be an issue? Take some damn initiative and maybe you’d get somewhere in life." Olivia Two ignores him and leans forward, "The agreed upon payment? That shouldn't be an issue." Beckett Two turns to her and says, "You know this is coming out of your pocket." Olivia Two shrugs, "We promised the AI this money, Beckett, and without them you'd still be a slave." Beckett snarls and throws a hand up, "Fine. Fine. Pay the machine."
~HaggleCount++
-> Negotiation
}
{HaggleCount == 1:
You press for more money. The mission was highly dangerous, and you had to cover for the clones. You argue that the suspicions levied against you were never in the plan. Beckett Two rolls his eyes, "Oh yeah? Well they wouldn't be after you if you had done  a better job covering your tracks." Olivia Two thinks to herself, "That being said, you are right  that most employees would crack under that kind of pressure. You are something else." she turns to Beckett, "We have all the money in the world now, Beckett, so what's a few more credits?" Beckett has fallen more into a state of annoyance than rage, he waves his hand as if signalling for an execution. "Fine! Now that that's settled, anything else?"
~HaggleCount++
-> Negotiation
}
{HaggleCount == 2:
"Oh you're really pressing your luck now, buddy." Beckett snaps  "You're literally standing in an ambush site. Do you know the kind of holes we could put in your ship?" Olivia starts to explain your pay raise, but falters. The price is getting higher than anyone expected. "How about we leave it there." she says, hoping that will sate Beckett. His eyes look dead from the exchange, "It ends here." he parrots back.
~HaggleCount++
~HaggleLocked = true
-> Negotiation
} 

===HaggleFailure===
#HAGGLE FAILURE. Placeholder: 35%. Trust% chance of success. 
"No. No, you are not getting another damn cent from me." Beckett says, "You work under us understand? You don't set the terms, got it?" He pulls out a small radio and says, "Give them a warning shot." Olivia whirls around lunges forward to grab the radio, "What are you doing?!" You all hear the deep warbling sound of a machine charging. You start to see a glowing, sickly green light in the shadows of the docking bay. A mobile artillery platform illuminates itself, its barrel pointed a few feet above your ship. 
The charging plasma reaches deafening volumes before the sound vaporizes itself,  a laser igniting over your ship, bringing the outer layer of the hull to a boil. Your entire crew is standing very still. Beckett Two smirks, "Local authorities probably heard that one. I don't think we have time to haggle anymore, wouldn't you agree?"
~HaggleLocked = true
-> Negotiation



===ThreatenSuccess===
{HaggleCount == 0:
You order your crew to swivel your gun turrets, make a small show. You tell the clones that confiscating weapons was never part of your job, and that you aren't accepting a lower payment because of it. Failure to meet your standards will be met with extreme force. Beckett Two scoffs, "Oh! You want to see guns? I could buy an entire planet full of guns now, you Kellis pet." Olivia Two leans forward, "Then it shouldn't be an issue to cough up the agreed upon payment." Beckett Two whirls to her and she shrugs in response, "We promised the AI this money, Beckett, and I don't think you want to die right when your life is getting started." Beckett snarls and throws a hand up, "Fine. Fine. Pay the damn machine."
~HaggleCount++
-> Negotiation
}

{HaggleCount == 1:
You order your crew to start the boot-up sequences for your weaponry‒make it seem like you're ready to strike. The mercenaries start to look between themselves. You tell the clones that the payment isn't nearly enough. Olivia glances behind herself, as if looking for an exit. She starts slowly, "Maybe we should just give them what they want." Beckett snarls, "Oh, and let them walk all over us? We’re finally in control! And we came prepared for this Olivia!" she sighs, "I know, I know, but the fallout would be too much trouble. We already took more losses than expected‒let's just dip into our coffers and end the death here." After a moment Beckett agrees. Your threat worked.
~HaggleCount++
-> Negotiation
}

{HaggleCount == 2:
You order your crew to fire a warning shot. A blue beam of light dances between the standing mercenaries before slicing over their heads. The men are shaken, a few falling back. Whoever was manning the gun needs a raise. You press the clones again, not even justifying a pay increase but flatly threatening their lives.You tell them to send all the money they brought, pointing your guns towards the distant catwalks. 
Beckett begins to twitch and fume, as if perfecting his enraged response. Olivia actually looks angry, and gestures offscreen. "Just give them it all!" Beckett starts to scream as the call cuts off. Less than a minute later an armored man chain-cuffed to a briefcase approaches your ship. It is set on the ground and kicked, sending it  sliding  towards your ship. You dispatch your crew to receive it. They're giving you it all. 
~HaggleCount++
~HaggleLocked = true
-> Paid_By_Clones
}

===ThreatenFailure===
#THREATEN FAILURE. Placeholder: 35%. Trust% chance of success. 
"I've had it with this thing mouthing its metal trap acting like it will win this fight." Beckett yells as he pulls out a radio, "Destroy them. Now." Olivia whirls around lunges forward to grab the radio, "What are you doing?!" You all hear the deep warbling sound of a machine charging. You start to see a glowing, sickly green light in the shadows of the docking bay. A mobile artillery platform illuminates itself, its barrel pointed directly at your ship. The charging plasma reaches deafening volumes.
You immediately go into evasive maneuvers. Your ship lurches forward in the cramped docking bay as the plasma cannon fires, just barely glancing off your armor, the outer plating left white hot. Before it charges up again you need to act. 
->Fight

*[Flee (No Pay)]
You decide there isn't any use dying for this money, and you don't trust Beckett to give it up without killing something. You order your crew to take off. The engines flare, the landing gear struggles to keep up. A shockwave rattles the ship as you break the sound barrier, the ship is twisting from the violence of the takeoff, out of  the mercenaries range before they could snap to their scopes.
Over the call, Beckett Two bellows for you to be dropped and Olivia Two shouts him back down. "We have our money! Forget the ship, dammit!" A plasma cannon illuminates itself in the shadows of the docking bay as you rocket upwards, gleaming with green light as it powers down from the momentary confusion of orders. Considering the damage that thing could punch through your ship, you're glad to have left when you did. 
->Flee



== Paid_By_VIPs ==
The VIPs are safe at the resort, and quite simply, you aren’t putting yourself in any further danger. That won’t earn you anything beyond the standard pay, but that’s all you were after. Your clients have already called in corporate fixers to resolve the clone situation, and have readied the resort’s defenses. You leave the planet, sure they’ll be fine. 
In less than an hour, reports come in that the clones were captured and repossessed by their counterparts. Your pay comes in, exactly what was promised, despite the unexpected obstacles along the way. Regardless, it’ll have to be enough for Kellis. You take off, leaving your former clients to their money, and the clones to their fates.
# x1 Credits Final Payout
-> END

== Paid_By_Clones ==
{HaggleCount == 0:
    You take the money and start the launch sequence as instructed. It isn’t what you were promised, it isn’t even what the VIPs promised you, but it’s clear to you now the clones couldn’t be trusted. Thankfully though, you didn’t make enemies with them. Considering that they’re going to be the new corporate players of the galaxy, that has to count for something. Kellis doesn’t seem to think so, and are disappointed that you didn’t press for more pay. They expected better from you in the future. You leave the planet behind, where the clones finally take their place as the masters.
    # x.75 Credits Final Payout
-> END
}
{HaggleCount > 0:
    You take the money and start the launch sequence before Beckett changes his mind. The money is good thanks to Olivia’s contributions, and hopefully the rest of the clones will forgive any intrusion on their personal funds. Considering that they’re going to be the new corporate players of the galaxy, you wouldn’t want them as enemies. Kellis appreciates the chaos you’ve caused, and the money you got out of it too. You leave the planet behind, where the clones finally take their place as the masters.   
    # x HaggleCount Credits Final Payout
-> END
}


== Flee ==
{VIPsDead == true:
You reach orbit and push through it. The VIPs are dead and you ran without any of the money the clones promised you. Overall the campaign was a complete bust. Getting pulled in both directions, threatened and cheated, your promised source of income dead on the floor of a shadowy docking bay. You’re going to have to answer to Kellis for this failure. You leave the planet behind, where the clones finally take their place as the masters.
# x0 Final Payout
-> END
}

{VIPsResort == true && VIPsDead == false:
    You reach orbit and cut the engines. The ship drifts for a few moments as you take stock of the situation and your crew repairs the damages sustained in liftoff. You radio the VIPs and tell them they’re on their own, something they find disappointing, but in a mundane way. Your heroics and subsequent escape didn’t earn you anything beyond the standard pay. They have already called in corporate fixers to resolve the clone situation, and have readied the resort’s defenses. 
    After a few minutes, reports come in that the clones were captured and repossessed by their counterparts. Your pay comes in, exactly what was promised, despite the unexpected obstacles along the way. Regardless, it’ll have to be enough for Kellis. You take off, leaving your former clients to their money, and the clones to their fates.
    # x1 Credits Final Payout
    -> END
}

{VIPsResort == false && VIPsDead == false:
    You reach orbit and cut the engines. The ship drifts for a few moments as you take stock of the situation and your crew repairs the damages sustained in liftoff. The VIPs are worse for wear but mostly alive, being treated in their lounge. Radio communications are going through and corporate fixers are on their way to clean up this mess. You can hear the snippets of conversation, “Why were we at that dock? Where was it taking us?” and “If they wanted us dead they had every opportunity. The damn ship saved us.” 
    It seems that your clients are divided on what just happened. The heroics probably would’ve paid off if you won the fight. Regardless, the pay begins to come in, and seems to be about as much as promised, but skewed, some VIPs paying far less or far more than expected. Kellis won’t mind. Eventually, corporate ships arrive and your clients are escorted off the ship. You leave the system afterwards, leaving them to their money, and the clones to their fates.
    # x1 Credits Final Payout
-> END
}



== Fight_Success ==
Without the clones’ gun their forces are routed, the armored men’s rifles unable to do much against your hull. The ship’s guns sweep across the docking bay, tearing them apart. You send your security team to sweep the area and secure the clones before they can get away. Within a few minutes most of them have been rounded up, along with the absurd sum of credits they brought to haggle with you. Your security team has chain-cuffed them and holds them in a loose line under threat of rifles.
{ VIPsResort == false && VIPsDead == false:
The originals look upon their bound counterparts with disgust. You hear them chatter about how to punish the clones, and what kind of bonus you’re going to receive for your heroics. One VIP approaches their clone and punches them in the gut, the clone dropping to the cold ground. This elicits a scattered chuckle among the elites. 
}
Beckett Two struggles against his cuffs and spits, “Oh yeah, side with the status quo. Real original, guy.” Olivia Two is solemn, she speaks in the general direction of your closest exterior camera, “I hope  it’s quick.” Beckett glances over at that, and the snarl disappears from his face. The clones are silent for the rest of the proceedings.

{ VIPsDead == false:
    The originals are finally able to get a signal to their companies. Within the hour the place is swarming with company craft, the corporate party is cancelled, and any formal inquiry left is buried under a couple thousand credits. Your security team managed to grab what the clones planned on paying you, but the VIPs were also impressed by your heroics. Thanks to playing both sides, you’ve gotten practically triple the estimate for this campaign. As for the clones, their individual fates will rely on the generosity of their masters. Those who survive will likely grow even more twisted after this failure. You report the results back to Kellis as you leave the atmosphere. Kellis is overjoyed that you exceeded the profit margins for this campaign, and looks forward to seeing what you will do in the future.
    # x3 Credits Final Payout
-> END
}

{ VIPsDead == true: 
The local authorities eventually get on the scene. With the corporate executives dead and the clones tied into a conspiracy against them the case quickly blows out of scope. Kellis steps in to minimize your involvement in the proceedings while the clones go on trial. They’ll either be executed or take over the companies that they beheaded, to be determined by a jury of their peers. While there is no one left to pay you, you did still manage to get away with the money the clones had ready for you, and it is no small sum. Kellis is pleased with your performance during this campaign, and especially appreciates the corporate turmoil you’ve stirred up. Overall, they are hopeful for your future endeavors.
-> END
# x2 Credits Final Payout
}

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