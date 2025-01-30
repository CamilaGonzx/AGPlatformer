# Procedural Level Generation with Controlled Difficulty Using Genetic Algorithms 

## Project Overview
This project implements a genetic algorithm to generate procedurally designed platformer levels with a controlled difficulty curve. By evolving level layouts over multiple generations, the algorithm optimizes enemy placement, platform positioning, and reward distribution to create engaging and balanced gameplay experiences.


### Why is this project useful?
Automates level design: Reduces manual work in creating balanced levels.
Adapts to player skill: Ensures appropriate difficulty progression.
Scalable: Can generate an infinite number of unique levels.

The system consists of two main parts:

Unity C# Script – Collects gameplay data, calculates a fitness score for each level, and exports the data as a JSON file.
Python Script – Reads the JSON file, applies a simple genetic algorithm (elitism, tournament selection, crossover, and mutation), and generates five new levels.
### How It Works
Play the Game in Unity

The game records data such as player deaths, jump difficulty, coin collection, and enemy encounters.
A fitness function evaluates how balanced and challenging the level is.
The level data is saved in a JSON file.
Run the Python Script Manually

Reads the JSON file with level data.
Selects the best levels (elitism).
Uses tournament selection to pick parents.
Applies crossover and mutation to generate new levels.
Saves the new levels in a JSON file for use in the next game session.
