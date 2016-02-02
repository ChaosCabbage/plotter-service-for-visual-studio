# plotter-service-for-visual-studio

This is an extension for Visual Studio 2013 onwards. 

It does two things: 
- it provides a dockable window inside Visual Studio with a cartesian coordinate grid draw on it.
- it provides a service for other extensions who want to draw some 2D line geometry on the grid.

I use this myself in conjunction with a UIVisualizer (See near the bottom of https://msdn.microsoft.com/en-us/library/jj620914.aspx) to 
help me in debugging 2D geometry algorithms. 
(Maybe I'll upload a stripped down version of my visualizer project as an example.)

