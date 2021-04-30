VAR randomEnd = -> Fix1
VAR randomEnd2 = -> Fix2


The ship's mechanic, Mateo, contacts you, "Now I swear this may sound like the start of one of my jokes, but the waste disposal systems aren't working correctly. The tanks aren't emptying properly, and they're filling up too quick. If we don't fix it —and soon—, all that... stuff may damage the ship.
"We could have some crew members fix it, but that may not be safe or pleasant for them. You could also fix it remotely, but that may allow some pressure to build up. That's something we definitely <i>don't</i> want." 
* [Fix with Crew ]
    ->randomEnd
* [Fix Remotely ]
    ->randomEnd2

=== Fix1 ===
You send a couple crew members to fix the waste disposal systems. Their suits help to protect them from the disgusting muck inside the tanks, and they fix the problem quickly before any damage occurs.
-> END

=== Drown ===
You send a couple crew members to fix the waste disposal systems. They manage to fix the problem before any damage is done to the ship. Unfortunately, one of crew member's suits malfunctions, causing it to rapidly fill with sewage. This leads to one of the most disgusting deaths you have ever had the displeasure of witnessing.
-> END

=== Fix2 ===
You decide to fix the problem remotely. Using various on-board systems, you manage to repair the disposal systems. While the process was relatively slow, you managed to luck out, as no damage was done to the ship in the meantime.
-> END

=== Damage ===
You decide to fix the problem remotely. Using various on-board systems, you manage to repair the disposal systems. Unfortunately, due to the slow speed of this method, the pressure build-up have caused the tanks to put some strain on the hull, leaving them weakened.
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