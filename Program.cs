using Notekeeper;

Menus menus = new();

// Build the main menu.
menus.ConstructMainMenu();

//Connect to and initialize SQL
await NoteTable.SeedTable();

// Run the main menu.
menus.MainMenu();