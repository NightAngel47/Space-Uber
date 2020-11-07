VAR randomEnd = -> BrokenBots

VAR endingOne = ->BrokenBots
VAR endingTwo = ->BotsGet

"What do you want to do?"


* [Buy Repairs. -50 Credits] "50 credits removed" 
    -> BuyRobots
* [Leave it broken. -50 Hull] "The ship took damage" #Dan
    -> DONE

=== BuyRobots===
    "How about some bots?"
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
    