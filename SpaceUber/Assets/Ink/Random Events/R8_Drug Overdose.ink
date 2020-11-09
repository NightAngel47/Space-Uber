VAR randomEnd = ->Survive1
VAR randomEnd2 = -> Survive2

The head doctor, Kelly, calls you, calm despite worried voices in the background, "It seems that one of the crew member's was misusing a recreational drug. He’s here in the medbay suffering an overdose. He might not survive the night. I am asking for permission to administer some Conaxel to help stabilize him." Pausing, she lets out a brief sigh, "I can understand if you refuse. Our stock of Conaxel is small and refilling it will cost some credits. Plus, while it would bolster his chances to survive, it wouldn't guarantee it."
* [Don't Use Medicine ]#(60% Chance - 1 Crew)
    -->randomEnd

* [Use Medicine ]#(20% Chance - 1 Crew -20 Credits)
    -->randomEnd2

=== Survive1 ===
You tell Kelly to hold off on using the medicine, as it would be a waste to use it and have the crewman die on you anyway. Luckily, he manages to survive the night anyway. Kelly says that he will be able to get back to work in a day or two.
-> END

=== Dies1 ===
You tell Kelly to hold off on using the medicine, as it would be a waste to use it and have the crewman die on you anyway. Within a few hours, he's dead. Whether the medicine would have helped him survive  will go unknown, but it is better than having a dead crewman as well as lost credits.
-> END

=== Survive2 ===
You have Kelly administer the Conaxel. The crewman survives the night, and Kelly says he will be able to go back to work after a few days' rest. You make it clear that any more drug abuse will not be tolerated, and that the credits for the Conaxel are coming out of his pay.
-> END

=== Dies2 ===
You have Kelly administer the Conaxel. Unfortunately, while the medicine stabilizes his condition at first, he worsens overnight, dying early the next day. The credits to refill your stock of Conaxel will have to come from your account.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Survive1
        ~randomEnd2 = -> Survive2
    - rng == 1:
        ~randomEnd = -> Dies1
        ~randomEnd2 = -> Dies2
    - else:
         ~randomEnd = -> Dies1
        ~randomEnd2 = -> Dies2
}