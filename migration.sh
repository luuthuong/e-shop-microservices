#!/bin/bash

source $(pwd)/scripts/colors.sh
source $(pwd)/scripts/project_fn.sh

color_echo "magenta" "======= Select projects: ======="
color_echo "cyan" "[1]. Product service"
color_echo "cyan" "[2]. Order service"
color_echo "cyan" "[3]. Customer service"
color_echo "cyan" "[4]. Payment service"
color_echo "cyan" "[5]. Identity server"
color_echo "cyan" "[6]. Api gateway"
color_echo "magenta" "================================"
read projectId

prjName=""

case $projectId in
   1)
      color_echo "green" "Product sevice selected."
      prjName="ProductSerivce"
      ;;
   2)
      color_echo "green" "Order sevice selected."
      prjName="OrderSerivce"
      ;;
   3)
      color_echo "green" "Customer sevice selected."
      prjName="CustomerSerivce"
      ;;
   4)
      color_echo "green" "Payment sevice selected."
      prjName="PaymentSerivce"
      ;;
   5)
      color_echo "green" "Identity server selected."
      prjName="IdentityServer"
   ;;
   6)
      color_echo "green" "Api Gateway selected."
      prjName="ApiGateway"
   ;;
   *)
      color_echo "red" "Invalid choice."
      exit 1
   ;;
esac

sPrj=$(get_project_paths "$projectId" | head -n 1)
pPrj=$(get_project_paths "$projectId" | tail -n 1)

echo $sPrj
echo $pPrj

color_echo "green" "======================================"
echo "Choose an option for $prjName:"
echo "1. Add migration"
echo "2. Update database"
echo "3. Remove migrate (force)"
color_echo "green" "======================================"
read choice

case $choice in
    1)
    	color_echo "yellow" "----- Add migration -----"	
    	echo "input message:"
    	read msg
      color_echo "green" "Migration:\nMessage: $msg\n-----------\n"
      
      if [ [$pPrj == ""] ]; then
         echo dotnet ef migrations add "$msg" -p $sPrj 
         dotnet ef migrations add "$msg" -p $sPrj
      else
         echo dotnet ef migrations add "$msg" -p $pPrj -s $sPrj
         dotnet ef migrations add "$msg" -p $pPrj -s $sPrj
      fi
        ;;
    2)
        color_echo "green" "Updating migration..."
        dotnet ef database update -p $pPrj -s $sPrj
        ;;
    3)
        color_echo "green" "Removing latest migration..."
        dotnet ef migrations remove --force -p $pPrj -s $sPrj
        ;;
    *)
        color_echo "red" "Invalid choice."
        ;;
esac

