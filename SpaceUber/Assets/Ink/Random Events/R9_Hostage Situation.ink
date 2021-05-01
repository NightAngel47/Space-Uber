VAR randomEnd = -> Kill1
The security chief, Kuon, contacts you:  "We have a situation. A crew member has taken a hostage. She is demanding a sum of credits and some food. She says she has an associate that will pass by our ship and pick her up, at which point she will let the hostage go." 
Gritting his teeth, he continues. "While you could comply with these outrageous demands, my team can take care of the situation. We are in position to act as soon as you give word."
* [Comply] 
-> Comply
* [Take Her Out]
    ->randomEnd

=== Comply ===
You order that her demands be met. Giving her the food and credits she demanded, you allow her to make her getaway. True to her word, she lets the hostage go, completely unharmed. Although you alert the authorities afterward, you'll never see any of those credits again.
Kuon seems to be frustrated by this outcome. "Did you even consider trying to rescue the hostage? Giving into demands like this only emboldens other would-be captors." You take his words into consideration as you continue on your journey.
-> END

=== Kill1 ===
You order your security crew to dispose of her. They manage to take her down with swift and precise shots from their weapons. Her former hostage is alright, albeit shaken up by the close call. You have the body of the would-be criminal thrown out the airlock.
Kuon seems pleased, "Good riddance. Disloyalty of this caliber deserves whatever's coming to them." You couldn't agree more.
-> END

=== Kill2 ===
You order your security crew to subdue. While they manage to strike her down with swift and precise shots from their weapons, she executes her hostage in the split second before her own demise. While you store the body of the hostage in the medbay's morgue, you have the murderer's corpse thrown out the airlock.
Kuon slams his fist into the wall. "That coward! Such a death is too good for them." You continue on your journey.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Kill1
    -else:
        ~randomEnd = -> Kill2
}