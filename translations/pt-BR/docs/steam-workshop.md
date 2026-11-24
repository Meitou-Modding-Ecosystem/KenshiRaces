<a id="publishing-to-the-steam-workshop"></a>
# Publicação no Steam Workshop

Você cria o item de Workshop de uma vez, por mão, de uma versão ZIP. Depois disso, [Workshop do Watcher](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md) correndo no seu PC Windows e carregando cada novo GitHub para esse item em um calendário. Todos os passos estão em **Windows PowerShell** No seu PC, assine-se à conta do Steam que será proprietária do item.

Os comandos usam esses valores. Configure-os uma vez na janela do PowerShell, com o nome da pasta do seu mod (como em `release.yml` `mod-folder`e a sua primeira versão:

```powershell
$Mod = "ExampleMod"
$Version = "0.1.0"
$Work = "$HOME\Downloads\$Mod-Workshop"
$Watcher = "$env:LOCALAPPDATA\WorkshopWatcher\WorkshopWatcher.ps1"
```

<a id="1-release-the-mod"></a>
## 1. Livre o Mod

1. Mergulho `beta` Entrar em `main`Então, abra e combine um PR `main` Entrar em `release` ([Brancas](../README.md#branches)).
2. Espere para o fluxo de trabalho de lançamento `release`. O que significa que a fusão `<slug>-v<version>` Com a versão em `meitou.json` (por exemplo, a
`example-mod-v0.1.0`e cria uma libertação com `<Mod>-<version>.zip` e `<Mod>-<version>.zip.sha256`. Se o trabalho não for feito antes de ser marcado, `powershell -ExecutionPolicy Bypass -File tools\Tag-Release.ps1` Aplicando a marca pela mão (`-DryRun` Apenas imprimir os comandos).

<a id="2-install-workshop-watcher-and-sign-in-to-steam"></a>
## 2. Instale o Workshop Watcher e inscreva-se no Steam

Seguindo [Instalação](https://github.com/TheIvoryDisaster/GutenStructure/blob/main/docs/workshop-watcher.md#install) O guia do Workshop Watcher. O instalador descarrega o SteamCMD para `%LOCALAPPDATA%\WorkshopWatcher\steamcmd` e assine-o na sua conta de Steam. Quando ele pede `Add a mod to watch now?`A resposta `n`O item ainda não existe. Desligue este passo se o Workshop Watcher já for instalado.

<a id="3-prepare-the-items-files"></a>
## 3. Prepare os arquivos do item

1. Baixe a libertação ZIP e apagá-lo em `$Work`:

   ```powershell
   New-Item -ItemType Directory -Force $Work
   Invoke-WebRequest "https://github.com/<owner>/<repo>/releases/download/<slug>-v$Version/$Mod-$Version.zip" -OutFile "$Work\$Mod.zip"
   Expand-Archive "$Work\$Mod.zip" -DestinationPath $Work
   ```

`$Work\$Mod` Agora mantém `$Mod.dll`, `meitou.json` e `$Mod.mod`.
2. Salve a imagem de pré-visualidade do item como `$Work\preview.png`: PNG ou JPG, abaixo de 1 MB, quadrado (512 x 512 pixels funcionam).

<a id="4-write-the-vdf"></a>
## 4. Escreva o VDF

A ACMD cria o item a partir de VDF arquivo: um arquivo de texto de `"key" "value"` Pais dentro de um `"workshopitem"` O bloco. Este mandamento escreve `$Work\create.vdf` com os caminhos preenchidos. Edição `title` e `description` Primeiro de tudo:

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

| chave | Valor |
| --- | --- |
| `appid` | `233860`, KenshiID do aplicativo Steam. |
| `publishedfileid` | `0` Crie um novo item. Qualquer outro número de atualizações que existem. |
| `contentfolder` | Caminho completo para a pasta cujos arquivos se tornam o conteúdo do item: a pasta mod da ZIPE não seus pais. |
| `previewfile` | O caminho para a imagem prévia. |
| `visibility` | `0` O público, `1` Amigos apenas, `2` Privado em `3` Não listado Comece com `2` Mude na página do item. |
| `title` | O nome do item no Workshop. Comece com `[Meitou]`Como outros modos de código Meitou fazem. |
| `description` | Uma linha de texto. Escreva a descrição completa na página do item e depois (suporta o Steam BBCode). |
| `changenote` | A primeira entrada do item na guia Notas de Mudança. |

Regras para valores:

- Escreva os caminhos com os avanços (`C:/Users/me/Downloads/...`). A duplicação deve ser feita (`C:\\Users\\me\\...`).
- Uma citação dupla dentro de um valor `\"`.
- Mantenha o arquivo ASCII.

<a id="5-create-the-item"></a>
## 5. Crie o item

```powershell
& "$env:LOCALAPPDATA\WorkshopWatcher\steamcmd\steamcmd.exe" +login <steam account name> +workshop_build_item "$Work\create.vdf" +quit
```

`<steam account name>` é o nome que você inscreve para o Steam com, como dado ao instalador. steamcmd usa o login salvo. Se ele pedir uma senha ou código Steam Guard, execute `powershell -ExecutionPolicy Bypass -File $Watcher login`Depois, execute o comando novamente.

A corrida termina com `Success.`. Abra o Steam > seu perfil > Itens de Workshop, abra o novo item e copie o número depois `?id=` Em seu URLEsse é o ID do item.

<a id="6-finish-the-items-page"></a>
## 6. Fim da página do item

Na página do item no Steam:

1. Aceite o acordo legal do Steam Workshop se a página mostrar o banner. Até que você faça, apenas você pode ver o item.
2. Escreva a descrição completa.
3. Se o mód precisa de outro Mod de Workshop (por exemplo, Rivalry Core), adicione-o abaixo. **Add/Remove Objetos necessários**. Os assinantes são
Ele também pediu para assinar. Com a dependência de `meitou.json` `dependencies`O arranjo do lançador coloca-o primeiro na ordem de carga.
4. Mude a visibilidade para **Público** Quando a página estiver pronta.

<a id="7-watch-the-item"></a>
## 7. Veja o item

```powershell
powershell -ExecutionPolicy Bypass -File $Watcher add-item -Repo <owner>/<repo> -AppId 233860 -ItemId <item ID> -Asset "$Mod-*.zip"
powershell -ExecutionPolicy Bypass -File $Watcher run -DryRun
```

A sequência informa o lançamento atual, como já foi carregado. Remover `$Work` Uma vez que faz.

<a id="each-release-after-that"></a>
## Todos os lançamentos depois de

Em seguida, lança o mód como no passo 1. O próximo lançamento do The Watchers descreve o ZIPVerifique seu SHA-256 e carrega-o para o item, com o título de edição e o link como a nota de mudança. Nunca muda o título, a descrição, a visibilidade do item: muda as páginas do item. `powershell -ExecutionPolicy Bypass -File $Watcher status` Mostra o calendário, a próxima corrida e o último upload.
