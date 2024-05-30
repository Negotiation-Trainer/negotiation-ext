<img src="https://github.com/Negotiation-Trainer/negotiation-trainer/blob/a1cf9afacd9900871d50bc5115f7474f1c6a1523/Assets/Connor's%20Paradise%20Logo.png" width="200px"/>

# Connor's Paradise Negotiation Trainer - Library
*Based on [Connor Paradise by The Negotiation Challenge](https://professionals.thenegotiationchallenge.org/downloads/connor-paradise/)*  
## General Information
| :exclamation:  This is a Library! Not the full game!   |
|-----------------------------------------|

![release](https://img.shields.io/github/v/release/Negotiation-Trainer/negotiation-ext?include_prereleases) ![GitHub last commit](https://img.shields.io/github/last-commit/Negotiation-Trainer/negotiation-ext) ![GitHub License](https://img.shields.io/github/license/Negotiation-Trainer/negotiation-ext)  
[![Test & Build on commit](https://github.com/Negotiation-Trainer/negotiation-ext/actions/workflows/build-on-commit.yml/badge.svg)](https://github.com/Negotiation-Trainer/negotiation-ext/actions/workflows/build-on-commit.yml) [![Test & Pre-Release](https://github.com/Negotiation-Trainer/negotiation-ext/actions/workflows/create-new-release.yml/badge.svg)](https://github.com/Negotiation-Trainer/negotiation-ext/actions/workflows/create-new-release.yml)

### Overview
Connor Paradise Negotiation Game is a strategic and collaborative game designed to simulate the challenges and complexities of resource management and negotiation among three distinct tribes on the fictional Mediterranean island of Connor Paradise. Players take on the roles of tribal leaders tasked with rebuilding their communities after a catastrophic storm.

### Game Story
Once upon a time, the island of Connor Paradise, home to three tribes – the Azari in the east, the Beluga in the north, and the Cinatu in the south – enjoyed a peaceful and prosperous existence. However, a recent devastating storm has shattered their infrastructure, compelling the tribes to work together to restore and improve their island. Players must negotiate and collaborate to construct eight vital infrastructure projects, each essential for the island's survival and future resilience.

### Objective
The primary goal is to maximize your tribe’s score by negotiating effectively with other tribe leaders to share resources and assign construction responsibilities. The successful completion of projects ensures the island's recovery and protection against future disasters.

### Game Goal
Our game is designed to help individuals enhance their negotiation skills through the power of artificial intelligence. By integrating the OpenAI GPT-3 model, we create an immersive and authentic experience that adapts dynamically to the player's responses. This advanced AI allows for personalized and realistic interactions, providing players with tailored feedback and scenarios that closely mimic real-world negotiations. Whether you're looking to improve your professional negotiation tactics or simply want to refine your persuasive communication, our game attmempts to show the possibilities of incorporating AI into the learning process.

## Purpose of this Library
This library contains the Services, Models and HttpClient that are needed to make the game function. To not tie the project down to one game engine, the choice was made to seperate some parts out of the main project. The principle when it doesn't need Unity, we do not make it depended on unity was applied. This library contains the logic, the aglorithm and every service that is not depended on Unity. So the project can be easily adapted to another game engine, by using the DLLs from this project.

## Demo
Want to take the latest version of the game for a spin? Try our [DEMO](https://negotiation-trainer.github.io/negotiation-trainer/production/index.html)!

## 🛠️ Tech Stack
- [.NET Standard 2.1](https://dotnet.microsoft.com/en-us/)

## 🛠️ Usage
| :exclamation:  This is a Library! Not the full game!   |
|-----------------------------------------|

### Use Releases
1. Go to the releases page of the repo [here](https://github.com/Negotiation-Trainer/negotiation-ext/releases)
2. Download the zip file.
3. Extract the zip file locally
4. Copy the ModelLibrary, ServiceLibrary, UnityHttpClientsLibrary DLLs
6. Past those in the Resource folder of the main project.
7. Start playing, you now use the most up to date version of this Library!

### Use Development builds
| :warning: These builds can be unstable!       |
|:---------------------------|

1. Clone/Download this [repo](https://github.com/Negotiation-Trainer/negotiation-ext)
2. Follow the instructions [here](https://github.com/Negotiation-Trainer/negotiation-trainer) to clone the main game.
3. Run `dotnet build` to build the Library.
4. Go to the following path: `./ServiceLibrary/bin/Debug/netstandard2.1`
5. Copy the ModelLibrary, ServiceLibrary, UnityHttpClientsLibrary DLLs
6. Past those in the Resource folder of the main project.
7. Start playing, you now use the most up to date version of this Library!

## 🍰 Contributing    
Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are **greatly appreciated**.

Before contributing, please read the [code of conduct](CODE_OF_CONDUCT.md).

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement". Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch `(git checkout -b feature/AmazingFeature)`
3. Commit your Changes `(git commit -m 'Add some AmazingFeature')` (We would suggest using [atomic commits](https://dev.to/this-is-learning/the-power-of-atomic-commits-in-git-how-and-why-to-do-it-54mn) 😉)
4. Push to the Branch `(git push origin feature/AmazingFeature)`
5. Open a Pull Request

## ➤ License
Distributed under the GPL-3.0 License. See [LICENSE](LICENSE) for more information.
