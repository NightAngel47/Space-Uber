VAR randomEnd = ->Win
A ship pulls up alongside your own and wastes no time hijacking your communications channel. A man with questionable fashion appears on the monitor, “Hey there, ya space slugs! How fast does that hunk o' junk fly? I bet that ya' can’t beat my beautiful Maxine! First one to the nearest moon wins.”
Lanri opens a private channel with you, "Now I'm not saying we should agree to this, but I'd be lying if I said I didn't want to see how fast this ship can go. Knowing our maximum speed could be very valuable data, furthermore...". She continues to speak for a few minutes, straying from the topic at hand only a few times.

"...and that's why you shouldn't try to splice humans and animals together. So have you made up your mind about the race yet?"
The man has been waiting for a response this whole time, impatiently tapping his fingers, will you take him up on his offer?
+[Accept the Challenge]
->randomEnd

+[Chicken Out]->Leave

== Win ==
Both ships rocket forward, neck and neck. You swiftly approach the moon. At the last second, you reroute more power to the engines. Your ship manages to pull ahead, and you win the race. The man says nothing as your crew cheer, and departs.
->DONE
== Lose ==
Both ships rocket forward, neck and neck. As you approach the moon the man speaks once more, “You didn't think I was givin' it all she's got did you?”. Your crew can only watch as the opposing ship blasts ahead to claim victory. The crew, disheartened, listens to the man laugh as you depart.
->DONE
== Pull ==
Both ships rocket forward, neck and neck. It's anyone's guess as to who will win. However, just as both ships reach the moon the race is cut short. An enforcement ship appears upon the horizon and demands you come to a halt. You comply; the other racer doesn't. He blasts off, leaving you alone. For breaching the speed limit without authorization, your ship is fined a sum of credits, but is otherwise free to go.
->DONE

== Leave ==
The man cackles, “I knew you lot were slugs, but it turns out you're chickens too! Bawk! Bawk! Bawk!” The man's mocking squawks can be heard until your ship leaves communication range.
->DONE

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Win
    - rng == 1:
        ~randomEnd = -> Lose
    - rng == 2:
        ~randomEnd = -> Pull
    - else:
        ~randomEnd = -> Pull
}