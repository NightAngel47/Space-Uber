VAR randomEnd = -> ForceTheDoorOpen
Kuon slams the wall with such force that the sound echoes throughout the ship. "I <i>know</i> you could hear that. We have a situation. One of the doors in the Power Core has malfunctioned and can’t be opened. Four crewmates are trapped and losing oxygen inside. Let me blast that sucker open, we need those crewmates for this mission. You could let the engineers fix it, but it may be too late for those crewmates by the time they get it open."

+ [Force the door open]
-> ForceTheDoorOpen
+ [Wait for engineers] 
    ->randomEnd

==ForceTheDoorOpen==
You give Kuon the go-ahead. 20 minutes later, the entire ship violently shakes. Dripping gunpowder, he appears on the camera feed. Kuon takes his time cracking his neck before stating through a smirk "Job is done. Doesn't look too pretty in there though. Might cost you something decent to get that fixed."
-> END

==CrewLives==
After several hours, Kuon jolts back into frame of your cameras. "They got 'em. They're all out and safe. We got lucky this time, captain. If something like this happens again, for the sake of our lives and your success, let me handle the life or death situations on this ship."
-> END

==CrewDies==
After several hours, the dense, heavy footsteps of Kuon roam throughout the halls before he perches in front of another camera with pure fire in his eyes. "Two lived, two suffocated to death. Maybe next time, you should listen to me instead of those algorithms."
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> ForceTheDoorOpen
    - rng == 1:
        ~randomEnd = -> CrewLives
    - else:
        ~randomEnd = -> CrewDies
}