#!/bin/bash

# Function to display text in different colors
color_echo() {
    local color=$1
    local message=$2
    case $color in
        "red")
            echo -e "\e[31m$message\e[0m" # Red color
            ;;
        "green")
            echo -e "\e[32m$message\e[0m" # Green color
            ;;
        "yellow")
            echo -e "\e[33m$message\e[0m" # Yellow color
            ;;
        "blue")
            echo -e "\e[34m$message\e[0m" # Blue color
            ;;
        "magenta")
            echo -e "\e[35m$message\e[0m" # Magenta color
            ;;
        "cyan")
            echo -e "\e[36m$message\e[0m" # Cyan color
            ;;
        *)
            echo "Usage: color_echo color message"
            echo "Available colors: red, green, yellow, blue, magenta, cyan"
            ;;
    esac
}
