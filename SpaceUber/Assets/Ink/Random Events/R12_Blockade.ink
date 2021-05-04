VAR randomEnd = ->FightWin
Kuon contacts you on an emergency signal. "Game time," he says, "There's a military blockade ahead. Three ships. Galactic government. Say they're keeping everyone off this route in case of 'spies' or 'pirates.' I'm sure Lexa would try to smooth this over with credits, but their weapons look outdated." Kuon sports a devious smirk. "Let me handle this."

* [Bribe]
-> Bribe
* [Fight]
    ->randomEnd


==Bribe==
You decide the trouble of fighting even a small, understaffed fleet isn't worth it. You follow Lexa's lead and give them their credits. Their price is highway robbery and you're surprised anyone would pay it. You pass through the resctricted zone, and continue on your job. Kuon cuts the call without another word.
-> END

==FightWin==
Kuon gives you a solemn nod. He readies the weapons, clearly enjoying himself. The ship lurches down and blasts the underside of the military ships. They have difficulty maneuvering to hit you, as their technology appears severely outdated. 
Over the course of the next minute, Kuon obliterates them all one by one, ripping through each one like wet paper. You send crew to the destroyed ships to salvage anything worth something. They manage to find some of the money these ships had acquired from previous shakedowns.
-> END

==FightLose==
Kuon gives you a solemn nod. Soon, the weapons are readied and Kuon is in control. The ship lurches down and blasts the underside of the military ships. Even with outdated tech though, the military ships are covering all avenues of attack, and are getting hits in. 
Over the course of the next minute, you're peppered with holes. Kuon finds an opening in their formation and you accelerate past, ramping up to light speed as fast as you're able.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> FightWin
    - rng == 1:
        ~randomEnd = -> FightLose
    - else:
        ~randomEnd = -> FightWin
}