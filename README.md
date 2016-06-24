# Image Album using ASP.NET MVC and MongoDB

This is a small project (part of an university's assignment) presenting the basic capabilities of communication with `Mongo` database 
using `C#` and suplied `MongoDB` Driver. It uses ASP.NET MVC (I'm learning to use this technology thus if you find any gross bugs/mistakes
I would be grateful to receive some constructive feedback).

It allows you to register an account (for this purpose I've used the ready-made capabilities of registering users with SQL Database, if
I get some time, I'm willing to convert it to also using `Mongo`, just for practice) and then create albums and manage adding/removing 
photos to them. The information about albums/pictures are all kept in the local collection called `albums`.
