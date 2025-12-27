Français | [English](README_en.md)

# 🧮 Modélisation Géométrique – Travaux Pratiques
![License](https://img.shields.io/badge/License-UNLICENSE-red)
![Statut](https://img.shields.io/badge/Statut-Projet_universitaire-green)
![Université](https://img.shields.io/badge/Université-Lyon_2-blue)

Ce dépôt regroupe l’ensemble des travaux pratiques de Modélisation Géométrique, réalisés lors de mon cursus universitaire sous la direction du professeur Monsieur Marwan Ait Addi.

Ces TP abordent les fondements de la modélisation 3D, depuis la génération de primitives géométriques jusqu’à la simplification de maillages et aux modèles volumiques.

## 📁 Structure

Chaque TP est contenu dans un dossier séparé et contient les élémént suivants:
- Le pdf du TP en question
- Un PDF de cours afin d'apprendre / réviser les notions de ce TP
- Un dossier exercise étant le projet Unity du TP

## 🎓 Projet universitaire

Il s’agit de travaux pratiques réalisés dans le cadre du cours de "Modélisation Géométrique".  
L’objectif principal est de mettre en œuvre des algorithmes de géométrie 3D, sans utiliser de primitives ou d’outils 3D déjà existants.
Pour ce faire, le moteur Unity (version 6000.2.9f1) sera utilisé pour la simulation et la visualisation. 

## 🧱 TP1 – Polyèdres et Quadriques

Objectif: Générer des objets géométriques à l’aide de facettes triangulaires.

### Contenu
- Création d’un plan à partir de triangles
- Génération procédurale de différents objets :
  - Cylindre
  - Sphère
  - Cône
- Paramétrisation :
  - Rayon
  - Hauteur
  - Nombre de méridiens et parallèles
  - Objets tronqués ou non

Résumé: Ce TP pose les bases de la représentation surfacique et de la triangulation.

## 📁 TP2 – Lecture et écriture de fichiers OFF

Objectif: Manipuler des maillages 3D via le format **OFF**.

### Contenu
- Lecture et parsing de fichiers `.off`
- Stockage des sommets et des faces
- Calcul du centre de gravité et recentrage du maillage
- Normalisation de la taille (coordonnées comprises entre -1 et 1)
- Calcul des normales par face
- Export de maillages modifiés au format OFF

Résumé: Ce TP met l’accent sur la gestion de données géométriques et l’interopérabilité avec des outils comme MeshLab.

---

## 🧊 TP3 – Modèles volumiques et surfaces implicites

Objectif: Représenter des objets 3D sous forme volumique.

### Contenu
- Énumération spatiale d’une sphère dans une boîte englobante
- Gestion de plusieurs objets volumiques
- Opérateurs booléens:
  - Union
  - Intersection
- Gestion de la résolution (taille des voxels)
- Généralisation à d’autres formes (quadriques)
- Surfaces implicites discrètes:
  - Champ de potentiel
  - Ajout / suppression de matière via un outil

Résumé: Ce TP introduit les représentations volumiques et les surfaces implicites discrètes.

## 🧩 TP4 – Simplification de maillages

Objectif: Réduire la complexité géométrique des maillages 3D.

### Vertex Clustering
- Construction d’une grille 3D à partir de la boîte englobante du maillage
- Regroupement des sommets selon une tolérance `ε`
- Création de sommets représentatifs (moyenne simple ou pondérée)
- Reconstruction de la géométrie et des nouvelles faces

### Étude avec MeshLab
- Comparaison de méthodes de simplification :
  - Quadric Edge Collapse Decimation
  - Clustering Decimation
- Tests sur :
  - Objets sans arêtes vives (ex. Bouddha)
  - Objets avec arêtes vives (ex. maison, immeuble)
- Analyse visuelle et topologique des résultats

Résumé: Ce TP permet de comprendre les enjeux de la réduction de polygones et les limites des algorithmes de simplification.

## 🎯 Objectifs pédagogiques

- Comprendre les bases de la modélisation géométrique
- Implémenter des algorithmes de génération 3D
- Manipuler des maillages et des formats standards
- Comparer des méthodes académiques et industrielles
- Développer une approche algorithmique de la 3D

## 🤝 Crédits

Travaux réalisés dans le cadre du cours de Modélisation Géométrique 
Université Lumière Lyon 2  
Intervenant: Marwan Ait Addi
Développeur: JOURNOUD Lucas

## 📄 License

Ce logiciel est sous [Unlicense](https://web.archive.org/web/20230703162904/https://unlicense.org/), dont les termes sont disponibles dans [LICENSE](LICENSE)
