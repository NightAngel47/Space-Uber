VAR name = "Player"
VAR shipIntegrity = 7
"What do you want to do?."


* [Buy Repairs. -50 Credits] "50 credits removed" 
    -> BuyRobots
* [Leave it broken. -50 Hull] "The ship took damage" #Dan
    -> DONE

=== BuyRobots===
    "How about some bots?"
    * [Yes]
    "Cool, here ya go."
    -> DONE
    * [No]
    "Alright, see ya."
    -> DONE