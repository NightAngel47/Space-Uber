VAR randomEnd = -> Ravenous
    
    Your head of R&D, Lanri, calls you. She appears on the monitor, seemingly very excited, 
    "Sir‒! Ma'am‒ ...Boss. There’s a strange anomaly, impossible lights, no discernable source, encoded data within the color variations, the <i>works.</i> 
    Our flight plan is putting us right through it, so I was hoping I could study the crew afterwards, you know, see what happens?" 
    Protocol insists that exposing your crew to an unidentified aurora is a terrible idea, though the alternative is have the crew fly blind, which could easily lead to collisions.
    
* [Blind Pilots]
-> Blinded
* [Expose Crew to Aurora]
-> randomEnd

== Blinded ==
You quickly turn down Lanri's offer to "just see what happens" and shutter all viewports across the ship, including the cockpit. Despite no blips on the radar and other external sensors, the ship impacts something. Within a few seconds the ship scrapes off of another large object. 



You reduce your speed and continue, until you leave the range of the aurora. Opening the viewports, you see that there were no asteroids in your flight plan, no objects at all. And yet something hit you. 

-> END

== Ravenous ==
You accept Lanri's scientific advice to "just see what happens" and fly through the aurora normally. Your sensors pixelate, as many of your crew stand and begin to run towards food storage. They begin to rip open packets of rations, wrest food from another's hands, and scarf down what they can. 
By the time you clear the aurora, everyone is back to normal and your food stores have been heavily depleted. Lanri doesn't recall what happened and insists you fly through again. You politely decline.

-> END

== Murderous ==
You accept Lanri's scientific advice to "just see what happens" and fly through the aurora normally. Your sensors pixelate, as you start to see the color red quickly fill your lenses. The crew has armed themselves with service tools and are bludgeoning each other to death. 
You lock down the doors to separate the murderers and wait out the path through the aurora. Once you're clear of the lights, the killers suddenly scramble away from the fresh corpses, not realizing who disposed of them. Lanri is horrified, but doesn't recall that this was her idea.
-> END

== Surge ==
You accept Lanri's scientific advice to "just see what happens" and fly through the aurora normally. Suddenly your crew rushes to their stations and your pilots take you off course. Your attempts to wrest control of the ship do not resolve. The ship swerves into the brightest point of the aurora. 
Your sensors pixelate, radiation spikes, and you use the malfunction to rip the ship out of the anomaly. Your crew returns to normal, but strangely your power grid is overproducing now, like your reactor is moving just slightly faster than it should. Regardless, it doesn't seem like there was any damage.
-> END

===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = -> Ravenous
    - rng == 1:
        ~randomEnd = -> Murderous
    - else:
        ~randomEnd = -> Surge

}