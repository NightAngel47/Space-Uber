While the ship takes a short breather you peruse some of the non-urgent messages and come across an intriguing advertisement. It asks that anyone willing to donate do so in order to fund an experimental project. If enough money is raised it will increase the rewards funders receive. Shall you donate to the cause?
+[Donate a large amount]->Larg
== Larg ==
You contribute a hefty chunk of change to the project , the results of which you receive promptly.
+[Proceed]
{shuffle:
-->IMP
-->SHM
-->Drone
-->Drink
-->Irri
-->Scam
}
+[Donate a small amount]->Smol
== Smol ==
You provide a small amount of credits to the project, and receive the results rather quickly.
+[Proceed]
{shuffle:
-->Drone
-->Drink
-->Irri
-->Scam
}
+[Ignore the Ad]->Ignore

== IMP ==
You’re rewarded with Improved Power Cells, they can hold even more energy. 
->DONE
== SHM ==
You receive instructions on how to improve your hull durability. They prove incredibly effective, further strengthening the hull.
->DONE
== Drone ==
Your donation is rewarded with a pleasant little Security Drone. It will monitor the ship corridors alongside the regular detail.
->DONE
== Drink ==
The revolutionary product your donation helped to fund is a drink machine that never needs to be refilled. The crew are pleased after the machine is installed in the mess hall and thank you for the addition to the ship.
->DONE
== Irri ==
Your compensation comes in the form of a recipe for improved irrigation. After some tweaks to hydroponics it yields wonderful results.
->DONE
== Scam ==
You get nothing. After the credits leave your account so too does the message asking for donations. It appears you’ve been had. Best keep an eye out for something like this in the future.
->DONE
== Ignore ==
You promptly ignore the advertisement and continue on with the journey at hand, content in the credits you’ve managed to keep.
->DONE