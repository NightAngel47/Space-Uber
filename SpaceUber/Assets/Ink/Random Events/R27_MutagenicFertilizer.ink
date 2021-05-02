VAR randomEnd = -> Good

Lanri messages you, saying that she “Has something incredibly to show you!” in Hydroponics. Patching into the cameras, you find that some of the plants have grown considerably bigger than they should be. Lanri excitedly explains that she’s been experimenting with a growth formula in an effort to help produce more food. “I’ve finally cracked it! The plants don’t wither and die when this formula is applied. Now we need only see how the plant grows. Well...what do you think?” On one hand, having more food for your organic crew members is good. On the other, this feels like it might have the potential to backfire considering her past experiments.
+[Let it Grow] -->randomEnd
+[Dispose of It]->Dispose

== Good ==
You give Lanri the go-ahead to continue with the experiment. She promises not to disappoint, and she stays true to her word. The serum proves incredibly effective, drastically increasing growth rate. Lanri is proud that her experiment was successful but says she has a confession to make. “So, I’m going to be honest with you, I didn’t really know what was going to happen. For all we know, these plants could have just as easily developed carnivorous tendencies, but I’m glad this is what happened instead.” You assure her that you are equally pleased with the outcome. She can hardly stop smiling.
->DONE
== Bad ==
You decide to let Lanri see this project to the end. You are surprised when the harvested crops come out looking perfectly ripe and ludicrously large. Unfortunately, it seems these super crops are completely inedible. Some of your crew had to be admitted to the Medbay from their intestinal distress. Lanri sheepishly apologizes for her blunder, “Now we know…” she nervously laughs to herself. Complaints of stomach pain ring out from the Medbay. It seems that you aren’t the only one Lanri should apologize to. She scurries away, embarrassed.
->DONE
== Dispose ==
Seeing as it’s not currently known what would happen if these plants grew to maturity, you explain that you want Lanri to stop the experiment and dispose of the plants already affected. She seems disheartened and doesn’t speak much as she gets rid of her notes. A security officer assigned to clean up finds out the hard way that the plant stems have mutated to share the properties of stinging nettles. It will take time for his hands to heal. Lanri thanks you for your input and says she’ll keep you updated on future prospects.
->DONE

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Good
    - else:
        ~randomEnd = -> Bad
}