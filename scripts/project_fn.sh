#!/bin/bash

source $(pwd)/scripts/colors.sh

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

get_project_paths() {
    case $projectId in
        1)
            sPrj="$SCRIPT_DIR/../src/ProductSyncService/ProductSyncService.API"
            pPrj="$SCRIPT_DIR/../src/ProductSyncService/ProductSyncService.Infrastructure"
            ;;
        2)
            sPrj="$SCRIPT_DIR/../src/OrderService/OrderService.API"
            pPrj="$SCRIPT_DIR/../src/OrderService/OrderService.Infrastructure"
            ;;
        3)
            sPrj="$SCRIPT_DIR/../src/CustomerService/CustomerService.API"
            pPrj="$SCRIPT_DIR/../src/CustomerService/CustomerService.Infrastructure"
            ;;
        4)
            sPrj="$SCRIPT_DIR/../src/PaymentService/PaymentService.API"
            pPrj="$SCRIPT_DIR/../src/PaymentService/PaymentService.Infrastructure"
            ;;
        *)
            echo "Invalid projectId."
            ;;
    esac

    echo "$sPrj"
    echo "$pPrj"
}

