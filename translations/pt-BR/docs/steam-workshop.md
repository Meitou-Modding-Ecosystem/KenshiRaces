<a id="publishing-to-the-steam-workshop"></a>
# Publicar na Oficina Steam

Você cria o item Workshop do mod uma vez, manualmente, a partir de um lançamento ZIP. Depois disso, o [Workshop Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) é executado no Windows PC e carrega cada nova versão do GitHub para esse item de acordo com uma programação. Todas as etapas são executadas no **Windows PowerShell**no seu PC, conectado à conta Steam que será a proprietária do item.

Os comandos usam esses valores. Defina-os uma vez por janela do PowerShell, com o nome da pasta do seu mod (como em `release.yml`  `mod-folder`) e sua primeira versão de lançamento:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Libere o mod

1. Mesclar `beta` em `main` e, em seguida, abrir e mesclar um PR de `main` em `release` ([Ramos](../README.md#branches)).
2. Aguarde o fluxo de trabalho de liberação em `release`. Ele marca a mesclagem `<slug>-v<version>` com a versão em `meitou.json` (por exemplo `example-mod-v0.1.0`) e cria uma versão com `<Mod>-<version>.zip` e `<Mod>-<version>.zip.sha256`. Se a execução falhar antes da marcação, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` envia a tag manualmente (`-DryRun` imprime apenas os comandos).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Instale o Workshop Watcher e faça login no Steam

Siga [Instalar](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) no guia Workshop Watcher. O instalador baixa o steamcmd para `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` e faz login na sua conta Steam. Quando perguntar `Add a mod to watch now?`, responda `n` : o item ainda não existe. Pule esta etapa se o Workshop Watcher já estiver instalado.

<a id="3-prepare-the-items-files"></a>
## 3. Prepare os arquivos do item

1. Baixe a versão ZIP e descompacte-a em `$Work` :

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` agora contém `$Mod.dll`, `meitou.json` e `$Mod.mod`.
2. Salve a imagem de visualização do item como `$Work\preview.png` : PNG ou JPG, abaixo de 1 MB, quadrado (512 x 512 pixels funciona).

<a id="4-write-the-vdf"></a>
## 4. Escreva o VDF

steamcmd cria o item a partir de um arquivo VDF : um arquivo de texto de pares `"key" "value"` dentro de um bloco `"workshopitem"`. Este comando grava `$Work\create.vdf` com os caminhos preenchidos. Edite `title` e `description` primeiro:

```powershell
$Dir = $Work.Replace('\', '/')
@"
"workshopitem"
{
    "appid"           "233860"
    "publishedfileid" "0"
    "contentfolder"   "$Dir/$Mod"
    "previewfile"     "$Dir/preview.png"
    "visibility"      "2"
    "title"           "[Meitou] My Mod"
    "description"     "What the mod does. Requires MeitouClient."
    "changenote"      "$Mod $Version"
}
"@ | Set-Content "$Work\create.vdf" -Encoding ASCII
```

| Chave | Valor |
| --- | --- |
| `appid` | `233860`, aplicativo Steam de Kenshi  ID. |
| `publishedfileid` | `0` cria um novo item. Qualquer outro número atualiza o item existente. |
| `contentfolder` | Caminho completo para a pasta cujos arquivos se tornam o conteúdo do item: a pasta mod do ZIP, não seu pai. |
| `previewfile` | Caminho completo para a imagem de visualização. |
| `visibility` | `0` público, `1` apenas amigos, `2` privado, `3` não listado. Comece com `2` e altere na página do item. |
| `title` | O nome do item na Oficina. Comece com `[Meitou]`, como fazem outros mods de código Meitou. |
| `description` | Uma linha de texto. Depois escreva a descrição completa na página do item (ele suporta BBCode do Steam). |
| `changenote` | A primeira entrada da guia Alterar Notas do item. |

Regras para valores:

- Escreva caminhos com barras (`C:/Users/me/Downloads/...`). Uma barra invertida deve ser duplicada (`C:\\Users\\me\\...`).
- Uma aspa dupla dentro de um valor é escrita `\"`.
- Mantenha o arquivo ASCII.

<a id="5-create-the-item"></a>
## 5. Crie o item

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` é o nome com o qual você entra no Steam, conforme fornecido ao instalador. steamcmd usa o login salvo. Se for solicitada uma senha ou código do Steam Guard, execute `powershell -ExecutionPolicy Bypass -File $Watcher login` e execute o comando novamente.

A execução termina com `Success.`. Abra Steam > seu perfil > Workshop Items, abra o novo item e copie o número após `?id=` em seu URL : esse é o item ID.

<a id="6-finish-the-items-page"></a>
## 6. Finalize a página do item

Na página do item no Steam:

1. Aceite o acordo legal do Steam Workshop se a página exibir o banner. Até que você faça isso, somente você poderá ver o item.
2. Escreva a descrição completa.
3. Se o mod precisar de outro mod do Workshop (por exemplo, Rivalry Core), adicione-o em **Adicionar/Remover Itens Necessários**. Os assinantes são então solicitados a assiná-lo também. Com a dependência em `meitou.json`  `dependencies`, o Arrange do inicializador o coloca em primeiro lugar na ordem de carregamento.
4. Altere a visibilidade para **Público**quando a página estiver pronta.

<a id="7-watch-the-item"></a>
## 7. Observe o item

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

A simulação relata a versão atual como já carregada. Remova `$Work` assim que isso acontecer.

<a id="each-release-after-that"></a>
## Cada lançamento depois disso

Libere o mod como na etapa 1. A próxima execução agendada do observador baixa o ZIP, verifica seu SHA -256 e carrega-o no item, com o título do lançamento e o link como nota de alteração. Nunca altera o título, a descrição, a visualização ou a visibilidade do item: altere-os na página do item. `powershell -ExecutionPolicy Bypass -File $Watcher status` mostra a programação, a próxima execução e o último upload.

[English](../../../docs/steam-workshop.md) · [Kenshi Races](../README.md)
