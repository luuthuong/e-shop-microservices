#!/usr/bin/env bash
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
   exit 1;
   ;;
esac

# Stop any existing backend API process
while pid=$(pgrep $prjName); do
   echo $pid | sed 's/\([0-9]\{4\}\) .*/\1/' | xargs kill
done

sPrj=$(get_project_paths "$projectId" | head -n 1)

read -p "Run build before start Y/N (N): " isBuild

isBuild=$(echo "$isBuild" | xargs)

if [[ "$isBuild" == "Y" || "$isBuild" == "y" ]]; then 
   dotnet run --project $sPrj 
else
   dotnet run --project $sPrj --no-build
fi


