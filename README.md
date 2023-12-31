# JumpchainCharacterBuilderWPF
## A new way to store your Jumper data.

## Overview
The Jumpchain Character Builder is a program for storing, managing, and exporting Jumpchain character data. It is designed to take as much of the burden off the user's shoulders in favor of allowing them to focus on the less tedious parts of making builds. Budgeting, discounts, replication of purchases to character sheets, and even the process of exporting a build to share with the Jumpchain community are all automated, among other things.

The goal of this project is primarily to suit my own needs, but I have also made an effort to include functionality that I feel others will find useful. It is my hope that those of you that choose to make use of this program will find it convenient to use. If it doesn't quite work for you for any reason then don't hesitate to let me know. I can't promise I'll take action on every piece of feedback that I receive, but I will make note of it.


## Feature List
1. Jumpchain Overview
   - Full storage of all Jumps and their builds.
   - Store all Origin-related data, including but not limited to the actual Origin, Location, Age, and Species.
   - Per-character lists of Purchases, Drawbacks, and Scenarios.
   - Purchases can have various 'traits' associated with them, which will be combined and calculated in the Cosmic Passport.
   - Companion Import options, which allow a stipend to be granted to selected characters.
   - A complete variety of Purchase categories for both Perks and Items, including the ability to tag a Purchase as a Body-Mod or Warehouse Addon.
   - Various Jump settings, such as whether it is a Gauntlet or not, what the default budget is, stipends, and special Purchase types (Such as a Power-builder section).
2. Cosmic Passport
   - Store all character data, including cosmetic notes such as their personality or physical description.
   - List of Alt-Forms.
   - Full list of Perks, split into categories and automatically replicated from all Jump builds.
   - Attributes and Skills to easily reference Jumper progression, calculated from the traits chosen for Purchases.
   - Learning Rates section, to store data on those essential training boosters.
   - And, of course, all Body-Mod data with the option to choose either a generic Supplement or preset configuration for the SB Body-Mod or Essential Body-Mod.
3. Cosmic Warehouse
   - Store all Warehouse data, with the option to select a Personal Reality-focused configuration.
   - List of Warehouse Addons, stored from all character builds.
4. Cosmic Locker
   - Store all Item data, split into categories just like the character Perk lists.
5. Drawback Supplement
   - Store House-Rules and Universal Drawbacks, with different configurations available. (Currently only my U.U. Supplement has any unique features.)
   - Drawback points are granted in every Jump unless suspended for that Jump or revoked at or before that Jump number.
6. Export
   - This is the big one. Export various forms of data with BBCode, Markdown, or no visual formatting, with a variety of configuration options to customize the look of the output data.
   - Each output option can have its various sections enabled, disabled, and reordered to suit the user's preferences.
   - Configuration options have been drawn from my own observations of how various people format their builds for sharing with the community, to make this feature as useful as possible.
   - Character sheets output, with all of the data stored in the Cosmic Passport feature (save for the Body-Mod) available to output.
   - Jump build output, which can output one or all builds for a given Jump, and which can additionally output the details given for the document itself.
   - Supplement output for Body-Mod, Warehouse, and Drawback Supplement data.
7. Statistics
   - Display statistics data for individual characters and the overall Jumper save file.
   - Displays data such as completed Jumps and Gauntlets, various Purchase types bought, Addons, and a summary of points spent.
   - Additionally displays per-category data for Perks and Items.
8. Jumpchain Options
   - Configure defaults for newly created Jumps. 
   - Select Body-Mod, Warehouse, and Drawback Supplement used, and configure options for each. 
   - These options depend on the specific selections made, and can include various modes for certain Supplements, or on which Jump the Jumper took the Supplement.
   - Add your own user-defined Perk and Item categories, for sorting and displaying your Jumper's purchases.
9. Input Formatter
   - Correctly format text copied from PDF files to remove unwanted line breaks automatically, for easier pasting into various Jump build fields.
   - Can choose to remove or leave intended line breaks (blank lines) in.
10. Jump Randomizer
   - Create any number of lists of Jumps in the Jump Randomizer Settings.
   - Jumps can have a name, a weight to determine likelihood of selection, and a link to the Jump document.
   - Use the Random Jump Selector to pick one or more randomly selected Jumps from these lists.
   - Jump lists are not per-save and will be available for all Jumper saves.


## Use Instructions
(Todo: Write use instructions.)

Download the latest version of the software by navigating to the Releases page on the right side of this page and downloading the JumpchainCharacterBuilder.zip file located in the Assets section of the most recent release. If you download anything from the front page of the repository (where you are right now) then you will only receive the source code of this software. Once you have downloaded the correct file, you can unzip it into its own folder. If you have downloaded the correct file then you should have an executable file in the main folder. If you do not see the JumpchainCharacterBuilder.exe file then you have downloaded the incorrect .zip file.

When running the program, Windows Smartscreen may complain about it being unrecognized. This can be resolved either by clicking 'More Info' in the warning window and then 'Run Anyway'. Alternatively, you can right click on the exe, select Properties in the context menu, and then look for a security notice at the bottom of the properties window. There should be a checkbox that is labeled 'Unblock', and checking this will tell Smartscreen to allow it.

## Credits
Although this program was coded solely by myself, some aspects of the design do call back to my original Jumpchain Character Sheet. As such, I would like to share my thanks once again to those that helped me along the way.
- TobiasCook (Reddit): For creating the original Jumpchain character spreadsheet that one day led to my own creation, thank you.
- He_Who_Writes (Reddit): In addition to producing the expanded form of the Jumpchain character spreadsheet that I eventually used, they also designed the first take on a "skill" system.

***

This project uses the following Nuget packages as dependencies:
- CommunityToolkit.Mvvm - Using the MIT license.
- Microsoft.Extensions.DependencyInjection - Using the MIT license.
