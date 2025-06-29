# SowynskyCalorie

*SowynskyCalorie* is a WPF desktop app built as a student project for my 4th semester. track your daily calorie intake by letting you manage foods, meals, and nutrition info
*IMPORTANT* later features of the projects were rushed due to (reasons) some classes do way too much, overall could use a refcactor etc etc, will fix later (propably not)
---
## Features

- Register and log in as a user  
- Add and manage foods with nutritional values  
- Create and track meals made of various foods  
- Store everything locally in a MySQL database  
- calculates your daily protein carbs fats kcal intake (insane)
- can rate meals (wow)
- mediocre ui (crrazy)
#TODO
- refactor, some chunks of code are ugly
- like/dislike for each product
---

## Architecture

- Built using the MVVM design pattern  
- Uses a local MySQL database
- All data stays on your computer  

---

## Security and bad practices

This project is mainly a learning exercise. Since there’s no external server, all data — including the database — is stored locally on your machine (i have to use a mysql db for the project to pass..). That means users can freely access or modify the local data if they wanted to.
and also they will have to change their credentials that are written into the source code (diabolical)

**I’m not using any APIs or backend proxies here, this project is not made to be actually used, simply made to pass the semester lol**. 

---

## Requirements

- Windows OS  
- .NET Framework / .NET Core compatible with WPF  
- Local installation of MySQL Community Server  

---

## Usage

1. Register a new user/log in  
2. Add foods with their nutritional details  
3. Create meals 
4. Track your calories
5. yep
