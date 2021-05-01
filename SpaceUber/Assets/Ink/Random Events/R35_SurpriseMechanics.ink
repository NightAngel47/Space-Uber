VAR randomEnd = -> Garbo
After some rummaging through the cargo hold, a few crew members find a some strange boxes. You recognize them as a discontinued Kellis product: “The Surprise Box”. All one needs to do is deposit an appropriate amount of credits for the box to open, revealing the surprise. The contents could range from worthless garbage to valuables; shall you test your luck?
+[Pay to Play]->randomEnd
+[Jettison the Boxes]->Jett

== Garbo ==
After the box has registered the transfer of credits, it's sides fall away to reveal the surprise within. It held little more than garbage, various toys and trinkets that you have no use for.
->DONE
== Food ==
After the box has registered the transfer of credits it's sides fall away to reveal the surprise within. Inside, you find military grade rations. While they hold little flavor the extra food is welcome.
->DONE
== Munitions ==
After the box has registered the transfer of credits it's sides fall away to reveal the surprise within. Inside, you find a supply of munitions. The security detail will make good use of this windfall.
->DONE
== Power ==
After the box has registered the transfer of credits it's sides fall away to reveal the surprise within. It held some spare power cells. While some of the crew seem disappointed, you know the extra power will help more than they realize.
->DONE
== Jett ==
You decide to simply rid yourself of the boxes. You'll never know what they held , but you figure that you’re not missing much.
->DONE

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Garbo
    - rng == 1:
        ~randomEnd = -> Food
    - rng == 2:
        ~randomEnd = -> Munitions
    - rng == 3:
        ~randomEnd = -> Power
    - else:
        ~randomEnd = -> Power
}