VAR randomEnd = -> Gluttony
The ship finds itself in the midst of an asteroid field. However, the journey is interrupted by a blinding flash. A massive aurora stretches into the horizon, shimmering with colors known and unknown, and tt seems to be having adverse effects on any crew member that looks at it. Something needs to be done.
+[Fly Blind]->Blind
+[Continue as Normal] -> randomEnd

== Blind ==
The crew all close their eyes. As a result, you find that controlling the ship without their assistance is more difficult than you had anticipated. After a few collisions with asteroids, you manage to maneuver the ship out aurora's range. The ship has sustained some damage, but the crew are safe.
->DONE
== Gluttony ==
You manage to escape the aurora, but it seems to have affected the crew. They all complain of ceaseless hunger, ransacking the stores of food onboard. They eat until they are unable to continue. You've lost food, but that is all.
->DONE
== Greed ==
You manage to escape the aurora, but it seems to have affected the crew. All of them begin taking what they feel they “deserve”. They steal and hoard credits from any place they can find, including the ship's stockpiled wealth. After they regain their senses, they apologize, but it's unclear who took money from the ship.
->DONE
== Wrath ==
You manage to escape the aurora, but it seems to have affected the crew. They  get belligerent, attacking each other over every perceived slight. Before the ensuing pandemonium is brought to a close, a few crew members are gravely injured. They will survive, but can no longer perform their duties.
->DONE
== Lust ==
You manage to escape the aurora, but it seems to have affected the crew. They become completely enamored with you, believing you can do no wrong. The crew proclaim that they would lay down their lives if you asked it of them. Thankfully, this shameful display doesn’t last long, but you find yourself wishing the crew thought like that more often.
->DONE

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Gluttony
    - rng == 1:
        ~randomEnd = -> Greed
    - rng == 2:
        ~randomEnd = -> Wrath
    - rng == 3:
        ~randomEnd = -> Lust
    - else:
        ~randomEnd = -> Lust
}