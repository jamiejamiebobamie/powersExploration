
This project evolved to become a Rogue-like, action-adventure video game called "Psychic Asylum".

The gameplay involves escaping an insane asylum for psychics. You play as one of three psychics or as one of the asylum guards, all descending from the [humanoid](https://github.com/jamiejamiebobamie/powersExploration/blob/master/Assets/Scripts/Humanoid.cs) class.

![Image of A Modular Hallway](https://github.com/jamiejamiebobamie/powersExploration/blob/master/Screen Shot 2019-11-05 at 5.26.37 PM.png?raw=true.)

The insane asylum is [dynamically generated](https://github.com/jamiejamiebobamie/ModularHallway/blob/master/Assets/Scripts/createAsylum/CreateAsylum.cs) before the start of every match and contains randomly placed scene [objects](https://github.com/jamiejamiebobamie/powersExploration/blob/master/Assets/Scripts/World/Scenery.cs) that the psychics interact with when using their powers.

The powers include [psychokinesis](https://github.com/jamiejamiebobamie/powersExploration/blob/master/Assets/Scripts/Powers/PyrokinesisPower.cs), [telekinesis](https://github.com/jamiejamiebobamie/powersExploration/blob/master/Assets/Scripts/Powers/TelekinesisPower.cs), and [shapeshifting](https://github.com/jamiejamiebobamie/powersExploration/blob/master/Assets/Scripts/Powers/CopycatPower.cs). The [guard's "power"](https://github.com/jamiejamiebobamie/powersExploration/blob/master/Assets/Scripts/Powers/GuardPower.cs) is having guns that shoot tranquilizer darts.

This was a first attempt at making a game in Unity and at the moment I am no longer working on it and it remains unfinished.

The main focus would be polishing and debugging everything as well as implementing the NPCs AI (which is no small task) as well as implementing basic game logic for starting/stopping a match.

I've implemented basic [sensing](https://github.com/jamiejamiebobamie/powersExploration/tree/master/Assets/Scripts/AI/Senses) that allows NPCs to see, touch, and hear their environment (senses that are fooled by the shapeshifting, "Copycat" power).

The senses are based around "stimuli" and origins of those stimuli. Each humanoid has a [stimulus](https://github.com/jamiejamiebobamie/powersExploration/blob/0b5a59e3b320f51eed1c0d70dcc4ccdf0725cc62/Assets/Scripts/AI/Senses/Stimulus.cs#L2) origin that informs other NPCs what they are.

If someone would like to use my starter code to develop a more fleshed-out game, please feel free to clone the repo! Just please if you do, reach out to me. I'd love to connect and see where you take it or if you need any help or have any questions. Some of the code is pretty rough.

My email is:
jmccrory@vt.edu
