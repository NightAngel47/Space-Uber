VAR randomEnd1 = -> FindCred
VAR randomEnd2 = -> FindCredV2
VAR randomEnd3 = -> FindCredV3

As the ship soars past a nearby planetoid, the crew is treated to a grisly sight. Ships of all makes and models lie utterly decimated, left floating through the dark. You have stumbled upon a cosmic graveyard. Scans indicate that there are no lifeforms present on any ship, but perhaps you could benefit from these circumstances?

+[Salvage what remains]->Sal1

+[Leave]->Leave

== Sal1 ==
Crew members are initially hesitant to venture out, but with some coaxing, they agree.
->randomEnd1

== Sal2 ==
Tensions rise as you ask them to continue. Some refuse. They relent after you inform them they can either return with something or not at all.
->randomEnd2

== Sal3 ==
The crew are practically begging to return to the ship at this point. They claim to see something darting around the graveyard, but cannot seem to keep their eyes on it for more than a moment.
->randomEnd3

== FindCred ==
The crew report finding a few credit stashes. Not much, but some nonetheless.
+[Salvage More]->Sal2
+[Leave]->Leave

== FindCredV2 ==
The crew report finding a few credit stashes. Not much, but some nonetheless.
+[Salvage More]->Sal3
+[Leave]->Leave

== FindCredV3 ==
The crew report finding a few credit stashes. Not much, but some nonetheless.

->Success

== FindFood ==
The crew radio that they’ve managed to find some preserved food, still fit for consumption.

+[Salvage More]->Sal2
+[Leave]->Leave

== FindFoodV2 ==
The crew radio that they’ve managed to find some preserved food, still fit for consumption.
+[Salvage More]->Sal3
+[Leave]->Leave

== FindFoodV3 ==
The crew radio that they’ve managed to find some preserved food, still fit for consumption.
->Success

== GetBent ==
A scream echoes through the radio channel, followed by another, and another. One by one the signals to the crew are lost. Before you have a chance to process what just happened the ship is struck by a forceful impact. You re-orient the propulsion system as the ship is struck once more. You leave the graveyard faster than you have arrived, and you somehow managed to survive whatever had claimed the lives of the less fortunate ships, but only barely.
->DONE

== Success ==
The crew return to the ship despite your instructions. No amount of persuasion can convince them to venture back out. You decide to leave, satisfied with what you have managed to salvage from the lifeless wrecks. However, you can’t help but feel that you’ve dodged a bullet.
->DONE

== Leave ==
You cannot seem to shake the feeling that something is terribly wrong here, and you decide to not stick around and find out. The ship quickly leaves the graveyard, much to the crew's relief.
->DONE

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd1 = -> FindCred
        ~randomEnd2 = -> FindCredV2
        ~randomEnd3 = -> FindCredV3
    - rng == 1:
        ~randomEnd1 = -> FindFood
        ~randomEnd2 = -> FindFoodV2
        ~randomEnd3 = -> FindFoodV3
    - rng == 2:
        ~randomEnd1 = -> GetBent
        ~randomEnd2 = -> GetBent
        ~randomEnd3 = -> GetBent
}