# CircularRoads (Aka. Procedually generated road) in Unity


Trello documentation
https://trello.com/b/Nl8nXBK3/little-racer

# Introduction
This program is based on cubic interpolation. To create a Bezier, four points are mandatory.
Every third point is a anchor point, where the Bezier travels through. Every else are the control points to define the shape of the Bezier. 
# Concept
1.
An intial point is created on the right to the center point, which is the transform.position of the object, in between a minimum and maximum value.
2.
Based on 2 x pi/n, where n = amount of points, the next points position is on x = cos(phi) x radius, where r is a radius between the given values min and max. Do until the point count is reached.
3.
Lay the foundation of the cubic Bezier chain or "path" by adding to every generated its corresponding control points and the next anchor points. 
4. 
Correct the control points.
5.
With cubic interpolation, points on the path are created. A given width sets vertices perpendicular to the path point.
6. 
Create mesh based on the created vertices. Add texture.
7. 
Apply collider to the road based on its mesh. (mesh collider)

Misc.
Car controller based on Unity's wheel collider system
