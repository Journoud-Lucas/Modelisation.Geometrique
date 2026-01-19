English | [Français](README.md)

# 🧮 Geometric Modeling – Practical Assignments
![License](https://img.shields.io/badge/License-UNLICENSE-red)
![Status](https://img.shields.io/badge/Status-University_Project-green)
![University](https://img.shields.io/badge/University-Lyon_2-blue)

This repository gathers all the practical assignments for the Geometric Modeling course, carried out during my university studies under the supervision of Professor Marwan Ait Addi.

These assignments cover the fundamentals of 3D modeling, from the generation of geometric primitives to mesh simplification and volumetric models.

## 📁 Structure

Each practical assignment is contained in a separate folder and includes the following elements:
- The PDF of the corresponding assignment
- A course PDF to learn / review the concepts related to the assignment
- An exercise folder containing the Unity project for the assignment

## 🎓 University Project

These practical assignments were completed as part of the Geometric Modeling course.  
The main objective is to implement 3D geometry algorithms without using any pre-existing 3D primitives or tools.

To achieve this, the Unity engine (version 6000.2.9f1) is used for simulation and visualization.

## 🧱 TP1 – Polyhedra and Quadrics

Objective: Generate geometric objects using triangular facets.

### Content
- Creation of a plane from triangles
- Procedural generation of various objects:
  - Cylinder
  - Sphere
  - Cone
- Parameterization:
  - Radius
  - Height
  - Number of meridians and parallels
  - Truncated or non-truncated objects

Summary: This assignment lays the foundations of surface representation and triangulation.

## 📁 TP2 – Reading and Writing OFF Files

Objective: Manipulate 3D meshes using the OFF format.

### Content
- Reading and parsing `.off` files
- Storage of vertices and faces
- Computation of the center of gravity and mesh recentering
- Size normalization (coordinates between -1 and 1)
- Face normal computation
- Export of modified meshes to the OFF format

Summary: This assignment focuses on geometric data management and interoperability with tools such as MeshLab.

## 🧊 TP3 – Volumetric Models and Implicit Surfaces

Objective: Represent 3D objects using volumetric representations.

### Content
- Spatial enumeration of a sphere within a bounding box
- Management of multiple volumetric objects
- Boolean operators:
  - Union
  - Intersection
- Resolution management (voxel size)
- Generalization to other shapes (quadrics)
- Discrete implicit surfaces:
  - Potential field
  - Adding / removing matter using a tool

Summary: This assignment introduces volumetric representations and discrete implicit surfaces.

## 🧩 TP4 – Mesh Simplification

Objective: Reduce the geometric complexity of 3D meshes.

### Vertex Clustering
- Construction of a 3D grid from the mesh bounding box
- Vertex grouping based on a tolerance `ε`
- Creation of representative vertices (simple or weighted average)
- Reconstruction of geometry and new faces

### Study with MeshLab
- Comparison of simplification methods:
  - Quadric Edge Collapse Decimation
  - Clustering Decimation
- Tests on:
  - Objects without sharp edges
  - Objects with sharp edges
- Visual and topological analysis of the results

Summary: This assignment helps understand the challenges of polygon reduction and the limitations of mesh simplification algorithms.

## 🎯 Educational Objectives

- Understand the basics of geometric modeling
- Implement 3D generation algorithms
- Manipulate meshes and standard file formats
- Compare academic and industrial methods
- Develop an algorithmic approach to 3D modeling

## 🤝 Credits

Work carried out as part of the Geometric Modeling course  
Université Lumière Lyon 2  
Instructor: Marwan Ait Addi 
Developer: JOURNOUD Lucas

## 📄 License

This software is released under the [Unlicense](https://web.archive.org/web/20230703162904/https://unlicense.org/),  
the terms of which are available in the [LICENSE](LICENSE) file.
