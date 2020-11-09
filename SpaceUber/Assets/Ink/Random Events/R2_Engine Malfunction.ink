VAR randomEnd = -> Survive


The chief engineer, Ratchet, contacts you, "There seems to be a problem with the engine. It's starting to overheat, and if something isn't done soon, it might cause some damage to the hull. A few parts are malfunctioning, and they need to be fixed. We could have some crew members suit up and do the work, but I don't trust the protection suits the company issued us. We could make it safer if we use parts from around the ship, but they would have to be replaced."
* [Risk Engineers (50% chance to lose some crew)]
    ->randomEnd
+ [Salvage Parts (-100 Credits)] -> Repair
+ [Leave It Be (-20 Hull Durability)] -> Leave


=== Survive ===
You order the engineers to fix the engine. Luckily their quick and efficient methods mean the engine is stabilized without much incident.
-> END

=== Sacrifice ===
You order the engineers to fix the engine. While the engine is stabilized, the heat results in the messy demise of two of your engineers.
-> END

=== Repair ===
Not wanting to risk the lives of your engineers, you have the engine repaired with parts of the ship. While the ship is now safe, the various parts that were used are going to cost a good chunk of change to be replaced.
-> END

=== Leave ===
You leave the engine be and hope for the best. While the engine manages to stay intact, its heat starts melting its surroundings, doing a good amount of damage to your ship's integrity.
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