VAR randomEnd = -> Survive


The ship mechanic, Mateo, contacts you. "There seems to be a major problem with the engine. It's starting to overheat, and if something isn't done soon, the surrounding hull is going to start melting like wax in the desert. A few parts are malfunctioning, and they need to be fixed, pronto. We could have some crew members suit up and do the work, but I don't trust the protection suits the company issued us. They're a century outdated. We could make it safer if we use parts from around the ship, but replacing those would be costly." It's clear that he wants to avoid sending the crew out, but the choice is yours.
* [Risk Crew Members]
    ->randomEnd
+ [Salvage Parts] -> Repair
+ [Leave It Be] -> Leave


=== Survive ===
You order the crew members to fix the engine. Luckily, their quick and efficient methods mean stabilizing the engine occurs without much incident. Despite Mateo's warning, their safety gear holds up just fine.
"Well I'll be damned. I've never been so glad to be wrong before." The tension leaves his face as he chuckles softly, "Let's get a move on before something else goes wrong."
-> END

=== Sacrifice ===
You order the crew members to fix the engine. While the engine is stabilized, Mateo's fears were well founded. Two crew members suits tear wide open. They are burned to cinders by the heat still emanating off the engine.
Mateo lets out a heavy sigh, "Never thought I'd hate being right so much. Let's get the hell out of here before anything else can go wrong."
-> END

=== Repair ===
Not wanting to risk the lives of your crew members, you have the engine repaired with materials from the ship. While the engines are fuctional once again, the parts used in the process will need to be replaced. Doing so isn't going to be cheap.
-> END

=== Leave ===
You leave the engine be and hope for the best. Luckily, the engine eventually stablizes on its own, but the blistering heat has melted much of its surroundings, leaving the hull in a poor state.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Survive
    - rng == 1:
        ~randomEnd = -> Sacrifice
    - else:
        ~randomEnd = -> Sacrifice
}