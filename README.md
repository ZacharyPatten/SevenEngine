SevenEngine
===========

THIS PROJECT IS STILL IN ITS EARLY STAGES. My name is Zachary Patten, and as of writing this I am the only person working on the project. I am a student at Kansas State University, and besides just doing what I love, I am also writing the engine as school project.

SevenEngine is a game engine being written using C# with the OpenTK bindings for OpenGL. Although I realize certain aspects of the C# language can make it slower than other languages (like C++), but C# is very well designed, easy to write, easy to debug, and it has the best IDE to date (Visual Studio).

I will try to write documentation once the project matures a bit (It's changing too much atm).

Update (9/19/2013): I finished some basic 2D sprite coding, so it is easy to load and draw 2D images (".bmp" files currently the only file type supported).

Update (9/20/2013): I have the bare-bones for a static (non-animated) model class. It correctly renders using a vertex buffer object (unlike GL.Begin()/GL.End() it uses DrawArrays()). In the next week I'll be finishing the static model class and writing an ".obj" importer. After that I'll be writing a bone-hierarchy animated model class.
