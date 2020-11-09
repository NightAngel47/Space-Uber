VAR randomEnd = -> BrokenBots

VAR endingOne = ->BrokenBots
VAR endingTwo = ->BotsGet

"What do you want to do?"


* [Buy Repairs. -50 Credits]
    -> BuyRobots
* [Leave it broken. -50 Hull]
    -> DONE

=== BuyRobots===
    "How about some bots?" #randomEnd
    * [Yes]
    -> randomEnd
   
    * [No]
    "Alright, see ya."
    -> DONE
    
===BrokenBots===
    "Sorry, all of our bots are actually broken."
    ->DONE

===BotsGet===
    "Just as requested"
    ->DONE
    
===function RandomizeEnding(rng)===
{ 
    - rng == 0: 
        ~randomEnd = BrokenBots
    - rng == 1:
        ~randomEnd = BotsGet
    -else:
        ~randomEnd = BotsGet
}