VAR randomEnd = -> Goodcore
VAR randomEnd2 = -> Badcore

The ship's power readings begin to fluctuate. Gradually at first, but the fluctuations continue to grow in severity. Mateo contacts you through the intercom, informing you that there's something stuck to the outside of the ship right next to the power core. You take a peek through the external cameras to find a number of long, fleshy lumps. It takes only a moment for you to recognize them. These aptly named “Shock Suckers” are energy leeches, and you need to remove them if at all possible.
+[Send the Crew] ->Send
+[Let them Eat] ->Eat
+[Overload the Core] ->randomEnd2
== Send ==
You order a few crew members to suit up, they're going to be taking a little spacewalk. You instruct them to remove the leeches from the exterior of the ship and provide them with plasma prods to get the job done.
->randomEnd
== Bad ==
You watch on the cameras as the crew make their way towards the leeches, until they stand within striking distance. One of the crew ignites the plasma prod and begins attempting to dislodge one of the creatures. Soon enough, most of the leeches have been removed. As they attempt to get rid of the infestation one of the crew members gets engulfed by one of the leeches, hungry for the power supply in the space-suit. Sadly, the leech manages to untether the crew member, who you watch float away. Thankfully, the last of the leeches are removed without incident.
->DONE
== Good ==
You watch on the cameras as the crew make their way towards the leeches, until they stand within striking distance. One of the crew ignites the plasma prod and begins attempting to dislodge one of the creatures. Soon enough,the leeches have been removed. You watch as they drift away, still hungry, looking for their next meal.
->DONE
== Eat ==
You reluctantly leave the leeches to their feast. Surely they can only eat so much? Unfortunately, it appears that they were hungrier than you had anticipated. It will take some time for the core to recover from this experience. You hope that the remaining power will last until you can finish this job.
->DONE
== Badcore ==
 You tell the crew to clear out of the core room and set to overloading the power core. The various gauges and indicators flash red and you watch as the leeches start to convulse. One by one, they writhe and squirm until they fall limp and lifeless off of the ship. You return the core to normal levels, but it seems that this has damaged the hull plating around the core.
->DONE
== Goodcore ==
You tell the crew to clear out of the core room and set to overloading the power core. The various gauges and indicators flash red and you watch as the leeches start to convulse. One by one, they writhe and squirm until they fall limp and lifeless off of the ship. Your gamble appears to have paid off. Aside from the stress on the power core, the ship sustained no significant damage.
->DONE

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Good
        ~randomEnd2 = -> Goodcore
    - else:
        ~randomEnd = -> Bad
        ~randomEnd2 = -> Badcore
}