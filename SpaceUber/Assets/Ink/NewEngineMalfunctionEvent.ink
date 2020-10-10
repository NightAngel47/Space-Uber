A warning comes from the engine room. A malfunction is causing the engine to overheat, and the intense temperature may cause damage to the ship. You can risk engineers to fix it, use other parts of the ship to do the repairs, or leave it be and hope for the best.
* [Risk Engineers (50% chance to lose some crew)]
{shuffle:
    -->Survive
    -->Sacrifice
}
+ [Salvage Parts (-Credits)] -> Repair
+ [Leave It Be (-HP over time)] -> Leave


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