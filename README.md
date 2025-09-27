# Overview

This project acts as an introduction to object-oriented programming in C# with an emphasis on file manipulation and asynchronous functioning.
Additionally, this project has helped me learn to integrate basic local SQL into my C# file.

Notekeeper is a console application for creating, viewing, and editing notes via a local SQL database. All CRUD actions are available.

I created this application for quick jot-down notes. I run a tabletop game, and it's useful to have somewhere to write things down that I can see them later.
Integrating a basic database setup allows me to later connect to an online database and have access to my notes from anywhere.

# Development Environment

I used several console menuing packages during the construction of this application, but none of them seemed to quite serve the purpose I need; they were all either quite barren, or a little too inflexible when it came to leaving and reentering their menu flows. Eventually, I stripped them away and made a very basic C# menu class myself. I'm interested in expanding on it in the future, perhaps even making my own console menuing package.

# Relational Database

I'm using a simple SQLite database structure with a Notes table that contains the following columns: NoteID (Primary Key), NoteName, CreatedBy, Created, EditedBy, LastEdited, Content. The CreatedBy/EditedBy are for potential future multi-user integration.

# Useful Websites

{Make a list of websites that you found helpful in this project}

- https://www.nuget.org/
- https://learn.microsoft.com/en-us/dotnet/csharp/
- https://hevodata.com/learn/c-sql-server/
- https://sqlable.com/sqlite/#

# Future Work

{Make a list of things that you need to fix, improve, and add in the future.}

- I want to make a cloud to store notes, rather than relying on a local database. I'd like to be able to search and sort by more than just "days old", too.
- I want to further improve my menuing with more robust options, though I successfully added an in-app note deletion function.
- I want to clean up the menu 'front-end' with more aesthetic and performant navigation.