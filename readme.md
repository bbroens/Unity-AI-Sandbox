# Unity AI playground

### Crowd A* pathfinding AI with flee response from threat
Crowd (green) is pathfinding between random (blue) goals and panics/flees whenever a threat (red) is detected. The threat is passive, all AI and pathfinding happens on the green agents. 

Logic to make the red (threat) agent actively broadcast it's threat is available and can be set to a wider range, or can be used to set off a wide range threat; usable for gunshots for example.

![Preview](https://i.imgur.com/WW3tgjE.gif)

### Fish flocking AI with collision avoidance and swarm intelligence
This looks cute, but is more complex than you might think. The fish are aware of their neighbours, avoid collision and meanwhile follow the famous 3 flocking rules in nature: 

* Move toward the center of the group
* Move towards the average heading of the group
* Steer to avoid crowding the group

Also, when a fish strays too far from the group, it returns to the average centre. The algo has some 'chance' calculations as well to keep it light on cpu.

![Preview](https://i.imgur.com/0BIUaUk.gif)

### Crowd movement AI based on A* pathfinding
Different groups try to reach their predefined goal as quickly as possible following A* path logic. They do however have no respect for other groups, resulting in this chaotic "fire drill" behaviour.

![Preview](https://i.imgur.com/1PLeu5y.gif)
