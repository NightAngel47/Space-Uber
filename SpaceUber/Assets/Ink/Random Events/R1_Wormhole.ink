VAR randomEnd = -> MinorDamage


Something catches your attention in the void between the stars. A distortion in space where light is warped, mangled, and trapped in the cosmic maw of a wormhole. 
As you draw closer, a nearby star is ripped from its position in the void, stretching into stellar spaghetti as it crosses the event horizon. A quick number crunch reveals that there is only a small chance that the same would happen to your ship should you venture closer. 
+ [Leave it be] -> Leave
+ [Approach the Wormhole] ->randomEnd 


=== Leave ===
While it is tantalizing to see just what lies beyond the wormhole, risking your cargo and crew over a passing curio wouldn't be the best course of action. Besides, the galaxy is a big place. Chances are this won't be the last wormhole you’ll run into.
->END

=== MinorDamage ===
As you approach the umbral blot, the alarms start blaring and the gravitational pull spikes. All engines are at full power, but the gravity is too strong to resist. However, rather than being completely swallowed, the ship only skirts along the event horizon before slingshotting free. Thankfully, this shouldn't affect your flight path too heavily, but the pressure has torn off the outer plating of your ship.
->END

=== Pranked ===
As you approach the umbral blot, the alarms start blaring and the gravitational pull spikes. To the horror of those comprised of meat and bone, their bodies begin to stretch and distort. In the few seconds you have, you rout all power towards the engines and only barely manage to escape the event horizon. Many members of your crew are dead or disfigured, their fragile flesh contorted across your ship. Thankfully, your hull and cargo were unaffected. 
->END

=== Money ===
As you approach the umbral blot, the alarms start blaring and the gravitational pull spikes. In an instant, the ship is wrenched from its position in space into the center of the wormhole. The gravitational density grows exponentially around you, with a metallic groan echoing through the ship. However, the wormhole somehow fails to condense you into its singularity. 

After a few short seconds, the ship lurches beyond the wormhole's influence, seemingly unaffected. However, it wasn't for nothing. Upon further inspection, you find a hyper-condensed diamond in the direct center of the ship. 
->END

=== Shuffle ===
As you approach the umbral blot, the alarms start blaring and the gravitational pull spikes. The closer end of the ship is wrenched into the void, and the ship begins to spin wildly. Crewmates grip onto the walls and violently retch. It's times like these that make you thankful for your lack of biological components. 

When the spinning comes to a stop you realize that the wormhole is behind you now. Strangely, the plants onboard are significantly overgrown, heavy with food. Your crew, on the other hand, seems to have aged several months.
->END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> MinorDamage
    - rng == 1:
        ~randomEnd = -> Pranked
    - rng == 2:
        ~randomEnd = -> Money
    - rng == 3:
        ~randomEnd = -> Shuffle
    - else:
        ~randomEnd = -> Shuffle
}