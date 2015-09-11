# Entitas 2D Roguelike

This is simple post apocalyptic roguelike game written for the [Unity](http://unity3d.com/) engine.
This project is a ground up rewrite of the [Unity 2D Roguelike Tutorial](https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial)
architected to use [Entitas](https://github.com/sschmid/Entitas-CSharp).
Entitas is an Entity Component System which allows projects to be architected in a clean and extendable manner.

![screenshot](screenshot.gif)

## Rationale

Unity is a powerful tool for game creation but leaves something to be desired in
terms of code architecture. Looking for a better way to organize and architect my
code I found [Entitas](https://github.com/sschmid/Entitas-CSharp). Eager to test
it out, I rewrote one the standard Unity example projects in it. I am hoping this
code will serve as an example on how to use and convert existing project to
Entitas and demonstrate the Entity Component System style of architecture.

## Entity Component System

Entity Component System (ECS) is an architectural pattern for game development that
allows for a high degree of flexibility and maintainability.

Articles on ECS:

* [What is an entity system framework for game development?](http://www.richardlord.net/blog/what-is-an-entity-framework)
* [Wikipedia Article](https://en.wikipedia.org/wiki/Entity_component_system)
* [Introduction to Component Based Architecture in Games](http://www.raywenderlich.com/24878/introduction-to-component-based-architecture-in-games)
* [Understanding Component-Entity-Systems](http://www.gamedev.net/page/resources/_/technical/game-programming/understanding-component-entity-systems-r3013)

## License

This project is licensed under the [MIT license](http://opensource.org/licenses/MIT).
It uses assets from the original tutorial project which is owned by Unity Technologies.
Use of these assets is granted in the [Unity FAQ](http://unity3d.com/unity/faq):

> all assets in our tutorial and example projects may be used in both commercial
and non-commercial use in Unity-based projects. The assets themselves may not be resold, however.

Full details can be found in the [LICENSE](license) file.