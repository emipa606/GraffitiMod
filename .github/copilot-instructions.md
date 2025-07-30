# .github/copilot-instructions.md

## Mod Overview and Purpose

**Graffiti Enhancement Mod** is designed to expand the cultural and recreational activities available in RimWorld by introducing graffiti painting and cleaning as new interactions for colonists. This mod aims to provide a unique artistic outlet, enhance the game's aesthetic diversity, and create new social dynamics through art.

## Key Features and Systems

- **Artistic Expression:** Colonists can paint graffiti on designated surfaces as a form of art, improving their joy levels and reinforcing individual creativity.
- **Graffiti Cleaning:** Dedicated colonists or those with particular job assignments can clean graffiti when necessary, maintaining colony aesthetics and hygiene.
- **Mental Breaks and Spree Activities:** New mental states lead colonists into graffiti painting sprees, creating unexpected art pieces and interactions.
- **Integration into Colony Life:** Graffiti impacts social interactions and colony beauty, influencing colonists' mood and social standings.
  
## Coding Patterns and Conventions

- **Naming Conventions:** Classes are named for their specific functions, following the naming pattern `ClassName_Action`. This pattern aids in understanding their roles within the mod (e.g., `JobDriver_PaintGraffiti`, `MentalState_GraffitiPaintingSpree`).
- **Class Structure:** Most classes derive from base game classes like `JobDriver`, `ThinkNode_JobGiver`, and `MentalState`. This inheritance structure helps integrate mod features seamlessly into the game.
- **Method Accessibility:** Most methods are public unless a specific design pattern requires encapsulation or protection against external modifications (e.g., `private void ChooseNextTarget()`).

## XML Integration

- **XML File Parsing:** Due to errors in parsing XML content, ensure that all XML files strictly follow the expected schema for RimWorld mods. This includes defining custom `ThingDefs`, `JobDefs`, and `MentalBreakDefs` pertinent to graffiti actions.
- **Defining Resources:** Use XML files to declare new items, jobs, and mental states essential for graffiti interactions. Ensure compatibility with existing game data structures by referencing base game definitions as needed.

## Harmony Patching

- **Selective Patching:** Employ Harmony patches thoughtfully to alter game behaviors related to artwork and social interactions. This could involve patching methods responsible for aesthetic calculations or job selection processes when graffiti is present.
- **Preservation of Base Methods:** Ensure that patches introduce minimal side effects to maintain the core game logic's integrity and avoid conflicts with other mods by using `HarmonyPrefix` and `HarmonyPostfix` appropriately.

## Suggestions for Copilot

- **Auto-generation of Repetitive Patterns:** Suggest code snippets for common tasks like job initialization, mental state transitions, and graffiti rendering mechanics.
- **XML Templates:** Provide templates for XML definitions with commonly needed fields populated (e.g., default `label`, `description` for new definitions).
- **Debugging Assistance:** Offer debugging tips for XML validation errors, especially when errors involve schema mismatch or path references.
- **Alternative Implementations:** Suggest alternative methods or design patterns in C# when more efficient or readable forms are possible, particularly in handling game events and state transitions.
- **Patch Recommendations:** Provide suggestions on where Harmony patches might enhance or optimize mod functionality, such as intercepting specific game event triggers.

This guide aims to assist developers in effectively utilizing GitHub Copilot for developing a graffiti-focused mod in RimWorld, ensuring both creativity and functionality.
