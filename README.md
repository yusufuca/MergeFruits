# Merge Fruits (Suika Game Clone)

This project is a Suika Game clone built in Unity as a learning exercise, exploring 2D physics-based gameplay, data-driven design, and mobile input handling.

Fruits are standard GameObjects with Rigidbody2D and CircleCollider2D components. The decision to use GameObjects over UI elements was deliberate — UI elements are affected by Canvas scaling and resolution changes, while GameObjects interact directly with Unity's physics engine, which is essential for collision-based merge detection.

Each fruit type is defined as a ScriptableObject containing its sprite, explosion sprite, initial scale, score value, max merge count, and a reference to the next fruit in the evolution chain. This makes adding new fruit types a matter of creating a new asset rather than touching any code.

The spawn system maintains a queue of pending fruits. On drop, the first entry in the queue is activated by enabling its Rigidbody2D simulation and assigning it a unique Instance ID used for merge arbitration. The queue refills automatically to a configurable count.

Merge detection runs in OnCollisionEnter2D. Both fruits must have isDropped set to true, must share the same FruitData type, and the fruit with the higher Instance ID survives while the other is destroyed. Each merge increments a mergeCount on the surviving fruit. When mergeCount reaches maxMergeCount, instead of scaling up further, the fruit evolves — a new fruit is instantiated at the same position using the toEvolve reference from the ScriptableObject, with physics simulation enabled immediately.

Every merge triggers an explosion: Physics2D.OverlapCircleAll finds nearby fruits within a configurable radius, and each one receives an impulse force scaled by inverse distance. A coroutine briefly swaps the surviving fruit's sprite to an explosion sprite before restoring it, providing visual feedback.

The danger line system tracks fruits that remain above a threshold height. Each fruit runs its own timer in Update; if any fruit stays above the line beyond a configurable duration, the game ends. A vignette UI image fades in proportionally as the timer progresses, giving the player a visual warning before game over triggers. The vignette alpha uses SmoothDamp for a smooth transition above the 0.3 threshold.

Input handles both mouse and touch through ClawController. simulateMouseWithTouches is explicitly disabled to prevent mobile browsers from firing both events simultaneously and triggering double drops. Touch input takes priority; mouse input only fires when no touch is active.
