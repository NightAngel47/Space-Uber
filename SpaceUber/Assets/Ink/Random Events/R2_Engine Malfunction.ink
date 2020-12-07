VAR randomEnd = -> Survive


The ship mechanic, Mateo, contacts you. "There seems to be a major problem with the engine. It's starting to overheat, and if something isn't done soon, the surrounding hull is going to start melting like wax in the desert. A few parts are malfunctioning, and they need to be fixed, pronto."



"We could have some crew members suit up and do the work, but I don't trust the protection suits the company issued us. They're about a century outdated. We could make it safer if we use parts from around the ship, but replacing those would be costly."
* [Risk Crew Members]
    ->randomEnd
+ [Salvage Parts] -> Repair
+ [Leave It Be] -> Leave


=== Survive ===
You order the crew members to fix the engine. Luckily their quick and efficient methods mean the engine is stabilized without much incident. Despite Mateo's warning, the safety gear holds up just fine.
-> END

=== Sacrifice ===
You order the crew members to fix the engine. While the engine is stabilized, Mateo's fears were well rounded, as two crew members suits tear wide open. They are burned to cinders by the heat still emanating off the engine.
-> END

=== Repair ===
Not wanting to risk the lives of your crew members, you have the engine repaired with parts of the ship. While the ship is now safe, the parts used in the process are need to be replaced, and doing so isn't going to be cheap.
-> END

=== Leave ===
You leave the engine be and hope for the best. Luckily, the engine eventually stablizes on its own, but the intense heat has melted much of its surroundings, leaving the hull in a poor state.
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