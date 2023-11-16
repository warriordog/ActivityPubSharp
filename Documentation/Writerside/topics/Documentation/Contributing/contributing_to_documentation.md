# Contributing to Documentation

## Expectations

* When a code change requires a change to documentation, then the documentation changes should be in the same commit or PR.
* Contributors are expected to update documentation when needed, or to arrange for someone else to do it.
* Standalone documentation updates (not linked to a code change) are also welcome!

## Standards

* Folder names should be in __title case__ - words are separated by spaces, and significant words are capitalized.
* File name should be in __snake case__ - words are lowercase and separated by underscores.
* Markdown is preferred for all files. If markdown can't / shouldn't be used, then use HTML.
* If possible, place one sentence per line.
* Folder name should match the section structure.
* File name should match the page title. This *can* be ignored when updating a file, if renaming it would break too many links.
* Each section should contain an `index.md` file, which acts as a landing page for the section. This special page is *exempt* from the above title rule.
* When linking to a section, link to the folder, *not* the index file.

## Tips

* To create a link including a space, wrap the link URL in angle brackets. Like this: `[your link text](<your link/url>)`.