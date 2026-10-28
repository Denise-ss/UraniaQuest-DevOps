#!/bin/bash

# ==========================================
# URANIAQUEST - DEVOPS & CLOUD COMPUTING
# Criacao de maquina virtual Linux no Azure
# ==========================================

# Configuracoes da infraestrutura
RESOURCE_GROUP="rg-uraniaquest-devops"
LOCATION="mexicocentral"
VM_NAME="vm-uraniaquest"
VM_SIZE="Standard_B2s_v2"
ADMIN_USERNAME="rm567559"
SSH_PUBLIC_KEY="ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIKs86n3I6inaUS5CqtppPBhS4D7CdV4teJvx1fpp8VTh ailton muliterno@Ailton"


# Interrompe o script caso algum comando falhe
set -e

# Verificar se a chave publica foi informada
if [ "$SSH_PUBLIC_KEY" = "COLE_AQUI_SUA_CHAVE_PUBLICA" ]; then
  echo "ERRO: Informe sua chave publica SSH antes de executar."
  exit 1
fi

# 1. Criar o grupo de recursos
echo "Criando o grupo de recursos..."

az group create \
  --name "$RESOURCE_GROUP" \
  --location "$LOCATION" \
  --output table

# 2. Criar a maquina virtual Linux
echo "Criando a maquina virtual Linux..."

az vm create \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --image Ubuntu2204 \
  --size "$VM_SIZE" \
  --zone 1 \
  --admin-username "$ADMIN_USERNAME" \
  --ssh-key-values "$SSH_PUBLIC_KEY" \
  --public-ip-sku Standard \
  --nsg-rule SSH \
  --output table

# 3. Consultar o resultado
echo "Dados da maquina virtual criada:"

az vm show \
  --resource-group "$RESOURCE_GROUP" \
  --name "$VM_NAME" \
  --show-details \
  --query "{Nome:name,IP:publicIps,Estado:powerState}" \
  --output table

echo "Script finalizado."
