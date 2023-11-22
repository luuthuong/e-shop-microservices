#!/usr/bin/env bash

# Installs the requirements for running projects.

set -e

# Add .NET's package repository to the system
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install the requirements
sudo apt update;
sudo apt install dotnet-sdk-7.0 -y;

echo ""
echo "DOTNET $(dotnet --version) installed."
