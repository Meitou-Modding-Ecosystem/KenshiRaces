<a id="engine-references-refcheck"></a>
# Verificar a compatibilidade com novas versões do Meitou

Um mod é construído em um mecanismo (o submódulo `engine` deste repositório) e reproduzido em qualquer versão Meitou que o jogador tenha. Quando um lançamento altera a assinatura de algo que o mod chama, a fonte do mod ainda pode ser compilada, mas o DLL que já foi compilado solicita ao jogo o membro antigo e falha em tempo de execução com `MissingMethodException`.

Isto não é hipotético. Meitou 0.21 deu a `Inventory.Add(ItemInstance)` um segundo parâmetro opcional (`Add(ItemInstance, bool mainShut = false)`). A fonte que chama `bag.Add(item)` compila em qualquer uma das versões, mas um DLL construído em 0.20 ou anterior chama o método de um parâmetro, que 0.21 não possui mais. Em The Wasteland Remembers isso quebrou as lembranças em jogo, e a exceção parou todo um sistema pelo resto da sessão. Nenhum teste detectou: os testes são compilados em um mecanismo.

<a id="what-it-checks"></a>
## O que verifica

`tools/RefCheck` lê os metadados de um mod DLL construído e lista todos os membros do mecanismo que ele faz referência (métodos, construtores, campos em tipos em `Meitou*.dll`), por nome e tipos de parâmetros, que uma determinada versão não possui. Nada do mod ou do jogo funciona.

`tools/refcheck.sh` cria todos os mods no repositório (cada pasta de nível superior com `meitou.json` e `<Folder>/<Folder>.csproj`), baixa o zip win-x64 de uma versão uma vez (mantendo apenas `Meitou*.dll` e `0Harmony.dll`) e verifica cada montagem nas listas `meitou.json` do mod.

```
tools/refcheck.sh                   # the newest full Meitou release
tools/refcheck.sh v0.21.0 v0.20.1   # given releases, e.g. the oldest your meitou.json allows
CACHE=<dir> tools/refcheck.sh       # where downloaded release DLLs are kept
```

Saída, uma linha por versão e montagem:

```
v0.21.0 ExampleMod/ExampleMod.dll: 1 engine references checked, 0 missing
```

ou, para uma pausa:

```
MISSING Meitou.Simulation.Items.Inventory::Add(Meitou.Simulation.Items.ItemInstance)
```

<a id="in-ci"></a>
## Em CI

A tarefa `Engine references` executa `tools/refcheck.sh` na versão mais recente em cada PR e push, e `Release readiness` aguarda por ela. Um mod que envia um DLL construído em um mecanismo mais antigo também deve verificar esse DLL em cada versão que seu `meitou.json` permite.

<a id="when-it-finds-something"></a>
## Quando encontra algo

Chame o membro de uma maneira que funcione em todas as versões que você suporta: reconstrua no mecanismo mais recente se você descartar os mais antigos ou encontre o membro por reflexão e preencha os parâmetros que você não passa com seus padrões. " `InventoryCompat` do Wasteland Remembers faz o último para `Inventory.Add` ".

As chamadas de reflexão para o mecanismo não são verificadas: elas falham suavemente onde você as escreveu.

[English](../../../docs/refcheck.md) · [Kenshi Races](../README.md)
