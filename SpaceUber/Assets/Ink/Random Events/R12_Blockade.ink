VAR randomEnd = ->FightWin

The hinges of the door to your office crack in many different places as Lexa throws it open to enter the room. “Game time baby” she roars over and over with unwavering intensity. “There’s a blockade beginning to surround the ship. They won’t let us pass unless we bribe them. You could do that. Or, you let me have em. Which one?”

+ [Bribe (-100 Credits)]
-> Bribe
+ [Fight (???)] #(50% FightWin (+100 credits) 50% FightLose (--hull --weapons))
    -->randomEnd


==Bribe==
You decide the trouble of fighting an entire blockade at once isn’t worth it. You give them their credits, pass through the blockade, and continue on your job.
-> END

==FightWin==
Lexa gives you a devious smirk and an appreciative nod before slamming her fist into her other hand and walking out. She readies the weapons, and starts blasting away. One by one, she obliterates them all. Ripping through each one like wet space paper. You send crew to the destroyed ships to see if anything can be salvaged. They find a nice chunk of Credits intact on one of them.
-> END

==FightLose==
Lexa gives you a devious smirk and an appreciative nod before slamming her fist into her other hand and walking out. She readies the weapons, and starts blasting away. Many of the opposing ships are destroyed, but the defense team can’t hold them all off. The blockade starts to move in and deal significant damage to your ship. Luckily, you are able to hold on long enough to charge light speed and get away before it’s too late.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> FightWin
    - rng == 1:
        ~randomEnd = -> FightLose
    - else:
        ~randomEnd = -> FightWin
}