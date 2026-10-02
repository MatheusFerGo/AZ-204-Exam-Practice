#!/bin/bash

# Cenário: a empresa precisa mover os blobs do container 'legacy-attachments'
# para a nova storage account da sua NotificationService API.

# o AzCopy exige a URL do blob juntamente com um token de segurança (SAS Token)
# para ter autorização de leitura na origem e escrita no destino.

SOURCE_URL="https://legacystorage.blob.core.windows.net/legacy-attachments?<SAS-TOKEN-ORIGEM>"
DESTIONATION_URL="https://newstorage.blob.core.windows.net/notification-attachments?<SAS-TOKEN-DESTINO>"

# o parâmetro --recursive garante que ele entre em todas as pastas virtuais do contêiner
azcopy copy "$SOURCE_URL" "$DESTINATION_URL" --recursive

echo "Migração massiva concluída com sucesso via server-side copy."