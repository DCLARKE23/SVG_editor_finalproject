# SVG_editor_finalproject

This is a simple Windows Forms app developed in C# which allows the user to draw a variety of shapes to create an SVG document.

The primary features as as follows:
-> Shape tool which supports Squares, Rectangles, Circles and Ellipses
-> Colour tools which show the Hexadecimal value of the current colour
-> Undo and Redo changes
-> Background grid for easy alignment
-> Saving and Opening JSON files
-> Export file to SVG
-> Select tool 
-> Editable Layers

# Libraries Used

Aside from using Windows Forms, the <a href="https://github.com/svg-net/SVG">SVG.NET</a> library was used to allow for SVG graphics. I also used the project at <a href="https://github.com/cdiggins/svg-editor">https://github.com/cdiggins/svg-editor</a> written by my professor for this course. I used it as a project skeleton and for ideas throughout the development process.

# Screenshots

![Daytime Scene](Screenshots/screenshot1.png)
![UFO](Screenshots/screenshot2.png)
![Default App](Screenshots/screenshot3.png)
![Grid Demo](Screenshots/screenshot3.png)

# Known Issues

-> Undo and Redo are broken, not allowing the user to continue drawing afterwards
-> The Selection tool does not allow the user to deselect a shape after selecting

# What's Next

-> Improve the Hexadecimal colour feature to allow the user to enter a specific Hexadecimal value to select a colour
-> Refactoring and reorganizing of code
-> Polish the application UI
