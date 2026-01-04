#!/bin/sh
printf '\033c\033]0;%s\a' Platformer2D
base_path="$(dirname "$(realpath "$0")")"
"$base_path/Platformer2D.x86_64" "$@"
