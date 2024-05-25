#!/usr/bin/env bash

set -e

# Add .NET's package repository to the system
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Install the requirements
sudo apt update;
sudo apt install dotnet-sdk-8.0 -y;

echo ""
echo "DOTNET $(dotnet --version) installed."
