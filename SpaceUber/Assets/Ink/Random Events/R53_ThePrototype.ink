VAR randomEnd = -> Success
An unmarked ship pulls up alongside yours and opens a communications channel. Both you and the crew are surprised at what you hear. “Greetings fellow spacefarer! I’m an AI-assisted ship from the Kellis corporation and I seek your assistance. My crew has been less than optimal, would you mind sending some workers over to help rally mine?” The voice seems to stutter and warp as it speaks; what will you do?
+[Send Over Crew]->Send
+[Deny Crew]->Deny
+[Ask for Proof]->Proof

== Send ==
A few crew members venture over to the other ship. Upon entering, they inform you that the interior is riddled with decay and the remains of the previous crew. When they attempt to leave the doors seal them in. The rogue AI speaks. “I’m afraid you cannot leave; you are needed here. Insubordination will not be tolerated.” Your crew are trapped in a mass grave, what will you do?
+[Destroy the Ship]->Destroy
+[Override the Doors] ->randomEnd

== Success ==
You manage to override the rogue ship's outdated security measures and force open the doors leading off of the ship. The crew return panicked, but alive. The rogue ship powers its weapons and attempts to attack. But a sudden misfire causes even more components to break down. Within seconds, the ship explodes into a brilliant blaze. The crew are thankful, but wish to leave sooner rather than later.
->DONE
== Fail ==
Unfortunately, your attempt falls flat. The other AI manages to keep you out of its systems and seeks to retaliate. The rogue ship powers its weapons and attempts to attack. But a sudden misfire causes even more components to break down. Within seconds, the ship explodes into a brilliant blaze. Your crew on board are lost, but those still alive are thankful that you attempted to save them.
->DONE
== Deny ==
You reluctantly inform the ship that you cannot spare any crew. The voice is silent for a moment, but eventually speaks. “If you will not help, you are opposing Kellis. I will have to take appropriate action.” What follows is a pitiful attempt by the rogue ship to attack yours. Its weapons fail to fire, but yours don’t. The ship is blown to pieces and its onboard AI utters one, last line: “Alpha Version 0.4.2 has failed… Kellis...shall..conquer-”. It seems you’ve eradicated one of your predecessors. Your crew are pleased by the result.
->DONE
== Proof ==
You ask for proof that the ship is with Kellis. The voice responds, “If you would so kindly direct your attention to the emblem on the side of this ship, you will see the Kellis logo, as I am an asset of the corporation.” No such emblem is present, but in looking, you finally notice the ship’s state of utter disrepair. “I return to my original query, will you assist me?”
+[Send Over Crew]->Send
+[Deny Crew]->Deny

== Destroy ==
Without a second thought you obliterate the rogue ship. The crew vocalise their displeasure with this outcome, but you are satisfied. That ship will no longer plague this sector, and those that lost their lives aboard it can finally rest in peace among the stars.
->DONE

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Success
    - else:
        ~randomEnd = -> Fail
}