VAR randomEnd = -> Fix1
VAR randomEnd2 = -> Fix2


The chief engineer, Ratchet, contacts you, "Wee have a little bit of a problem. You see, the waste disposal systems aren't working correctly. The tanks aren't emptying properly, and they're filling up quick. If we don't fix it and soon, all that stuff may damage the ship. We could have some crew members fix it, but it may not be the safest option, y'know. You could also fix it remotely, but that may allow some pressure to build up, and we don't want that." #randomEnd
* [Fix with Crew ] #50% Chance - 1 Crew
    ->randomEnd


* [Fix Remotely ] #(50% Chance - Hull Integrity)
    ->randomEnd2

=== Fix1 ===
You send a couple crew members to fix the waste disposal systems. Their suits help to protect them from the disgusting muck inside the tanks, and they fix the problem quickly before any damage is done.
-> END

=== Drown ===
You send a couple crew members to fix the waste disposal systems. Their suits help to protect them from the disgusting muck inside the tanks, and they manage to fix the problem before any damage is done to the ship. Unfortunately, one of the suits malfunctions, causing it to rapidly fill. Your crew member suffers one of the most disgusting deaths possible.
-> END

=== Fix2 ===
You decide to fix the problem remotely. Using various on-board systems, you manage to repair the disposal systems. While the process was relatively slow, you managed to luck out, as no damage was done to the ship in the meantime.
-> END

=== Damage ===
You decide to fix the problem remotely. Using various on-board systems, you manage to repair the disposal systems. Unfortunately, due to the slow speed of this method, the pressure build-up caused some damage to the ship's hull.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Fix1
        ~randomEnd2 = -> Fix2
    - rng == 1:
        ~randomEnd = -> Drown
        ~randomEnd2 = -> Damage
    -else:
        ~randomEnd = -> Drown
        ~randomEnd2 = -> Damage
}