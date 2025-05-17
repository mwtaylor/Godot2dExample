#!/bin/bash

find . -name "*.cs" -print0 | xargs -0 xgettext -o locale/codestrings.pot -L "C#" -k"Translate"

xgettext -o locale/all.pot locale/strings.pot locale/codestrings.pot
