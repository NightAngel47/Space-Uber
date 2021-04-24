VAR randomEnd = ->Survive1
VAR randomEnd2 = -> Survive2

 Doctor Lormay calls you, calm despite worried voices in the background, "It seems that one of the crew members was misusing a recreational drug. He was found in the bunks suffering an overdose. He might not survive the night. I am asking for permission to administer some Conaxel to help stabilize him."
 Pausing, she lets out a brief sigh, "I can understand if you refuse. Our stock of Conaxel is minuscule and refilling it will cost some credits. Plus, while it would bolster his chances to survive, it wouldn't guarantee it."
* [Don't Use Medicine]
    ->randomEnd

* [Use Medicine]
    ->randomEnd2
    
+[Administer Treatment in Medbay]->Medbay

=== Survive1 ===
You tell her to hold off on using the medicine, as it would be a waste to use it if he is likely to die anyway. Luckily, he manages to survive the night. Lormay says that he will be able to get back to work in a day or two, and that she will make sure he doesn't continue his drug abuse.
-> END

=== Dies1 ===
You tell her to hold off on using the medicine, as it would be a waste to use it with the chance he'll die anyway. Within a few hours, he's dead. Whether the medicine would have helped him survive will go unknown. That said, it's better than having a dead crewman as well as lost credits.
-> END

=== Survive2 ===
You have her administer the Conaxel. The crewman survives the night, and Lormay says he will be able to get back to work after a few days rest. You make it clear that any further drug use will not be tolerated, and that the credits for the Conaxel are coming out of his pay.
-> END

=== Dies2 ===
You have her administer the Conaxel. Unfortunately, while the medicine stabilizes his condition at first, he worsens overnight. By morning, he is dead. The credits to refill your stock of Conaxel will have to come from your account. Lormay apologizes for suggesting it in the first place. 
-> END

== Medbay ==
You instruct Ripley to prepare a bed in the Medbay for the man. After administering some stabalizing agents through an IV the man drifts in and out of consciousness. While he recovers you have a security officer confiscate the rest of the man's drug stash. A few hours later the man, now sober, vows to "never touch that shit again".
->DONE

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