VAR name = "Player"

<color=green>"Choose an Event."</color>


* ["Event1"] #Choice 1
    ->CallEvent1
    -> DONE
* ["Event2"] #Choice 2
    ->CallEvent2
    -> DONE
  
  
  
    
==CallEvent1==
    "Call Event 1"
    * ["Event1"] #Choice 1 #Stat Requirement #Food #> #1 
                 #Choice 2 #Some requirement
        ->Event1
        -> DONE
        
==CallEvent2==
    * ["Event2"] #List Action Requirements for each choice here
        ->Event1
        -> DONE




==Event1==
    "Event 1"
                    //Action result code example
    * ["1Food - 1"] #Modify Stat #Food #- #1 #Event 1 
        -> DONE
    * ["1Trigger event"] #Trigger Event #Test Trigger Event #Event 1
        -> DONE
    
==Event2==
    * ["2Food - 1"] #Modify Stat #Food #- #1 #Event 2
        -> DONE
    * ["2Trigger event"] #Trigger Event #Test Trigger Event #Event 2
        -> DONE