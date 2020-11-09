VAR randomEnd = -> Kill1

The head of security, Lexa, contacts you,  "It seems we have a situation. A crew member has taken another hostage. She is demanding fifty credits and some food. She says she has a 'buddy' that will pass by our ship and pick her up, at which point she will let the hostage go." Gritting her teeth, she continues, "While you could comply with these outrageous demands, I recommend you simply let us take care of the situation."
+ [Comply (-50 Credits, -2 Food, -1 Crew)] #(-50 Credits, -2 Food, -1 Crew)
-> Comply
* [Take Her Out ] #(50% Chance - 1 Crew, 50 Chance -2 Crew)
    -->randomEnd

=== Comply ===
You order that her demands be met. Giving her the food and credits she asked for, you allow her to be picked up by another ship. True to her word, she lets the hostage go, completely unharmed. Although you alert the authorities afterward, it is unlikely you will get back any of the credits.
-> END

=== Kill1 ===
You order your security crew to take her out. They manage to take her down with swift and precise shots from their weapons. Her former hostage, although currently in a state of shock, will be alright. You have the body of the would-be criminal thrown out the airlock.
-> END

=== Kill2 ===
You order your security crew to take her out. While they manage to take her down with swift and precise shots from their weapons, she was able to put a shot through the head of her hostage in the split second before her own demise. While you store the body of the hostage in the medbay's morgue, you have the murderer's corpse thrown out the airlock.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = Kill1
    - rng == 1:
        ~randomEnd = Kill2
    -else:
        ~randomEnd = Kill2
}