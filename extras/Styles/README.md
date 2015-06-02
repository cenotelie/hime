# README #

This directory contains the styling specifications for various text edition software.
They can be used for the follwing files:

* Hime grammars (.gram)
* Hime test suites (.suite)


### Notepad++ ###

To import a language specification in Notepad++, click on the menu `View->User Define Dialog...`, then on the button `Import`.
Import the files:

* Notepadpp_Grammar.xml
* Notepadpp_Suite.xml


### Gedit and derivatives ###

To install the language specifications, run:

```
# For GTK Source View 2.0
$ sudo cp extras/Styles/gtk2_hgram.lang /usr/share/gtksourceview-2.0/language-specs/hgram.lang
$ sudo cp extras/Styles/gtk2_hsuite.lang /usr/share/gtksourceview-2.0/language-specs/hsuite.lang
$ sudo chmod go+r /usr/share/gtksourceview-2.0/language-specs/hgram.lang
$ sudo chmod go+r /usr/share/gtksourceview-2.0/language-specs/hsuite.lang

# For GTK Source View 3.0
$ sudo cp extras/Styles/gtk3_hgram.lang /usr/share/gtksourceview-3.0/language-specs/hgram.lang
$ sudo cp extras/Styles/gtk3_hsuite.lang /usr/share/gtksourceview-3.0/language-specs/hsuite.lang
$ sudo chmod go+r /usr/share/gtksourceview-3.0/language-specs/hgram.lang
$ sudo chmod go+r /usr/share/gtksourceview-3.0/language-specs/hsuite.lang
```
