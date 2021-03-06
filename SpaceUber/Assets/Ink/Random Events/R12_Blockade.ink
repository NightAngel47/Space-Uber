VAR randomEnd = ->FightWin
Kuon reaches you on an emergency signal, "Game time," he says, "There’s a military blockade ahead. Three ships. Galactic government. Say they're keeping everyone off this route in case of 'spies' or 'pirates.' I'm sure the former captain would try to smooth this over with coin, but their weapons look outdated." Kuon gives you a devious smirk. "Let me handle this."

* [Bribe]
-> Bribe
* [Fight] #(50% FightWin (+100 credits) 50% FightLose (--hull --weapons))
    ->randomEnd


==Bribe==
You decide the trouble of fighting even a small, understaffed fleet isn't worth it. You follow the lead of the former captain and give them their credits. Their price is highway robbery. You pass through the resctricted zone, and continue on your job. Kuon cuts the call without another word.
-> END

==FightWin==
Kuon gives you a solemn nod. Soon, the weapons are readied and Kuon is in control. The ship lurches down and blasts the underside of the military ships. They have difficulty maneuvering to hit you, their tech severely outdated. 
Over the course of the next minute, Kuon obliterates them all, one by one, ripping through each one like wet space paper. You send crew to the destroyed ships and salvage anything worth anything, finding some of their shakedown money intact.
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