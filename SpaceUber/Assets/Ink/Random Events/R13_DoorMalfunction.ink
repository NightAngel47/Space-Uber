VAR randomEnd = -> ForceTheDoorOpen

The metal door to your office flies open and slams the wall with such force the whole ship could hear it echo. Lexa barges in. "We have a situation. One of the doors in the Power Core malfunctioned and can’t be opened. 4 crewmates are trapped and losing oxygen inside. Let me blast that fucker open, we need those crewmates for this mission. You could let the engineers fix it, but it may be too late for those crewmates by the time they get it open.”

+ [Force the door open (-100 Credits)]
-> ForceTheDoorOpen
+ [Wait for engineers (???)] 
    -->randomEnd

==ForceTheDoorOpen==
Option 1: Force the door open (-100 Credits)
You give Lexa the go ahead. 20 minutes later the entire ship violently shakes. Dripping gunpowder, Lexa leisurely strolls back into your office. She takes her time cracking her neck before stating through a smirk “Job is done. Doesn’t look too pretty in there though. Might cost ya something decent to get that fixed”
-> END

==CrewLives==
(Crewmates are saved) After several hours, Lexa jolts back into the room. “They got em. Their all out and safe. We got lucky this time captain. But next time, for the sake of our lives and your success, let me handle the life or death situations on the ship”.
-> END

==CrewDies==
(2 crewmates die) After several hours, the dense, heavy footsteps of Lexa roam throughout the halls before she stops in the middle of your office with pure fire in her eyes. “2 lived, 2 suffocated to death. Next time your algorithms should realize you listen to me in these situations if you want to succeed”.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = ForceTheDoorOpen
    - rng == 1:
        ~randomEnd = CrewLives
    - else:
        ~randomEnd = CrewDies
}